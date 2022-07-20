namespace PointFit;

 public class Algorithm
    {
        /// <summary>
        /// 计算点与与直线的距离
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static double CalcDistance((double X, double Y) point, Line line)
        {
            // 计算点到直线的距离
            var lineco = line.Coefficient;
            return Math.Abs(lineco.A * point.X + lineco.B * point.Y + lineco.C)
                 / Math.Sqrt(lineco.A * lineco.A + lineco.B * lineco.B);
        }
        /// <summary>
        /// 计算两个直线的交点
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static (double X, double Y)? CalcCrossPoint(Line line1, Line line2)
        {
            var line1co = line1.Coefficient;
            var line2co = line2.Coefficient;
            
            if (Math.Abs(line1co.A * line2co.B - line2co.A * line1co.B) < 0.000001)
            {
                // 平行
                // Console.WriteLine("平行");
                return null;
            }
            double x = (line1co.B * line2co.C - line2co.B * line1co.C) / (line1co.A * line2co.B - line2co.A * line1co.B);
            double y = (line1co.A * line2co.C - line2co.A * line1co.C) / (-line1co.A * line2co.B + line2co.A * line1co.B);
            // Console.WriteLine("答案组");
            // Console.WriteLine($"{line1co.A} {line1co.B} {line1co.C}");
            // Console.WriteLine($"{line2co.A} {line2co.B} {line2co.C}");
            // Console.WriteLine((x, y));
            
            return (x, y);
        }
        // public static IsOnSegment((double X, double Y) point, Line line) {

        // }
        /// <summary>
        /// 计算线段交点
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static (double X, double Y)? CalcSegmentCrossPoint(Line line1, Line line2)
        {
            var ans = CalcCrossPoint(line1, line2);
            if (ans == null
             || (ans?.X - line1.Start.X) * (line1.End.X - ans?.X) < -0.000001
             || (ans?.X - line2.Start.X) * (line2.End.X - ans?.X) < -0.000001
             || (ans?.Y - line1.Start.Y) * (line1.End.Y - ans?.Y) < -0.000001
             || (ans?.Y - line2.Start.Y) * (line2.End.Y - ans?.Y) < -0.000001
             ) return null;
            return ans;
        }
        
        /// <summary>
        /// 计算点在矢量左边还是右边
        /// </summary>
        /// <param name="point"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static double CalcCross((double X, double Y) point, Line line2)
        {
            return (point.X - line2.Start.X) * (line2.End.Y - line2.Start.Y)
            - (line2.End.X - line2.Start.X) * (point.Y - line2.Start.Y);
        }
        
        /// <summary>
        /// 计算点是否在矩形内部
        /// </summary>
        /// <param name="point"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static bool CheckPointInRect((double X, double Y) point, Rect rect)
        {
            // 计算次数太多了 --- 可优化
            var lines = rect.GetLines();
            for (int i = 0; i < lines.Count; ++i)
            {
                // Console.WriteLine(CalcCross(point, lines[i]));
                if (CalcCross(point, lines[i]) > 0.000001) return false;
            }
            return true;
        }
        /// <summary>
        /// 计算区域面积
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double CalcArea(List<(double X, double Y)> points)
        {
            if (points.Count < 3) return 0;
            points = points.OrderBy(s => s.Y).ThenBy(s => s.X).ToList();
            
            var baseP = points[0];
            points = points.OrderBy(
                s => Math.Abs(s.Y - baseP.Y) < 0.000001 ? -double.MaxValue : (s.X - baseP.X) / (s.Y - baseP.Y))
                .ThenBy(s => s.X - baseP.X)
                .ToList();
            // Console.WriteLine(string.Join(", ", points));
            double S = 0;
            for (int i = 2; i < points.Count; ++i) {
                Line di = new Line{ Start = points[0], End = points[i - 1]};
                S += CalcDistance(points[i], di) * di.Length / 2;
                // Console.WriteLine($"Distance = {CalcDistance(points[i], di)}");
                // Console.WriteLine($"S = {S}");
            }
            return S;
        }
        static Random random = new Random();
        public void test()
        {
            CalcOverlayArea(
                new Rect
                {
                    Angle = random.Next(180),
                    LW = (random.NextDouble() * 1000, random.NextDouble() * 1000),
                    Pos = (random.NextDouble() * 100, random.NextDouble() * 100)
                },
             new Rect
             {
                 Angle = random.Next(180),
                 LW = (random.NextDouble() * 1000, random.NextDouble() * 1000),
                 Pos = (random.NextDouble() * 100, random.NextDouble() * 100)
             }
            );
        }
        /// <summary>
        /// 计算两矩形重叠面积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double CalcOverlayArea(Rect a, Rect b)
        {
            // 变为相对坐标

            var aLines = a.GetLines();
            var bLines = b.GetLines();
            List<(double X, double Y)> points = new();
            for (int i = 0; i < 4; ++i)
            {
                // 遍历线段 一条v线
                for (int j = 0; j < 4; ++j)
                {
                    // 如果相交
                    var cPoint = CalcSegmentCrossPoint(aLines[i], bLines[j]);
                    // Console.WriteLine(cPoint);
                    // 把交点全部存入
                    if (cPoint != null)
                    {
                        points.Add(cPoint.Value);
                    }
                }
            }
            // Console.WriteLine("---------------");
            // Console.WriteLine(string.Join(", ", points));
            // Console.WriteLine("---------------");
            var aPoints = a.GetPoints();
            var bPoints = b.GetPoints();
            for (int i = 0; i < 4; ++i)
            {
                // 加入顶点
                if (CheckPointInRect(aPoints[i], b))
                {
                    points.Add(aPoints[i]);
                }
                if (CheckPointInRect(bPoints[i], a))
                {
                    points.Add(bPoints[i]);
                }
            }
            // Console.WriteLine(string.Join(", ", points));
            return CalcArea(points);
        }
    }
