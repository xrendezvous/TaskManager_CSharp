using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    public sealed class FilterTaskDto
    {
        public string SearchText { get; init; } = string.Empty;
        public Priority? Priority { get; init; }
        public bool? IsFinished { get; init; }
        public bool? OnlyOverdue { get; init; }
        public SortTask SortBy { get; init; } = SortTask.ByPriority;
    }
}
