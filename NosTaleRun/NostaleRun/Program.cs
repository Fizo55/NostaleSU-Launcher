using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace NostaleRun
{
    internal static class Program
    {
        public static string IP = "127.0.0.1";
        public static int Port = 4002;

        [STAThread]
        private static void Main()
        {
            string Test = "IP=";
            string Test2 = "Port=";
            string IP2 = (Test + IP);
            string PORT2 = (Test2 + Port);
            string path1 = Path.Combine(Environment.CurrentDirectory, "NostaleClientX.exe");
            if (!Process.GetProcessesByName("Launcher").Any())
            {
                MessageBox.Show("Merci d'utiliser le launcher afin de lancer votre jeu correctement !");
                return;
            }
            if (!File.Exists(path1))
            {
                MessageBox.Show("Merci de placer le launcher dans le dossier officiel de nostale contenant NostaleClientX.exe");
            }
            else
            {
                string path = Path.Combine(Environment.CurrentDirectory, "Config.ini");
                bool Checked = false;
                bool port___1 = false;
                bool ip___2 = false;
                if (File.Exists(path))
                {
                    List<string> list = new List<string>();
                    using (StreamReader r = new StreamReader(path))
                    {
                        string line;
                        line = r.ReadLine();
                        while (line != null)
                        {
                            list.Add(line);
                            Console.WriteLine(line);
                            line = r.ReadLine();
                        }
                        r.Close();
                    }
                    if (list.Contains(PORT2))
                        port___1 = true;
                    if (list.Contains(IP2))
                        ip___2 = true;
                    if (ip___2 && port___1)
                        Checked = true;
                    else
                    {
                        File.Delete(path);
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine("[NosTale_Network]");
                            sw.WriteLine(IP2);
                            sw.WriteLine(PORT2);
                            Checked = true;
                            sw.Close();
                        }
                    }
                }
                else
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("[NosTale_Network]");
                        sw.WriteLine(IP2);
                        sw.WriteLine(PORT2);
                        Checked = true;
                        sw.Close();
                    }
                if (Checked)
                {
                    string nostalex = (path1);
                    string path2 = "\"";
                    path2 += nostalex;
                    path2 += "\"";
                    string argument = "/c START ";
                    argument += "\"";
                    argument += "\"";
                    argument += " ";
                    argument += path2;
                    argument += " EntwellNostaleClientLoadFromIni";
                    Process p = new Process
                    {
                        StartInfo = new ProcessStartInfo("cmd.exe", argument)
                    };
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.Start();
                    Environment.Exit(0);
                }
            }
        }
    }
}