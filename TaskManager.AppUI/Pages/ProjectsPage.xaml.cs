using TaskManager.AppUI.ViewModels;

namespace TaskManager.AppUI;

public partial class ProjectsPage : ContentPage
{
    public ProjectsPage(ProjectsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}