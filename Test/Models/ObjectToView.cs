using System;
using System.Windows.Media;

namespace Test.Models
{
    public class ObjectToView : BaseModel
    {
        ImageSource image;
        public ImageSource Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        string path;
        public string Path
        {
            get => path;
            set
            {
                path = ValueTuple;
                OnPropertyChanged();
            }
        }

        string name;
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
