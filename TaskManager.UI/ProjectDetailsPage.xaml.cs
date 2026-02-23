using TaskManager.Services;

namespace TaskManager.AppUI;

[QueryProperty(nameof(ProjectId), "projectId")]
public partial class ProjectDetailsPage : ContentPage
{
    private readonly RepositoryService _repo;
    public string ProjectId { get; set; }

    public ProjectDetailsPage(RepositoryService repo)
    {
        InitializeComponent();
        _repo = repo;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        int id = int.Parse(ProjectId);

        var project = _repo.GetProjects().First(p => p.Id == id);

        NameLabel.Text = project.Name;
        TypeLabel.Text = $"Type: {project.Type}";
        DescLabel.Text = $"Description: {project.Description}";
        ProgressLabel.Text = $"Progress: {project.Progress}%";

        TasksList.ItemsSource = _repo.GetTasksByProjects(id);
    }

    private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
    {
        var task = e.CurrentSelection.FirstOrDefault();
        if (task == null)
            return;

        var t = (Views.TaskView)task;

        await Shell.Current.GoToAsync($"taskdetails?taskId={t.Id}");
    }
}