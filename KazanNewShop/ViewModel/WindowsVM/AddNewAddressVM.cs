using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GMap.NET;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.View.Windows;
using Microsoft.EntityFrameworkCore;
using System;

namespace KazanNewShop.ViewModel.WindowsVM
{
    public partial class AddNewAddressVM : ObservableValidator
    {
        public static AddNewAddressVM Instance { get; private set;}

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _lat;

        [ObservableProperty]
        private string _lot;

        public AddNewAddressVM()
        {
            Instance = this;
        }

        [RelayCommand]
        private void AddPickupPoint()
        {
            if (Lat == null || Lot == null || Name == null)
                return;

            AddNewAddress.CreatingPointOfAddress(Convert.ToDouble(Lat), Convert.ToDouble(Lot), Name);

            NavigationWindow.CreatingDialogMessageBoxWithOk("Новая точка выдачи сохранена", "Уведомление");
        }
    }
}
