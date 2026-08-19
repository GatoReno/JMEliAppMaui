using System;
using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.Models
{
    public class ContractTypeModel : BaseChildModel
    {
    }

    public class ContractModel : BaseChildModel, IHasId
    {
        public string? Url { get; set; }
        public string? Status { get; set; }
        public string? ClientId { get; set; }
        public string? StudentId { get; set; }
        public string? Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? SignedDate { get; set; }

        /// <summary>HTML completo del contrato generado. Se guarda en Firebase para consulta.</summary>
        public string? HtmlContent { get; set; }

        /// <summary>Nombre del alumno al momento de generar (denormalizado para display).</summary>
        public string? StudentName { get; set; }

        /// <summary>Nombre del cliente al momento de generar (denormalizado para display).</summary>
        public string? ClientName { get; set; }

        /// <summary>Nombre del ciclo al momento de generar.</summary>
        public string? CycleName { get; set; }
    }
}

