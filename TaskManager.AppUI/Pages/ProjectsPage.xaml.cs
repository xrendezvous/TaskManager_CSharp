using TaskManager.AppUI.ViewModels;

namespace TaskManager.AppUI;

/// <summary>
/// represents the page that displays the list of projects
/// </summary>
public partial class ProjectsPage : ContentPage
{
    public ProjectsPage(ProjectsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}