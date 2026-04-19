using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManager.AppUI.Services;
using TaskManager.Repositories.Enums;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.AppUI.ViewModels;

/// <summary>
/// viewmodel for the projects list page,
/// includes filtering, sorting, navigation, and project management
/// </summary>
public sealed class ProjectsViewModel : BaseViewModel
{
    private readonly IProjectService _projectService;
    private readonly INavigateService _navigationService;

    private string _searchText = string.Empty;
    private string _selectedTypeOption = "All";
    private string _selectedSortOption = "By Id";

    /// <summary>
    /// gets the collection of projects displayed
    /// </summary>
    public ObservableCollection<ProjectListDto> Projects { get; } = new();

    public IReadOnlyList<string> TypeOptions { get; } =
    [
        "All",
        "Work",
        "Study",
        "Personal",
        "Other"
    ];

    public IReadOnlyList<string> SortOptions { get; } =
    [
        "By Id",
        "By Name",
        "By Progress Asc",
        "By Progress Desc"
    ];

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public string SelectedTypeOption
    {
        get => _selectedTypeOption;
        set => SetProperty(ref _selectedTypeOption, value);
    }

    public string SelectedSortOption
    {
        get => _selectedSortOption;
        set => SetProperty(ref _selectedSortOption, value);
    }

    public ICommand ReloadProjectsCommand { get; }
    public ICommand ClearFiltersCommand { get; }
    public ICommand OpenProjectCommand { get; }
    public ICommand AddProjectCommand { get; }
    public ICommand DeleteProjectCommand { get; }

    /// <summary>
    /// initializes a new instance of the <see cref="ProjectsViewModel"/> class
    /// </summary>
    /// <param name="projectService">service used to load and modify project data</param>
    /// <param name="navigationService">service used for navigation between pages</param>
    public ProjectsViewModel(
        IProjectService projectService,
        INavigateService navigationService)
    {
        _projectService = projectService;
        _navigationService = navigationService;

        ReloadProjectsCommand = new Command(async () => await LoadProjectsAsync());
        ClearFiltersCommand = new Command(async () => await ClearFiltersAsync());
        OpenProjectCommand = new Command<int>(async id => await OpenProjectAsync(id));
        AddProjectCommand = new Command(async () => await AddProjectAsync());
        DeleteProjectCommand = new Command<int>(async id => await DeleteProjectAsync(id));
    }

    public async Task InitializeAsync()
    {
        await LoadProjectsAsync();
    }

    /// <summary>
    /// loads projects using the busy state wrap
    /// </summary>
    private async Task LoadProjectsAsync()
    {
        await RunBusyAsync(LoadProjectsCoreAsync);
    }

    /// <summary>
    /// loads projects using the current search, filter, and sorting options
    /// </summary>
    private async Task LoadProjectsCoreAsync()
    {
        Projects.Clear();

        var items = await _projectService.GetProjectsForListAsync(new FilterProjectDto
        {
            SearchText = SearchText,
            Type = MapTypeFilter(SelectedTypeOption),
            SortBy = MapProjectSort(SelectedSortOption)
        });

        foreach (var project in items)
        {
            Projects.Add(project);
        }
    }

    private async Task ClearFiltersAsync()
    {
        SearchText = string.Empty;
        SelectedTypeOption = "All";
        SelectedSortOption = "By Id";
        await LoadProjectsAsync();
    }

    /// <summary>
    /// opens the details page for the specified project
    /// </summary>
    /// <param name="projectId">identifier of the project to open</param>
    private async Task OpenProjectAsync(int projectId)
    {
        if (IsBusy)
            return;

        await _navigationService.GoToProjectDetailsAsync(projectId);
    }

    private async Task AddProjectAsync()
    {
        await RunBusyAsync(async () =>
        {
            try
            {
                var name = await Shell.Current.DisplayPromptAsync(
                    "New project",
                    "Enter project name:",
                    initialValue: string.Empty,
                    maxLength: 100,
                    keyboard: Keyboard.Text);

                if (string.IsNullOrWhiteSpace(name))
                    return;

                var description = await Shell.Current.DisplayPromptAsync(
                    "New project",
                    "Enter description:",
                    initialValue: string.Empty,
                    maxLength: 500,
                    keyboard: Keyboard.Text);

                if (description is null)
                    return;

                var type = await PickProjectTypeAsync();
                if (type is null)
                    return;

                await _projectService.CreateProjectAsync(new CreateProjectDto
                {
                    Name = name.Trim(),
                    Description = description.Trim(),
                    Type = type.Value
                });

                await LoadProjectsCoreAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        });
    }

    private async Task DeleteProjectAsync(int projectId)
    {
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

                await _projectService.DeleteProjectAsync(projectId);
                await LoadProjectsCoreAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        });
    }

    /// <summary>
    /// converts the selected project type option string into a null
    /// project type enum val
    /// </summary>
    /// <param name="selected">selected project type option text</param>
    private static TypeOfProject? MapTypeFilter(string selected)
    {
        return selected switch
        {
            "Work" => TypeOfProject.Work,
            "Study" => TypeOfProject.Study,
            "Personal" => TypeOfProject.Personal,
            "Other" => TypeOfProject.Other,
            _ => null
        };
    }

    /// <summary>
    /// converts the selected project sorting option string into the 
    /// corresponding sort enum val
    /// </summary>
    /// <param name="selected">selected project sort option text</param>
    private static ProjectSortOption MapProjectSort(string selected)
    {
        return selected switch
        {
            "By Name" => ProjectSortOption.ByName,
            "By Progress Asc" => ProjectSortOption.ByProgressAsc,
            "By Progress Desc" => ProjectSortOption.ByProgressDesc,
            _ => ProjectSortOption.ById
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
}