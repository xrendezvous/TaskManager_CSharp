using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Storage
{
    public sealed class StorageDataModel
    {
        public List<ProjectRecord> Projects { get; set; } = new();
        public List<TaskRecord> Tasks { get; set; } = new();
    }
}