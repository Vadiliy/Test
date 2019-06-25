using System;
using System.IO;

namespace Test.Models
{
    public class ShortFileInfo : BaseModel
    {
        public ShortFileInfo()
        {

        }

        public ShortFileInfo(FileInfo info)
        {
            Size = info.Length;
            LastAccessTime = info.LastAccessTime;
            Name = GetName(info.Name);
            Format = GetFormat(info.Name);
        }

        #region Service Methods
        protected string GetFormat(string fileName)
        {
            char dot = '.';
            int index = fileName.LastIndexOf(dot);
            if (index == -1) return "";
            return fileName.Remove(0, index + 1);
        }

        protected string GetName(string fileName)
        {
            char dot = '.';
            int index = fileName.LastIndexOf(dot);
            if (index == -1) return fileName;
            return fileName.Remove(index, fileName.Length - index);
        }

        #endregion
        #region Protected properties
        protected long size;
        protected string format;
        protected DateTime lastAccessTime;
        protected string name;
        #endregion


        public long Size
        {
            get => size;
            set
            {
                size = value;
                OnPropertyChanged();
            }
        }
       
        public string Format
        {
            get => format;
            set
            {
                format = value;
                OnPropertyChanged();
            }
        }
        
        public DateTime LastAccessTime
        {
            get => lastAccessTime;
            set
            {
                lastAccessTime = value;
                OnPropertyChanged();
            }
        }
      
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
    }
}
