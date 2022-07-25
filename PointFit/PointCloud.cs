using System.Numerics;

namespace PointFit;

public class PointCloud
{
    public IReadOnlyList<Vector3> Points;

    public readonly float MinX, MinY, MaxX, MaxY, MinZ, MaxZ;

    public PointCloud(List<Vector3> points)
    {
        Points = points;
        MinX = points.Min(s => s.X);
        MinY = points.Min(s => s.Y);
        MaxX = points.Max(s => s.X);
        MaxY = points.Max(s => s.Y);
        MinZ = points.Min(s => s.Z);
        MaxZ = points.Max(s => s.Z);
    }
    
    
}