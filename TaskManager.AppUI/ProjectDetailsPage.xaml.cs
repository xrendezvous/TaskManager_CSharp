using TaskManager.Services;
using TaskManager.Views;

namespace TaskManager.AppUI;

[QueryProperty(nameof(ProjectId), "projectId")]
public partial class ProjectDetailsPage : ContentPage
{
    private readonly IRepositoryService _repo;
    public string? ProjectId { get; set; }

    public ProjectDetailsPage(IRepositoryService repo)
    {
        InitializeComponent();
        _repo = repo;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (string.IsNullOrWhiteSpace(ProjectId))
            return;

        int id = int.Parse(ProjectId);

        var project = _repo.GetProjects().First(p => p.Id == id);

        ProjectTitle.Text = project.Name;
        ProjectDescription.Text =
            $"Type: {project.Type}\n" +
            $"Description: {project.Description}\n" +
            $"Progress: {project.Progress}%";

        TasksList.ItemsSource = _repo.GetTasksByProjects(id);
    }

    private async void OnOpenClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
            button.BindingContext is TaskView task)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(TaskDetailsPage)}?taskId={task.Id}");
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (TasksList.ItemsLayout is GridItemsLayout layout)
        {
            if (width > 1200)
                layout.Span = 4;
            else if (width > 900)
                layout.Span = 3;
            else if (width > 600)
                layout.Span = 2;
            else
                layout.Span = 1;
        }
    }
}
