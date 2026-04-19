namespace TaskManager.AppUI
{
    public partial class AppShell : Shell
    {
        /// <summary>
        /// init a new instance of the <see cref="AppShell"/> class
        /// configuration of the main nav structure of the app
        /// </summary>
        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            var projectsPage = serviceProvider.GetRequiredService<ProjectsPage>();

            var projectsContent = new ShellContent
            {
                Title = "Projects",
                Route = nameof(ProjectsPage),
                ContentTemplate = new DataTemplate(() => projectsPage)
            };

            var rootItem = new FlyoutItem
            {
                Title = "Projects"
            };

            var rootSection = new ShellSection();
            rootSection.Items.Add(projectsContent);
            rootItem.Items.Add(rootSection);

            Items.Add(rootItem);
            CurrentItem = rootItem;

            Routing.RegisterRoute(nameof(ProjectDetailsPage), typeof(ProjectDetailsPage));
            Routing.RegisterRoute(nameof(TaskDetailsPage), typeof(TaskDetailsPage));
        }
    }
}