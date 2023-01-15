using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWaterMark
{
    internal class FontManager
    {
        public FontManager()
        {

        }

        public Font DefineFont()
        {
            Font font;
            string FontName = Config.IniData["text"]["font"];
            int size = Config.GetInt("text", "font_size", 20);

            FontStyle style = FontStyle.Regular;

            if (Config.GetInt("text", "bold", 0, new int[] { 1 }) == 1)
                style |= FontStyle.Bold;

            if (Config.GetInt("text", "italic", 0, new int[] { 1 }) == 1)
                style |= FontStyle.Italic;

            try
            {
                font = new Font(FontName, size, style);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании Шрифта! {ex.Message}");
                font = new Font("Arial", 20);
            }

            return font;
        }
    }
}
