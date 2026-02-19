using TaskManager.Storage.Enums;

namespace TaskManager.Views
{
    public sealed class ProjectView
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public TypeOfProject Type { get; }
        public int TasksAmount { get; }
        public int FinishedTasks { get; }
        public int Progress => TasksAmount == 0 ? 0 : (int)Math.Round(FinishedTasks * 100.0 / TasksAmount);
        public ProjectView(
            int id,
            string name,
            string desc,
            TypeOfProject type,
            int tasksAmount,
            int finishedTasks) 
        {
            Id = id;
            Name = name;
            Description = desc;
            Type = type;
            TasksAmount = tasksAmount;
            FinishedTasks = finishedTasks;
        }
    }
}
