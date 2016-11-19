using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfChosenControl
{
    public class Node : INotifyPropertyChanged
    {

        public object DataModel;
        private bool _isSelected;
        #region ctor
        public Node(object dataModel)
        {
            DataModel = dataModel;
        }
        #endregion

        #region Properties

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }



    }

}
