using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Storage
{
    /// <summary>
    /// async funcs for reading and writing project and task data
    /// in the storage layer
    /// </summary>
    public interface IStorageContext
    {

        /// <summary>
        /// gets all projects from storage
        /// </summary>
        /// <returns>read-only collection of project records</returns>
        Task<IReadOnlyList<ProjectRecord>> GetProjectsAsync();

        /// <summary>
        /// gets a project by its id
        /// </summary>
        /// <param name="projectId">project id</param>
        /// <returns>matching project record, or <see langword="null"/> if not found</returns>
        Task<ProjectRecord?> GetProjectAsync(int projectId);

        /// <summary>
        /// adds a new project to storage
        /// </summary>
        /// <param name="name">project name</param>
        /// <param name="description">project description</param>
        /// <param name="type">project type</param>
        /// <returns>created project record</returns>
        Task<ProjectRecord> AddProjectAsync(string name, string description, TypeOfProject type);

        /// <summary>
        /// updates an existing project in storage
        /// </summary>
        /// <param name="project">project record with updated vals</param>
        Task UpdateProjectAsync(ProjectRecord project);

        /// <summary>
        /// deletes a project from storage
        /// </summary>
        /// <param name="projectId">project id</param>
        Task DeleteProjectAsync(int projectId);

        /// <summary>
        /// gets all tasks from storage
        /// </summary>
        /// <returns>read-only collection of task records</returns>
        Task<IReadOnlyList<TaskRecord>> GetTasksAsync();

        /// <summary>
        /// gets all tasks that belong to the specified project
        /// </summary>
        /// <param name="projectId">owner project id</param>
        /// <returns>read-only collection of task records for the project</returns>
        Task<IReadOnlyList<TaskRecord>> GetTasksByProjectAsync(int projectId);

        /// <summary>
        /// gets a task by its id
        /// </summary>
        /// <param name="taskId">task id</param>
        /// <returns>matching task record, or <see langword="null"/> if not found</returns>
        Task<TaskRecord?> GetTaskAsync(int taskId);

        /// <summary>
        /// adds a new task to storage
        /// </summary>
        /// <param name="projectId">owner project id</param>
        /// <param name="name">task name</param>
        /// <param name="description">task description</param>
        /// <param name="priority">task priority</param>
        /// <param name="dueDate">task due date</param>
        /// <param name="isFinished">completion flag</param>
        /// <returns>created task record</returns>
        Task<TaskRecord> AddTaskAsync(
            int projectId,
            string name,
            string description,
            Priority priority,
            DateTime dueDate,
            bool isFinished);

        /// <summary>
        /// updates an existing task in storage
        /// </summary>
        /// <param name="task">task record with updated vals</param>
        Task UpdateTaskAsync(TaskRecord task);

        /// <summary>
        /// deletes a task from storage
        /// </summary>
        /// <param name="taskId">task id</param>
        Task DeleteTaskAsync(int taskId);
    }
}