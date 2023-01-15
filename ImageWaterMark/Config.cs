using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWaterMark
{
    internal static class Config
    {
        // Параметры из файла ini
        public static IniData IniData;

        static Config()
        {
            var parser = new FileIniDataParser();
            parser.Parser.Configuration.CommentRegex = new Regex(@"^[#].*$");
            IniData = parser.ReadFile("config.ini");
        }

        public static int GetInt(string section, string name, int defaultValue, IEnumerable<int> allowedValues = null)
        {
            int result = defaultValue;

            if (int.TryParse(IniData[section][name], out int parseResult))
            {
                if (!(allowedValues != null && !allowedValues.Contains(parseResult)))
                    result = parseResult;
            }

            return result;
        }

        public static float GetFloat(string section, string name, float defaultValue)
        {
            string s = IniData[section][name];
            s = s.Replace('.', ',');
            float result = defaultValue;

            if (float.TryParse(s, out float parseResult))
            {
                result = parseResult;
            }

            return result;
        }
    }
}
