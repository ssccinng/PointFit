using System.Drawing;
using System.Numerics;

namespace PointCloudSeg;

public class Algorithm
{
    /// <summary>
    /// 通过四个顶点点获得矩形
    /// </summary>
    /// <returns></returns>
    public static Rect GetRectBy4Point(List<Vector2> points)
    {
        // 求解中间点
        float avgX = points.Average(s => s.X);
        float avgY = points.Average(s => s.Y);
        var basePoint = new Vector2(avgX, avgY);
        double avgDis = points.Average(s => CalcDistance(basePoint, s));
        // bitmap.WriteCircle(new Point((int)basePoint.X, (int)basePoint.Y), 2 * (float)avgDis);
        // 画个⚪看看
        bool[] visit = new bool[4];

        List<Line> res = new();
        for (int i = 0; i < 4; i++)
        {
            // 算出两条对角线 先作为测试 直接沿用斜率
            if (visit[i]) continue;
            var xx = points.Skip(i).MinBy(s => CalcDistance(basePoint, new Line(points[i], s)));
            var idx = points.IndexOf(xx);
            visit[i] = visit[idx] = true;
            // 求得一个角度
            res.Add(new Line(basePoint, new Line(points[i], points[idx]).K) );
            ;
            if (idx == 1)
            {
                res.Add(new Line(basePoint, new Line(points[2], points[3]).K) );
            }
            else if (idx == 2)
            {
                res.Add(new Line(basePoint, new Line(points[1], points[3]).K) );

            }
            else
            {
                res.Add(new Line(basePoint, new Line(points[1], points[2]).K) );
            }
            
            break;;
        }
        Console.WriteLine(avgDis);

        var po = res.Select(s => 
            s.CutASegment(s.Start.X - s.GetDeltaXFromDistance((float)avgDis),
                s.Start.X + s.GetDeltaXFromDistance((float)avgDis))).ToList();
        List<Vector2> bb = new();
        foreach (var segment in po)
        {
            bb.Add(segment.Start);
            bb.Add(segment.End);
        }
        bb = bb.OrderBy(s => s.Y).ThenBy(s => s.X).ToList();

        var baseP = bb[0];
        bb = bb.OrderBy(
                s => Math.Abs(s.Y - baseP.Y) < 0.000001 ? -double.MaxValue : (s.X - baseP.X) / (s.Y - baseP.Y))
            .ThenBy(s => s.X - baseP.X)
            .ToList();
        var l =  CalcDistance(bb[0], bb[1]);
        var w = CalcDistance(bb[0], bb[3]);
        Console.WriteLine(l);
        Console.WriteLine(w);
        // points = points.OrderBy(
        //         s => Math.Abs(s.Y - baseP.Y) < 0.000001 ? -double.MaxValue : (s.X - baseP.X) / (s.Y - baseP.Y))
        //     .ThenBy(s => s.X - baseP.X)
        //     .ToList();
        // 假定avgx y 为中心点
        return new Rect(new Vector2(avgX, avgY), l, w, 
            Math.Atan((bb[1].Y - bb[0].Y) / (bb[1].X - bb[0].X)) * 180 / Math.PI);
        // return basePoint;
        // return new Rect();
    }

    // public static List<(Vector2, Vector2)>
    /// <summary>
    /// 通过一系列点得出矩形
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Rect GetRectByManyPoint()
    {
        throw new NotImplementedException();
    }

    public static double CalcDistance(Vector2 point, Line line)
    {
        // 计算点到直线的距离
        var lineco = line.Coefficient;
        return Math.Abs(lineco.A * point.X + lineco.B * point.Y + lineco.C)
               / Math.Sqrt(lineco.A * lineco.A + lineco.B * lineco.B);
    }
    public static double CalcDistance(Vector2 point1, Vector2 point2)
    {
        // 计算点到直线的距离
        return  Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
    }
    /// <summary>
    /// 计算两个直线的交点
    /// </summary>
    /// <param name="line1"></param>
    /// <param name="line2"></param>
    /// <returns></returns>
    public static Vector2? CalcCrossPoint(Line line1, Line line2)
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

        return new Vector2((float)x, (float)y);
    }
    // public static IsOnSegment((double X, double Y) point, Line line) {

    // }
    /// <summary>
    /// 计算线段交点
    /// </summary>
    /// <param name="line1"></param>
    /// <param name="line2"></param>
    /// <returns></returns>
    public static Vector2? CalcSegmentCrossPoint(Line line1, Line line2)
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
    public static double CalcCross(Vector2 point, Line line2)
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
    public static bool CheckPointInRect(Vector2 point, Rect rect)
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
    public static double CalcArea(List<Vector2> points)
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
        for (int i = 2; i < points.Count; ++i)
        {
            Segment di = new Segment(points[0], points[i - 1]);
            S += CalcDistance(points[i], di) * di.Length / 2;
            // Console.WriteLine($"Distance = {CalcDistance(points[i], di)}");
            // Console.WriteLine($"S = {S}");
        }

        return S;
    }

    static Random random = new Random();

    public void test()
    {
        // CalcOverlayArea(
        //     new Rect
        //     {
        //         Angle = random.Next(180),
        //         LW = (random.NextDouble() * 1000, random.NextDouble() * 1000),
        //         Pos = (random.NextDouble() * 100, random.NextDouble() * 100)
        //     },
        //  new Rect
        //  {
        //      Angle = random.Next(180),
        //      LW = (random.NextDouble() * 1000, random.NextDouble() * 1000),
        //      Pos = (random.NextDouble() * 100, random.NextDouble() * 100)
        //  }
        // );
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
        List<Vector2> points = new();
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