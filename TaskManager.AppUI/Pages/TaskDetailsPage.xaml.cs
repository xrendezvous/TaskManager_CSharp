using TaskManager.AppUI.ViewModels;

namespace TaskManager.AppUI
{
    public partial class TaskDetailsPage : ContentPage
    {
        private readonly TaskDetailsViewModel _viewModel;

        public TaskDetailsPage(TaskDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InitializeAsync();
        }
    }
}