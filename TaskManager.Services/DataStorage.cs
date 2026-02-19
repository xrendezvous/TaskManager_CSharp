using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Storage.Entities;
using TaskManager.Storage.Enums;

namespace TaskManager.Services
{
    internal class DataStorage
    {
        internal static List<ProjectRecord> Projects = new()
        {
            new ProjectRecord(1, "Diploma", "Models, statistics, analysis", TypeOfProject.Study),
            new ProjectRecord(2, "Book club", "Prepare for monthly meeting", TypeOfProject.Personal),
            new ProjectRecord(3, "MindCare", "Add frontend to the project", TypeOfProject.Work),
        };

        internal static List<TaskRecord> Tasks = new()
        {
            new TaskRecord(106, 1, "Code refactoring", "Move utils", Priority.Low, DateTime.Today.AddDays(14), true),
            new TaskRecord(107, 1, "Desc of dataset", "Fields, size etc.", Priority.Medium, DateTime.Today.AddDays(4), true),
            new TaskRecord(108, 1, "Metrics", "Accuracy/F1/AUC", Priority.High, DateTime.Today.AddDays(6), false),
            new TaskRecord(109, 1, "Error analysis", "FP/FN examples", Priority.Medium, DateTime.Today.AddDays(9), false),
            new TaskRecord(110, 1, "Presentation draft", "10-12 slides", Priority.High, DateTime.Today.AddDays(12), false),

            new TaskRecord(201, 2, "Read the assignmented book", "Until the page 269", Priority.High, DateTime.Today.AddDays(3), false),
            new TaskRecord(202, 2, "Make a presentation", "10-12 slides", Priority.Critical, DateTime.Today.AddDays(8), false),
        };
    }
}
