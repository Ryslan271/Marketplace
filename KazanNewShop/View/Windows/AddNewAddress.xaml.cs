using GMap.NET;
using KazanNewShop.Database.Models;
using KazanNewShop.ViewModel.WindowsVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace KazanNewShop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddNewAddress.xaml
    /// </summary>
    public partial class AddNewAddress : UiWindow
    {
        public static AddNewAddress Instance { get; private set; } = null!;

        public AddNewAddress()
        {
            InitializeComponent();

            Instance = this;

            Instance.DataContext = new AddNewAddressVM();
        }

        /// <summary>
        /// Отрисовка карты при загрузки
        /// </summary>
        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            gMapControl1.MinZoom = 10;
            gMapControl1.MaxZoom = 25;
            gMapControl1.Zoom = 12;
            gMapControl1.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButton.Left;
            gMapControl1.ShowCenter = false;
            gMapControl1.ShowTileGridLines = false;

            PositioningChangeGmap(55.7892148521873, 49.1186121009394);
        }

        /// <summary>
        /// Позиционирование карты
        /// </summary>
        public static void PositioningChangeGmap(double lat, double lot)
        {
            Instance.gMapControl1.Position = new PointLatLng(lat, lot);
        }

        /// <summary>
        /// Рисовка маркера
        /// </summary>
        /// <param name="item">Текст</param>
        /// <returns>StackPanel</returns>
        public static StackPanel InitUIElement(PointOfIssue item)
        {
            Border border = new()
            {
                Style = (Style)Application.Current.FindResource("PointInMap")
            };

            TextBlock textBlock = new()
            {
                Text = item.Name,
                FontSize = 14,
                Foreground = Brushes.Blue,
                FontWeight = FontWeights.Bold,
            };

            StackPanel stackPanel = new();
            stackPanel.Children.Add(border);
            stackPanel.Children.Add(textBlock);

            border.InputBindings.Add
               (
                   new MouseBinding()
                   {
                       Gesture = new MouseGesture()
                       {
                           MouseAction = MouseAction.LeftClick
                       },
                       Command = ((AddSelectorAddressVM)Instance.DataContext).SelectorAddressPointCommand,
                       CommandParameter = item
                   }
               );

            return stackPanel;
        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            double X = mapexplr.FromLocalToLatLng(e.X, e.Y).Lng;
            double Y = mapexplr.FromLocalToLatLng(e.X, e.Y).Lat;
        }
    }
}
