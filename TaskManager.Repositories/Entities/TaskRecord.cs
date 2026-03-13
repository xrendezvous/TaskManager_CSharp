/*
 * Клас для зберігання даних про завдання, поки відсутня логіка з дедлайнами
 */
using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Entities
{
    public sealed class TaskRecord
    {
        public int Id { get; }
        public int ProjectId { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Priority Priority { get; private set; }
        public DateTime DueDate { get; private set; }
        public bool IsFinished { get; private set; }

        public TaskRecord(
            int id,
            int projectId,
            string title,
            string desc,
            Priority priority,
            DateTime dueDate,
            bool finished)
        {
            Id = id;
            ProjectId = projectId;
            Title = title;
            Description = desc;
            Priority = priority;
            DueDate = dueDate;
            IsFinished = finished;
        }

        public void UpdateRecord(
            string title,
            string desc,
            Priority priority,
            DateTime dueDate,
            bool finished)
        {
            Title = title;
            Description = desc;
            Priority = priority;
            DueDate = dueDate;
            IsFinished = finished;
        }
    }
}
