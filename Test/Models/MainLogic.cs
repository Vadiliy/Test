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
            SelectedDriver = LogicalDrivers[0];
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
                     if (parent.Childs.Count > 0)  return;
                     
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
                         ShowChilds.Execute(path);
                     
                     else
                     {
                         ProcessStart(path); // в разных случаях тут могут быть исключения                       
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

        RelayCommand startFile;
        public RelayCommand StartFile => startFile ??
                 (startFile = new RelayCommand(obj =>
                 {
                     string pathToFile = obj as string;
                     ProcessStart(pathToFile);
                 }));

        RelayCommand showFolder;
        public RelayCommand ShowFolder => showFolder ??
                 (showFolder = new RelayCommand(obj =>
                 {
                     string pathToFolder = obj as string;
                     try
                     {
                         Process.Start("explorer.exe", pathToFolder);
                     }
                     catch { }
                 }));

        RelayCommand showChilds;
        public RelayCommand ShowChilds => showChilds ??
                 (showChilds = new RelayCommand(obj =>
                 {
                     string pathToFolder = obj as string;
                     ObjectToView parent = FoldersAndFiles.First(x => x.Path == pathToFolder);
                     if(parent.Childs.Count > 0)
                     {
                         parent.IsExpanded = true;
                         return;
                     }
                     List<string> foldersAndFiles = Service.GetFoldersAndFilesFromPath(pathToFolder);
                     foldersAndFiles = Service.GetNamesFromPaths(foldersAndFiles);
                     foreach (string @object in foldersAndFiles)
                     {
                         ObjectToView item = new ObjectToView()
                         {
                             Name = @object,
                             Path = pathToFolder + "\\" + @object,
                             Image = Service.GetImageByPath(pathToFolder + "\\" + @object)
                         };
                         AddContextAction(item);
                         parent.Childs.Add(item);
                     }
                     parent.IsExpanded = true;
                 }));
        #endregion

        #region Methods
        void ProcessStart(string pathToFile)
        {
            try
            {
                Process.Start(pathToFile);
            }
            catch { }
        }

        void AddContextAction(ObjectToView @object)
        {
            try
            {
                if (Service.IsDirectory(@object.Path)) // тут может быть исключение если запрашиваем недоступную папку
                {
                    ContextAction showChild = new ContextAction
                    {
                        Header = "Развернуть дочерние элементы",
                        Action = ShowChilds,
                        Path = @object.Path
                    };
                    @object.Menu.Add(showChild);

                    ContextAction showFolder = new ContextAction
                    {
                        Header = "Открыть",
                        Action = ShowFolder,
                        Path = @object.Path
                    };
                    @object.Menu.Add(showFolder);
                }
                else
                {
                    ContextAction start = new ContextAction
                    {
                        Header = "Открыть",
                        Action = StartFile,                       
                        Path = @object.Path
                    };
                    @object.Menu.Add(start);
                }
            }
            catch { }
        }
        #endregion
    }
}
