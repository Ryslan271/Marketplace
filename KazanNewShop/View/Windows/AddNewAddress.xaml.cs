using GMap.NET;
using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.ViewModel.WindowsVM;
using System;
using System.Data.Entity;
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
        /// Создание PointOfAddress
        /// </summary>
        public static void CreatingPointOfAddress(double Lat, double Lot, string Name)
        {
            PositioningChangeGmap(Convert.ToDouble(Lat!), Convert.ToDouble(Lot!));

            PointOfIssue pointOfIssue = new()
            {
                Lat = Convert.ToDecimal(Lat!),
                Lot = Convert.ToDecimal(Lot!),
                Name = Name
            };

            GMap.NET.WindowsPresentation.GMapMarker gMapMarker = new(new PointLatLng(Convert.ToDouble(Lat!), Convert.ToDouble(Lot!)))
            {
                Shape = InitUIElement(pointOfIssue)
            };

            Instance.gMapControl1.Markers.Add(gMapMarker);

            DatabaseContext.Entities.PointOfIssues.Local.Add(pointOfIssue);

            DatabaseContext.Entities.SaveChanges();
            DatabaseContext.Entities.PointOfIssues.Load();
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
                       CommandParameter = item
                   }
               );

            return stackPanel;
        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(gMapControl1);

            GMap.NET.WindowsPresentation.GMapControl gmap = (sender as GMap.NET.WindowsPresentation.GMapControl)!;

            if (gmap == null)
                return;
            
            // Получаем координаты места, на которое был произведен щелчок мыши
            PointLatLng point = gmap.FromLocalToLatLng((int)p.X, (int)p.Y);
            double lat = point.Lat;
            double lng = point.Lng;

            AddNewAddressVM.Instance.Lat = lat.ToString();
            AddNewAddressVM.Instance.Lot = lng.ToString();
        }
    }
}
