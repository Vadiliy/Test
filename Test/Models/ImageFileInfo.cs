using System.IO;

namespace Test.Models
{
    public class ImageFileInfo : ShortFileInfo
    {
        public ImageFileInfo()
        {

        }

        public ImageFileInfo(FileInfo info)
        {
            Size = info.Length;
            LastAccessTime = info.LastAccessTime;
            Name = GetName(info.Name);
            Format = GetFormat(info.Name);
            ImagePath = info.FullName;
        }

        string imagePath;
        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged();
            }
        }
    }
}
