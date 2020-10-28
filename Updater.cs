using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Medicale_Updater
{
    class Updater
    {
        private properties pr = new properties();
        private String local_version;
        private String serverVersion;
        private bool perform_update;

        public Updater(String local_version)
        {
            this.local_version = local_version;
        }
        public bool GetLatest()
        {

            Process p = new Process();
            p.StartInfo.FileName = "\""+pr.getPropertie("curl")+"\"";
            p.StartInfo.Arguments = " --insecure --netrc-file \"" + pr.getPropertie("appPath") + "\\config\\curl_config\" " + pr.getPropertie("update_server_ip") + pr.getPropertie("update_location") + "updater/version.txt";

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            serverVersion = p.StandardOutput.ReadToEnd().Trim();
        
            p.WaitForExit();
            if (p.ExitCode==0 && serverVersion.Length>0 && local_version != serverVersion)
            {
                Console.WriteLine("new version available");
                getAllPaths();
            }
            Console.WriteLine("Output:");
            Console.WriteLine(serverVersion);
            return false;
        }
      
        public void getAllPaths()
        {
            Process proccess = new Process();
            proccess.StartInfo.FileName = "\"" + pr.getPropertie("curl") + "\"";
            proccess.StartInfo.Arguments = " --insecure --netrc-file \"" + pr.getPropertie("appPath") + "\\config\\curl_config\" -X PROPFIND -H \"Depth: infinity\" " + pr.getPropertie("update_server_ip")+pr.getPropertie("update_location") +"updater/" + serverVersion;
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
                            try
                            {
                                Console.WriteLine(h.Split(pr.getPropertie("update_location")+"updater/" + serverVersion)[1]);
                                download(h.Split("/" +pr.getPropertie("update_location") + "updater/" + serverVersion+"/")[1]);
                            }
                            catch (Exception d)
                            {

                            }
                           
                        }
                    }
                }


            }
            if (perform_update)
            {
                Process p = new Process();
                //         p.StartInfo.FileName = @pr.getPropertie("update_root") + "\\updater\\app.exe";
                p.StartInfo.FileName = pr.getPropertie("update_root")+"\\updater\\app.exe";
                p.StartInfo.Arguments = "complete-update";
                p.Start();
              
                System.Environment.Exit(0);
            }





        }
        public void download(String path)
        {
            if (!Directory.Exists(pr.getPropertie("update_root") + "\\updater\\" ))
            {
                Directory.CreateDirectory(pr.getPropertie("update_root") + "\\updater" );
            }
            if (path.EndsWith("/"))
            {
                if (!Directory.Exists(pr.getPropertie("update_root") + "\\updater\\" + path))
                {
                    Directory.CreateDirectory(pr.getPropertie("update_root") + "\\updater" + path);
                }
            }
            else
            {
                Process process = new Process();
                process.StartInfo.FileName = "\"" + pr.getPropertie("curl") + "\"";
                process.StartInfo.Arguments = " --insecure --netrc-file \"" + pr.getPropertie("appPath") + "\\config\\curl_config\" " +
                    pr.getPropertie("update_server_ip") + pr.getPropertie("update_location") +"/updater/" +serverVersion+"/"+ path + " --output \"" + pr.getPropertie("update_root") + "\\updater\\" + path+"\"";
                      process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                Console.WriteLine(process.StandardError.ReadToEnd());
                process.WaitForExit();

                perform_update = true; 
            }


        }
        public void CompleteUpdate()
        {
         
            properties pr = new properties();
            String[] newDirFiles = Directory.GetFiles(pr.getPropertie("update_root") + "\\updater\\" + serverVersion);
            if(!Directory.Exists( pr.getPropertie("appPath") + "\\app\\updater" ))
            {
                Directory.CreateDirectory( pr.getPropertie("appPath") + "\\app\\updater");
            }
            String[] oldFiles=Directory.GetFiles(pr.getPropertie("appPath") + "\\app\\updater");
            foreach(var h in oldFiles)
            {
                File.Delete(h);
               
            }
            foreach (var nd in newDirFiles)
            {
                if (Path.GetFileName(nd).Trim().ToLower().Equals("app.exe")){
                    File.Copy(nd, pr.getPropertie("appPath") + "\\app\\updater\\updater.exe");
                } else {
                    File.Copy( nd , pr.getPropertie("appPath") + "\\app\\updater\\" + Path.GetFileName(nd) );
                } }

        }
    }

}
