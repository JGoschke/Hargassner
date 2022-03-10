using System;

namespace Hargassner2
{
    class Program
    {
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            logger.Info("Guten Tach");
            logger.Trace("Guten Trace");
            Console.WriteLine("nach'm logging");
            System.IO.File.Create("/app/log/test.xxx");
        }
    }
}
