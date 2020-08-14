using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HSEApiTraining
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}//The branch of notIra, if you are here, that means you are me or Ira or shouldn't be here fа
