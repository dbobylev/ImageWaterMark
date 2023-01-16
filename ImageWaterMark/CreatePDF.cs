using iTextSharp.text;
using System;
using System.Collections.Generic;
using dotnetIamge = System.Drawing;
using System.IO;
using iTextSharp.text.pdf;

namespace ImageWaterMark
{
    internal class CreatePDF
    {
        public readonly Rectangle _A4;

        public CreatePDF()
        {
            _A4 = PageSize.A4;
        }

        public void CreateFromImg(List<dotnetIamge.Image> iamges, string targetFileName)
        {
            using (FileStream stream = new FileStream(targetFileName, FileMode.Create, FileAccess.Write))
            {
                Document document = new Document(_A4, 0f, 0f, 0f, 0f);

                using (PdfWriter writer = PdfWriter.GetInstance(document, stream))
                {
                    document.Open();

                    foreach (var image in iamges)
                    {
                        iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(image, dotnetIamge.Imaging.ImageFormat.Jpeg);

                        float percentage;
                        if (pic.Height > _A4.Height)
                        {
                            percentage = _A4.Height / pic.Height;
                            pic.ScalePercent(percentage * 100);
                        }

                        if (pic.Width > _A4.Width)
                        {
                            percentage = _A4.Width / pic.Width;
                            pic.ScalePercent(percentage * 100);
                        }
                        
                        document.Add(pic);
                        document.NewPage();
                    }

                    document.Close();
                    writer.Close();
                }
                stream.Close();
            }
        }

        public void MergePdfs(IEnumerable<string> fileNames, string targetFileName)
        {
            using (FileStream stream = new FileStream(targetFileName, FileMode.Create))
            {
                Document document = new Document();
                PdfCopy pdf = new PdfCopy(document, stream);
                PdfReader reader = null;

                try
                {
                    document.Open();
                    foreach (string file in fileNames)
                    {
                        reader = new PdfReader(file);
                        pdf.AddDocument(reader);
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Program.LogInfo($"Произошла ошибка при объединении документ: {ex.Message}");
                    reader.Close();
                }
                finally
                {
                    document.Close();
                }
            }
        }
    }
}
