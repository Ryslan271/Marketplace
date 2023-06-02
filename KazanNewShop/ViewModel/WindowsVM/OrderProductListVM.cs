using CommunityToolkit.Mvvm.ComponentModel;
using KazanNewShop.Database.Models;
using System.Collections.Generic;
using System.Linq;

namespace KazanNewShop.ViewModel.WindowsVM
{
    public partial class OrderProductListVM : ObservableValidator
    {
        public Order CurrentOrder = null!;

        public IEnumerable<Product> Products { get; }

        public OrderProductListVM(Order order)
        {
            CurrentOrder = order;

            Products = CurrentOrder.ProductListOrders.Select(p => p.Product);
        }
    }
}
