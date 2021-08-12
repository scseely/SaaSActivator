using System.Windows;

namespace SaaSActivator_Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ActivatorData()
            {
                ClientId = Properties.Settings.Default["clientId"].ToString(),
                ClientSecret = Properties.Settings.Default["clientSecret"].ToString(),
                TenantId = Properties.Settings.Default["tenantId"].ToString(),
            };
        }
    }
}
