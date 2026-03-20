using TaskManager.AppUI.ViewModels;

namespace TaskManager.AppUI
{
    /// <summary>
    /// Represents the page that displays detailed information about a task.
    /// </summary>
    public partial class TaskDetailsPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDetailsPage"/> class.
        /// </summary>
        /// <param name="viewModel">The view model used as the binding context for the page.</param>
        public TaskDetailsPage(TaskDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}