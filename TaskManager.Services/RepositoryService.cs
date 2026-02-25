/*
 * Сервіс для роботи зі сховищем, інкапсулює доступ до DataStorage
 */ 
using TaskManager.Views;

namespace TaskManager.Services
{
    public sealed class RepositoryService : IRepositoryService
    {
        /*
         * Отримує сирі дані зі сховища, для кожного проєкту знаходить відповідні завдання. Потім виконує
         * обчислення кількості завершених завдань та формує ProjectView для відображення в інтерфейсі. Після чого 
         * сортує проєкти за id
         */
        public List<ProjectView> GetProjects()
        {
            var projects = DataStorage.Projects;
            var tasks = DataStorage.Tasks;

            return projects
            .Select(p =>
            {
                var projectTasks = tasks.Where(t => t.ProjectId == p.Id).ToList();
                var done = projectTasks.Count(t => t.IsFinished);
                return new ProjectView(p.Id, p.Name, p.Description, p.Type, projectTasks.Count, done);
            })
            .OrderBy(p => p.Id)
            .ToList();
        }
        /*
         * Фільтрує завдання за ProjectId, потім сортує за пріоритетом та за датою виконання,
         * в кінці перетворює TaskRecord на TaskView
         */
        public List<TaskView> GetTasksByProjects(int projectId)
        {
            return DataStorage.Tasks
            .Where(t => t.ProjectId == projectId)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .Select(t => new TaskView(t.Id, t.ProjectId, t.Title, t.Description, t.Priority, t.DueDate, t.IsFinished))
            .ToList();
        }
        /*
         * Повертає одне завдання за taskId, якщо завдання не знайдено, кидає exception
         */
        public TaskView GetTask(int taskId)
        {
            var t = DataStorage.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (t is null)
                throw new KeyNotFoundException($"Task with ID {taskId} was not found.");

            return new TaskView(
                t.Id,
                t.ProjectId,
                t.Title,
                t.Description,
                t.Priority,
                t.DueDate,
                t.IsFinished
            );
        }
    }
}
