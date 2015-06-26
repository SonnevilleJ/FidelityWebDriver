using System;
using Ninject;
using Sonneville.FidelityWebDriver.Demo.Ninject;

namespace Sonneville.FidelityWebDriver.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new KernelBuilder().Build().Get<IApp>().Run(args);

            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }
    }
}
