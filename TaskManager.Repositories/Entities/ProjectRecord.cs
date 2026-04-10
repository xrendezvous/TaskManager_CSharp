using System.Text.Json.Serialization;
using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Entities
{
    /// <summary>
    /// project entity stored in the storage layer
    /// </summary>
    public sealed class ProjectRecord
    {
        public int Id { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public TypeOfProject Type { get; private set; }

        /// <summary>
        /// initializes a new instance of the <see cref="ProjectRecord"/> class
        /// </summary>
        /// <param name="id"/>
        /// <param name="name"/>
        /// <param name="desc">project description</param>
        /// <param name="type"/>
        
        [JsonConstructor]
        public ProjectRecord(int id, string name, string description, TypeOfProject type)
        {
            Id = id;
            Name = name;
            Description = description;
            Type = type;
        }

        /// <summary>
        /// updates the project fields
        /// </summary>
        /// <param name="name"/>
        /// <param name="desc"/>
        /// <param name="type"/>
        public void UpdateRecord(string name, string description, TypeOfProject type)
        {
            Name = name;
            Description = description;
            Type = type;
        }
    }
}
