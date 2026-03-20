using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Entities
{
    /// <summary>
    /// Represents a project entity stored in the storage layer.
    /// </summary>
    public sealed class ProjectRecord
    {
        public int Id { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public TypeOfProject Type { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectRecord"/> class.
        /// </summary>
        /// <param name="id">The project identifier.</param>
        /// <param name="name">The project name.</param>
        /// <param name="desc">The project description.</param>
        /// <param name="type">The project type.</param>
        public ProjectRecord(int id, string name, string desc, TypeOfProject type)
        {
            Id = id;
            Name = name;
            Description = desc;
            Type = type;
        }

        /// <summary>
        /// Updates the mutable project fields.
        /// </summary>
        /// <param name="name">The new project name.</param>
        /// <param name="desc">The new project description.</param>
        /// <param name="type">The new project type.</param>
        public void UpdateRecord(string name, string desc, TypeOfProject type)
        {
            Name = name;
            Description = desc;
            Type = type;
        }
    }
}
