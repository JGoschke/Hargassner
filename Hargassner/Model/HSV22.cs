using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Hargassner.Model
{
    class HSV22 : BindableBase, IDisposable
    {
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        const string HEIZUNG = "10.0.2.100";
        enum AnlagenStatus {gestoppt, starten, läuft, stoppen};
        AnlagenStatus JobStatus;
        public event EventHandler<string> NeueMeldung;

        readonly Task task;
        SynchronizationContext context = null;
        public HSV22()
        {
            JobStatus = AnlagenStatus.gestoppt;
            task = new Task(() => TheJob());
            context = SynchronizationContext.Current;
            logger.Trace("ctor");
        }
        ~HSV22()
        {
            Dispose(false);
            logger.Trace("Destructor");
        }
        public void Start()
        {
            task.Start();
            logger.Trace("Start");
        }
        public void Stop()
        {
            logger.Trace("Stop");
            if (JobStatus == AnlagenStatus.läuft)
            {
                JobStatus = AnlagenStatus.stoppen;
                task.Wait(3000);
                JobStatus = AnlagenStatus.gestoppt;
            }
        }
        async void TheJob()
        {
            JobStatus = AnlagenStatus.läuft;
            TcpClient tcpClient = new TcpClient();
            var ip = IPAddress.Parse(HEIZUNG);
            logger.Trace($"Connect {ip}");
            await tcpClient.ConnectAsync(ip, 4001);
            var stream = tcpClient.GetStream();
            using (var sr = new StreamReader(stream)) {
                while (JobStatus == AnlagenStatus.läuft)
                {
                    var neueZeileLesen = sr.ReadLineAsync();
                    await neueZeileLesen.ContinueWith((t) =>
                    {
                        var zeile = t.Result.Replace('.',',');
                        if (!string.IsNullOrEmpty(zeile))
                        {
                            context.Send((_) => NeueMeldung?.Invoke(this, zeile), null);
                            if (zeile.StartsWith("z"))
                            {
                                zeile = '"' + zeile + '"';
                            }
                            logger.Info(zeile);
                        }
                    });

                }
            }
            tcpClient.Close();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (JobStatus == AnlagenStatus.läuft)
                {
                    Stop();
                }
            }
        }
    }
}
