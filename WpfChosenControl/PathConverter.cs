using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WpfChosenControl
{
    public class PathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //if passed element Is Node.
            Node node = values[1] as Node;
            if (node != null)
            {
                try
                {
                    var type = node.DataModel.GetType().GetProperty(values[0].ToString());
                    var ret = type.GetValue(node.DataModel, null);
                    return ret;
                }
                catch (Exception exception)
                {
                    return node.DataModel.GetType().ToString();
                }
            }
            else
            {
                try
                {
                    //if Passed Element is Data Model itself
                    var type = values[1].GetType().GetProperty(values[0].ToString());
                    var ret = type.GetValue(values[1], null);
                    return ret;
                }
                catch (Exception exception)
                {
                  return  values[1].GetType().ToString();
                }
            }
        }



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
