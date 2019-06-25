namespace Test.Models
{
    public class ContextAction : BaseModel
    {
        public string Header { get; set; }

        public RelayCommand Action { get; set; }

        public string Path { get; set; }
    }
}
