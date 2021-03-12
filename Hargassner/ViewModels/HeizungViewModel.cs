using Hargassner.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Hargassner.ViewModels
{
    public class HeizungViewModel : BindableBase
    {
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private string meldung;
        public string Meldung {
            get { return meldung; }
            set { SetProperty(ref meldung, value); }
        }
        private string kesselzustand;
        public string Kesselzustand
        {
            get { return kesselzustand; }
            set { SetProperty(ref kesselzustand, value); }
        }
        private double istCO2;
        public double IstCO2
        {
            get { return istCO2; }
            set { SetProperty(ref istCO2, value); }
        }
        private int temparaturKessel;
        public int TemparaturKessel
        {
            get { return temparaturKessel; }
            set { SetProperty(ref temparaturKessel, value); }
        }
        private int temparaturAbgas;
        public int TemparaturAbgas
        {
            get { return temparaturAbgas; }
            set { SetProperty(ref temparaturAbgas, value); }
        }
        private double temparaturAussen;
        public double TemparaturAussen
        {
            get { return temparaturAussen; }
            set { SetProperty(ref temparaturAussen, value); }
        }
        private double temparaturAussenMW;
        public double TemparaturAussenMW
        {
            get { return temparaturAussenMW; }
            set { SetProperty(ref temparaturAussenMW, value); }
        }
        private double temparaturVorlaufIst;
        public double TemparaturVorlaufIst
        {
            get { return temparaturVorlaufIst; }
            set { SetProperty(ref temparaturVorlaufIst, value); }
        }
        private double temparaturVorlaufSoll;
        public double TemparaturVorlaufSoll
        {
            get { return temparaturVorlaufSoll; }
            set { SetProperty(ref temparaturVorlaufSoll, value); }
        }
        private int temparaturBoilerIst;
        public int TemparaturBoilerIst
        {
            get { return temparaturBoilerIst; }
            set { SetProperty(ref temparaturBoilerIst, value); }
        }
        private int temparaturPufferIst;
        public int TemparaturPufferIst
        {
            get { return temparaturPufferIst; }
            set { SetProperty(ref temparaturPufferIst, value); }
        }
        HSV22 Model;
        KesselzustandMap ZustandMap;

        private DelegateCommand delegateCommand;
        public DelegateCommand StartupCommand =>
            delegateCommand ?? (delegateCommand = new DelegateCommand(ExecuteStartupCommand));

        void ExecuteStartupCommand()
        {
            Model.Start();
            PropertyChanged += HeizungViewModel_PropertyChanged;
        }

        private async void HeizungViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Meldung))
            {
                if (Meldung.StartsWith("pm"))
                {
                    var werte = Meldung.Split(' ');
                    int zustand = int.Parse(werte[1]);
                    if (ZustandMap.ContainsKey(zustand))
                    {
                        Kesselzustand = ZustandMap[zustand];
                    }
                    else
                    {
                        Kesselzustand = werte[1];
                    }
                    IstCO2 = double.Parse(werte[2]);
                    TemparaturKessel = int.Parse(werte[3]);
                    TemparaturAbgas = int.Parse(werte[4]);
                    TemparaturAussen = double.Parse(werte[5]);
                    TemparaturAussenMW = double.Parse(werte[6]);
                    TemparaturVorlaufIst = double.Parse(werte[7]);
                    TemparaturVorlaufSoll = double.Parse(werte[9]);
                    TemparaturBoilerIst = int.Parse(werte[11]);
                    TemparaturPufferIst = int.Parse(werte[12]);
                }
                else if (Meldung.StartsWith("z"))
                {
                    Kesselzustand = Meldung;
                }
            }
            else if (e.PropertyName == nameof(Kesselzustand))
            {
                logger.Trace($"{e.PropertyName} geändert");
                using (HttpClient Homematic= new HttpClient())
                {
                   var _ = await Homematic.GetStringAsync($"http://192.168.1.102:8181/e.exe?wert=dom.GetObject(\"Hzg Kesselzustand\").State(\"{Kesselzustand}\")");
                }
            }
            else if (e.PropertyName == nameof(TemparaturKessel))
            {
                logger.Trace($"{e.PropertyName} geändert");
                using (HttpClient Homematic = new HttpClient())
                {
                    var _ = await Homematic.GetStringAsync($"http://192.168.1.102:8181/e.exe?wert=dom.GetObject(\"Hzg Temperatur Kessel\").State(\"{temparaturKessel}\")");
                }
            }
            else if (e.PropertyName == nameof(TemparaturAbgas))
            {
                logger.Trace($"{e.PropertyName} geändert");
                using (HttpClient Homematic = new HttpClient())
                {
                    var _ = await Homematic.GetStringAsync($"http://192.168.1.102:8181/e.exe?wert=dom.GetObject(\"Hzg Temperatur Abgas\").State(\"{temparaturAbgas}\")");
                }
            }
            else if (e.PropertyName == nameof(TemparaturBoilerIst))
            {
                logger.Trace($"{e.PropertyName} geändert");
                using (HttpClient Homematic = new HttpClient())
                {
                    var _ = await Homematic.GetStringAsync($"http://192.168.1.102:8181/e.exe?wert=dom.GetObject(\"Hzg Temperatur Boiler\").State(\"{temparaturBoilerIst}\")");
                }
            }
            else if (e.PropertyName == nameof(TemparaturPufferIst))
            {
                logger.Trace($"{e.PropertyName} geändert");
                using (HttpClient Homematic = new HttpClient())
                {
                    var _ = await Homematic.GetStringAsync($"http://192.168.1.102:8181/e.exe?wert=dom.GetObject(\"Hzg Temperatur Puffer\").State(\"{temparaturPufferIst}\")");
                }
            }

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
            ZustandMap = new KesselzustandMap();
            Model.NeueMeldung += Model_NeueMeldung;
        }
        private void Model_NeueMeldung(object sender, string e)
        {
            Meldung = e;
        }
    }
}
