using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using TaskManager.AppUI.Services;
using TaskManager.Repositories.Enums;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.AppUI.ViewModels;

/// <summary>
/// viewmodel for the project details page,
/// includes project info, task filtering and management
/// </summary>
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

    /// <summary>
    /// gets the collection of tasks for the current project
    /// </summary>
    public ObservableCollection<TaskListDto> Tasks { get; } = new();

    /// <summary>
    /// gets the available task priorities for filter
    /// </summary>
    public IReadOnlyList<string> PriorityOptions { get; } =
    [
        "All",
        "Low",
        "Medium",
        "High",
        "Critical"
    ];

    /// <summary>
    /// gets the available task statuses for filter
    /// </summary>
    public IReadOnlyList<string> StatusOptions { get; } =
    [
        "All",
        "Active",
        "Finished",
        "Overdue"
    ];

    /// <summary>
    /// gets the available task sort options for filter
    /// </summary>
    public IReadOnlyList<string> SortOptions { get; } =
    [
        "By Priority",
        "By Due Date",
        "By Name"
    ];

    /// <summary>
    /// display name of project
    /// </summary>
    public string ProjectName
    {
        get => _projectName;
        set => SetProperty(ref _projectName, value);
    }

    /// <summary>
    /// type of project
    /// </summary>
    public string ProjectTypeText
    {
        get => _projectTypeText;
        set => SetProperty(ref _projectTypeText, value);
    }

    /// <summary>
    /// description of project
    /// </summary>
    public string ProjectDescription
    {
        get => _projectDescription;
        set => SetProperty(ref _projectDescription, value);
    }

    /// <summary>
    /// progress of project
    /// </summary>
    public string ProjectProgressText
    {
        get => _projectProgressText;
        set => SetProperty(ref _projectProgressText, value);
    }

    /// <summary>
    /// stats for project
    /// </summary>
    public string ProjectStatsText
    {
        get => _projectStatsText;
        set => SetProperty(ref _projectStatsText, value);
    }

    /// <summary>
    /// for searching tasks
    /// </summary>
    public string TaskSearchText
    {
        get => _taskSearchText;
        set => SetProperty(ref _taskSearchText, value);
    }

    // next three strings are getters/setters
    // for selected filter options
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

    /// <summary>
    /// initializes a new instance of the <see cref="ProjectDetailsViewModel"/> class
    /// </summary>
    /// <param name="projectService">service used to load and modify project data</param>
    /// <param name="taskService">service used to load and modify task data</param>
    /// <param name="navigationService">service used for app navigation</param>
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

    /// <summary>
    /// applies nav query params passed through Shell routing
    /// </summary>
    /// <param name="query">dict containing route query params</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("projectId", out var value))
            return;

        if (!int.TryParse(value?.ToString(), out var projectId))
            return;

        _projectId = projectId;
    }

    /// <summary>
    /// used to initialize the viewmodel by loading
    /// current project
    /// </summary>
    public async Task InitializeAsync()
    {
        if (_projectId <= 0)
            return;

        await LoadProjectAsync();
    }

    /// <summary>
    /// for loading the currect project and tasks using
    /// busy state wrap
    /// </summary>
    private async Task LoadProjectAsync()
    {
        if (_projectId <= 0)
            return;

        await RunBusyAsync(LoadProjectCoreAsync);
    }

    /// <summary>
    /// used for automatic refreshing of pages during
    /// project/tasks management
    /// </summary>
    private async Task LoadProjectCoreAsync()
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

    /// <summary>
    /// loads tasks for the current proj
    /// using the selected search, filter, and sort options
    /// </summary>
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

    /// <summary>
    /// clears all task filters and reloads project data
    /// </summary>
    private async Task ClearTaskFiltersAsync()
    {
        TaskSearchText = string.Empty;
        SelectedPriorityOption = "All";
        SelectedStatusOption = "All";
        SelectedSortOption = "By Priority";
        await LoadProjectAsync();
    }

    /// <summary>
    /// opens the details page for the specified task
    /// </summary>
    /// <param name="taskId">id of the task to open</param>
    private async Task OpenTaskAsync(int taskId)
    {
        if (IsBusy)
            return;

        await _navigationService.GoToTaskDetailsAsync(taskId);
    }

    /// <summary>
    /// asks user for task data, creates a new task 
    /// and refreshes the current proj view
    /// </summary>
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

                await LoadProjectCoreAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        });
    }

    /// <summary>
    /// asks the user for confirmation, deletes the selected task
    /// and refreshes the current proj view
    /// </summary>
    /// <param name="taskId">id of the task to delete</param>
    private async Task DeleteTaskAsync(int taskId)
    {
        await RunBusyAsync(async () =>
        {
            try
            {
                var confirmed = await Shell.Current.DisplayAlertAsync(
                    "Delete task",
                    "Delete this task?",
                    "Delete",
                    "Cancel");

                if (!confirmed)
                    return;

                await _taskService.DeleteTaskAsync(taskId);
                await LoadProjectCoreAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        });
    }

    /// <summary>
    /// asks user to edit the current proj 
    /// and saves the updated data
    /// </summary>
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
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        });
    }

    /// <summary>
    /// asks the user for confirmation and 
    /// deletes the current proj
    /// </summary>
    private async Task DeleteProjectAsync()
    {
        if (_projectId <= 0)
            return;

        await RunBusyAsync(async () =>
        {
            try
            {
                var confirmed = await Shell.Current.DisplayAlertAsync(
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
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        });
    }

    /// <summary>
    /// converts the selected priority option string 
    /// into a null priority enum val
    /// </summary>
    /// <param name="selected">selected priority option text</param>
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

    /// <summary>
    /// converts the selected status option string 
    /// into a null finished flag
    /// </summary>
    /// <param name="selected">selected status option text</param>
    private static bool? MapStatusToFinishedFlag(string selected)
    {
        return selected switch
        {
            "Active" => false,
            "Finished" => true,
            _ => null
        };
    }

    /// <summary>
    /// converts the selected task sort option string 
    /// into the sort enum val
    /// </summary>
    /// <param name="selected">selected sort option text</param>
    private static SortTask MapTaskSort(string selected)
    {
        return selected switch
        {
            "By Due Date" => SortTask.ByDueDate,
            "By Name" => SortTask.ByName,
            _ => SortTask.ByPriority
        };
    }

    /// <summary>
    /// lets the user select a task priority
    /// </summary>
    /// <param name="currentValue">currently selected priority val used as fallback</param>
    private static async Task<Priority?> PickPriorityAsync(Priority? currentValue = null)
    {
        var selected = await Shell.Current.DisplayActionSheetAsync(
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

    /// <summary>
    /// lets the user select a project type
    /// </summary>
    /// <param name="currentValue">currently selected project type used as fallback</param>
    private static async Task<TypeOfProject?> PickProjectTypeAsync(TypeOfProject? currentValue = null)
    {
        var selected = await Shell.Current.DisplayActionSheetAsync(
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

    /// <summary>
    /// asks user to enter a date and parses it using the expected format
    /// </summary>
    /// <param name="title">dialog title</param>
    /// <param name="initialValue">initial date val shown to the user</param>
    private static async Task<DateTime?> PromptDateAsync(string title, DateTime initialValue)
    {
        var input = await Shell.Current.DisplayPromptAsync(
            title,
            "Enter due date in format dd-MM-yyyy:",
            initialValue: initialValue.ToString("dd-MM-yyyy"),
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (DateTime.TryParseExact(
                input.Trim(),
                "dd-MM-yyyy",
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

        await Shell.Current.DisplayAlertAsync("Invalid date", "Use format dd-MM-yyyy.", "OK");
        return null;
    }
}