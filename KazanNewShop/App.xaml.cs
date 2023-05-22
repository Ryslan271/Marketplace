using KazanNewShop.Database.Models;
using KazanNewShop.View.Windows;
using System.Windows;

namespace KazanNewShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User CarrentUser { get; set; } = null!;

        public App()
        {
            Database.DatabaseContext.LoadEntities();
        }
    }
}
