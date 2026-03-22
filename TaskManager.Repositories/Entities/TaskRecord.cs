using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Entities
{
    /// <summary>
    /// task entity stored in the storage layer
    /// </summary>
    public sealed class TaskRecord
    {
        public int Id { get; }
        public int ProjectId { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Priority Priority { get; private set; }
        public DateTime DueDate { get; private set; }
        public bool IsFinished { get; private set; }

        /// <summary>
        /// initializes a new instance of the <see cref="TaskRecord"/> class
        /// </summary>
        /// <param name="id"/>
        /// <param name="projectId">id of the project that owns the task</param>
        /// <param name="title"/>
        /// <param name="desc"/>
        /// <param name="priority"/>
        /// <param name="dueDate"/>
        /// <param name="finished">indicates whether the task is finished</param>
        public TaskRecord(
            int id,
            int projectId,
            string name,
            string desc,
            Priority priority,
            DateTime dueDate,
            bool finished)
        {
            Id = id;
            ProjectId = projectId;
            Name = name;
            Description = desc;
            Priority = priority;
            DueDate = dueDate;
            IsFinished = finished;
        }

        /// <summary>
        /// updates the mutable task fields
        /// </summary>
        /// <param name="title"/>
        /// <param name="desc"/>
        /// <param name="priority"/>
        /// <param name="dueDate"/>
        /// <param name="finished"/>
        public void UpdateRecord(
            string name,
            string desc,
            Priority priority,
            DateTime dueDate,
            bool finished)
        {
            Name = name;
            Description = desc;
            Priority = priority;
            DueDate = dueDate;
            IsFinished = finished;
        }
    }
}
