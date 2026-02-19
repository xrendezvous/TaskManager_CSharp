using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Services;

namespace TaskManager.App
{
    internal static class Program
    {
        private static readonly RepositoryService Repo = new();
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                var projects = Repo.GetProjects();

                Console.WriteLine("Projects:");
                foreach (var p in projects)
                {
                    Console.WriteLine($"{p.Id}. [{p.Type}] {p.Name} — {p.Progress}% ({p.FinishedTasks}/{p.TasksAmount})");
                }
                Console.WriteLine();
                Console.Write("Enter project ID for checking, or letter Q for exit: ");
                var input = Console.ReadLine()?.Trim();

                if (string.Equals(input, "Q", StringComparison.OrdinalIgnoreCase))
                    return;

                if (!int.TryParse(input, out var projectId) || projects.All(p => p.Id != projectId))
                    continue;

                ShowProject(projectId);
            }
        }

        private static void ShowProject(int projectId)
        {
            while (true)
            {
                Console.Clear();
                var projects = Repo.GetProjects();
                var project = projects.First(p => p.Id == projectId);

                Console.WriteLine($"=== Project #{project.Id}: {project.Name} ===");
                Console.WriteLine($"Type: {project.Type}");
                Console.WriteLine($"Desc: {project.Description}");
                Console.WriteLine($"Progress: {project.Progress}% ({project.FinishedTasks}/{project.TasksAmount})");
                Console.WriteLine();

                var tasks = Repo.GetTasksByProjects(projectId);
                if (tasks.Count == 0)
                {
                    Console.WriteLine("No tasks found in this project.");
                }
                else
                {
                    Console.WriteLine("List of tasks (priority ↓, due ↑):");
                    foreach (var t in tasks)
                    {
                        var status = t.IsFinished ? "DONE" : (t.IsOverdue ? "OVERDUE" : "TODO");
                        Console.WriteLine($"{t.Id} | {status,-7} | {t.Priority,-8} | due {t.DueDate:yyyy-MM-dd} | {t.Title}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Enter command: [task ID] for details | R to update | B go back");
                Console.Write(">> ");
                var cmd = Console.ReadLine()?.Trim();

                if (string.Equals(cmd, "B", StringComparison.OrdinalIgnoreCase))
                    return;

                if (string.Equals(cmd, "R", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (int.TryParse(cmd, out var taskId))
                {
                    var task = Repo.GetTask(taskId);
                    if (task is not null && task.ProjectId == projectId)
                        ShowTask(task);
                }
            }
        }

        private static void ShowTask(TaskManager.Views.TaskView task)
        {
            Console.Clear();
            Console.WriteLine($"=== Task #{task.Id} ===");
            Console.WriteLine($"ProjectId: {task.ProjectId}");
            Console.WriteLine($"Title: {task.Title}");
            Console.WriteLine($"Priority: {task.Priority}");
            Console.WriteLine($"Due: {task.DueDate:yyyy-MM-dd}");
            Console.WriteLine($"Done: {task.IsFinished}");
            Console.WriteLine($"Overdue: {task.IsOverdue}");
            Console.WriteLine();
            Console.WriteLine("Desc:");
            Console.WriteLine(task.Description);
            Console.WriteLine();
            Console.Write("Press Enter to go back...");
            Console.ReadLine();
        }
    }
}

