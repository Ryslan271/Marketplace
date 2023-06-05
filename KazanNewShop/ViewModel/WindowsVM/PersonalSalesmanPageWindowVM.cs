using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Services;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.Base;
using KazanNewShop.ViewModel.PageVM;
using System;
using System.ComponentModel.DataAnnotations;

namespace KazanNewShop.ViewModel.WindowsVM
{
    public partial class PersonalSalesmanPageWindowVM : WindowViewModelBase
    {
        [ObservableProperty]
        private byte[] _photo
            = App.CurrentUser!.Salesman!.ProfilePhoto! == null ? CommonMethods.MainForProfileClientNullPhoto : App.CurrentUser.Salesman!.ProfilePhoto!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _nameCompany = App.CurrentUser.Salesman!.NameCompany!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _description = App.CurrentUser.Salesman!.Description!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private DateTime? _dateOnMarketplace = App.CurrentUser.Salesman!.DateOnMarketplace;

        /// <summary>
        /// Изменение картинки пользователя
        /// </summary>
        [RelayCommand]
        public void EditUserPhoto()
        {
            byte[]? image = CommonMethods.ConvertImage();

            Photo = image != null ? image! : CommonMethods.MainForProfileClientNullPhoto!;
        }

        /// <summary>
        /// Сохранение измененний
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanSaveChanges))]
        public void SaveChanges()
        {
            ValidateAllProperties();

            if (HasErrors)
                return;

            App.CurrentUser!.Salesman!.NameCompany = NameCompany;
            App.CurrentUser!.Salesman!.Description = Description;
            App.CurrentUser!.Salesman!.DateOnMarketplace = DateOnMarketplace;
            App.CurrentUser!.Salesman!.ProfilePhoto = Photo;

            DatabaseContext.Entities.SaveChanges();

            NavigationPageMarketplaceVM.Instance.ClientPhoto = App.CurrentUser.Salesman!.ProfilePhoto!;

            CloseWindow();
        }
        private bool CanSaveChanges() => HasErrors == false;

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        [RelayCommand]
        public void Logout()
        {
            App.CurrentUser = null;

            new АuthorizationWindows().Show();

            NavigationWindow.Instance.Close();

            CloseWindow();
        }
    }
}
