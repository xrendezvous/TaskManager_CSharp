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
            new ProjectRecord(1, "", "", TypeOfProject.Study),
            new ProjectRecord(2, "", "", TypeOfProject.Personal),
            new ProjectRecord(3, "", "", TypeOfProject.Work),
            new ProjectRecord(4, "", "", TypeOfProject.Work),
        };

        internal static List<TaskRecord> Tasks = new()
        {
            new TaskRecord(101, 1, "", "", Priority.High, DateTime.Today.AddDays(2), false),
            new TaskRecord(102, 1, "", "", Priority.Medium, DateTime.Today.AddDays(5), false),
            new TaskRecord(103, 1, "", "", Priority.Critical, DateTime.Today.AddDays(-1), false),
            new TaskRecord(104, 1, "", "", Priority.Medium, DateTime.Today.AddDays(7), false),

            new TaskRecord(201, 2, "", "", Priority.High, DateTime.Today.AddDays(3), false),
            new TaskRecord(202, 2, "", "", Priority.Critical, DateTime.Today.AddDays(8), false),
        };
    }
}
