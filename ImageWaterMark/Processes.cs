using Org.BouncyCastle.Utilities.IO.Pem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageWaterMark
{
    internal class Processes
    {
        private const string FILETAGWM = "wm";
        private const string FILETAGCOMB = "combined";

        ImageExtraction _ImageExtraction;
        CreatePDF _CreatePDF;
        WaterMark _WaterMark;

        public Processes()
        {
            _CreatePDF = new CreatePDF();
        }

        public void AddWaterMark()
        {
            _WaterMark = new WaterMark();
            _ImageExtraction = new ImageExtraction();

            // Получаем список файлов PDF
            string[] pdffilespathes = Directory.GetFiles(".", "*.pdf").Where(x => !x.ToUpper().Contains($"{FILETAGWM.ToUpper()}.PDF")).ToArray();

            if (pdffilespathes.Length.Equals(0))
                Program.LogInfo("Не найдено файлов с расширением pdf");

            // Обрабатываем PDF
            for (int i = 0; i < pdffilespathes.Length; i++)
            {
                var pdfpath = pdffilespathes[i];
                Program.LogInfo($"Начинаем обработку {pdfpath}");

                // Достаём изображения из PDF
                List<Image> images = _ImageExtraction.ExtractImage(pdfpath);
                if (images == null) return;
                Program.LogInfo($"Достали сканы. Кол-во: {images.Count}");

                // Накладываем WM на изобаржения
                _WaterMark.AddWaterMark(ref images);
                Program.LogInfo($"Добавили WaterMark");

                // Создаем новый PDF
                string targetFilename = GetFileNameWM(pdfpath);
                _CreatePDF.CreateFromImg(images, targetFilename);
                Program.LogInfo($"Готово. Сохранён файл {targetFilename}");
            }
        }

        public void CombinePdf()
        {
            // Получаем список файлов PDF
            string[] pdffilespathes =
                Directory
                .GetFiles(".", "*.pdf")
                .Where(x => !x.ToUpper().Contains($"{FILETAGCOMB.ToUpper()}.PDF"))
                .OrderBy(x => x)
                .ToArray();

            Program.LogInfo("Файлы будут объеденены в следующем порядке: ");
            for (int i = 0; i < pdffilespathes.Length; i++)
                Program.LogInfo($"{i + 1} - {pdffilespathes[i]}");

            string targetFileName = GetGileNameCombo();

            _CreatePDF.MergePdfs(pdffilespathes, targetFileName);
            Program.LogInfo($"Готово. Сохранён файл: {targetFileName}");
        }

        private string GetFileNameWM(string filepath)
        {
            string filename = Regex.Match(filepath, @"^\.\\(.*)\.pdf$", RegexOptions.IgnoreCase).Groups[1].Value;
            return  $"{filename}_{FILETAGWM}.pdf";
        }

        private string GetGileNameCombo()
        {
            return $"{FILETAGCOMB}.pdf";
        }
    }
}
