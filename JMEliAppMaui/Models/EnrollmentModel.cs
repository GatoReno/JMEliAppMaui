using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.Models
{
    /// <summary>
    /// Inscripción: vincula un alumno a un ciclo escolar con su estado actual.
    /// Un alumno puede tener múltiples inscripciones (una por ciclo).
    /// </summary>
    public class EnrollmentModel : IHasId
    {
        public string? Id { get; set; }
        public string? StudentId { get; set; }
        public string? CycleId { get; set; }
        public string? ClientId { get; set; }

        /// <summary>
        /// Estado actual: Inscrito, Activo, Baja, Suspendido, Egresado, Adeudo
        /// </summary>
        public string? Status { get; set; }

        /// <summary>Costo de inscripción para este ciclo. Puede ser 0.</summary>
        public string? InscriptionFee { get; set; }

        public string? EnrollmentDate { get; set; }
        public string? StatusChangeDate { get; set; }
        public string? Notes { get; set; }

        // Denormalized for display (filled from join)
        public string? StudentName { get; set; }
        public string? ClientName { get; set; }
        public string? CycleName { get; set; }
        public string? Level { get; set; }
        public string? Grade { get; set; }
        public string? Tuition { get; set; }
    }
}
