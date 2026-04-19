using System.Text.Json;
using TaskManager.Repositories.Entities;
using TaskManager.Repositories.Enums;

namespace TaskManager.Repositories.Storage
{
    /// <summary>
    /// implements JSON-based storage context for 
    /// saving projects and tasks
    /// in a local app data file
    /// </summary>
    public sealed class JsonStorageContext : IStorageContext
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _syncLock = new(1, 1);
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true
        };

        /// <summary>
        /// init of a new instance of the <see cref="JsonStorageContext"/> class
        /// configuration of the path to the local JSON
        /// </summary>
        public JsonStorageContext()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _filePath = Path.Combine(appDataPath, "taskmanager-data.json");
        }

        /// <summary>
        /// checks if the storage file exists and 
        /// contains initial seed data on first app launch
        /// </summary>
        private async Task EnsureInitializedAsync()
        {
            if (File.Exists(_filePath))
                return;

            var seedData = new StorageDataModel
            {
                Projects = new List<ProjectRecord>
                {
                    new(1, "Diploma", "Models, statistics, analysis", TypeOfProject.Study),
                    new(2, "Book club", "Prepare for monthly meeting", TypeOfProject.Personal),
                    new(3, "MindCare", "Add frontend to the project", TypeOfProject.Work),
                },
                Tasks = new List<TaskRecord>
                {
                    new(101, 1, "Section 1 Plan", "Update structure", Priority.High, DateTime.Today.AddDays(2), false),
                    new(102, 1, "Find 10 sources", "Articles and books", Priority.Medium, DateTime.Today.AddDays(5), false),
                    new(103, 1, "Baseline experiment", "TF-IDF and logreg", Priority.Critical, DateTime.Today.AddDays(-1), false),
                    new(104, 1, "Create graphs", "Matplotlib", Priority.Medium, DateTime.Today.AddDays(7), false),
                    new(105, 1, "Conclusions for the section", "2-3 pages", Priority.High, DateTime.Today.AddDays(10), false),
                    new(106, 1, "Code refactoring", "Move utils", Priority.Low, DateTime.Today.AddDays(14), true),
                    new(107, 1, "Desc of dataset", "Fields, size etc.", Priority.Medium, DateTime.Today.AddDays(4), true),
                    new(108, 1, "Metrics", "Accuracy/F1/AUC", Priority.High, DateTime.Today.AddDays(6), false),
                    new(109, 1, "Error analysis", "FP/FN examples", Priority.Medium, DateTime.Today.AddDays(9), false),
                    new(110, 1, "Presentation draft", "10-12 slides", Priority.High, DateTime.Today.AddDays(12), false),

                    new(201, 2, "Read the assigned book", "Until page 269", Priority.High, DateTime.Today.AddDays(3), false),
                    new(202, 2, "Make a presentation", "10-12 slides", Priority.Critical, DateTime.Today.AddDays(8), false),
                }
            };

            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            await using var createStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(createStream, seedData, _jsonOptions);
        }

        /// <summary>
        /// reads the full data model from the JSON
        /// </summary>
        private async Task<StorageDataModel> ReadDataAsync()
        {
            await EnsureInitializedAsync();

            await using var stream = File.OpenRead(_filePath);
            var data = await JsonSerializer.DeserializeAsync<StorageDataModel>(stream, _jsonOptions);
            return data ?? new StorageDataModel();
        }

        /// <summary>
        /// writes the full data model to the JSON
        /// </summary>
        /// <param name="data">data model to write</param>
        private async Task WriteDataAsync(StorageDataModel data)
        {
            await using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, data, _jsonOptions);
        }

        public async Task<IReadOnlyList<ProjectRecord>> GetProjectsAsync()
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();
                return data.Projects.OrderBy(p => p.Id).ToList();
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task<ProjectRecord?> GetProjectAsync(int projectId)
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();
                return data.Projects.FirstOrDefault(p => p.Id == projectId);
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task<ProjectRecord> AddProjectAsync(string name, string description, TypeOfProject type)
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();
                var newId = data.Projects.Count == 0 ? 1 : data.Projects.Max(p => p.Id) + 1;

                var project = new ProjectRecord(newId, name, description, type);
                data.Projects.Add(project);

                await WriteDataAsync(data);
                return project;
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task UpdateProjectAsync(ProjectRecord project)
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();
                var existing = data.Projects.FirstOrDefault(p => p.Id == project.Id);

                if (existing is null)
                    throw new KeyNotFoundException($"Project with ID {project.Id} was not found.");

                existing.UpdateRecord(project.Name, project.Description, project.Type);
                await WriteDataAsync(data);
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();

                var project = data.Projects.FirstOrDefault(p => p.Id == projectId);
                if (project is null)
                    throw new KeyNotFoundException($"Project with ID {projectId} was not found.");

                data.Projects.Remove(project);

                var tasksToDelete = data.Tasks.Where(t => t.ProjectId == projectId).ToList();
                foreach (var task in tasksToDelete)
                {
                    data.Tasks.Remove(task);
                }

                await WriteDataAsync(data);
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task<IReadOnlyList<TaskRecord>> GetTasksAsync()
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();
                return data.Tasks.ToList();
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task<IReadOnlyList<TaskRecord>> GetTasksByProjectAsync(int projectId)
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();
                return data.Tasks
                    .Where(t => t.ProjectId == projectId)
                    .ToList();
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task<TaskRecord?> GetTaskAsync(int taskId)
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();
                return data.Tasks.FirstOrDefault(t => t.Id == taskId);
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task<TaskRecord> AddTaskAsync(
            int projectId,
            string name,
            string description,
            Priority priority,
            DateTime dueDate,
            bool isFinished)
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();

                if (data.Projects.All(p => p.Id != projectId))
                    throw new KeyNotFoundException($"Project with ID {projectId} was not found.");

                var newId = data.Tasks.Count == 0 ? 1 : data.Tasks.Max(t => t.Id) + 1;

                var task = new TaskRecord(
                    newId,
                    projectId,
                    name,
                    description,
                    priority,
                    dueDate,
                    isFinished);

                data.Tasks.Add(task);
                await WriteDataAsync(data);

                return task;
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task UpdateTaskAsync(TaskRecord task)
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();
                var existing = data.Tasks.FirstOrDefault(t => t.Id == task.Id);

                if (existing is null)
                    throw new KeyNotFoundException($"Task with ID {task.Id} was not found.");

                existing.UpdateRecord(
                    task.Name,
                    task.Description,
                    task.Priority,
                    task.DueDate,
                    task.IsFinished);

                await WriteDataAsync(data);
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            await _syncLock.WaitAsync();
            try
            {
                var data = await ReadDataAsync();
                var task = data.Tasks.FirstOrDefault(t => t.Id == taskId);

                if (task is null)
                    throw new KeyNotFoundException($"Task with ID {taskId} was not found.");

                data.Tasks.Remove(task);
                await WriteDataAsync(data);
            }
            finally
            {
                _syncLock.Release();
            }
        }
    }
}