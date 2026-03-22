using TaskManager.Repositories.Entities;

namespace TaskManager.Repositories.Storage
{
    public interface IStorageContext
    {
        /// <summary>
        /// gets all projects from storage
        /// </summary>
        /// <returns>collection of all stored projects</returns>
        IEnumerable<ProjectRecord> GetProjects();

        /// <summary>
        /// gets a project by its id
        /// </summary>
        /// <param name="projectId"/>
        /// <returns>the matching project record, or null if it was not found</returns>
        ProjectRecord? GetProject(int projectId);

        /// <summary>
        /// gets all tasks from storage
        /// </summary>
        /// <returns>collection of all stored tasks</returns>
        IEnumerable<TaskRecord> GetTasks();

        /// <summary>
        /// gets tasks that belong to the specified project.
        /// </summary>
        /// <param name="projectId"/>
        /// <returns>collection of task records for the specified project</returns>
        IEnumerable<TaskRecord> GetTasksByProject(int projectId);

        /// <summary>
        /// gets a task by its id
        /// </summary>
        /// <param name="taskId"/>
        /// <returns>the matching task record, or null if it was not found</returns>
        TaskRecord? GetTask(int taskId);
    }
}