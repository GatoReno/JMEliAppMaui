using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.Models
{
    /// <summary>
    /// Anuncio publicado por la administración.
    /// Se muestra en el feed de CJMApp para los padres.
    /// Firebase: /Announcements/{id}
    /// </summary>
    public class AnnouncementModel : IHasId
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }  // Max 250 chars

        // Imágenes (URLs de Firebase Storage, max 3)
        public List<string>? ImageUrls { get; set; }

        // Fecha/hora (opcionales)
        public string? EventDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }

        // Ubicación (opcional)
        public string? Location { get; set; }

        // Links sociales (opcionales)
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TiktokUrl { get; set; }

        // Meta
        public string? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
