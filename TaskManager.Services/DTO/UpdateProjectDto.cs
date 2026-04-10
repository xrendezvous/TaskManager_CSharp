using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    public sealed class UpdateProjectDto
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public TypeOfProject Type { get; init; }
    }
}