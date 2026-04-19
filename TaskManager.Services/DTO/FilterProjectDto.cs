using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    /// <summary>
    /// search, filter and sort params for project list
    /// </summary>
    public sealed class FilterProjectDto
    {
        public string SearchText { get; init; } = string.Empty;
        public TypeOfProject? Type { get; init; }
        public ProjectSortOption SortBy { get; init; } = ProjectSortOption.ById;
    }
}
