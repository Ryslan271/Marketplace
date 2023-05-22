using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace KazanNewShop.ViewModel.Base
{
    public partial class WindowViewModelBase : ObservableValidator
    {
        [ObservableProperty]
        private string? _title;

        public Action? CloseWindowMethod;
        public void CloseWindow() => CloseWindowMethod?.Invoke();
        public virtual bool OnClosing() => true;
    }
}
