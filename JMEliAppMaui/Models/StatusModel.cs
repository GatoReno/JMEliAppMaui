using System;

namespace JMEliAppMaui.Models
{
    public class StatusModel : BaseChildModel
    {
        public string? Description { get; set; }
        public string? Color { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

