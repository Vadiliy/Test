using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Test.Models
{
    public class MainLogic : BaseModel
    {
        public MainLogic()
        {
            LogicalDrivers = Environment.GetLogicalDrives().ToList();
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

                FoldersAndFiles.Clear();

                List<string> foldersAndFiles = Service.GetFoldersAndFilesFromPath(value);
                foldersAndFiles = Service.GetNamesFromPaths(foldersAndFiles);
                foreach(string @object in foldersAndFiles)
                {
                    ObjectToView item = new ObjectToView()
                    {
                        Name = @object,
                        Path = value + @object,
                        Image = Service.GetImageByPath(value + @object)
                    };
                    AddContextAction(item);
                    FoldersAndFiles.Add(item);
                }
            }
        }

        BaseModel contentModel;
        public BaseModel ContentModel
        {
            get => contentModel;
            set
            {
                contentModel = value;
                OnPropertyChanged();
            }
        }

        #region Collections

        ObservableCollection<ObjectToView> foldersAndFiles = new ObservableCollection<ObjectToView>();
        public ObservableCollection<ObjectToView> FoldersAndFiles
        {
            get => foldersAndFiles;
            set
            {
                foldersAndFiles = value;
                OnPropertyChanged();
            }
        }

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

        #endregion

        #region Commands
        RelayCommand itemDoubleClick;
        public RelayCommand ItemDoubleClick => itemDoubleClick ??
                 (itemDoubleClick = new RelayCommand(obj =>
                 {
                     ObjectToView parent = obj as ObjectToView;
                     if (parent == null) return;
                     string path = parent.Path;
                     bool isDirectory;
                     try
                     {
                         isDirectory = Service.IsDirectory(path);
                     }
                     catch
                     {
                         return;
                     }

                     if(isDirectory)
                     {
                         List<string> foldersAndFiles = Service.GetFoldersAndFilesFromPath(path);
                         foldersAndFiles = Service.GetNamesFromPaths(foldersAndFiles);
                         foreach (string @object in foldersAndFiles)
                         {
                             ObjectToView item = new ObjectToView()
                             {
                                 Name = @object,
                                 Path = path + "\\" + @object,
                                 Image = Service.GetImageByPath(path + "\\" + @object)
                             };
                             parent.Childs.Add(item);
                         }
                     }
                     else
                     {
                         try
                         {
                             Process.Start(parent.Path); // в разных случаях тут могут быть исключения
                         }
                         catch { }
                     }
                 }));

        RelayCommand selectedItemChanged;
        public RelayCommand SelectedItemChanged => selectedItemChanged ??
                 (selectedItemChanged = new RelayCommand(obj =>
                 {
                     ObjectToView parent = obj as ObjectToView;
                     if (parent == null) return;
                     string path = parent.Path;
                     bool isDirectory;
                     try
                     {
                         isDirectory = Service.IsDirectory(path);
                     }
                     catch
                     {
                         return;
                     }

                     if (isDirectory)
                     {
                         DirectoryInfo dirInfo = new DirectoryInfo(path);
                         ContentModel = new ShortDirectoryInfo(dirInfo);                        
                     }
                     else
                     {
                         ContentModel = Service.GetFileInfoModel(path);
                     }
                 }));

        #endregion

        void AddContextAction(ObjectToView @object)
        {
            try
            {
                if (Service.IsDirectory(@object.Path)) // тут может быть исключение если запрашиваем недоступную папку
                {
                    ContextAction showChild = new ContextAction
                    {
                        Header = "Развернуть дочерние элементы",
                        Path = @object.Path
                    };
                    @object.Menu.Add(showChild);

                    ContextAction showFolder = new ContextAction
                    {
                        Header = "Открыть",
                        Path = @object.Path
                    };
                    @object.Menu.Add(showFolder);
                }
                else
                {
                    ContextAction start = new ContextAction
                    {
                        Header = "Открыть",
                        Path = @object.Path
                    };
                    @object.Menu.Add(start);
                }
            }
            catch { }
        }
    }
}
