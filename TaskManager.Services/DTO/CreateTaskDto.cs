using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    /// <summary>
    /// required data to create a new task
    /// </summary>
    public sealed class CreateTaskDto
    {
        public int ProjectId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Priority Priority { get; init; }
        public DateTime DueDate { get; init; }
        public bool IsFinished { get; init; }
    }
}