using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    /// <summary>
    /// required data to create a new project
    /// </summary>
    public sealed class CreateProjectDto
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public TypeOfProject Type { get; init; }
    }
}