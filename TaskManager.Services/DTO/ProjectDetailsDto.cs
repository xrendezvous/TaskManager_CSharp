using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    /// <summary>
    /// detailed info about project
    /// </summary>
    public sealed class ProjectDetailsDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public TypeOfProject Type { get; init; }
        public int TasksAmount { get; init; }
        public int FinishedTasks { get; init; }
        public int Progress { get; init; }
    }
}