using JMEliAppMaui.Models;

namespace JMEliAppMaui.Services.Abstractions
{
    /// <summary>
    /// Genera contratos PDF a partir de plantillas HTML con datos del alumno/cliente/ciclo.
    /// </summary>
    public interface IContractGeneratorService
    {
        /// <summary>
        /// Genera el HTML del contrato llenando la plantilla con los datos proporcionados.
        /// </summary>
        Task<string> GenerateContractHtmlAsync(StudentModel student, ClientModel client, CycleModel cycle, string contractType);

        /// <summary>
        /// Genera el contrato y lo guarda como archivo HTML/PDF local.
        /// Retorna la ruta del archivo generado.
        /// </summary>
        Task<string> GenerateAndSaveContractAsync(StudentModel student, ClientModel client, CycleModel cycle, string contractType);

        /// <summary>
        /// Lista los tipos de plantilla disponibles.
        /// </summary>
        List<string> GetAvailableTemplates();
    }
}
