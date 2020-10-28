using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Medicale_Updater
{

    class Medicale
    {
        private properties pr = new properties();
        private String latest_version;
        private bool appDownloaded = false;

        public bool GetLatest()
        {

            Process p = new Process();
            p.StartInfo.FileName = "\"" + pr.getPropertie("curl") + "\"";
            p.StartInfo.Arguments = " --insecure --netrc-file \"" + pr.getPropertie("appPath") + "\\config\\curl_config\"  " + pr.getPropertie("update_server_ip") + pr.getPropertie("update_location") + "medicale/version.txt";
                 p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            latest_version = p.StandardOutput.ReadToEnd();

            p.WaitForExit();
            if (p.ExitCode == 0 && pr.getPropertie("version") != latest_version)
            {
                Console.WriteLine("new version available for medicale");
                pr.update("latest_release", "false");
                pr.save();
            }
            else if (pr.getPropertie("version") == latest_version)
            {
                pr.update("latest_release", "true");
                pr.save();
            }
            Console.WriteLine("Output:");
            Console.WriteLine(latest_version);
            return false;
        }


        public void Update()
        {
            GetLatest();
            Process proccess = new Process();
            proccess.StartInfo.FileName = "\"" + pr.getPropertie("curl") + "\"";
            proccess.StartInfo.Arguments = " --insecure --netrc-file \"" + pr.getPropertie("appPath") + "\\config\\curl_config\" -X PROPFIND -H \"Depth: infinity\" " + pr.getPropertie("update_server_ip") + pr.getPropertie("update_location") + "medicale/" + latest_version;
            proccess.StartInfo.UseShellExecute = false;
            proccess.StartInfo.RedirectStandardOutput = true;
            proccess.Start();
            String input = proccess.StandardOutput.ReadToEnd();





            foreach (var v2 in input.Split("<d:href>"))
            {

                if (v2.Contains("/remote.php"))
                {

                    foreach (var h in v2.Split("</d:href>"))
                    {
                        if (h.Contains("/remote.php"))
                        {
                           
                            download(h.Split(pr.getPropertie("update_location") + "medicale/" + latest_version)[1]);
                        }
                    }
                }


            }
            if (appDownloaded)
            {
                Process.Start("java.exe", " -jar \"" + pr.getPropertie("appPath") + "\\app\\medicale\\Cabinet Medical.jar\"");
                
                System.Environment.Exit(0);
            }


        }
        public void download(String path)
        {
            if (path.Length > 0 && !path.Equals("/"))
            {
                if (!Directory.Exists(pr.getPropertie("appPath") + "\\app\\medicale"))
                {
                    Directory.CreateDirectory(pr.getPropertie("appPath") + "\\app\\medicale");
                }

                if (path.EndsWith("/"))
                {
                    if (!Directory.Exists(pr.getPropertie("appPath") + "\\app\\medicale\\" + path))
                    {
                        Directory.CreateDirectory(pr.getPropertie("appPath") + "\\app\\medicale\\" + path);
                    }
                }
                else
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "\"" + pr.getPropertie("curl") + "\"";
                    if (path.EndsWith("app.jar"))
                    {
                        process.StartInfo.Arguments = " --insecure --netrc-file \"" +pr.getPropertie("appPath")+ "\\config\\curl_config\"  " +
pr.getPropertie("update_server_ip") + pr.getPropertie("update_location") + "medicale/" +
latest_version + "/" + path + " --output \"" + pr.getPropertie("appPath") + "\\app\\medicale\\Cabinet Medical.jar\"";

                        appDownloaded = true;
                    }
                    else
                    {
                        process.StartInfo.Arguments = " --insecure --netrc-file \"" + pr.getPropertie("appPath") + "\\config\\curl_config\"  " + pr.getPropertie("update_server_ip") + pr.getPropertie("update_location") + "medicale/" + latest_version + "/" + path + " --output \"" + pr.getPropertie("appPath") + "\\app\\medicale\\" + path.Substring(1) + "\"";

                    }


                    Console.WriteLine(" --insecure -u admin1:Admin@Admin123 " +
                        pr.getPropertie("update_server_ip") + pr.getPropertie("update_location") + "medicale /" + latest_version + "/" + path + " --output \"" + pr.getPropertie("appPath") + "\\medicale\\" + path.Substring(1) + "\"");
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();
                    Console.WriteLine(process.StandardError.ReadToEnd());
                    process.WaitForExit();




                }
            }

        }
        private void complete_update()
        {

        }
    }


}
