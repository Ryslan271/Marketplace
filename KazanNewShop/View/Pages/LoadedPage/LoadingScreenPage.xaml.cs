using KazanNewShop.Database;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace KazanNewShop.View.Pages.LoadedPage
{
    /// <summary>
    /// Логика взаимодействия для loadingScreen.xaml
    /// </summary>
    public partial class LoadingScreenPage : UiPage
    {
        public LoadingScreenPage()
        {
            InitializeComponent();

            // подгрузка основных сущностей
            Task loadEntities = new(DatabaseContext.LoadEntitesForMarketplace);
            loadEntities.Start();

            // Ожидание загрузки
            loadEntities.Wait(1000);

        }
    }
}
