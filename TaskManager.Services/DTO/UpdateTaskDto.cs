using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    /// <summary>
    /// required data to update an existing task
    /// </summary>
    public sealed class UpdateTaskDto
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Priority Priority { get; init; }
        public DateTime DueDate { get; init; }
        public bool IsFinished { get; init; }
    }
}