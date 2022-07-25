using System.Drawing;

namespace PointFit;

public struct Line
{
    public (double X, double Y) Start;
    public (double X, double Y) End;

    public double Length => Math.Sqrt((End.Y - Start.Y) * (End.Y - Start.Y) + (End.X - Start.X) * (End.X - Start.X));
    // 斜率
    public double K => (End.Y - Start.Y) / (End.X - Start.X);
    public (double A, double B, double C) Coefficient
    {
        get
        {
            // var k = K;
            // Console.WriteLine((double.IsNaN(K)));
            // return (K, -1, Start.Y - K * Start.X);
            if (Math.Abs(-End.X + Start.X) < 0.000001) {
                return (1, 0, -Start.X);
            }
            else {
                return (End.Y - Start.Y, -End.X + Start.X, (Start.Y - K * Start.X) * (End.X - Start.X));
            }
            // return (End.Y - Start.Y, -End.X + Start.X, Math.Abs(-End.X + Start.X) < 0.000001 ? 0 : ((Start.Y - K * Start.X) * (End.X - Start.X)));

        }
    }

    public Line((double x, double y) start, double k)
    {
        // Start = start;
        Start = (0, (start.y - start.x * k) + 0*k);;
        End = (2000, (start.y - start.x * k) + 2000*k);
    }

    public Point GetPointFromX(int x)
    {
        return new Point(x, (int)(Start.Y - Start.X * K + K * x));
    }
}