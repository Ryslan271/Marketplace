using KazanNewShop.Database.Models;
using KazanNewShop.ViewModel.WindowsVM;
using Wpf.Ui.Controls;

namespace KazanNewShop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrderProductList.xaml
    /// </summary>
    public partial class OrderProductList : UiWindow
    {
        public static OrderProductList Instance = null!;

        public OrderProductList(Order order)
        {
            InitializeComponent();
            Instance = this;

            Instance.DataContext = new OrderProductListVM(order);
        }
    }
}
