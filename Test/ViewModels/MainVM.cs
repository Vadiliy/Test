using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public BaseModel ContentModel
        {
            get => mainModel.ContentModel;
            set
            {
                mainModel.ContentModel = value;
                OnPropertyChanged();
            }
        }



        public ObservableCollection<ObjectToView> FoldersAndFiles
        {
            get => mainModel.FoldersAndFiles;
            set
            {
                mainModel.FoldersAndFiles = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ItemDoubleClick => mainModel.ItemDoubleClick;   

        public RelayCommand SelectedItemChanged => mainModel.SelectedItemChanged;

        
    }
}

