using System.Text.Json.Serialization;
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
        
        [JsonConstructor]
        public TaskRecord(
            int id,
            int projectId,
            string name,
            string description,
            Priority priority,
            DateTime dueDate,
            bool isFinished)
        {
            Id = id;
            ProjectId = projectId;
            Name = name;
            Description = description;
            Priority = priority;
            DueDate = dueDate;
            IsFinished = isFinished;
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
            string description,
            Priority priority,
            DateTime dueDate,
            bool isFinished)
        {
            Name = name;
            Description = description;
            Priority = priority;
            DueDate = dueDate;
            IsFinished = isFinished;
        }
    }
}
