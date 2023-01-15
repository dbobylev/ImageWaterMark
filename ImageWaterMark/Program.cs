using IniParser.Model;
using IniParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWaterMark
{
    internal static class Program
    {
        private static bool _debuglogEnable = false;

        public const string TESTPDFNAME = "test_wm.pdf";

        static void Main(string[] args)
        {
            Processes process = new Processes();

            if (args.Length > 0 && args.Contains("debug"))
                _debuglogEnable = true;

            if (args.Length > 0 && args.Contains("combine"))
            {
                process.CombinePdf();
            }
            else
            {
                process.AddWaterMark();

                if (File.Exists(TESTPDFNAME))
                    Process.Start(TESTPDFNAME);
            }

            Console.ReadLine();
        }

        public static void LogDebug(string msg)
        {
            if (_debuglogEnable)
                Console.WriteLine(msg);
        }

        public static void LogInfo(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
