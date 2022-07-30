using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;

namespace PointCloudSeg;

public class DirectBitmap : IDisposable
{
    public Graphics Graphics;
    public Bitmap Bitmap { get; private set; }
    public Int32[] Bits { get; private set; }
    public bool Disposed { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }

    protected GCHandle BitsHandle { get; private set; }

    public DirectBitmap(int width, int height)
    {
        Width = width;
        Height = height;
        Bits = new Int32[width * height];
        BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
        Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        Graphics = Graphics.FromImage(Bitmap);

    }

    public void SetPixel(int x, int y, Color colour)
    {
        int index = x + (y * Width);
        int col = colour.ToArgb();

        Bits[index] = col;
    }

    public Color GetPixel(int x, int y)
    {
        int index = x + (y * Width);
        int col = Bits[index];
        Color result = Color.FromArgb(col);

        return result;
    }

    public void Dispose()
    {
        if (Disposed) return;
        Disposed = true;
        Graphics.Dispose();
        Bitmap.Dispose();
        BitsHandle.Free();
    }
    
    public void WriteALine(Line line, Brush color = null)
    {
        color ??= Brushes.Red;
        using Graphics graphics = Graphics.FromImage(Bitmap);
        var p1 = line.GetPointFromX(0);
        var p2 = line.GetPointFromX(2000);
        graphics.DrawLine(new Pen(color), (float)p1.X, (float)p1.Y, (float)p2.X, (float)p2.Y);
    }
    public void WriteASegment(Segment line, Brush color = null)
    {
        color ??= Brushes.Red;

        using Graphics graphics = Graphics.FromImage(Bitmap);
        graphics.DrawLine(new Pen(color), (float)line.Start.X, (float)line.Start.Y,
            (float)line.End.X, (float)line.End.Y);
    }
    public void WritePoint( Point point, Brush color = null)
    {
        color ??= Brushes.Red;
        using Graphics graphics = Graphics.FromImage(Bitmap);
        graphics.FillEllipse(color, point.X - 5, point.Y - 5, 10, 10);
    }
    public void WritePoint( Vector2 point, Brush color = null)
    {
        WritePoint(new Point((int)point.X, (int)point.Y), color);
    }
    public void WriteRect( Rect point, Brush color = null)
    {
        color ??= Brushes.Red;
        using Graphics graphics = Graphics.FromImage(Bitmap);
        foreach (var line in point.Lines)
        {
            graphics.DrawLine(new Pen(color, 5), (float)line.Start.X, (float)line.Start.Y, (float)line.End.X, (float)line.End.Y);

        }

    }
    
    public void WriteCircle(Point point, float dis, Brush color = null)
    {
        color ??= Brushes.Red;
        using Graphics graphics = Graphics.FromImage(Bitmap);
        graphics.DrawEllipse(new Pen(color), (point.X - dis / 2), (point.Y - dis / 2), dis, dis);
    }
}