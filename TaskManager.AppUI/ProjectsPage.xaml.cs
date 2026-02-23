using TaskManager.Services;
using TaskManager.Views;

namespace TaskManager.AppUI;

public partial class ProjectsPage : ContentPage
{
    private readonly RepositoryService _repo;

    public ProjectsPage(RepositoryService repo)
    {
        InitializeComponent();
        _repo = repo;

        LoadProjects();
    }

    private void LoadProjects()
    {
        ProjectsList.ItemsSource = _repo.GetProjects();
    }

    private async void OnProjectSelected(object sender, SelectionChangedEventArgs e) 
    { 
        var project = e.CurrentSelection.FirstOrDefault(); 
        
        if (project == null) return; 
        var p = (ProjectView)project;

        await Shell.Current.GoToAsync($"{nameof(ProjectDetailsPage)}?projectId={p.Id}");
    }
}
