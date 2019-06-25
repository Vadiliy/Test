using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using Test.Models;

namespace Test.ViewModels
{
    public class MainVM : BaseModel
    {
        MainLogic mainModel;
        public MainVM()
        {
            mainModel = new MainLogic();
            mainModel.PropertyChanged += (s, e) => { OnPropertyChanged(e.PropertyName); };
        }


        public List<string> LogicalDrivers
        {
            get => mainModel.LogicalDrivers;
        }

        public string SelectedDriver
        {
            set
            {
                mainModel.SelectedDriver = value;
            }
        }

        public ObservableCollection<TreeViewItem> TreeItems
        {
            get => mainModel.TreeItems;
            set
            {
                mainModel.TreeItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ObjectToView> Objects
        {
            get => mainModel.TreeItems;
            set
            {
                mainModel.TreeItems = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SelectedItemChanged
        {
            get => mainModel.SelectedItemChanged;           
        }

        public ImageSource Image
        {
            get => mainModel.Image;
            set
            {
                mainModel.Image = value;
                OnPropertyChanged();
            }
        }
    }
}
