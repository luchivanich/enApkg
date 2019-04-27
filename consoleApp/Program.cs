using Autofac;
using CommandLine;
using System;

namespace consoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Init();

            var result = Parser.Default.ParseArguments<InputOptions>(args);
            result.WithParsed(o =>
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    var app = scope.Resolve<IApplication>();
                    app.Run(o);
                }
            });

            Console.WriteLine("Yo! Package created");
            Console.Read();
        }
    }
}
