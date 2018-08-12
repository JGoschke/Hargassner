using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace Hargassner
{
    class Modul : IModule
    {
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        public Modul(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
            logger.Trace("ctor");
        }
        public void Initialize()
        {
            logger.Trace("init");
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(Views.Heizung));

        }
    }
}
