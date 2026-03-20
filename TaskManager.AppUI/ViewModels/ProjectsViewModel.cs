using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManager.AppUI.Services;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.AppUI.ViewModels;

/// <summary>
/// Represents the view model for the projects list page.
/// </summary>
public sealed class ProjectsViewModel : BaseViewModel
{
    private readonly IProjectService _projectService;
    private readonly INavigateService _navigationService;

    public ObservableCollection<ProjectListDto> Projects { get; } = new();

    public ICommand OpenProjectCommand { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectsViewModel"/> class.
    /// </summary>
    /// <param name="projectService">The service used to load project data.</param>
    /// <param name="navigationService">The service used for page navigation.</param>
    public ProjectsViewModel(
        IProjectService projectService,
        INavigateService navigationService)
    {
        _projectService = projectService;
        _navigationService = navigationService;

        OpenProjectCommand = new Command<int>(async id =>
            await _navigationService.GoToProjectDetailsAsync(id));

        LoadProjects();
    }

    /// <summary>
    /// Loads project data into the observable collection.
    /// </summary>
    private void LoadProjects()
    {
        Projects.Clear();

        foreach (var project in _projectService.GetProjectsForList())
        {
            Projects.Add(project);
        }
    }
}