using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Storage.Enums;

namespace TaskManager.Storage.Entities
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
