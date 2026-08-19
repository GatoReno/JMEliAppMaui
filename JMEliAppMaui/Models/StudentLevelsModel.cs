using System;

namespace JMEliAppMaui.Models
{
    public class StudentLevelsModel : BaseChildModel
    {
        public List<StudentGradesModel>? Grades { get; set; }
    }

    public class StudentGradesModel
    {
        public string? Name { get; set; }
        public string? LevelId { get; set; }
    }
}

