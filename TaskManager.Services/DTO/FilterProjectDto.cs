using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    public sealed class FilterProjectDto
    {
        public string SearchText { get; init; } = string.Empty;
        public TypeOfProject? Type { get; init; }
        public ProjectSortOption SortBy { get; init; } = ProjectSortOption.ById;
    }
}
