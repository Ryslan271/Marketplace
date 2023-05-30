using KazanNewShop.Database.Models;
using System.Windows;

namespace KazanNewShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User? CurrentUser { get; set; }

        public App()
        {
            Database.DatabaseContext.LoadEntitesForAuthReg();
        }
    }
}
