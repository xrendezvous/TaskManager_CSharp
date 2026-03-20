using TaskManager.AppUI.ViewModels;
namespace TaskManager.AppUI;

/// <summary>
/// Represents the page that displays detailed information about a project.
/// </summary>
public partial class ProjectDetailsPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectDetailsPage"/> class.
    /// </summary>
    /// <param name="viewModel">The view model used as the binding context for the page.</param>
    public ProjectDetailsPage(ProjectDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}