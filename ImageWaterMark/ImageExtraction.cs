using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageWaterMark
{
    internal class ImageExtraction
    {
        public ImageExtraction()
        {

        }

        public List<Image> ExtractImage(string pdfFile)
        {
            List<Image> images = new List<Image>();

            PdfReader pdfReader = new PdfReader(pdfFile);
            for (int pageNumber = 1; pageNumber <= pdfReader.NumberOfPages; pageNumber++)
            {
                PdfDictionary pg = pdfReader.GetPageN(pageNumber);
                PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
                PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
                foreach (PdfName name in xobj.Keys)
                {
                    PdfObject obj = xobj.Get(name);
                    if (obj.IsIndirect())
                    {
                        PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
                        string width = tg.Get(PdfName.WIDTH).ToString();
                        string height = tg.Get(PdfName.HEIGHT).ToString();
                        ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject(new GraphicsState(), (PRIndirectReference)obj, tg);
                        PdfImageObject image = imgRI.GetImage();
                        Image dotnetImg = image.GetDrawingImage();
                        if (dotnetImg!= null)
                        {
                            images.Add(dotnetImg);
                        }
                    }
                }
            }

            if (pdfReader.NumberOfPages != images.Count)
            {
                Program.LogInfo($"Кол-во изображений PDF ({images.Count}) не совпадает c кол-вом страниц ({pdfReader.NumberOfPages}");
                Program.LogInfo("Обработка остановлена");
                return null;
            }

            return images;
        }
    }
}
