using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    /// <summary>
    /// search, filter and sort params for task list
    /// </summary>
    public sealed class FilterTaskDto
    {
        public string SearchText { get; init; } = string.Empty;
        public Priority? Priority { get; init; }
        public bool? IsFinished { get; init; }
        public bool? OnlyOverdue { get; init; }
        public SortTask SortBy { get; init; } = SortTask.ByPriority;
    }
}
