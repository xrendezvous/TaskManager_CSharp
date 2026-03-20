using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManager.AppUI.Services;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.AppUI.ViewModels;

/// <summary>
/// Represents the view model for the project details page.
/// </summary>
public sealed class ProjectDetailsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IProjectService _projectService;
    private readonly ITaskService _taskService;
    private readonly INavigateService _navigationService;

    private string _projectName = string.Empty;
    public string ProjectName
    {
        get => _projectName;
        set => SetProperty(ref _projectName, value);
    }

    private string _projectDescription = string.Empty;
    public string ProjectDescription
    {
        get => _projectDescription;
        set => SetProperty(ref _projectDescription, value);
    }

    public ObservableCollection<TaskListDto> Tasks { get; } = new();

    public ICommand OpenTaskCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectDetailsViewModel"/> class.
    /// </summary>
    /// <param name="projectService">The service used to load project data.</param>
    /// <param name="taskService">The service used to load task data.</param>
    /// <param name="navigationService">The service used for page navigation.</param>
    public ProjectDetailsViewModel(
        IProjectService projectService,
        ITaskService taskService,
        INavigateService navigationService)
    {
        _projectService = projectService;
        _taskService = taskService;
        _navigationService = navigationService;

        OpenTaskCommand = new Command<int>(async id =>
            await _navigationService.GoToTaskDetailsAsync(id));
    }

    /// <summary>
    /// Applies query parameters passed through Shell navigation.
    /// </summary>
    /// <param name="query">A dictionary containing navigation query parameters.</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("projectId", out var value))
            return;

        if (!int.TryParse(value?.ToString(), out var projectId))
            return;

        LoadProject(projectId);
    }

    /// <summary>
    /// Loads project details and related tasks into the view model.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    private void LoadProject(int projectId)
    {
        var project = _projectService.GetProjectDetails(projectId);

        ProjectName = project.Name;
        ProjectDescription =
            $"Type: {project.Type}\n" +
            $"Description: {project.Description}\n" +
            $"Progress: {project.Progress}%";

        Tasks.Clear();

        foreach (var task in _taskService.GetTasksForProject(projectId))
        {
            Tasks.Add(task);
        }
    }
}