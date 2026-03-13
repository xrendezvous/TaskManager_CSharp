/*
 * Клас для зберігання даних про проєкт, не містить обчислюваних полів та колекції завдань.
 * Використовується як Storage layer.
 */
using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Entities
{
    public sealed class ProjectRecord
    {
        public int Id { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public TypeOfProject Type { get; private set; }

        public ProjectRecord(int id, string name, string desc, TypeOfProject type)
        {
            Id = id;
            Name = name;
            Description = desc;
            Type = type;
        }

        public void UpdateRecord(string name, string desc, TypeOfProject type)
        {
            Name = name;
            Description = desc;
            Type = type;
        }
    }
}
