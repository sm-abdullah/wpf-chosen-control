using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WpfChoosenControlDemo
{
    public class MainModel : ViewModelBase
    {
        private ObservableCollection<Student> _items;
        private List<Student> _selectedItems;


        public ObservableCollection<Student> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                NotifyPropertyChanged("Items");
            }
        }

        public List<Student> Students
        {
            get
            {
                return _selectedItems;
            }
            set
            {
                _selectedItems = value;
                NotifyPropertyChanged("SelectedItems");
            }
        }




        public MainModel()
        {
            var items = new ObservableCollection<Student>();
            items.Add(new Student() { Id = 1, Name = "Ali" });
            items.Add(new Student() { Id = 2, Name = "Ammad" });
            items.Add(new Student() { Id = 3, Name = "Waseem" });
            items.Add(new Student() { Id = 4, Name = "Hakan" });
            items.Add(new Student() { Id = 5, Name = "Asim" });
            items.Add(new Student() { Id = 6, Name = "Nouman" });
            items.Add(new Student() { Id = 7, Name = "Kashif" });
            items.Add(new Student() { Id = 8, Name = "Raees" });
            Items = items;
            var students = new List<Student>();
            students.Add(Items.First());
            students.Add(Items.LastOrDefault());

            this.Students = students;
        }

        private void Submit()
        {

        }


    }


    public class Student : ViewModelBase
    {

        private int _Id;
        public int Id
        {
            get { return _Id; }

            set
            {
                _Id = value;
                NotifyPropertyChanged("Id");
            }
        }
        private string _Name;
        public string Name
        {
            get { return _Name; }

            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }

    }
}

