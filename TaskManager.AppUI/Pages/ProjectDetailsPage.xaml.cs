using TaskManager.AppUI.ViewModels;
namespace TaskManager.AppUI;

public partial class ProjectDetailsPage : ContentPage
{
    public ProjectDetailsPage(ProjectDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}