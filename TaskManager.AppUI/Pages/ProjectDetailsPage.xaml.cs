using TaskManager.AppUI.ViewModels;

namespace TaskManager.AppUI;

public partial class ProjectDetailsPage : ContentPage
{
    private readonly ProjectDetailsViewModel _viewModel;

    public ProjectDetailsPage(ProjectDetailsViewModel viewModel)
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