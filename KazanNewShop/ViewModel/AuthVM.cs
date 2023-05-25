using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.Services;
using KazanNewShop.View.Base;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KazanNewShop.ViewModel
{
    public partial class AuthVM : WindowViewModelBase
    {
        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string? _login = "asd";

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string? _password = "asd";

        [RelayCommand]
        private void Authorized()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            App.CarrentUser = AuthRegService.AuthorizeUser(Login!, Password!)!;

            if (App.CarrentUser == null) return;
            
            new NavigationWindow().Show();

            CloseWindow();
        }

        [RelayCommand]
        private void OpenRegWindow()
        {
            new RegistrationWindows().Show();
            CloseWindow();
        }
    }
}
