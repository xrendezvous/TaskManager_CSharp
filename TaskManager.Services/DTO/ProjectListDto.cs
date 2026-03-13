using TaskManager.Repositories.Enums;

namespace TaskManager.Services.DTO
{
    public sealed class ProjectListDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public TypeOfProject Type { get; init; }
        public int Progress { get; init; }
    }
}