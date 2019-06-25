using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace Test.Models
{
    public class MainLogic : BaseModel
    {
        public MainLogic()
        {
            LogicalDrivers = Environment.GetLogicalDrives().ToList();
        }

        private static string CurrentPath { get; set; }  // текущий каталог

        List<string> logicalDrivers = new List<string>();
        public List<string> LogicalDrivers
        {
            get => logicalDrivers;
            set
            {
                logicalDrivers = value;
                OnPropertyChanged();
            }
        }

        string selectedDriver;
        public string SelectedDriver
        {
            get => selectedDriver;
            set
            {
                selectedDriver = value;
                if (value == null)
                    return;

                CurrentPath = value;
                TreeItems.Clear();

                List<string> folders = Service.GetFolders(value);
                folders = Service.GetNamesFromPaths(folders);
                foreach(string folder in folders)
                {
                    TreeViewItem item = new TreeViewItem()
                    {
                        Header = folder,
                        Tag = value + folder
                    };
                    TreeItems.Add(item);
                }

                List<string> files = Service.GetFiles(value);
                files = Service.GetNamesFromPaths(files);
                foreach (string file in files)
                {
                    TreeViewItem item = new TreeViewItem()
                    {
                        Header = file,
                        Tag = value + file
                    };
                    TreeItems.Add(item);
                }
            }
        }


        ObservableCollection<TreeViewItem> treeItems = new ObservableCollection<TreeViewItem>();
        public ObservableCollection<TreeViewItem> TreeItems
        {
            get => treeItems;
            set
            {
                treeItems = value;
                OnPropertyChanged();
            }
        }


        ObservableCollection<ObjectToView> objects = new ObservableCollection<ObjectToView>();
        public ObservableCollection<ObjectToView> Objects
        {
            get => objects;
            set
            {
                objects = value;
                OnPropertyChanged();
            }
        }


        #region Commands
        RelayCommand selectedItemChanged;
        public RelayCommand SelectedItemChanged => selectedItemChanged ??
                 (selectedItemChanged = new RelayCommand(obj =>
                 {
                     TreeViewItem parent = obj as TreeViewItem;
                     string path = parent.Tag.ToString();
                     bool isDirectory = Service.IsDirectory(path);

                     if(isDirectory)
                     {
                         List<string> folders = Service.GetFolders(path);
                         folders = Service.GetNamesFromPaths(folders);
                         foreach(string folder in folders)
                         {
                             TreeViewItem newItem = new TreeViewItem()
                             {
                                 Header = folder,
                                 Tag = parent.Tag + "\\" + folder
                             };
                             parent.Items.Add(newItem);
                         }

                         List<string> files = Service.GetFiles(path);
                         files = Service.GetNamesFromPaths(files);
                         foreach (string file in files)
                         {
                             TreeViewItem newItem = new TreeViewItem()
                             {
                                 Header = file,
                                 Tag = parent.Tag + "\\" + file
                             };
                             parent.Items.Add(newItem);
                         }
                     }
                     else
                     {
                         Icon icon = Service.GetIconByPath(path);
                         Image = Service.ToImageSource(icon);
                     }
                 }));

        #endregion
    }
}
