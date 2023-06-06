﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.DataTypes.Enums;
using KazanNewShop.Services;
using KazanNewShop.View.Base;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.Base;
using KazanNewShop.ViewModel.PageVM;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KazanNewShop.ViewModel.WindowsVM
{
    public partial class AuthVM : WindowViewModelBase
    {
        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string? _login = "zxc";

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string? _password = "zxc";

        [RelayCommand]
        private void Authorized()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            App.CurrentUser = AuthRegService.AuthorizeUser(Login!, Password!)!;

            if (App.CurrentUser == null) return;

            new NavigationWindow().Show();

            ValidateRoleUser();

            if (App.CurrentUser.Client is not null)
                NavigationWindow.TransitionProductList(typeof(NavigationPageMarketplaceVM));
            else if (App.CurrentUser.Employee is not null)
                NavigationWindow.TransitionProductList(typeof(NavigationEmployeePageMarketplaceVM));
            else
                NavigationWindow.TransitionProductList(typeof(NavigationSelecmanPageMarketplaceVM));

            CloseWindow();
        }

        /// <summary>
        /// Проверка на роль пользователя 
        /// </summary>
        private void ValidateRoleUser()
        {
            App.CurrentUser!.Client = DatabaseContext.Entities.Clients.Local.FirstOrDefault(c => c.User == App.CurrentUser);
            App.CurrentUser!.Salesman = DatabaseContext.Entities.Salesmen.Local.FirstOrDefault(c => c.User == App.CurrentUser);
            App.CurrentUser!.Employee = DatabaseContext.Entities.Employees.Local.FirstOrDefault(c => c.User == App.CurrentUser);
        }

        [RelayCommand]
        private void OpenRegWindow()
        {
            new RegistrationWindows().Show();
            CloseWindow();
        }
    }
}
