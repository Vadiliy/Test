using System;
using System.IO;

namespace Test.Models
{
    public class ShortDirectoryInfo : BaseModel
    {
        public ShortDirectoryInfo()
        {

        }

        public ShortDirectoryInfo(DirectoryInfo info)
        {
            try
            {
                CountFiles = Directory.GetFiles(info.FullName).Length;
                CountFolders = Directory.GetDirectories(info.FullName).Length;
            }
            catch { return; }
            LastAccessTime = info.LastAccessTime;

            string[] files = Directory.GetFiles(info.FullName);
            foreach(string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                SizeAllFiles += fileInfo.Length;
            }
        }

        int countFolders;
        public int CountFolders
        {
            get => countFolders;
            set
            {
                countFolders = value;
                OnPropertyChanged();
            }
        }

        int countFiles;
        public int CountFiles
        {
            get => countFiles;
            set
            {
                countFiles = value;
                OnPropertyChanged();
            }
        }

        DateTime lastAccessTime;
        public DateTime LastAccessTime
        {
            get => lastAccessTime;
            set
            {
                lastAccessTime = value;
                OnPropertyChanged();
            }
        }

        long sizeAllFiles;
        public long SizeAllFiles
        {
            get => sizeAllFiles;
            set
            {
                sizeAllFiles = value;
                OnPropertyChanged();
            }
        }
    }
}
