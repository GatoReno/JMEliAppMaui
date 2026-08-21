using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.Models
{
    /// <summary>
    /// Historial académico: registra cada ciclo completado por el alumno.
    /// Se crea automáticamente cuando el ciclo cierra y el alumno egresa.
    /// Firebase: /AcademicHistory/{id}
    /// </summary>
    public class AcademicHistoryModel : IHasId
    {
        public string? Id { get; set; }
        public string? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? CycleId { get; set; }
        public string? CycleName { get; set; }
        public string? Level { get; set; }
        public string? Grade { get; set; }
        public string? FinalStatus { get; set; }  // Egresado, Baja, etc.
        public string? CompletedDate { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// Niveles de Primaria predefinidos (1-6, distribuidos en 3 fases SEP).
    /// </summary>
    public static class PrimaryLevels
    {
        public static readonly List<PrimaryGradeInfo> Grades = new()
        {
            new("1ero", "Fase 3"),
            new("2do", "Fase 3"),
            new("3ero", "Fase 4"),
            new("4to", "Fase 4"),
            new("5to", "Fase 5"),
            new("6to", "Fase 5"),
        };
    }

    public record PrimaryGradeInfo(string Grade, string Phase);
}
