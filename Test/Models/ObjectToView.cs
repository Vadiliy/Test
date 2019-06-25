using System.Collections.ObjectModel;
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
                path = value;
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

        ObservableCollection<ObjectToView> childs = new ObservableCollection<ObjectToView>();
        public ObservableCollection<ObjectToView> Childs
        {
            get => childs;
            set
            {
                childs = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ContextAction> Menu { get; set; } = new ObservableCollection<ContextAction>();

        bool isExpanded;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = true;
                OnPropertyChanged();
            }
        }
    }
}
