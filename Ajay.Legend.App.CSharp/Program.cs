using Microsoft.AspNetCore.Builder;

namespace Ajay.Legend.App.CSharp
{
    /// <summary>
    /// Starting class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Passed arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var startup = new Startup(builder.Configuration, builder.Environment);

            startup.AddServices(builder.Services);

            var app = builder.Build();

            startup.Configure(app);

            app.Run();
        }
    }
}