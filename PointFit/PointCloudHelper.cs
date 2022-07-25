using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace PointFit;

public static class PointCloudHelper
{
    /// <summary>
    /// 从点云文件中获取所有点
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>所有点的列表</returns>
    public static List<Vector3> GetPointsFromPointCloud(string path)
    {
        List<Vector3> points = new();
        var file = File.ReadAllLines(path);
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

        return points;
    }

    public static DirectBitmap GetBitmapFromPoints(List<Vector3> points)
    {
        float minX = points.Min(s => s.X);
        float minY = points.Min(s => s.Y);
        Console.WriteLine(minX);
        Console.WriteLine(minY);
        float maxX = points.Max(s => s.X);
        float maxY = points.Max(s => s.Y);
        Console.WriteLine(maxX);
        Console.WriteLine(maxY);

        DirectBitmap bitmap = new
            ((int)((maxX - minX) * 1) + 1, (int)((maxY - minY) * 1) + 1);
        Console.WriteLine(bitmap.Bitmap.Width);
        Console.WriteLine(bitmap.Bitmap.Height);
        for (int i = 0; i < points.Count; i++)
        {
            // Console.WriteLine((int)((points[i].X - minX) * 10));
            // Console.WriteLine((int)((points[i].Y - minY) * 10));
            bitmap.SetPixel((int)((points[i].X - minX) * 1),
                (int)((points[i].Y - minY) * 1),
                Color.White);
        }

        return bitmap;
    }

    /// <summary>
    /// 从深度图文件中获取所有点
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>所有点的列表</returns>
    public static List<Vector3> GetPointsFromEXR(string path)
    {
        List<Vector3> points = new();

        return points;
    }

    /// <summary>
    /// 返回2d点云
    /// 无点为-1 有点为z值
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public static int[][] GetSimplePointCloud(PointCloud points)
    {
        int[][] simplePoints
            = new int[(int)(points.MaxX - points.MinX + 1)][];
        // = new int[,];
        int height = (int)(points.MaxY - points.MinY + 1);
        for (int i = 0; i < simplePoints.Length; i++)
        {
            simplePoints[i] = new int[height];
            for (int j = 0; j < height; j++)
            {
                simplePoints[i][j] = -1;
            }
        }
        for (int i = 0; i < points.Points.Count; i++)
        {
            simplePoints
                        [(int)(points.Points[i].X - points.MinX)]
                    [(int)(points.Points[i].Y - points.MinY)]
                = (int)(points.Points[i].Z - points.MinZ);
        }

        return simplePoints;
    }


    public static void WriteALine(DirectBitmap bitmap, Line line)
    {
        using Graphics graphics = Graphics.FromImage(bitmap.Bitmap);
        
        graphics.DrawLine(new Pen(Color.Red), (float)line.Start.X, (float)line.Start.Y, (float)line.End.X, (float)line.End.Y);
    }

    public static void WritePoint(DirectBitmap bitmap, Point point)
    {
        using Graphics graphics = Graphics.FromImage(bitmap.Bitmap);
        graphics.FillEllipse(Brushes.Red, point.X - 5, point.Y - 5, 10, 10);
    }
}