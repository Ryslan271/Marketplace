using KazanNewShop.ViewModel.Base;
using Wpf.Ui.Controls;

namespace KazanNewShop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для АuthorizationWindows.xaml
    /// </summary>
    public partial class АuthorizationWindows : UiWindow
    {
        public АuthorizationWindows()
        {
            InitializeComponent();

            ((WindowViewModelBase)DataContext).CloseWindowMethod = Close;
        }
    }
}
