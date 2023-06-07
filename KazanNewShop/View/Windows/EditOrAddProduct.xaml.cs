using KazanNewShop.Database.Models;
using KazanNewShop.ViewModel.WindowsVM;
using Wpf.Ui.Controls;

namespace KazanNewShop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditOrAddProduct.xaml
    /// </summary>
    public partial class EditOrAddProduct : UiWindow
    {
        public static EditOrAddProduct Instance = null!;
        public EditOrAddProduct(Product product, string title)
        {
            InitializeComponent();

            Instance = this;

            Instance.DataContext = new EditOrAddProductVM(product, title);
        }
    }
}
