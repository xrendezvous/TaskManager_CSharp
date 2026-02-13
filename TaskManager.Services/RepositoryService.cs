using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Storage.Entities;
using TaskManager.Views;
using TaskManager.Services;

namespace TaskManager.Services
{
    public sealed class RepositoryService
    {
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

        public List<TaskView> GetTasksByProjects(int projectId)
        {
            return DataStorage.Tasks
            .Where(t => t.ProjectId == projectId)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .Select(t => new TaskView(t.Id, t.ProjectId, t.Title, t.Description, t.Priority, t.DueDate, t.IsFinished))
            .ToList();
        }

        public TaskView? GetTask(int taskId)
        {
            var t = DataStorage.Tasks.FirstOrDefault(x =>  x.Id == taskId);
            return t is null ? null : new TaskView(t.Id, t.ProjectId, t.Title, t.Description, t.Priority, t.DueDate, t.IsFinished);
        }
    }
}
