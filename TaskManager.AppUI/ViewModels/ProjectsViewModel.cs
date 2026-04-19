using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManager.AppUI.Services;
using TaskManager.Repositories.Enums;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.AppUI.ViewModels;

public sealed class ProjectsViewModel : BaseViewModel
{
    private readonly IProjectService _projectService;
    private readonly INavigateService _navigationService;

    private string _searchText = string.Empty;
    private string _selectedTypeOption = "All";
    private string _selectedSortOption = "By Id";

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

    private async Task LoadProjectsAsync()
    {
        await RunBusyAsync(async () =>
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
        });
    }

    private async Task ClearFiltersAsync()
    {
        SearchText = string.Empty;
        SelectedTypeOption = "All";
        SelectedSortOption = "By Id";
        await LoadProjectsAsync();
    }

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

                await LoadProjectsAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        });
    }

    private async Task DeleteProjectAsync(int projectId)
    {
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

                await _projectService.DeleteProjectAsync(projectId);
                await LoadProjectsAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        });
    }

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
}