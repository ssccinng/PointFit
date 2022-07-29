using System.Drawing;
using System.Numerics;

namespace PointCloudSeg;

/// <summary>
/// 点云求解器
/// </summary>
public class PCSolver
{
    public PointCloud PointCloud;
    public PCSolver(PointCloud pointCloud)
    {
        PointCloud = pointCloud;
    }

    private Random _rnd = new();
    public List<Vector2> HasNeighborPoint(int x, int y, int distance = 1, int offsetY = 1)
    {
        var spc = PointCloud.Graph;
        // 附近的所有点
        List<Vector2> nPoint = new();
        // JsonDocument cc = null;
        // File.WriteAllText("xx",cc.RootElement.GetRawText());
        // JsonNode.Parse("asc");
        // Console.WriteLine($"{x},{y}");
        // Console.WriteLine($"{spc[x][y]}");
        // 2种方式， 一是x+y == distance, 二是 x and y <= distance
        for (int dx = -distance; dx <= distance; dx++)
        {
            for (int dy = -distance; dy <= distance; dy++)
            {
                try
                {
                    if (dx == 0 && dy == 0) continue;
                    if (spc[x + dx][y + dy] != -1 && Math.Abs(spc[x + dx][y + dy] - spc[x][y]) <= offsetY)
                    {
                        nPoint.Add(new Vector2(x + dx, y + dy));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // throw;
                }
                // 在其中判除了自己的所有点
                
            }
        }
        return nPoint;
        
        for (int dx = 0; dx <= distance; dx++)
        {
            for (int dy = dx - distance; dy <= distance - x; dy++)
            {
                // 在其中判除了自己的所有点
                if (dx == 0 && dy == 0) continue;
                if (spc[x + dx][y + dy] != -1)
                {
                    nPoint.Add(new Vector2(x + dx, y + dy));
                }
                if (spc[x - dx][y + dy] != -1)
                {
                    nPoint.Add(new Vector2(x + dx, y + dy));
                }
            }
        }
        return nPoint;

    }
    /// <summary>
    /// 获取随机在矩形内得点
    /// </summary>
    /// <param name="cnt"></param>
    /// <returns></returns>
    public List<Point> GetRandomPointInRect(int cnt = 5)
    {
        List<Point> res = new();
        while (cnt > 0)
        {
            var rPoint = PointCloud.Points[_rnd.Next(PointCloud.Points.Count)];
            if (rPoint.X < 110 || rPoint.Y < 110) continue;
            if (rPoint.X + 110 > PointCloud.Width || rPoint.Y + 110 > PointCloud.Height) continue;
            if (HasNeighborPoint((int)(rPoint.X), (int)(rPoint.Y), 100, 2).Count >= 9999 * 2)
            {
                res.Add(new Point((int)(rPoint.X ), (int)(rPoint.Y)));
                cnt--;
            }
        }
        return res;
    }

    /// <summary>
    /// 构造点得放射线
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public List<Line> GetFSXFormPoint(Point point)
    {
        List<Line> res = new();
        for (int i = -82; i <= 180 - 82; i += 10)
        {
            
            var rr =new Line(new Vector2(point.X, point.Y), Math.Tan(i / 180.0 * Math.PI));
            res.Add(rr);
        }
        return res;
    }
    /// <summary>
    /// 获双射线线与点云的交点
    /// </summary>
    /// <param name="line"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public List<Point> GetCrossPoint(Line line, Point point)
    {
        List<Point> res = new();
        int offsetX = 0;
        bool flag = true;
        Queue<float> heightqueue = new();
        while (flag)
        {
            var y1 = line.GetPointFromX(point.X + offsetX).Y;
            var y2 = line.GetPointFromX(point.X + offsetX + 1).Y;
            int tag = 1;
            if (y1 > y2)
            {
                tag = -1;
            }
            for (int i = y1; i * tag <= y2 * tag; i += tag)
            {
                heightqueue.Enqueue(PointCloud.Graph[point.X + offsetX][i]);

                // 要直接判边界
                // if (HasNeighborPoint(point.X+ offsetX, i, 3, 1).Count <= 1)
                    // 先简单的判一下
                    // 具体阈值可以确认一下
                if (heightqueue.First() - heightqueue.Last() > 15 || heightqueue.Last() == -1)
                {
                    res.Add(new Point(point.X + offsetX, i));
                    flag = false;
                    break;
                }

                if (heightqueue.Count > 10) heightqueue.Dequeue();
            }
            offsetX++;
        }
        offsetX = 0;
        flag = true;
        heightqueue.Clear();
        while (flag)
        {
            
            var y1 = line.GetPointFromX(point.X - offsetX).Y;
            var y2 = line.GetPointFromX(point.X - offsetX - 1).Y;
            int tag = 1;
            if (y1 > y2)
            {
                tag = -1;
            }
            // 出现k为0的有点麻烦
            for (int i = y1; i * tag <= y2 * tag; i += tag)
            {
                heightqueue.Enqueue(PointCloud.Graph[point.X - offsetX][i]);

                // if (HasNeighborPoint(point.X - offsetX, i, 3, 2).Count <= 1)
                    if (heightqueue.First() - heightqueue.Last() > 15 || heightqueue.Last() == -1)

                {
                    res.Add(new Point(point.X - offsetX, i));
                    flag = false;
                    break;
                }
                    if (heightqueue.Count > 10) heightqueue.Dequeue();

            }
            offsetX++;
        }
        return res;

    }
}