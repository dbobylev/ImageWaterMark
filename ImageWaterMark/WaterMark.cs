using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace ImageWaterMark
{
    internal class WaterMark
    {
        private WaterMarkParams _params;

        public WaterMark()
        {
        }

        public void AddWaterMark(ref List<Image> images)
        {
            // Проходимся по изображениям
            for (int i = 0; i < images.Count; i++)
            {
                // Наше изображение, на которое будем накладывать текст
                Bitmap bitmap = (Bitmap)images[i];

                // Параметры для нанесения WM собраны в одном месте
                _params = new WaterMarkParams();

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // Размер текста
                    SizeF textSize = graphics.MeasureString(_params.Text, _params.Font);

                    _params.SetSizes(new PointF(bitmap.Width, bitmap.Height), new PointF(textSize.Width, textSize.Height));

                    if (_params.WMType == eWMType.MultiStamp)
                    {
                        // Текст наносится всегда горизонтально, что бы нанести его под углом, мы должны повернуть сам лист
                        graphics.RotateTransform(-_params.Angle);

                        int k = 1;
                        for (float posy = _params.yBeg; posy < _params.yEnd; posy += textSize.Height * _params.DeltaY)
                        {
                            // Смещаем строку относительно предыдущий
                            float deltaposx = textSize.Width * _params.DeltaX * (k * _params.DeltaStep % 1);  
                            for (float posx = _params.xBeg; posx < _params.xEnd; posx += textSize.Width * _params.DeltaX)
                            {
                                graphics.DrawString(_params.Text, _params.Font, _params.Brush, new PointF(posx + deltaposx, posy), _params.StringFormat);
                            }
                            k++;
                        }
                    }
                    else if (_params.WMType == eWMType.CenterSingle)
                    {
                        PointF centerPoint = new PointF(bitmap.Width / 2, bitmap.Height / 2);
                        graphics.RotateTransform(-_params.Angle);
                        graphics.DrawString(_params.Text, _params.Font, _params.Brush, _params.RotatePoint(centerPoint), _params.StringFormat);
                    }
                }

                images[i] = bitmap;
            }
        }
    }
}
