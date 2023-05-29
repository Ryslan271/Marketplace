using CommunityToolkit.Mvvm.ComponentModel;
using KazanNewShop.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KazanNewShop.ViewModel
{
    public partial class PersonalPageWindowVM : ObservableValidator
    {
        [ObservableProperty]
        private byte[] _photo = App.CarrentUser.Client!.ProfilePhoto! == null ? CommonMethods.MainForProfileClientNullPhoto : App.CarrentUser.Client!.ProfilePhoto!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _name = App.CarrentUser.Client!.Name!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _surname = App.CarrentUser.Client!.Surname!;

        [Required(ErrorMessage = "Заполните все поля")]
        [ObservableProperty]
        [NotifyDataErrorInfo]
        private string _patronymic = App.CarrentUser.Client!.Patronymic!;

        [ObservableProperty]
        private string? _numberOfCreditCard = App.CarrentUser.Client?.NumberOfCreditCard;
    }
}
