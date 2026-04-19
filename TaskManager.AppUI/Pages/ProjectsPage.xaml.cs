using TaskManager.AppUI.ViewModels;

namespace TaskManager.AppUI;

public partial class ProjectsPage : ContentPage
{
    private readonly ProjectsViewModel _viewModel;

    public ProjectsPage(ProjectsViewModel viewModel)
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