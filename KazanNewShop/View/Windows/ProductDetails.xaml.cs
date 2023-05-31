using KazanNewShop.Database.Models;
using KazanNewShop.ViewModel;
using Wpf.Ui.Controls;

namespace KazanNewShop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductDetails.xaml
    /// </summary>
    public partial class ProductDetails : UiWindow
    {
        public static ProductDetails Instance = null!;

        public ProductDetails(Product product)
        {
            InitializeComponent();

            Instance = this;

            Instance.DataContext = new ProductDetailsVM(product);
        }
    }
}
