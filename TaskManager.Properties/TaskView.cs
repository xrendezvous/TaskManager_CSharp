using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Storage.Enums;

namespace TaskManager.Views
{
    public class TaskView
    {
        public int Id { get; }
        public int ProjectId { get; }
        public string Title { get; }
        public string Description { get; }
        public Priority Priority { get; }
        public DateTime DueDate { get; }
        public bool IsFinished { get; }
        public bool IsOverdue => !IsFinished && DateTime.Today > DueDate.Date;
        public TaskView(
            int id,
            int projectId,
            string title,
            string desc,
            Priority priority,
            DateTime dueDate,
            bool isFinished)
        {
            Id = id;
            ProjectId = projectId;
            Title = title;
            Description = desc;
            Priority = priority;
            DueDate = dueDate;
            IsFinished = isFinished;
        }
    }
}
