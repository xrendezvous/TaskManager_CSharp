using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    /// <summary>
    /// required data to update an existing project
    /// </summary>
    public sealed class UpdateProjectDto
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public TypeOfProject Type { get; init; }
    }
}