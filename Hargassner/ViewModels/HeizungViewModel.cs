using Hargassner.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hargassner.ViewModels
{
    public class HeizungViewModel : BindableBase
    {
        private string meldung;
        public string Meldung {
            get { return meldung; }
            set { SetProperty(ref meldung, value); }
        }
        HSV22 Model;
        private DelegateCommand delegateCommand;
        public DelegateCommand StartupCommand =>
            delegateCommand ?? (delegateCommand = new DelegateCommand(ExecuteStartupCommand));

        void ExecuteStartupCommand()
        {
            Model.Start();
        }
        private DelegateCommand shutdownCommand;
        public DelegateCommand Shutdown => shutdownCommand ?? (shutdownCommand = new DelegateCommand(ExecuteShutdown));

        void ExecuteShutdown()
        {
            if (Model != null)
            {
                Model.Dispose();
                Model = null;
            }
        }

        public HeizungViewModel()
        {
            Commands.StartupCommand.RegisterCommand(StartupCommand);
            Commands.ShutdownCommand.RegisterCommand(Shutdown);
            Model = new HSV22();
            Model.NeueMeldung += Model_NeueMeldung;
        }

        private void Model_NeueMeldung(object sender, string e)
        {
            Meldung = e;
        }
    }
}
