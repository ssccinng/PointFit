using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PointFit;

public class PointSolve
{
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
    public List<Vector2> HasNeighborPoint(int[][] spc, int x, int y, int distance = 1)
    {

        // 附近的所有点
        List<Vector2> nPoint = new();
        JsonDocument cc = null;
        File.WriteAllText("xx",cc.RootElement.GetRawText());
        JsonNode.Parse("asc");
        
        // 2种方式， 一是x+y == distance, 二是 x and y <= distance
        for (int dx = -distance; dx <= distance; dx++)
        {
            for (int dy = -distance; dy <= distance; dy++)
            {
                // 在其中判除了自己的所有点
                if (dx == 0 && dy == 0) continue;
                if (spc[x + dx][y + dy] != -1)
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
}