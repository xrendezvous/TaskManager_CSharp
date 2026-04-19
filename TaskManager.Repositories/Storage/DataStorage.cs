using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Storage
{
    /// <summary>
    /// root data model stored in the JSON file,
    /// includes collections of projects and tasks
    /// </summary>
    public sealed class StorageDataModel
    {
        /// <summary>
        /// getters/setters of the collection of stored projects
        /// </summary>
        public List<ProjectRecord> Projects { get; set; } = new();

        /// <summary>
        /// getters/setters of the collection of stored tasks
        /// </summary>
        public List<TaskRecord> Tasks { get; set; } = new();
    }
}