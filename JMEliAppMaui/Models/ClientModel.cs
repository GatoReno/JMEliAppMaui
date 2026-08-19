using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.Models
{
    public class ClientModel : IHasId
    {
        public List<StudentModel>? Students { get; set; }

        public ClientModel()
        {
            Students = new List<StudentModel>();
        }

        #region string props
        public string? Id { get; set; }
        public string? Status { get; set; }
        public string? FullName { get; set; }
        public string? Scholarship { get; set; }
        public string? Occupation { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Office { get; set; }
        public string? Relationship { get; set; }
        public string? Work { get; set; }
        public string? State { get; set; }
        public string? Address { get; set; }
        public string? Contract { get; set; }
        public string? UrlImage { get; set; }
        #endregion
    }
}

