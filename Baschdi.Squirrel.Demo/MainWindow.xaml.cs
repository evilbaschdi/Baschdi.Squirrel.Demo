using System.IO;
using System.Windows;
using Squirrel;

namespace Baschdi.Squirrel.Demo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var isGitHubMaintained = true;

            IUpdateManager updateManager = new UpdateManager(Path.Combine(Directory.GetCurrentDirectory(), "Squirrel"));
            if (isGitHubMaintained)
            {
                updateManager = await UpdateManager.GitHubUpdateManager("https://github.com/evilbaschdi/Baschdi.Squirrel.Demo", prerelease: true);
            }

            ICheckForUpdates checkForUpdates = new CheckForUpdates(updateManager);
            IUpdate update = new Update(updateManager, checkForUpdates);
            await update.TaskRun();

            updateManager.Dispose();
        }
    }
}