using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace KazanNewShop.Converters
{
    class CollectionItemsCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable? val = value as IEnumerable;
            if (val == null)
                return value;

            int take = 2;
            if (parameter != null)
                int.TryParse(parameter as string, out take);


            if (take < 1)
                return value;
            var list = new List<object>();

            int count = 0;
            foreach (var li in val)
            {
                count++;
                if (count > take)
                    break;
                list.Add(li);
            }
            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
