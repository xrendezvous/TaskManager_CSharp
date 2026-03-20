using TaskManager.AppUI.ViewModels;

namespace TaskManager.AppUI;

/// <summary>
/// Represents the page that displays the list of projects.
/// </summary>
public partial class ProjectsPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectsPage"/> class.
    /// </summary>
    /// <param name="viewModel">The view model used as the binding context for the page.</param>
    public ProjectsPage(ProjectsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}