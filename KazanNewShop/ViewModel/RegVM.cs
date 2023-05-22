using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.DataTypes.Enums;
using KazanNewShop.Services;
using KazanNewShop.View.Base;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.Base;
using System.ComponentModel.DataAnnotations;

namespace KazanNewShop.ViewModel
{
    public partial class RegVM : WindowViewModelBase
    {
        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string? _login;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string? _password;

        [Required(ErrorMessage = "Выберите один из вариантов")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private UserRole _role;

        [RelayCommand]
        private void Register()
        {
            if (AuthRegService.RegisterUser(Login!, Password!)! == true)
            {
                new NavigationWindow().Show();
                
                if (Role == UserRole.Client)
                    NavigationWindow.Navigate(typeof(FillingClientDataVM), "Заполнение данных клиента");
                else if (Role == UserRole.Selesman)
                    NavigationWindow.Navigate(typeof(FillingSalesmanDataVM), "Заполнение данных продавца");

                CloseWindow();
            }
            else
                System.Windows.MessageBox.Show("Такой пользователь уже есть в системе", "Ошибка", System.Windows.MessageBoxButton.OK);
        }

        [RelayCommand]
        private void OpenAuthoWindow()
        {
            new АuthorizationWindows().Show();
            CloseWindow();
        }

        [RelayCommand]
        private void SelectRole(UserRole userRole) =>
            Role = userRole;
    }
}
