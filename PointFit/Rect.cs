namespace PointFit;

public struct Rect
    {
        // 中心点坐标
        public (double X, double Y) Pos;
        public (double L, double W) LW;
        public double Angle;
        private List<(double X, double Y)> Points;
        private List<Line> Lines;
        public void Clear() {
            Lines = null;
            Points = null;
        }
        public List<(double X, double Y)> GetPoints()
        {
            if (Points != null) return Points;
            // double halfXie = Math.Sqrt(LW.L * LW.L + LW.W * LW.W) / 2;
            // return new List<(double X, double Y)> {
            //     (Pos.X - (LW.L / 2 * Math.Cos(Angle / 180 * Math.PI)),
            // };

            // ---可优化
            // 检查一下，方向
            return Points = new List<(double X, double Y)> {
                (Pos.X - (LW.L / 2 * Math.Cos(Angle / 180 * Math.PI) - LW.W / 2 * Math.Sin(Angle / 180 * Math.PI)),
                Pos.Y - (LW.L / 2 * Math.Sin(Angle / 180 * Math.PI) + LW.W / 2 * Math.Cos(Angle / 180 * Math.PI))),

                (Pos.X + (LW.L / 2 * Math.Cos(Angle / 180 * Math.PI) + LW.W / 2 * Math.Sin(Angle / 180 * Math.PI)),
                Pos.Y - (- LW.L / 2 * Math.Sin(Angle / 180 * Math.PI) + LW.W / 2 * Math.Cos(Angle / 180 * Math.PI))),

                (Pos.X + (LW.L / 2 * Math.Cos(Angle / 180 * Math.PI) - LW.W / 2 * Math.Sin(Angle / 180 * Math.PI)),
                Pos.Y + (LW.L / 2 * Math.Sin(Angle / 180 * Math.PI) + LW.W / 2 * Math.Cos(Angle / 180 * Math.PI))),

                (Pos.X - (LW.L / 2 * Math.Cos(Angle / 180 * Math.PI) + LW.W / 2 * Math.Sin(Angle / 180 * Math.PI)),
                Pos.Y + (-LW.L / 2 * Math.Sin(Angle / 180 * Math.PI) + LW.W / 2 * Math.Cos(Angle / 180 * Math.PI))),
            };
        }
        public List<Line> GetLines()
        {
            if (Lines != null) return Lines;
            var points = GetPoints();
            return Lines = new List<Line> {
                new Line{Start = points[0], End = points[1]},
                new Line{Start = points[1], End = points[2]},
                new Line{Start = points[2], End = points[3]},
                new Line{Start = points[3], End = points[0]},
            };
        }

    }