using System;
namespace JMEliAppMaui.Models
{
	public class StudentModel
	{
        public string? Id { get; set; }

        public ClientModel? Client { get; set; }

        #region string props
        public string? UrlImage { get; set; }
        public string? Status { get; set; }
        public string? Grade { get; set; }
        public string? Level { get; set; }
        public string? Contract { get; set; }
        public string? Promotion { get; set; }
        public string? ActualCycle   { get; set; }

        public string? FullName { get; set; }
        public string? Alergies { get; set; }
        public string? BloodType { get; set; }
        public string? Size { get; set; }
        public string? Weight { get; set; }
        public string? Precedes { get; set; }

        public string? Clave { get; set; }

        //Colegiatura
        public string? Tuition { get; set; }

        public string? Insurance { get; set; }
      

        public string? Observations { get; set; }

        public string? Gender { get; set; }


        public string? State { get; set; }


        #endregion
    }
}

