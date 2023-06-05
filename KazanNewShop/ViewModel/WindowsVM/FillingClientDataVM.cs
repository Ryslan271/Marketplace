using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Services;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.PageVM;
using System.ComponentModel.DataAnnotations;

namespace KazanNewShop.ViewModel.WindowsVM
{
    public partial class FillingClientDataVM : ObservableValidator
    {
        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string? _name;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string? _surname;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string? _patronymic;

        [RelayCommand]
        private void Filled()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            AuthRegService.FillClientData(Name!, Surname!, Patronymic!);

            NavigationWindow.TransitionProductList(typeof(NavigationPageMarketplaceVM));
        }
    }
}
