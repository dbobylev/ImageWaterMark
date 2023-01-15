using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWaterMark
{
    internal class WaterMarkParams
    {
        private BrushManager _brushManager;
        private FontManager _FontManager;
        private GraphicsRotate _GraphicsRotate;

        public eWMType WMType { get; private set; }

        public string Text { get; private set; }

        public Brush Brush { get; private set; }
        public Font Font { get; private set; }
        public StringFormat StringFormat { get; private set; }

        public int Angle { get; private set; }
        public float DeltaX { get; private set; }
        public float DeltaY { get; private set; }  
        public float DeltaStep { get; private set; }


        public float xBeg { get; private set; }
        public float xEnd { get; private set; }
        public float yBeg { get; private set; }
        public float yEnd { get; private set; }

        public WaterMarkParams()
        {
            _brushManager = new BrushManager();
            _FontManager = new FontManager();

            Text = Config.IniData["text"]["text"].Replace('$', '\n');

            Brush = _brushManager.DefineBrush();
            Font = _FontManager.DefineFont();
            StringFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            Angle = Config.GetInt("stamp", "angle", 0, Enumerable.Range(1, 90));

            WMType = (eWMType)Config.GetInt("stamp", "mode", (int)eWMType.MultiStamp, Extensions.GetEnumVals<eWMType>());

            if (WMType == eWMType.MultiStamp)
            {
                DeltaX = Config.GetFloat("stamp", "stampX", 1.3f);
                DeltaY = Config.GetFloat("stamp", "stampY", 2.2f);
                DeltaStep = Config.GetFloat("stamp", "dStep", 0.5f);
            }
        }

        public void SetSizes(PointF ListSize, PointF TextSize)
        {
            Program.LogDebug($"ListSize: {ListSize}");
            Program.LogDebug($"TextSize: {TextSize}");

            _GraphicsRotate = new GraphicsRotate(Angle, ListSize, TextSize);

            if (WMType == eWMType.MultiStamp)
            {
                xBeg = _GraphicsRotate.GetBegX();
                xEnd = _GraphicsRotate.GetEndX();
                yBeg = _GraphicsRotate.GetBegY();
                yEnd = _GraphicsRotate.GetEndY();
            }
        }

        public PointF RotatePoint(PointF source)
        {
            if (_GraphicsRotate != null)
            {
                return _GraphicsRotate.RotatePoint(source);
            }
            else
            {
                Program.LogDebug("Внимание! Не задан _GraphicsRotate");
                return source;
            }
        }
    }
}
