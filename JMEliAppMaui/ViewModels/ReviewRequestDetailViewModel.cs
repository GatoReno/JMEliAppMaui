using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.ViewModels
{
    [QueryProperty(nameof(Request), "Request")]
    public class ReviewRequestDetailViewModel : BindableObject
    {
        private EnrollmentRequest _request;
        private string? _inscriptionFee, _tuition, _reviewNotes;

        public EnrollmentRequest Request
        {
            get => _request;
            set { _request = value; OnPropertyChanged(); }
        }
        public string? InscriptionFee { get => _inscriptionFee; set { _inscriptionFee = value; OnPropertyChanged(); } }
        public string? Tuition { get => _tuition; set { _tuition = value; OnPropertyChanged(); } }
        public string? ReviewNotes { get => _reviewNotes; set { _reviewNotes = value; OnPropertyChanged(); } }

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        private readonly IFirebaseService _firebase;
        private readonly IContractGeneratorService _contractGen;

        public ReviewRequestDetailViewModel(IFirebaseService firebase, IContractGeneratorService contractGen)
        {
            _firebase = firebase;
            _contractGen = contractGen;
            ApproveCommand = new Command(OnApprove);
            RejectCommand = new Command(OnReject);
        }

        private async void OnApprove()
        {
            if (Request == null) return;

            var confirm = await Shell.Current.DisplayAlert("Confirmar",
                $"¿Aprobar solicitud de {Request.FullName} y crear inscripción?", "Aprobar", "Cancelar");
            if (!confirm) return;

            try
            {
                // 1. Create Client
                var client = new ClientModel
                {
                    FullName = Request.FullName,
                    Email = Request.Email,
                    Phone = Request.Phone,
                    Address = Request.Address,
                    Occupation = Request.Occupation,
                    Relationship = Request.Relationship,
                    Status = "alta"
                };
                var clientId = await _firebase.AddAsync(client, "Clients");
                client.Id = clientId;
                await _firebase.UpdateAsync(client, "Clients", clientId);

                // 2. Create Students + Enrollments + Contracts
                if (Request.Students != null)
                {
                    foreach (var studentReq in Request.Students)
                    {
                        var student = new StudentModel
                        {
                            FullName = studentReq.FullName,
                            Gender = studentReq.Gender,
                            Allergies = studentReq.Allergies,
                            BloodType = studentReq.BloodType,
                            MedicalHistory = studentReq.MedicalHistory,
                            Level = studentReq.DesiredLevel,
                            Grade = studentReq.DesiredGrade,
                            Observations = studentReq.Observations,
                            ClientId = clientId,
                            Status = StudentStatus.Inscrito,
                            Tuition = Tuition
                        };
                        var studentId = await _firebase.AddAsync(student, "Students");
                        student.Id = studentId;
                        await _firebase.UpdateAsync(student, "Students", studentId);

                        // Enrollment
                        var enrollment = new EnrollmentModel
                        {
                            StudentId = studentId,
                            ClientId = clientId,
                            Status = StudentStatus.Inscrito,
                            EnrollmentDate = DateTime.Now.ToString("yyyy-MM-dd"),
                            StudentName = student.FullName,
                            ClientName = client.FullName,
                            Level = student.Level,
                            Grade = student.Grade,
                            Tuition = Tuition,
                            InscriptionFee = InscriptionFee ?? "0"
                        };
                        var enrollId = await _firebase.AddAsync(enrollment, "Enrollments");
                        enrollment.Id = enrollId;
                        await _firebase.UpdateAsync(enrollment, "Enrollments", enrollId);

                        // Contract
                        var cycle = new CycleModel { Name = "Ciclo Actual" };
                        var html = await _contractGen.GenerateContractHtmlAsync(student, client, cycle, "Inscripcion");
                        var contract = new ContractModel
                        {
                            Type = "Inscripcion",
                            Status = "Generado",
                            ClientId = clientId,
                            StudentId = studentId,
                            StudentName = student.FullName,
                            ClientName = client.FullName,
                            HtmlContent = html,
                            CreatedDate = DateTime.Now,
                            Name = $"Inscripcion - {student.FullName}"
                        };
                        var contractId = await _firebase.AddAsync(contract, "Contracts");
                        contract.Id = contractId;
                        await _firebase.UpdateAsync(contract, "Contracts", contractId);
                    }
                }

                // 3. Create AuthorizedPickups
                if (Request.AuthorizedPickups != null)
                {
                    foreach (var pickup in Request.AuthorizedPickups)
                    {
                        var auth = new AuthorizedPickupModel
                        {
                            ClientId = clientId,
                            FullName = pickup.FullName,
                            Relationship = pickup.Relationship,
                            Phone = pickup.Phone,
                            IdNumber = pickup.IdNumber,
                            IsActive = true
                        };
                        var authId = await _firebase.AddAsync(auth, "AuthorizedPickup");
                        auth.Id = authId;
                        await _firebase.UpdateAsync(auth, "AuthorizedPickup", authId);
                    }
                }

                // 4. Update request status
                Request.Status = RequestStatus.Aprobada;
                Request.ReviewDate = DateTime.Now.ToString("yyyy-MM-dd");
                Request.Notes = ReviewNotes;
                await _firebase.UpdateAsync(Request, "EnrollmentRequests", Request.Id!);

                await Shell.Current.DisplayAlert("✅ Aprobada",
                    $"Solicitud aprobada. Cliente, alumnos y contratos creados exitosamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo aprobar: {ex.Message}", "OK");
            }
        }

        private async void OnReject()
        {
            var reason = await Shell.Current.DisplayPromptAsync("Rechazar", "Motivo del rechazo:", "Rechazar", "Cancelar");
            if (string.IsNullOrEmpty(reason)) return;

            Request.Status = RequestStatus.Rechazada;
            Request.RejectionReason = reason;
            Request.ReviewDate = DateTime.Now.ToString("yyyy-MM-dd");
            await _firebase.UpdateAsync(Request, "EnrollmentRequests", Request.Id!);

            await Shell.Current.DisplayAlert("Rechazada", "Solicitud rechazada. El padre verá el motivo en su app.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
