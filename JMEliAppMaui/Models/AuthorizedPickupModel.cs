using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.Models
{
    /// <summary>
    /// Persona autorizada para recoger alumnos.
    /// Vinculada al ClientId (tutor que autoriza) y puede recoger a uno o más alumnos (StudentIds).
    /// Ejemplo: La abuela está autorizada por Mamá (ClientId) para recoger a Juan y María (StudentIds).
    /// </summary>
    public class AuthorizedPickupModel : IHasId
    {
        public string? Id { get; set; }

        /// <summary>El cliente (tutor) que da la autorización.</summary>
        public string? ClientId { get; set; }

        /// <summary>Lista de IDs de alumnos que esta persona puede recoger.</summary>
        public List<string>? StudentIds { get; set; }

        public string? FullName { get; set; }
        public string? Relationship { get; set; }
        public string? Phone { get; set; }
        public string? IdNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
    }
}
