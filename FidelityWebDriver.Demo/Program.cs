using System;
using Ninject;
using Sonneville.FidelityWebDriver.Demo.Ninject;

namespace Sonneville.FidelityWebDriver.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var app = new KernelBuilder().Build().Get<IApp>())
            {
                app.Run(args);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
