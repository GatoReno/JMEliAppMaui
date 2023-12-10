using System;
namespace JMEliAppMaui.Models
{
	public class ClientModel
	{
        public List<StudentModel>? Students { get; set; }

        public ClientModel()
        {
            Students = new List<StudentModel>();

        }

        #region string props
        public string? Status { get; set; }
        public string? FullName { get; set; }
        public string? Scholarity { get; set; }
        public string? Ocupation { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Office { get; set; }
        public string? Relationship { get; set; }
        public string? Work { get; set; }
        public string? State { get; set; }
        public string? Address { get; set; }
        public string? Contract { get; set; }
        public string? Id { get; set; }

        #endregion region

    }
}

