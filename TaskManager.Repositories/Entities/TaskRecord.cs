using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Entities
{
    /// <summary>
    /// Represents a task entity stored in the storage layer.
    /// </summary>
    public sealed class TaskRecord
    {
        public int Id { get; }
        public int ProjectId { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Priority Priority { get; private set; }
        public DateTime DueDate { get; private set; }
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRecord"/> class.
        /// </summary>
        /// <param name="id">The task identifier.</param>
        /// <param name="projectId">The identifier of the project that owns the task.</param>
        /// <param name="title">The task title.</param>
        /// <param name="desc">The task description.</param>
        /// <param name="priority">The task priority.</param>
        /// <param name="dueDate">The task due date.</param>
        /// <param name="finished">Indicates whether the task is finished.</param>
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

        /// <summary>
        /// Updates the mutable task fields.
        /// </summary>
        /// <param name="title">The new task title.</param>
        /// <param name="desc">The new task description.</param>
        /// <param name="priority">The new task priority.</param>
        /// <param name="dueDate">The new due date.</param>
        /// <param name="finished">The new completion state.</param>
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
