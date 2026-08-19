namespace JMEliAppMaui.Models
{
    /// <summary>
    /// Los 6 estatus predefinidos del ciclo de vida de un alumno.
    /// </summary>
    public static class StudentStatus
    {
        public const string Inscrito = "Inscrito";
        public const string Activo = "Activo";
        public const string Baja = "Baja";
        public const string Suspendido = "Suspendido";
        public const string Egresado = "Egresado";
        public const string Adeudo = "Adeudo";

        public static readonly List<StudentStatusInfo> All = new()
        {
            new(Inscrito, "Alumno registrado para el ciclo, pendiente de inicio", "#1565c0"),
            new(Activo, "Alumno cursando normalmente", "#2e7d32"),
            new(Baja, "Alumno dado de baja del ciclo", "#c62828"),
            new(Suspendido, "Alumno suspendido temporalmente", "#e65100"),
            new(Egresado, "Alumno que completó el ciclo exitosamente", "#6a1b9a"),
            new(Adeudo, "Alumno con pagos pendientes", "#f9a825"),
        };

        public static string GetColor(string status) =>
            All.FirstOrDefault(s => s.Name == status)?.Color ?? "#757575";

        public static string GetDescription(string status) =>
            All.FirstOrDefault(s => s.Name == status)?.Description ?? "";
    }

    public record StudentStatusInfo(string Name, string Description, string Color);
}
