using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.Models
{
    public class EnrollmentRequest : IHasId
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? ClientId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Occupation { get; set; }
        public string? Relationship { get; set; }
        public List<StudentRequestItem>? Students { get; set; }
        public List<AuthorizedPickupRequestItem>? AuthorizedPickups { get; set; }
        public string? Status { get; set; }
        public string? RequestDate { get; set; }
        public string? ReviewDate { get; set; }
        public string? ReviewedBy { get; set; }
        public string? RejectionReason { get; set; }
        public string? Notes { get; set; }
    }

    public class StudentRequestItem
    {
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Allergies { get; set; }
        public string? BloodType { get; set; }
        public string? MedicalHistory { get; set; }
        public string? DesiredLevel { get; set; }
        public string? DesiredGrade { get; set; }
        public string? Observations { get; set; }
    }

    public class AuthorizedPickupRequestItem
    {
        public string? FullName { get; set; }
        public string? Relationship { get; set; }
        public string? Phone { get; set; }
        public string? IdNumber { get; set; }
    }

    public static class RequestStatus
    {
        public const string Pendiente = "Pendiente";
        public const string EnRevision = "EnRevision";
        public const string Aprobada = "Aprobada";
        public const string Rechazada = "Rechazada";
    }
}
