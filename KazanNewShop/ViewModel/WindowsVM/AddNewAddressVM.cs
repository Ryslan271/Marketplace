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
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _lat;

        [ObservableProperty]
        private string _lot;

        [RelayCommand]
        private void AddPickupPoint()
        {
            if (Lat == null || Lot == null || Name == null)
                return;

            AddNewAddress.PositioningChangeGmap(Convert.ToDouble(Lat!), Convert.ToDouble(Lot!));

            PointOfIssue pointOfIssue = new PointOfIssue()
            {
                Lat = Convert.ToDecimal(Lat!),
                Lot = Convert.ToDecimal(Lot!),
                Name = Name
            };

            GMap.NET.WindowsPresentation.GMapMarker gMapMarker = new(new PointLatLng(Convert.ToDouble(Lat!), Convert.ToDouble(Lot!)))
            {
                Shape = AddNewAddress.InitUIElement(pointOfIssue)
            };

            AddNewAddress.Instance.gMapControl1.Markers.Add(gMapMarker);

            DatabaseContext.Entities.SaveChanges();
            DatabaseContext.Entities.PointOfIssues.Load();

            NavigationWindow.CreatingDialogMessageBoxWithOk("Новая точка выдачи сохранена", "Уведомление");
        }
    }
}
