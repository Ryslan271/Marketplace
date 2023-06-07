using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KazanNewShop.Database;
using KazanNewShop.View.Windows;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KazanNewShop.ViewModel.PageVM
{
    public partial class ProductShoppingSchedulePageVM : ObservableValidator
    {
        private readonly Random _r = new();
        private static List<(string?, double?)> s_initialData = new();


        [ObservableProperty]
        private ISeries[]? _seriesis;

        [ObservableProperty]
        private Axis[] _xAxes = { new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)) } };

        [ObservableProperty]
        private Axis[] _yAxes = { new Axis { IsVisible = false } };

        public ProductShoppingSchedulePageVM()
        {

            s_initialData.Clear();

            foreach (var product in DatabaseContext.Entities.ProductListOrders.Local.Where(p => p.Product.Salesman == App.CurrentUser!.Salesman).Select(p => p.Product))
                s_initialData.Add
                ((
                    product.Name,
                    DatabaseContext.Entities.ProductListOrders.Local.Where(p => p.Product == product).Count()
                ));

            FillSeries();
        }

        public void FillSeries()
        {
            _seriesis =
            s_initialData
                .Select(x => new RowSeries<ObservableValue>
                {
                    Values = new[] { new ObservableValue(x.Item2) },
                    Name = x.Item1,
                    Stroke = null,
                    MaxBarWidth = 25,
                    DataLabelsPaint = new SolidColorPaint(new SKColor(245, 245, 245)),
                    DataLabelsPosition = DataLabelsPosition.End,
                    DataLabelsTranslate = new LvcPoint(-1, 0),
                    DataLabelsFormatter = point => $"{point.Context.Series.Name} {point.PrimaryValue}"
                })
                .OrderByDescending(x => ((ObservableValue[])x.Values!)[0].Value)
                .ToArray();
        }

        public void RandomIncrement()
        {
            foreach (var item in Seriesis)
            {
                if (item.Values is null) continue;

                var i = ((ObservableValue[])item.Values)[0];
                i.Value += _r.Next(0, 100);
            }

            Seriesis = Seriesis.OrderByDescending(x => ((ObservableValue[])x.Values!)[0].Value).ToArray();
        }

        /// <summary>
        /// Открытие окна пользователя 
        /// </summary>
        [RelayCommand]
        private static void OpenPersonalPage() =>
            new PersonalSalesmanPageWindow().ShowDialog();

        /// <summary>
        /// Открытие списка всех заказов
        /// </summary>
        [RelayCommand]
        public void OpenOrdersList()
        {
            NavigationWindow.Navigate(typeof(ListOrderPageVM));
        }

        /// <summary>
        /// Открытие списка всех заказов
        /// </summary>
        [RelayCommand]
        public void OpenAllProduct()
        {
            NavigationWindow.Navigate(typeof(NavigationSelecmanPageMarketplaceVM));
        }
    }
}