using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    /// <summary>
    /// displayed task data in the list
    /// </summary>
    public sealed class TaskListDto
    {
        public int Id { get; init; }
        public int ProjectId { get; init; }
        public string Name { get; init; } = string.Empty;
        public Priority Priority { get; init; }
        public bool IsFinished { get; init; }
        public bool IsOverdue { get; init; }
    }
}