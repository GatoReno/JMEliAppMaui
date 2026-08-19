using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.ViewModels
{
    /// <summary>
    /// Personas autorizadas para recoger alumnos.
    /// Vinculadas a un ClientId (tutor que autoriza) y pueden recoger a uno o más alumnos.
    /// Se puede navegar aquí desde ClientDetails o desde el menú principal (muestra todos).
    /// </summary>
    [QueryProperty(nameof(ClientId), "ClientId")]
    [QueryProperty(nameof(ClientName), "ClientName")]
    public class AuthorizedPickupViewModel : BindableObject
    {
        #region Properties

        private string? _clientId;
        private string? _clientName;
        private bool _isFormVisible;
        private bool _isFiltered; // true = showing for specific client, false = showing all
        private string? _newFullName, _newRelationship, _newPhone, _newIdNumber;

        public string? ClientId
        {
            get => _clientId;
            set { _clientId = value; OnPropertyChanged(); _isFiltered = !string.IsNullOrEmpty(value); LoadPeople(); }
        }
        public string? ClientName { get => _clientName; set { _clientName = value; OnPropertyChanged(); OnPropertyChanged(nameof(HeaderText)); } }
        public string HeaderText => string.IsNullOrEmpty(ClientName)
            ? "Todas las personas autorizadas"
            : $"Autorizados por: {ClientName}";
        public bool IsFormVisible { get => _isFormVisible; set { _isFormVisible = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsFabVisible)); } }
        public bool IsFabVisible => !IsFormVisible;

        public string? NewFullName { get => _newFullName; set { _newFullName = value; OnPropertyChanged(); } }
        public string? NewRelationship { get => _newRelationship; set { _newRelationship = value; OnPropertyChanged(); } }
        public string? NewPhone { get => _newPhone; set { _newPhone = value; OnPropertyChanged(); } }
        public string? NewIdNumber { get => _newIdNumber; set { _newIdNumber = value; OnPropertyChanged(); } }

        public ObservableCollection<AuthorizedPickupDisplayItem> AuthorizedPeople { get; set; } = new();

        #endregion

        #region Commands

        public ICommand ShowFormCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AppearingCommand { get; }

        #endregion

        private readonly IFirebaseService _firebase;

        public AuthorizedPickupViewModel(IFirebaseService firebase)
        {
            _firebase = firebase;

            ShowFormCommand = new Command(() => IsFormVisible = true);
            CancelCommand = new Command(OnCancel);
            SaveCommand = new Command(OnSave);
            DeleteCommand = new Command<AuthorizedPickupDisplayItem>(OnDelete);
            AppearingCommand = new Command(() => LoadPeople());

            // If no ClientId is set (navigated from menu), load all
            LoadPeople();
        }

        private async void LoadPeople()
        {
            AuthorizedPeople.Clear();

            try
            {
                ObservableCollection<AuthorizedPickupModel> people;

                if (_isFiltered && !string.IsNullOrEmpty(ClientId))
                {
                    // Show only authorized people for this client
                    people = await _firebase.GetWhereAsync<AuthorizedPickupModel>(
                        "AuthorizedPickup", p => p.ClientId == ClientId && p.IsActive);
                }
                else
                {
                    // Show all active authorized people
                    people = await _firebase.GetWhereAsync<AuthorizedPickupModel>(
                        "AuthorizedPickup", p => p.IsActive);
                }

                foreach (var person in people)
                {
                    var studentNames = person.StudentIds != null && person.StudentIds.Count > 0
                        ? $"{person.StudentIds.Count} alumno(s)"
                        : "Sin alumnos asignados";

                    AuthorizedPeople.Add(new AuthorizedPickupDisplayItem
                    {
                        Id = person.Id,
                        FullName = person.FullName ?? "",
                        Relationship = person.Relationship ?? "",
                        Phone = person.Phone ?? "",
                        IdNumber = person.IdNumber ?? "",
                        PhotoUrl = person.PhotoUrl,
                        StudentSummary = studentNames
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AuthorizedPickup load error: {ex.Message}");
            }
        }

        private void OnCancel()
        {
            IsFormVisible = false;
            ClearForm();
        }

        private async void OnSave()
        {
            if (string.IsNullOrWhiteSpace(NewFullName) || string.IsNullOrWhiteSpace(NewPhone))
            {
                await Shell.Current.DisplayAlert("Error", "Nombre y teléfono son obligatorios", "OK");
                return;
            }

            var newPerson = new AuthorizedPickupModel
            {
                ClientId = ClientId,
                StudentIds = new List<string>(), // Will be assigned later from client's students
                FullName = NewFullName,
                Relationship = NewRelationship,
                Phone = NewPhone,
                IdNumber = NewIdNumber,
                IsActive = true
            };

            try
            {
                var id = await _firebase.AddAsync(newPerson, "AuthorizedPickup");
                newPerson.Id = id;
                await _firebase.UpdateAsync(newPerson, "AuthorizedPickup", id);

                AuthorizedPeople.Add(new AuthorizedPickupDisplayItem
                {
                    Id = id,
                    FullName = newPerson.FullName!,
                    Relationship = newPerson.Relationship ?? "",
                    Phone = newPerson.Phone!,
                    IdNumber = newPerson.IdNumber ?? "",
                    StudentSummary = "Sin alumnos asignados"
                });

                IsFormVisible = false;
                ClearForm();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo guardar: {ex.Message}", "OK");
            }
        }

        private async void OnDelete(AuthorizedPickupDisplayItem item)
        {
            if (item?.Id == null) return;

            var confirm = await Shell.Current.DisplayAlert(
                "Confirmar", $"¿Eliminar a {item.FullName} de la lista de autorizados?", "Eliminar", "Cancelar");

            if (confirm)
            {
                await _firebase.DeleteAsync("AuthorizedPickup", item.Id);
                AuthorizedPeople.Remove(item);
            }
        }

        private void ClearForm()
        {
            NewFullName = null;
            NewRelationship = null;
            NewPhone = null;
            NewIdNumber = null;
        }
    }

    public class AuthorizedPickupDisplayItem
    {
        public string? Id { get; set; }
        public string FullName { get; set; } = "";
        public string Relationship { get; set; } = "";
        public string Phone { get; set; } = "";
        public string IdNumber { get; set; } = "";
        public string? PhotoUrl { get; set; }
        public string StudentSummary { get; set; } = "";
    }
}
