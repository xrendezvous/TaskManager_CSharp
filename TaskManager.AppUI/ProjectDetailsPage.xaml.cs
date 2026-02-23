using TaskManager.Services;

namespace TaskManager.AppUI;

[QueryProperty(nameof(ProjectId), "projectId")]
public partial class ProjectDetailsPage : ContentPage
{
    private readonly RepositoryService _repo;
    public string? ProjectId { get; set; }

    public ProjectDetailsPage(RepositoryService repo)
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

    private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
    {
        var task = e.CurrentSelection.FirstOrDefault();
        if (task == null)
            return;

        var t = (Views.TaskView)task;

        await Shell.Current.GoToAsync($"{nameof(TaskDetailsPage)}?taskId={t.Id}");
    }
}
