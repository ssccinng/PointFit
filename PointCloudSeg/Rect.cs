using System.Drawing;
using System.Numerics;

namespace PointCloudSeg;


/// <summary>
/// 矩形结构
/// </summary>
public struct Rect
    {
        // 中心点坐标
        public (double X, double Y) Pos;
        // 长宽
        public (double L, double W) LW => (L, W);

        public readonly double L, W;
        public readonly double Angle;
        public List<Vector2> Points;
        public List<Line> Lines;

        public Rect(Vector2 pos, double l, double w, double angle)
        {
            L = l;
            W = w;
            Angle = angle;
            Pos = (pos.X, pos.Y);
            
            // 可以lazy 但可能没必要
            Points = new List<Vector2> {
                new Vector2( (float)(Pos.X - (L / 2 * Math.Cos(Angle / 180 * Math.PI) - W / 2 * Math.Sin(Angle / 180 * Math.PI))),
                    (float)(Pos.Y - (L / 2 * Math.Sin(Angle / 180 * Math.PI) + W / 2 * Math.Cos(Angle / 180 * Math.PI)))),

                new Vector2((float)(Pos.X + (L / 2 * Math.Cos(Angle / 180 * Math.PI) + W / 2 * Math.Sin(Angle / 180 * Math.PI))),
                    (float)(Pos.Y - (- L / 2 * Math.Sin(Angle / 180 * Math.PI) + W / 2 * Math.Cos(Angle / 180 * Math.PI)))),

                new Vector2((float)(Pos.X + (L / 2 * Math.Cos(Angle / 180 * Math.PI) - W / 2 * Math.Sin(Angle / 180 * Math.PI))),
                    (float)(Pos.Y + (L / 2 * Math.Sin(Angle / 180 * Math.PI) + W / 2 * Math.Cos(Angle / 180 * Math.PI)))),

                new Vector2((float)(Pos.X - (L / 2 * Math.Cos(Angle / 180 * Math.PI) + W / 2 * Math.Sin(Angle / 180 * Math.PI))),
                    (float)(Pos.Y + (-L / 2 * Math.Sin(Angle / 180 * Math.PI) + W / 2 * Math.Cos(Angle / 180 * Math.PI)))),
            };
            
            Lines = Lines = new List<Line> {
                new Line(Points[0], Points[1]),
                new Line(Points[1], Points[2]),
                new Line(Points[2], Points[3]),
                new Line(Points[3], Points[0]),
            };
        }
        public void Clear() {
            Lines = null;
            Points = null;
        }
        public List<Vector2> GetPoints()
        {
            if (Points != null) return Points;
            // double halfXie = Math.Sqrt(LW.L * LW.L + LW.W * LW.W) / 2;
            // return new List<(double X, double Y)> {
            //     (Pos.X - (LW.L / 2 * Math.Cos(Angle / 180 * Math.PI)),
            // };

            // ---可优化
            // 检查一下，方向
            return Points = new List<Vector2> {
                new Vector2( (float)(Pos.X - (LW.L / 2 * Math.Cos(Angle / 180 * Math.PI) - LW.W / 2 * Math.Sin(Angle / 180 * Math.PI))),
                    (float)(Pos.Y - (LW.L / 2 * Math.Sin(Angle / 180 * Math.PI) + LW.W / 2 * Math.Cos(Angle / 180 * Math.PI)))),

                new Vector2((float)(Pos.X + (LW.L / 2 * Math.Cos(Angle / 180 * Math.PI) + LW.W / 2 * Math.Sin(Angle / 180 * Math.PI))),
                    (float)(Pos.Y - (- LW.L / 2 * Math.Sin(Angle / 180 * Math.PI) + LW.W / 2 * Math.Cos(Angle / 180 * Math.PI)))),

                new Vector2((float)(Pos.X + (LW.L / 2 * Math.Cos(Angle / 180 * Math.PI) - LW.W / 2 * Math.Sin(Angle / 180 * Math.PI))),
                    (float)(Pos.Y + (LW.L / 2 * Math.Sin(Angle / 180 * Math.PI) + LW.W / 2 * Math.Cos(Angle / 180 * Math.PI)))),

                new Vector2((float)(Pos.X - (LW.L / 2 * Math.Cos(Angle / 180 * Math.PI) + LW.W / 2 * Math.Sin(Angle / 180 * Math.PI))),
                    (float)(Pos.Y + (-LW.L / 2 * Math.Sin(Angle / 180 * Math.PI) + LW.W / 2 * Math.Cos(Angle / 180 * Math.PI)))),
            };
        }
        public List<Line> GetLines()
        {
            if (Lines != null) return Lines;
            var points = GetPoints();
            return Lines = new List<Line> {
                new Line(points[0], points[1]),
                new Line(points[1], points[2]),
                new Line(points[2], points[3]),
                new Line(points[3], points[0]),
            };
        }

    }