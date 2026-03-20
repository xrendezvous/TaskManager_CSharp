using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManager.AppUI.Services;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.AppUI.ViewModels;

public sealed class ProjectsViewModel : BaseViewModel
{
    private readonly IProjectService _projectService;
    private readonly INavigateService _navigationService;

    public ObservableCollection<ProjectListDto> Projects { get; } = new();

    public ICommand OpenProjectCommand { get; }

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

    private void LoadProjects()
    {
        Projects.Clear();

        foreach (var project in _projectService.GetProjectsForList())
        {
            Projects.Add(project);
        }
    }
}