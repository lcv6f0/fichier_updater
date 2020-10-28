using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;

namespace Medicale_Updater
{
    class Program
    {
        private static String Version = "0.4.2";

        static void Main(string[] args)
        {
            Console.WriteLine("Version: " + Version);
            //args check-app-updater: check the update for the fichier app
            // args complete_update: will move the new installer to the correct path
            // will always check if updater is up to date before proceding
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "version":
                        Console.WriteLine(Version);
                        break;
                    case "complete-update":

                        //check all update
                        Updater u = new Updater(Version);
                        u.CompleteUpdate();
                        u.GetLatest();
                        Medicale m = new Medicale();
                        m.GetLatest();
                        break;
                    case "check":
                        Updater uxt = new Updater(Version);
                        uxt.GetLatest();
                        Medicale mx = new Medicale();
                        mx.GetLatest();
                        //m.getAllPaths();
                        //will check both app and ficher update
                        break;

                    case "update":
                        Medicale sd = new Medicale();
                        sd.GetLatest();
                        sd.Update();
                        break;
                }
            }
        }
    }
}
