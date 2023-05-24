using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Services;
using KazanNewShop.View.Windows;
using System.ComponentModel.DataAnnotations;

namespace KazanNewShop.ViewModel
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
    
            AuthRegService.FilledClientData(Name!, Surname!, Patronymic!);

            NavigationWindow.Navigate(typeof(NavigationPageMarketplaceVM));
        }
    }
}
