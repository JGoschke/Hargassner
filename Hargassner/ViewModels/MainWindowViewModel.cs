using Prism.Mvvm;

namespace Hargassner.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Hargassner";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string meldung;
        public string Meldung {
            get { return meldung; }
            set { SetProperty(ref meldung, value); }
        }
        public MainWindowViewModel()
        {

        }
    }
}
