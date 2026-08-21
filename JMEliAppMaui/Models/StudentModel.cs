using JMEliAppMaui.Services.Implementations;

namespace JMEliAppMaui.Models
{
    public class StudentModel : IHasId
    {
        public string? Id { get; set; }
        public string? ClientId { get; set; }

        #region Identity
        public string? FullName { get; set; }
        public string? NickName { get; set; }
        public string? Gender { get; set; }
        public string? Clave { get; set; }
        public string? UrlImage { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Religion { get; set; }
        public string? PreviousSchool { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? CURP { get; set; }
        #endregion

        #region Academic
        public string? Status { get; set; }
        public string? Grade { get; set; }
        public string? Level { get; set; }
        public string? Contract { get; set; }
        public string? Promotion { get; set; }
        public string? ActualCycle { get; set; }
        public string? State { get; set; }
        #endregion

        #region Medical
        public string? Allergies { get; set; }
        public string? BloodType { get; set; }
        public string? Size { get; set; }
        public string? Weight { get; set; }
        public string? MedicalHistory { get; set; }
        public string? Insurance { get; set; }
        public string? Observations { get; set; }
        public string? Phobias { get; set; }
        #endregion

        #region Behavioral
        public string? HomeLanguage { get; set; }
        public string? TrainedBath { get; set; }
        public bool IsLateDevelopment { get; set; } = false;
        public string? DevelopmentObservations { get; set; }
        public string? SpecialWords { get; set; }
        public string? MuscularControl { get; set; }
        #endregion

        #region Schedule
        public string? BathHour { get; set; }
        public string? SleepHour { get; set; }
        public string? AwakeHour { get; set; }
        public string? NapHour { get; set; }
        public string? BreakfastHour { get; set; }
        public string? MealHour { get; set; }
        public string? MealType { get; set; }
        #endregion

        #region Financial
        public string? Tuition { get; set; }
        public string? MonthlyPayment { get; set; }
        #endregion
    }
}

