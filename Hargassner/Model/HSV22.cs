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
        const string HEIZUNG = "10.0.2.106";
        enum AnlagenStatus {gestoppt, starten, läuft, stoppen};
        AnlagenStatus JobStatus;
        public event EventHandler<string> NeueMeldung;
        Task task;
        SynchronizationContext context = null;
        public HSV22()
        {
            JobStatus = AnlagenStatus.gestoppt;
            task = new Task(() => TheJob());
            context = SynchronizationContext.Current;
        }
        ~HSV22()
        {
            Dispose(false);
        }
        public void Start()
        {
            task.Start();
        }
        public void Stop()
        {
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
            await tcpClient.ConnectAsync(ip, 4001);
            var stream = tcpClient.GetStream();
            using (var sr = new StreamReader(stream)) {
                while (JobStatus == AnlagenStatus.läuft)
                {
                    var line = await sr.ReadLineAsync();
                    context.Send((_) => NeueMeldung?.Invoke(this, line), null);
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
