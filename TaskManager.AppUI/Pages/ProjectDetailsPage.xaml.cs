using TaskManager.AppUI.ViewModels;
namespace TaskManager.AppUI;

/// <summary>
/// represents the page that displays detailed information about a project
/// </summary>
public partial class ProjectDetailsPage : ContentPage
{
    public ProjectDetailsPage(ProjectDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}