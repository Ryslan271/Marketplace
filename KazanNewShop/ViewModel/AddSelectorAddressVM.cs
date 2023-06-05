using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KazanNewShop.ViewModel
{
    public partial class AddSelectorAddressVM : ObservableValidator
    {
        // заказ клиента
        [ObservableProperty]
        private Order? _currentOrder;

        // список продуктов связанных с заказом продуктов
        [ObservableProperty]
        private List<ProductListOrder>? _localListProductListOrder;

        // выбранная точка выдачи
        private PointOfIssue? _selectedItem;
        public PointOfIssue? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                    SelectorAddressPointInComboBox(_selectedItem!);

                OnPropertyChanged();
            }
        }

        // список всех точек выдачи
        [ObservableProperty]
        private IEnumerable<PointOfIssue> _pointOfIssues = DatabaseContext.Entities.PointOfIssues.ToList();

        public AddSelectorAddressVM(Order currentOrder, List<ProductListOrder> localListProductListOrder)
        {
            CurrentOrder = currentOrder;
            LocalListProductListOrder = localListProductListOrder;

            foreach (var item in LocalListProductListOrder)
                CurrentOrder.ProductListOrders.Add(item);
        }

        private void SelectorAddressPointInComboBox(PointOfIssue item) =>
            AddSelectorAddress.Instance.PositioningChangeGmap(Convert.ToDouble(item.Lat), Convert.ToDouble(item.Lot));

        [RelayCommand]
        private void SelectorAddressPoint(PointOfIssue item) 
        {
            AddSelectorAddress.Instance.PositioningChangeGmap(Convert.ToDouble(item.Lat), Convert.ToDouble(item.Lot));

            SelectedItem = item;
        }

        [RelayCommand]
        private void CreatingOrder()
        {   
            if (SelectedItem != null)
            {
                CurrentOrder!.PointOfIssue = SelectedItem!;
                CurrentOrder!.IdStatus = 1;
            }
            else
            {
                NavigationWindow.CreatingDialogMessageBoxWithOk("Проверьте введенные данные", "Ошибка");

                return;
            }

            DatabaseContext.Entities.Orders.Local.Add(CurrentOrder!);

            foreach (var item in LocalListProductListOrder!)
                DatabaseContext.Entities.ProductListOrders.Local.Add(item);

            DatabaseContext.Entities.SaveChanges();

            NavigationWindow.CreatingDialogMessageBoxWithOk("Данные успешно сохранены", "Успешно");

            AddSelectorAddress.Instance.Close();
        }

        [RelayCommand]
        private void CancelOrder()
        {
            AddSelectorAddress.Instance.Close();
        }
    }
}
