using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineCashRmk
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if(InstanceCheck())
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new Form1());
                var services = new ServiceCollection();

                ConfigureServices.ConfigureService(services);
                using(ServiceProvider provider = services.BuildServiceProvider())
                {
                    var form1 = provider.GetRequiredService<Form1>();
                    provider.GetRequiredService<Services.ISynchBackgroundService>();
                    Application.Run(form1);
                }
            }
        }
        // держим в переменной, чтобы сохранить владение им до конца пробега программы
        static Mutex InstanceCheckMutex;
        static bool InstanceCheck()
        {
            bool isNew;
            InstanceCheckMutex = new Mutex(true, "OnlineCashRmk", out isNew);
            return isNew;
        }
    }
}
