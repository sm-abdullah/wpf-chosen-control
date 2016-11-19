using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfChosenControl
{
    /// <summary>
    /// Interaction logic for ChosenControl.xaml
    /// </summary>
    public partial class ChosenControl : UserControl
    {
       
        #region Constructor
        public ChosenControl()
        {
            InitializeComponent();
            DefaultMessage = "No Items Selected.";
            _nodeList = new ObservableCollection<Node>();
            _SelectedDataItems = new ObservableCollection<object>();
        }
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ChosenControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ChosenControl.OnItemsSourceChanged)));

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(IList), typeof(ChosenControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ChosenControl.OnSelectedItemsChanged)));

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ChosenControl), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty ValueMemberPathProperty = DependencyProperty.Register("ValueMemberPath", typeof(string), typeof(ChosenControl), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty DefaultMessageProperty = DependencyProperty.Register("DefaultMessage", typeof(string), typeof(ChosenControl), new UIPropertyMetadata(string.Empty));


        #endregion
       
        #region Dependency Events
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ChosenControl)d;
            control.DisplayInControl();
            control.SelectNodes();

        }
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ChosenControl)d;
            control._SelectedDataItems = new ObservableCollection<object>();
            control.rootGrid.DataContext = control._SelectedDataItems;
            if (control.ItemsSource != null)
            {
                control.SelectNodes();
            }
        }

        #endregion
       
        #region Properties 
        public string DefaultMessage
        {
            get { return (string)GetValue(DefaultMessageProperty); }
            set
            {
                SetValue(DefaultMessageProperty, value);
            }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set
            {

                SetValue(ItemsSourceProperty, value);
            }
        }
        public IList SelectedItems
        {
            get
            {
                return (IList)GetValue(SelectedItemsProperty);
            }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        public string DisplayMemberPath
        {
            get
            {
                return (string)GetValue(DisplayMemberPathProperty);
            }
            set
            {
                SetValue(DisplayMemberPathProperty, value);
            }
        }
        public string ValueMemberPath
        {
            get
            {
                return (string)GetValue(ValueMemberPathProperty);
            }
            set
            {
                SetValue(ValueMemberPathProperty, value);
            }
        }
        #endregion;

        #region Private Fields

        protected ObservableCollection<Node> _nodeList;
        private ObservableCollection<object> _SelectedDataItems;

        #endregion 

        #region Commands
        RelayCommand _RemoveCommand;
        public RelayCommand RemoveCommand

        {
            get
            {
                return _RemoveCommand
                 ?? (_RemoveCommand = new RelayCommand((x) => true,
                    (x) =>
                    {
                         SelectedItems.Remove(x);
                        _SelectedDataItems.Remove(x);
                        Node node = _nodeList.FirstOrDefault(i => GetValueByPropertyName(ValueMemberPath, i.DataModel).Equals(GetValueByPropertyName(ValueMemberPath, x)));

                        node.IsSelected = false;

                    }));
            }

        }
        private RelayCommand _RemoveAllCommand;
        public RelayCommand RemoveAllCommand
        {
            get
            {
                return _RemoveAllCommand
                 ?? (_RemoveAllCommand = new RelayCommand((x) => true,
                    (x) =>
                    {
                        ClearSelectedItems();
                    }));
            }
        }
        private RelayCommand _SelectAllCommand;
        public RelayCommand SelectAllCommand
        {
            get
            {
                return _SelectAllCommand
                 ?? (_SelectAllCommand = new RelayCommand((x) => true,
                    (x) =>
                    {
                        SelectAllItems();
                    }));
            }
        }
        #endregion

        #region Methods
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItems == null)
            {
                SelectedItems = new ObservableCollection<object>();
            }
            if (_SelectedDataItems == null)
            {
                _SelectedDataItems = new ObservableCollection<object>();
            }
            SelectedItems.Clear();
            _SelectedDataItems.Clear();
            foreach (Node node in _nodeList)
            {
                if (node.IsSelected)
                {
                    SelectedItems.Add(node.DataModel);
                    _SelectedDataItems.Add(node.DataModel);
                }
            }

        }
        /// <summary>
        /// It will grace fully Clear the Collection Reset all Checkboxes
        /// </summary>
        private void ClearSelectedItems()
        {
            if (SelectedItems != null)
            {
                SelectedItems.Clear();
            }
            if (_SelectedDataItems != null)
            {
                _SelectedDataItems.Clear();
            }
            foreach (var item in _nodeList)
            {
                item.IsSelected = false;
            }
        }
        private void SelectAllItems()
        {
            if (SelectedItems == null)
            {
                SelectedItems = new List<object>();
            }
            foreach (var item in _nodeList)
            {
                item.IsSelected = true;
                SelectedItems.Add(item.DataModel);
                _SelectedDataItems.Add(item.DataModel);
            }
        }
        /// <summary>
        /// This method will sync checkboxes with Selected Items
        /// </summary>
        private void SelectNodes()
        {
            try
            {
                foreach (var v in SelectedItems)
                {
                    _SelectedDataItems.Add(v);
                    try
                    {
                        //If value member path is Null then use default object equality
                        if (string.IsNullOrWhiteSpace(ValueMemberPath))
                        {
                            var node = _nodeList.FirstOrDefault(i => i.DataModel.Equals(v));
                            if (node != null)
                            {
                                node.IsSelected = true;
                            }
                        }
                        else
                        {
                            //else identify the object by ValueMemeberPath property
                            var node = _nodeList.FirstOrDefault(i => GetValueByPropertyName(ValueMemberPath, i.DataModel).Equals(GetValueByPropertyName(ValueMemberPath, v)));
                            if (node != null)
                            {
                                node.IsSelected = true;
                            }
                        }
                       
                    }
                    catch (Exception)
                    {
                      //Ignore the Exception 
                    }
                }
            }
            catch (Exception)
            {
                //Ignore the Exception
            }
        }

        private object GetValueByPropertyName(string name, object dataModel)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var type = dataModel.GetType().GetProperty(name);
                var ret = type.GetValue(dataModel, null);
                return ret;
            }
            throw new ArgumentNullException("Property Name");

        }
       
        private void DisplayInControl()
        {
            _nodeList.Clear();
            foreach (var item in this.ItemsSource)
            {
                _nodeList.Add(new Node(item));
            }
            MultiSelectCombo.ItemsSource = _nodeList;


        }


        #endregion
    }
}
