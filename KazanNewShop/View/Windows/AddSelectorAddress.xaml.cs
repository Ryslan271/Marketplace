using GMap.NET;
using KazanNewShop.Database;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Controls;
using KazanNewShop.ViewModel;
using KazanNewShop.Database.Models;
using System.Linq;
using System.Windows.Shapes;

namespace KazanNewShop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddSelector.xaml
    /// </summary>
    public partial class AddSelectorAddress : UiWindow
    {
        public AddSelectorAddress()
        {
            InitializeComponent();
        }

        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            gMapControl1.MinZoom = 10;
            gMapControl1.MaxZoom = 25;
            gMapControl1.Zoom = 12;
            gMapControl1.Position = new PointLatLng(55.786419471, 49.121932983);
            gMapControl1.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButton.Left;
            gMapControl1.ShowCenter = false;
            gMapControl1.ShowTileGridLines = false;

            foreach (PointOfIssue item in DatabaseContext.Entities.PointOfIssues.ToList())
            {
                GMap.NET.WindowsPresentation.GMapMarker gMapMarker = new(new PointLatLng((double)item.Lat!, (double)item.Lot!))
                {
                    Shape = InitUIElement(item)
                };

                gMapControl1.Markers.Add(gMapMarker);
            }
        }

        /// <summary>
        /// Рисовка маркера
        /// </summary>
        /// <param name="item">Текст</param>
        /// <returns>StackPanel</returns>
        private StackPanel InitUIElement(PointOfIssue item)
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
                       Command = ((AddSelectorAddressVM)DataContext).SelectorAddressPointCommand,
                       CommandParameter = item
                   }
               );

            return stackPanel;
        }
    }
}
