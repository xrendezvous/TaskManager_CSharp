using TaskManager.AppUI.ViewModels;

namespace TaskManager.AppUI;

public partial class TaskDetailsPage : ContentPage
{
    public TaskDetailsPage(TaskDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}