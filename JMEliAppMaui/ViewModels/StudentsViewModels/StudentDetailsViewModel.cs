using System;
using JMEliAppMaui.Models;

namespace JMEliAppMaui.ViewModels.StudentsViewModels
{
    [QueryProperty(nameof(Student), "Student")]
    public class StudentDetailsViewModel : StudentBaseViewModel
	{
        private StudentModel _student;

        public StudentModel Student
        { get => _student; set { _student = value; OnPropertyChanged(); } }


        public StudentDetailsViewModel()
		{

            AppearingCommand = new Command(OnOnAppearingCommand);
           // AppearingCommand.Execute(null);

        }

        private void OnOnAppearingCommand()
        {
            Fullname = Student.FullName;
            Alergies = Student.Alergies;
            BloodType = Student.BloodType;
            Clave = Student.Clave;
            LevelSelected = Student.Level;
            Grade = Student.Grade;
            Gender = Student.Gender;
            Observations = Student.Observations;
            Tuition = Student.Tuition;
            State = Student.State;
            Precedes = Student.Precedes;
            Status = Student.Status;
            Weight = Student.Weight;
            Size = Student.Size;
            Insurance = Student.Insurance;
            ActualCycle = Student.ActualCycle;
            ImageUrl = Student.UrlImage;
            ClientId = Student.ClientId;
            Id = Student.Id;
        }
    }
}

