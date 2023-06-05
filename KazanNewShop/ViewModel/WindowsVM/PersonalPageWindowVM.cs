using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.Services;
using KazanNewShop.View.Windows;
using KazanNewShop.ViewModel.Base;
using KazanNewShop.ViewModel.PageVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace KazanNewShop.ViewModel.WindowsVM
{
    public partial class PersonalPageWindowVM : WindowViewModelBase
    {
        [ObservableProperty]
        private byte[] _photo
            = App.CurrentUser!.Client!.ProfilePhoto! == null ? CommonMethods.MainForProfileClientNullPhoto : App.CurrentUser.Client!.ProfilePhoto!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _name = App.CurrentUser.Client!.Name!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _surname = App.CurrentUser.Client!.Surname!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _patronymic = App.CurrentUser.Client!.Patronymic!;

        [ObservableProperty]
        private string? _numberOfCreditCard = App.CurrentUser.Client?.NumberOfCreditCard;

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

            App.CurrentUser!.Client!.Name = Name;
            App.CurrentUser!.Client!.Surname = Surname;
            App.CurrentUser!.Client!.Patronymic = Patronymic;
            App.CurrentUser!.Client!.NumberOfCreditCard = NumberOfCreditCard;
            App.CurrentUser!.Client!.ProfilePhoto = Photo;

            DatabaseContext.Entities.SaveChanges();

            NavigationPageMarketplaceVM.Instance.ClientPhoto = App.CurrentUser.Client.ProfilePhoto;

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
