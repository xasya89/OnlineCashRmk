namespace OnlineCashRmk.Launcher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            if (InstanceCheck)
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
        }

        static Mutex InstanceCheckMutex;
        static bool InstanceCheck
        {
            get {
                bool isNew;
                InstanceCheckMutex = new Mutex(true, "OnlineCashRmk_launcher", out isNew);
                return isNew;
            }
        }
    }
}