using System;
using Ninject;
using Sonneville.FidelityWebDriver.Demo.Ninject;

namespace Sonneville.FidelityWebDriver.Demo
{
    public class Program
    {
        public static IKernel Kernel { get; }

        static Program()
        {
            Kernel = new KernelBuilder().Build();
        }

        public static void Main(string[] args)
        {
            using (var app = Kernel.Get<IApp>())
            {
                app.Run(args);
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }
    }
}
