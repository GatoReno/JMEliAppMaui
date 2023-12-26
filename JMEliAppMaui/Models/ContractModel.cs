using System;
namespace JMEliAppMaui.Models
{
	public class ContractTypeModel : BaseChildModel
	{

    }

    public class ContractModel : BaseChildModel
    {
        public string? Url { get; set; }
        public string? Status { get; set; }
        public string? ClientId { get; set; }
        public string? StudentId { get; set; }
        public string? Type { get; set; }
    }
}

