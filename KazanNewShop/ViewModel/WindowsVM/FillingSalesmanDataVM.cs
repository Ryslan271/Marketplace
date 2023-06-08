using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database.Models;
using KazanNewShop.Services;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.PageVM;
using System;
using System.ComponentModel.DataAnnotations;

namespace KazanNewShop.ViewModel.WindowsVM
{
    public partial class FillingSalesmanDataVM : ObservableValidator
    {
        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _companyName = null!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _description = null!;

        [RelayCommand]
        private void Filled()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            AuthRegService.FillSalesmanData(Description!, CompanyName!);

            NavigationWindow.TransitionProductList(typeof(NavigationSelecmanPageMarketplaceVM));
        }
    }
}
