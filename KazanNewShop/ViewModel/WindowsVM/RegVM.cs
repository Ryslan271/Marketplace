using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.DataTypes.Enums;
using KazanNewShop.Services;
using KazanNewShop.View.Base;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.Base;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace KazanNewShop.ViewModel.WindowsVM
{
    public partial class RegVM : WindowViewModelBase
    {
        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _login = null!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _password = null!;

        [Required(ErrorMessage = "Выберите один из вариантов")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private UserRole _role;

        /// <summary>
        /// Команда регистрации пользователя
        /// </summary>
        [RelayCommand]
        private void Register()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            if (AuthRegService.RegisterUser(Login!, Password!)! == true)
            {
                new NavigationWindow().Show();

                ValidatedRole();

                CloseWindow();
            }
            else
                MessageBox.Show("Такой пользователь уже есть в системе", "Ошибка", MessageBoxButton.OK);
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

        /// <summary>
        /// Открытие нужного окна на основании роли пользователя
        /// Role == UserRole.Client => FillingClientDataVM
        /// Role == UserRole.Selesman => FillingSalesmanDataVM
        /// </summary>
        private void ValidatedRole()
        {
            if (Role == UserRole.Client)
                NavigationWindow.Navigate(typeof(FillingClientDataVM), "Заполнение данных клиента");
            else if (Role == UserRole.Selesman)
                NavigationWindow.Navigate(typeof(FillingSalesmanDataVM), "Заполнение данных продавца");
        }
    }
}
