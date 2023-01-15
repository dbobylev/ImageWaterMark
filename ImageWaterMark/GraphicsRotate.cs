using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageWaterMark
{
    internal class GraphicsRotate
    {
        private int _angleDegree;
        private double _angle;
        private PointF _listSize;
        private PointF _textSize;

        // Вершины листа listSize. Прямоугольник ABCD. 
        // Необходимы для расчета точек начала и конца заливки штампов при повороте листа на определнный угол
        // Поворот происходит относительно точки A, которая всегда остается 0,0
        /*
                        A1##############B1
                      ######            ##
                    ##  ##  ##          ##
                  ##    ##    ##        ##
                ##      ##      ##      ##
              ##        ##        B2    ##
            ##          ##      ##      ##
          ##            ##    ##        ##
        D2              ##  ##          ##
          ##            ####            ##
            ##          ##              ##
              ##      ##D1##############C1
                ##  ##                    
                  C2                      

        */

        private PointF _PointA;
        private PointF _PointB;
        private PointF _PointC;
        private PointF _PointD;

        private int _dX;
        private int _dY;

        public GraphicsRotate(int angle, PointF listSize, PointF TextSize)
        {
            _angleDegree = angle;
            _angle = Math.PI * angle / 180;
            _listSize = listSize;
            _textSize = TextSize;

            // Задаём координаты углов нашего листа (по умолчанию при угле наклона 0)
            _PointA = new PointF(0f, 0f);
            _PointB = new PointF(listSize.X, 0f);
            _PointC = listSize;
            _PointD = new PointF(0f, listSize.Y);

            // Если задан угол, поварачиваем наши вершины
            if (angle != 0)
            {
                _PointB = RotatePoint(_PointB);
                _PointC = RotatePoint(_PointC);
                _PointD = RotatePoint(_PointD);
            }

            _dX = Config.GetInt("stamp", "dX", 0);
            _dY = Config.GetInt("stamp", "dY", 0);

            Program.LogDebug($"PointA: {_PointA}");
            Program.LogDebug($"PointB: {_PointB}");
            Program.LogDebug($"PointC: {_PointC}");
            Program.LogDebug($"PointD: {_PointD}");
        }

        public PointF RotatePoint(PointF point)
        {
            if (_angleDegree == 0)
                return point;

            double x = point.X * Math.Cos(_angle) - point.Y * Math.Sin(_angle);
            double y = point.X * Math.Sin(_angle) + point.Y * Math.Cos(_angle);
            return new PointF((float)x, (float)y);
        }

        public float GetBegX()
        {
            float result = _dX + _PointD.X - _textSize.X * 1.5f;
            Program.LogDebug($"GetBegX: {result}");
            return result;
        }

        public float GetEndX()
        {
            float result = _dX + _PointB.X + _textSize.X * 1.5f;
            Program.LogDebug($"GetEndX: {result}");
            return result;
        }

        public float GetBegY()
        {
            float result = _dY + _PointA.Y - _textSize.Y * 1.5f;
            Program.LogDebug($"GetBegY: {result}");
            return result;
        }

        public float GetEndY()
        {
            float result = _dY + _PointC.Y + _textSize.Y * 1.5f;
            Program.LogDebug($"GetEndY: {result}");
            return result;
        }
    }
}