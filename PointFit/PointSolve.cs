using System.Drawing;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PointFit;

public class PointSolve
{
    public PointCloud FullPointCloud;
    public int[][] SimplePointCloud;

    public PointSolve(PointCloud pointCloud)
    {
        FullPointCloud = pointCloud;
        SimplePointCloud = PointCloudHelper.GetSimplePointCloud(FullPointCloud);
    }
    
    public List<SolveResult> Solve(List<Vector3> points)
    {
        // 置信度
        // 1. 所有分割出来的大小都要一样
        // 2. 边缘有噪点
        // 3. 长宽比限制
        
        List<SolveResult> results = new();
        PointCloud pointCloud = new PointCloud(points);
        var simplePC = PointCloudHelper.GetSimplePointCloud(pointCloud);
        
        

        return results;
    }
    /// <summary>
    /// 找出点云中的关键区域
    /// </summary>
    public void FindCritPoint()
    {
        
    }
    /// <summary>
    /// 判断距离内存不存在点
    /// </summary>
    /// <param name="spc"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="distance"></param>
    public List<Vector2> HasNeighborPoint(int x, int y, int distance = 1, int offsetY = 1)
    {
        var spc = SimplePointCloud;
        // 附近的所有点
        List<Vector2> nPoint = new();
        // JsonDocument cc = null;
        // File.WriteAllText("xx",cc.RootElement.GetRawText());
        // JsonNode.Parse("asc");
        Console.WriteLine($"{x},{y}");
        Console.WriteLine($"{spc[x][y]}");
        // 2种方式， 一是x+y == distance, 二是 x and y <= distance
        for (int dx = -distance; dx <= distance; dx++)
        {
            for (int dy = -distance; dy <= distance; dy++)
            {
                
                // 在其中判除了自己的所有点
                if (dx == 0 && dy == 0) continue;
                if (spc[x + dx][y + dy] != -1 && spc[x + dx][y + dy] - spc[x][y] <= offsetY)
                {
                    nPoint.Add(new Vector2(x + dx, y + dy));
                }
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
    /// 验证点云中的线段
    /// </summary>
    public void VerifyLineInPC(int[][] spc, Line line)
    {
        
    }
    
    /// <summary>
    /// 验证点云中的矩形
    /// </summary>
    public void VerifyRectInPC()
    {
        
    }
    /// <summary>
    /// 检测矩形边缘噪点
    /// </summary>
    public void VerifyRectBoxInPC()
    {
        
    }
    Random rnd = Random.Shared;
    public List<Point> SolveTest()
    {
        List<Point> res = new();
        for (int i = 0; i < 10; i++)
        {
            var rPoint = FullPointCloud.Points[rnd.Next(FullPointCloud.Points.Count)];
            if (HasNeighborPoint((int)(rPoint.X - FullPointCloud.MinX), (int)(rPoint.Y - FullPointCloud.MinY), 3, 2).Count > 7)
            {
                res.Add(new Point((int)(rPoint.X - FullPointCloud.MinX), (int)(rPoint.Y - FullPointCloud.MinY)));
            }
        }

        
        // 探索交接点  也许可用二分优化
        for (int i = 0; i < res.Count; i++)
        {
            
        }
        return res;
    }
    // 求解线的交点
    public List<Point> SolveTest2(Line line, Point point)
    {
        List<Point> res = new();
        int offsetX = 1;
        while (true)
        {
            // 如果自己是空的
            if (
                HasNeighborPoint(point.X + offsetX, line.GetPointFromX(point.X + offsetX).Y, 3, 2).Count <= 1 &&
                HasNeighborPoint(point.X + offsetX + 1, line.GetPointFromX(point.X + offsetX + 1).Y, 3, 2).Count <= 1
                
                )
            {
                res.Add(new Point(point.X + offsetX, line.GetPointFromX(point.X + offsetX).Y ));
                break;
            }

            offsetX++;
        }
        return res;

    }
}