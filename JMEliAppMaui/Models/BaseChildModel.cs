using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.Models
{
    public class BaseChildModel : IHasId
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}

