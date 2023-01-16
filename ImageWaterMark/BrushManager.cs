using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWaterMark
{
    internal class BrushManager
    {
        private readonly IEnumerable<int> _allowedColorValues;
        private readonly eBrushType _BrushType;

        public BrushManager()
        {
            _allowedColorValues = Enumerable.Range(0, 256);
            _BrushType = (eBrushType)Config.GetInt("color", "mode", (int)eBrushType.LegendaryBrush, Extensions.GetEnumVals<eBrushType>());
        }

        public Brush DefineBrush()
        {
            Brush brush;
            Color color1 = Color.FromArgb(alpha: Config.GetInt("color", "color1_A", 100, _allowedColorValues),
                                            red: Config.GetInt("color", "color1_R", 100, _allowedColorValues),
                                          green: Config.GetInt("color", "color1_G", 100, _allowedColorValues),
                                           blue: Config.GetInt("color", "color1_B", 100, _allowedColorValues));
            Color color2 = Color.FromArgb(alpha: Config.GetInt("color", "color2_A", 100, _allowedColorValues),
                                            red: Config.GetInt("color", "color2_R", 100, _allowedColorValues),
                                          green: Config.GetInt("color", "color2_G", 100, _allowedColorValues),
                                           blue: Config.GetInt("color", "color2_B", 100, _allowedColorValues));

            Program.LogDebug($"Color1: {color1}");
            Program.LogDebug($"Color2: {color2}");

            switch (_BrushType)
            {
                case eBrushType.SolidBrush:
                    brush = new SolidBrush(color1);
                    break;
                case eBrushType.TwoColorBrush:
                    brush = Get2PointGradientBrsh(color1, color2);
                    break;
                case eBrushType.LegendaryBrush:
                    brush = GetLegendaryBrush();
                    break;
                default:
                    brush = new SolidBrush(color1);
                    break;
            }

            return brush;
        }

        private Brush Get2PointGradientBrsh(Color color1, Color color2)
        {
            int GradientSize = Config.GetInt("color", "size", 400);

            LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(GradientSize, 0), Color.White, Color.White);
            ColorBlend cb = new ColorBlend();
            cb.Colors = new[] { color1, color2, color1 };
            cb.Positions = new[] { 0f, 0.5f, 1f };
            brush.InterpolationColors = cb;

            return brush;
        }

        private Brush GetLegendaryBrush()
        {
            int GradientSize = Config.GetInt("color", "size", 400);

            int alpha = Config.GetInt("color", "color1_A", 100, _allowedColorValues);
            int valMax = 255;
            int valMin = 150;

            Color c1 = Color.FromArgb(alpha, valMin, valMax, valMax);
            Color c2 = Color.FromArgb(alpha, valMax, valMin, valMax);
            Color c3 = Color.FromArgb(alpha, valMax, valMax, valMin);

            LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(GradientSize, 0), Color.White, Color.White);
            ColorBlend cb = new ColorBlend();
            cb.Colors = new[] { c1, c2, c3, c1 };
            cb.Positions = new[] { 0f, 0.33f, 0.66f, 1f };
            brush.InterpolationColors = cb;

            return brush;
        }
    }
}
