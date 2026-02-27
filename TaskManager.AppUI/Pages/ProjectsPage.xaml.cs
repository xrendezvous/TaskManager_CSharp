using TaskManager.Services;
using TaskManager.Views;

namespace TaskManager.AppUI;

public partial class ProjectsPage : ContentPage
{
    private readonly IRepositoryService _repo;

    public ProjectsPage(IRepositoryService repo)
    {
        InitializeComponent();
        _repo = repo;

        LoadProjects();
    }

    private void LoadProjects()
    {
        ProjectsList.ItemsSource = _repo.GetProjects();
    }

    private async void OnOpenClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.BindingContext is ProjectView project)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(ProjectDetailsPage)}?projectId={project.Id}");
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (ProjectsList.ItemsLayout is GridItemsLayout layout)
        {
            if (width > 1200)
                layout.Span = 4;
            else if (width > 800)
                layout.Span = 3;
            else
                layout.Span = 2;
        }
    }
}
