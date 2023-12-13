using System;
namespace JMEliAppMaui.Models
{
	public class StudentLevelsModel
	{
		public string? Name { get; set;}
        public string? Id { get; set; }
        public List<StudentGradesModel>? Grades { get; set; }

    }

    public class StudentGradesModel
    {
        public string? Name { get; set; }
        public string? IdLevel { get; set; }

    }
}

