using System.Drawing;
using System.Numerics;

namespace PointCloudSeg;

/// <summary>
/// 线段结构
/// </summary>
public class Line
{
    public Vector2 Start;
    public Vector2 End;
    // 斜率
    public readonly double K;

    public readonly double A;
    public readonly double B;
    public readonly double C;

    // kx + c
    public readonly double c;

    public Line(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
        K = (end.Y - start.Y) / (end.X - start.X);
        c = start.Y - start.X * K;

        if (Math.Abs(-End.X + Start.X) < 0.000001) {
            (A, B, C) = (1, 0, -Start.X);
        }
        else {
            (A, B, C) = (End.Y - Start.Y, -End.X + Start.X, (Start.Y - K * Start.X) * (End.X - Start.X));
        }
    }

    public Line(Vector2 point, double k)
    {
        Start = point;
        K = k;
        c = point.Y - point.X * k;
        // -kx + y = c
        (A, B, C) = (-K, 1, -c);
    }

    public (double A, double B, double C) Coefficient => (A, B, C);

    // public Line((double x, double y) start, double k)
    // {
    //     // Start = start;
    //     Start = (0, (start.y - start.x * k) + 0*k);;
    //     End = (2000, (start.y - start.x * k) + 2000*k);
    // }

    public Point GetPointFromX(int x)
    {
        // return new Point(x, (int)(Start.Y - Start.X * K + K * x));
        return new Point(x, (int)(K * x + c));
    }

    public Vector2 GetVector2FromX(float x)
    {
        return new Vector2(x, (float)(Start.Y - Start.X * K + K * x));

    }
    public Vector2 GetVector2FromY(float y)
    {
        return new Vector2((float)((y - c) / K), y);
    }

    public float GetDeltaXFromDistance(float dis)
    {
        // x * x + (kx + c) ** 2 = dis
        // (k2 + 1)x2 + 2kcx = dis - c
        var cc = (float)(dis * Math.Cos(Math.Atan(K)));
        Console.WriteLine(cc);
        return cc;
    }

    public Segment CutASegment(float x1, float x2)
    {
        return new Segment(new Vector2(x1, GetVector2FromX(x1).Y), new Vector2(x2, GetVector2FromX(x2).Y));
    }
    // public Segment
}

public class Segment : Line
{
    public double Length { get; set; }

    public Segment(Vector2 start, Vector2 end) : base(start, end)
    {
        Length = Math.Sqrt((End.Y - Start.Y) * (End.Y - Start.Y) + (End.X - Start.X) * (End.X - Start.X));
    }
    
} 