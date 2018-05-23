using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace Hargassner
{
    public static class Commands
    {
        public static readonly CompositeCommand ShutdownCommand = new CompositeCommand();
        public static readonly CompositeCommand StartupCommand = new CompositeCommand();
    }
}
