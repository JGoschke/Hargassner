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
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        public Modul(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }
        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(Views.Heizung));
        }
    }
}
