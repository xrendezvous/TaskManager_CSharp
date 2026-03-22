using TaskManager.AppUI.ViewModels;

namespace TaskManager.AppUI
{
    /// <summary>
    /// represents the page that displays detailed information about a task
    /// </summary>
    public partial class TaskDetailsPage : ContentPage
    {
        public TaskDetailsPage(TaskDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}