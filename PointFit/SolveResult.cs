using System.Numerics;

namespace PointFit;

public class SolveResult
{
    /// <summary>
    /// 矩形左上右下两点坐标
    /// </summary>
    // public List<(Vector2, Vector2)> MatrixPos { get; set; }
    public List<Rect> Matrix { get; set; }
    
}