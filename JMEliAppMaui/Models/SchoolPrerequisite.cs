using JMEliAppMaui.Views;

namespace JMEliAppMaui.Models
{
    /// <summary>
    /// Prerequisitos del sistema escolar. Si alguno falta, la app guía al usuario a crearlo.
    /// </summary>
    public enum SchoolPrerequisite
    {
        Cycles,
        Levels,
        Clients
    }

    public static class PrerequisiteInfo
    {
        public static readonly Dictionary<SchoolPrerequisite, PrerequisiteDetail> All = new()
        {
            [SchoolPrerequisite.Cycles] = new(
                "Ciclos Escolares",
                "Necesitas al menos un ciclo escolar para inscribir alumnos.",
                "📅",
                "Crear Ciclo",
                nameof(CyclesPage)
            ),
            [SchoolPrerequisite.Levels] = new(
                "Niveles y Grados",
                "Necesitas al menos un nivel con grados para asignar alumnos.",
                "📚",
                "Crear Nivel",
                nameof(LevelsPage)
            ),
            [SchoolPrerequisite.Clients] = new(
                "Clientes (Tutores)",
                "Necesitas al menos un cliente registrado para vincular al alumno.",
                "👥",
                "Agregar Cliente",
                nameof(ClientsPage)
            ),
        };
    }

    public record PrerequisiteDetail(
        string Title,
        string Description,
        string Emoji,
        string ActionLabel,
        string RouteName
    );
}
