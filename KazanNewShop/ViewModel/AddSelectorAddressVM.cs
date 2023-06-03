using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database.Models;
using KazanNewShop.Services;
using KazanNewShop.View.Windows;

namespace KazanNewShop.ViewModel
{
    public partial class AddSelectorAddressVM : ObservableValidator
    {
        [RelayCommand]
        private void SelectorAddressPoint(PointOfIssue item)
        {
            
        }
    }
}
