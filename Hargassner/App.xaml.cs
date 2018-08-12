using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Hargassner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        protected override void OnStartup(StartupEventArgs e)
        {
            logger.Trace("OnStartup 1");
            base.OnStartup(e);
            logger.Trace("OnStartup 2");

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
