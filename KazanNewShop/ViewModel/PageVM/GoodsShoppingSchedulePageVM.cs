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
    public partial class GoodsShoppingSchedulePageVM : ObservableValidator
    {
        private readonly Random _r = new();
        private static List<(string?, double?)> s_initialData = new();

        private string _diagramListSelectedItem;
        public string DiagramListSelectedItem 
        {
            get => _diagramListSelectedItem; 
            set 
            {
                _diagramListSelectedItem = value;
            } 
        }
       
        public List<string> DiagramList => _diagramList;
        private List<string> _diagramList 
            = new() 
            {
                "График",
                "Диграмма столбцы",
                "Диграмма круги"
            };

        [ObservableProperty]
        private ISeries[]? _series;

        [ObservableProperty]
        private Axis[] _xAxes = { new Axis { SeparatorsPaint = new SolidColorPaint(new SKColor(220, 220, 220)) } };

        [ObservableProperty]
        private Axis[] _yAxes = { new Axis { IsVisible = false } };

        public GoodsShoppingSchedulePageVM()
        {
            DiagramListSelectedItem = DiagramList.First();

            s_initialData.Clear();

            foreach (var salesman in DatabaseContext.Entities.Salesmen.Local)
                s_initialData.Add
                ((
                    salesman.NameCompany,
                    DatabaseContext.Entities.ProductListOrders.Local.Where(p => p.Product.Salesman == salesman).Count()
                ));

            FillSeries();
        }

        public void FillSeries()
        {
            _series =
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
            foreach (var item in Series)
            {
                if (item.Values is null) continue;

                var i = ((ObservableValue[])item.Values)[0];
                i.Value += _r.Next(0, 100);
            }

            Series = Series.OrderByDescending(x => ((ObservableValue[])x.Values!)[0].Value).ToArray();
        }

        /// <summary>
        /// Открытие окна создание нового пункта выдачи 
        /// </summary>
        [RelayCommand]
        public void CreatedPointOfAddress()
        {
            new AddNewAddress().ShowDialog();
        }

        /// <summary>
        /// Открытие списка всех заказов
        /// </summary>
        [RelayCommand]
        public void OpenOrdersList()
        {
            NavigationWindow.Navigate(typeof(ListOrderForEmployeePageVM));
        }

        /// <summary>
        /// Открытие списка всех товаров
        /// </summary>
        [RelayCommand]
        public void OpenNavigationEmployee()
        {
            NavigationWindow.Navigate(typeof(NavigationEmployeePageMarketplaceVM));
        }

    }
}