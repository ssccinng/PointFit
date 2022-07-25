using System.Drawing;
using System.Net;
using System.Numerics;

namespace PointCloudSeg;

public class PointCloud
{
    /// <summary>
    /// 带高度点
    /// </summary>
    public List<Vector3> Points { get; set; }
    /// <summary>
    /// 2.5D数组
    /// </summary>
    public int[][] Graph { get; set; }

    
    public readonly float MinX, MinY, MaxX, MaxY, MinZ, MaxZ;

    public readonly int Width, Height;

    public PointCloud(List<Vector3> points)
    {
        MinX = points.Min(s => s.X);
        MinY = points.Min(s => s.Y);
        MaxX = points.Max(s => s.X);
        MaxY = points.Max(s => s.Y);
        MinZ = points.Min(s => s.Z);
        MaxZ = points.Max(s => s.Z);
        Points = points.Select(s => new Vector3(s.X - MinX + 10, s.Y - MinY + 10, s.Z - MinZ)).ToList();
        Width = (int)(MaxX - MinX + 21);
        Height = (int)(MaxY - MinY + 21);

        Graph = GetSimplePointCloud();
        // Width = (int)(MaxX - MinX + 21);

        // points.Select(s => s.)
    }

    public static PointCloud GetPointCloudFromFile(string filePath)
    {
        List<Vector3> points = new();
        var file = File.ReadAllLines(filePath);
        bool flag = false;
        for (int i = 0; i < file.Length; i++)
        {
            if (!flag && file[i].Trim() == "end_header")
            {
                flag = true;
            }

            var data = file[i].Trim().Split();
            if (flag && data.Length == 3)
            {
                var point = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
                points.Add(point);
            }
        }

        return new PointCloud(points);
    }
    public int[][] GetSimplePointCloud()
    {
        int[][] simplePoints
            = new int[Width][];
        // = new int[,];
        for (int i = 0; i < simplePoints.Length; i++)
        {
            simplePoints[i] = new int[Height];
            for (int j = 0; j < Height; j++)
            {
                simplePoints[i][j] = -1;
            }
        }
        for (int i = 0; i < Points.Count; i++)
        {
            simplePoints
                        [(int)Points[i].X]
                    [(int)Points[i].Y]
                = (int)Points[i].Z;
        }

        return simplePoints;
    }

    public DirectBitmap GetBitmapFromPoints()
    {


        DirectBitmap bitmap = new
            (Width, Height);
        for (int i = 0; i < Points.Count; i++)
        {
            // Console.WriteLine((int)((points[i].X - minX) * 10));
            // Console.WriteLine((int)((points[i].Y - minY) * 10));
            bitmap.SetPixel((int)((Points[i].X) * 1),
                (int)((Points[i].Y) * 1),
                Color.White);
        }

        return bitmap;
    }
    
}