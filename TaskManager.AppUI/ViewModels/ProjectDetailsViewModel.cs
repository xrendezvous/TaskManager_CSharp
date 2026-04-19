using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using TaskManager.AppUI.Services;
using TaskManager.Repositories.Enums;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.AppUI.ViewModels;

public sealed class ProjectDetailsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IProjectService _projectService;
    private readonly ITaskService _taskService;
    private readonly INavigateService _navigationService;

    private int _projectId;
    private TypeOfProject _currentProjectType;
    private string _currentProjectDescription = string.Empty;

    private string _projectName = string.Empty;
    private string _projectTypeText = string.Empty;
    private string _projectDescription = string.Empty;
    private string _projectProgressText = string.Empty;
    private string _projectStatsText = string.Empty;

    private string _taskSearchText = string.Empty;
    private string _selectedPriorityOption = "All";
    private string _selectedStatusOption = "All";
    private string _selectedSortOption = "By Priority";

    public ObservableCollection<TaskListDto> Tasks { get; } = new();

    public IReadOnlyList<string> PriorityOptions { get; } =
    [
        "All",
        "Low",
        "Medium",
        "High",
        "Critical"
    ];

    public IReadOnlyList<string> StatusOptions { get; } =
    [
        "All",
        "Active",
        "Finished",
        "Overdue"
    ];

    public IReadOnlyList<string> SortOptions { get; } =
    [
        "By Priority",
        "By Due Date",
        "By Name"
    ];

    public string ProjectName
    {
        get => _projectName;
        set => SetProperty(ref _projectName, value);
    }

    public string ProjectTypeText
    {
        get => _projectTypeText;
        set => SetProperty(ref _projectTypeText, value);
    }

    public string ProjectDescription
    {
        get => _projectDescription;
        set => SetProperty(ref _projectDescription, value);
    }

    public string ProjectProgressText
    {
        get => _projectProgressText;
        set => SetProperty(ref _projectProgressText, value);
    }

    public string ProjectStatsText
    {
        get => _projectStatsText;
        set => SetProperty(ref _projectStatsText, value);
    }

    public string TaskSearchText
    {
        get => _taskSearchText;
        set => SetProperty(ref _taskSearchText, value);
    }

    public string SelectedPriorityOption
    {
        get => _selectedPriorityOption;
        set => SetProperty(ref _selectedPriorityOption, value);
    }

    public string SelectedStatusOption
    {
        get => _selectedStatusOption;
        set => SetProperty(ref _selectedStatusOption, value);
    }

    public string SelectedSortOption
    {
        get => _selectedSortOption;
        set => SetProperty(ref _selectedSortOption, value);
    }

    public ICommand ReloadProjectCommand { get; }
    public ICommand ClearTaskFiltersCommand { get; }
    public ICommand OpenTaskCommand { get; }
    public ICommand AddTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }
    public ICommand EditProjectCommand { get; }
    public ICommand DeleteProjectCommand { get; }
    public ProjectDetailsViewModel(
        IProjectService projectService,
        ITaskService taskService,
        INavigateService navigationService)
    {
        _projectService = projectService;
        _taskService = taskService;
        _navigationService = navigationService;

        ReloadProjectCommand = new Command(async () => await LoadProjectAsync());
        ClearTaskFiltersCommand = new Command(async () => await ClearTaskFiltersAsync());
        OpenTaskCommand = new Command<int>(async id => await OpenTaskAsync(id));
        AddTaskCommand = new Command(async () => await AddTaskAsync());
        DeleteTaskCommand = new Command<int>(async id => await DeleteTaskAsync(id));
        EditProjectCommand = new Command(async () => await EditProjectAsync());
        DeleteProjectCommand = new Command(async () => await DeleteProjectAsync());
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("projectId", out var value))
            return;

        if (!int.TryParse(value?.ToString(), out var projectId))
            return;

        _projectId = projectId;
    }

    public async Task InitializeAsync()
    {
        if (_projectId <= 0)
            return;

        await LoadProjectAsync();
    }

    private async Task LoadProjectAsync()
    {
        if (_projectId <= 0)
            return;

        await RunBusyAsync(async () =>
        {
            try
            {
                var project = await _projectService.GetProjectDetailsAsync(_projectId);

                _currentProjectType = project.Type;
                _currentProjectDescription = project.Description;

                ProjectName = project.Name;
                ProjectTypeText = $"Type: {project.Type}";
                ProjectDescription = project.Description;
                ProjectProgressText = $"Progress: {project.Progress}%";
                ProjectStatsText = $"Tasks: {project.TasksAmount} | Finished: {project.FinishedTasks}";

                await LoadTasksInternalAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        });
    }

    private async Task LoadTasksInternalAsync()
    {
        Tasks.Clear();

        var items = await _taskService.GetTasksForProjectAsync(_projectId, new FilterTaskDto
        {
            SearchText = TaskSearchText,
            Priority = MapPriorityFilter(SelectedPriorityOption),
            IsFinished = MapStatusToFinishedFlag(SelectedStatusOption),
            OnlyOverdue = SelectedStatusOption == "Overdue",
            SortBy = MapTaskSort(SelectedSortOption)
        });

        foreach (var task in items)
        {
            Tasks.Add(task);
        }
    }

    private async Task ClearTaskFiltersAsync()
    {
        TaskSearchText = string.Empty;
        SelectedPriorityOption = "All";
        SelectedStatusOption = "All";
        SelectedSortOption = "By Priority";
        await LoadProjectAsync();
    }

    private async Task OpenTaskAsync(int taskId)
    {
        if (IsBusy)
            return;

        await _navigationService.GoToTaskDetailsAsync(taskId);
    }

    private async Task AddTaskAsync()
    {
        if (_projectId <= 0)
            return;

        await RunBusyAsync(async () =>
        {
            try
            {
                var name = await Shell.Current.DisplayPromptAsync(
                    "New task",
                    "Enter task name:",
                    initialValue: string.Empty,
                    maxLength: 100,
                    keyboard: Keyboard.Text);

                if (string.IsNullOrWhiteSpace(name))
                    return;

                var description = await Shell.Current.DisplayPromptAsync(
                    "New task",
                    "Enter description:",
                    initialValue: string.Empty,
                    maxLength: 500,
                    keyboard: Keyboard.Text);

                if (description is null)
                    return;

                var priority = await PickPriorityAsync();
                if (priority is null)
                    return;

                var dueDate = await PromptDateAsync("New task", DateTime.Today.AddDays(7));
                if (dueDate is null)
                    return;

                await _taskService.CreateTaskAsync(new CreateTaskDto
                {
                    ProjectId = _projectId,
                    Name = name.Trim(),
                    Description = description.Trim(),
                    Priority = priority.Value,
                    DueDate = dueDate.Value,
                    IsFinished = false
                });

                await LoadProjectAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        });
    }

    private async Task DeleteTaskAsync(int taskId)
    {
        await RunBusyAsync(async () =>
        {
            try
            {
                var confirmed = await Shell.Current.DisplayAlert(
                    "Delete task",
                    "Delete this task?",
                    "Delete",
                    "Cancel");

                if (!confirmed)
                    return;

                await _taskService.DeleteTaskAsync(taskId);
                await LoadProjectAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        });
    }

    private async Task EditProjectAsync()
    {
        if (_projectId <= 0)
            return;

        await RunBusyAsync(async () =>
        {
            try
            {
                var name = await Shell.Current.DisplayPromptAsync(
                    "Edit project",
                    "Project name:",
                    initialValue: ProjectName,
                    maxLength: 100,
                    keyboard: Keyboard.Text);

                if (string.IsNullOrWhiteSpace(name))
                    return;

                var description = await Shell.Current.DisplayPromptAsync(
                    "Edit project",
                    "Description:",
                    initialValue: _currentProjectDescription,
                    maxLength: 500,
                    keyboard: Keyboard.Text);

                if (description is null)
                    return;

                var type = await PickProjectTypeAsync(_currentProjectType);
                if (type is null)
                    return;

                await _projectService.UpdateProjectAsync(_projectId, new UpdateProjectDto
                {
                    Name = name.Trim(),
                    Description = description.Trim(),
                    Type = type.Value
                });

                await LoadProjectAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        });
    }

    private async Task DeleteProjectAsync()
    {
        if (_projectId <= 0)
            return;

        await RunBusyAsync(async () =>
        {
            try
            {
                var confirmed = await Shell.Current.DisplayAlert(
                    "Delete project",
                    "Delete this project and all its tasks?",
                    "Delete",
                    "Cancel");

                if (!confirmed)
                    return;

                await _projectService.DeleteProjectAsync(_projectId);
                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        });
    }

    private static Priority? MapPriorityFilter(string selected)
    {
        return selected switch
        {
            "Low" => Priority.Low,
            "Medium" => Priority.Medium,
            "High" => Priority.High,
            "Critical" => Priority.Critical,
            _ => null
        };
    }

    private static bool? MapStatusToFinishedFlag(string selected)
    {
        return selected switch
        {
            "Active" => false,
            "Finished" => true,
            _ => null
        };
    }

    private static SortTask MapTaskSort(string selected)
    {
        return selected switch
        {
            "By Due Date" => SortTask.ByDueDate,
            "By Name" => SortTask.ByName,
            _ => SortTask.ByPriority
        };
    }

    private static async Task<Priority?> PickPriorityAsync(Priority? currentValue = null)
    {
        var selected = await Shell.Current.DisplayActionSheet(
            "Select priority",
            "Cancel",
            null,
            "Low",
            "Medium",
            "High",
            "Critical");

        if (selected == "Cancel")
            return currentValue;

        return selected switch
        {
            "Low" => Priority.Low,
            "Medium" => Priority.Medium,
            "High" => Priority.High,
            "Critical" => Priority.Critical,
            _ => currentValue
        };
    }

    private static async Task<TypeOfProject?> PickProjectTypeAsync(TypeOfProject? currentValue = null)
    {
        var selected = await Shell.Current.DisplayActionSheet(
            "Select project type",
            "Cancel",
            null,
            "Work",
            "Study",
            "Personal",
            "Other");

        if (selected == "Cancel")
            return currentValue;

        return selected switch
        {
            "Work" => TypeOfProject.Work,
            "Study" => TypeOfProject.Study,
            "Personal" => TypeOfProject.Personal,
            "Other" => TypeOfProject.Other,
            _ => currentValue
        };
    }

    private static async Task<DateTime?> PromptDateAsync(string title, DateTime initialValue)
    {
        var input = await Shell.Current.DisplayPromptAsync(
            title,
            "Enter due date in format yyyy-MM-dd:",
            initialValue: initialValue.ToString("yyyy-MM-dd"),
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (DateTime.TryParseExact(
                input.Trim(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var exactDate))
        {
            return exactDate;
        }

        if (DateTime.TryParse(input.Trim(), out var parsedDate))
        {
            return parsedDate;
        }

        await Shell.Current.DisplayAlert("Invalid date", "Use format yyyy-MM-dd.", "OK");
        return null;
    }
}