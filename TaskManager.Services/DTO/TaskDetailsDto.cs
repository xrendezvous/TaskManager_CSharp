using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    public sealed class TaskDetailsDto
    {
        public int Id { get; init; }
        public int ProjectId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Priority Priority { get; init; }
        public DateTime DueDate { get; init; }
        public bool IsFinished { get; init; }
        public bool IsOverdue { get; init; }
    }
}