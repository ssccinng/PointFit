// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Numerics;
using PointCloudSeg;


for (int i = 0; i < 20; ++i)
{
    var points = 
        // .GetPointsFromPointCloud(@"E:\WXWork\1688856549301066\Cache\File\2022-07\debug_result(1)\debug_result\00000001\point_w.ply");
        // PointCloud.GetPointCloudFromFile(@"E:\WXWork\1688856549301066\Cache\File\2022-07\debug_result\00000003\point_w.ply",
        PointCloud.GetPointCloudFromFile(@$"F:\VSHub\PointFit\PointCloudSeg\bin\Debug\net6.0\2022_07_27_18_06_26\000000{i:00}\point_w.ply",
        // PointCloud.GetPointCloudFromFile(@$"F:\点云\cv_system\2022_07_10_12_49_56\00000002\point_w.ply",
            true);
    PCSolver pcSolver = new PCSolver(points);

    var rpoints = pcSolver.GetRandomPointInRect(10);
    var bitmap = points.GetRGBBitmapFromPoints();
    // bitmap.WriteRect(new Rect(new Vector2(100, 100), 100, 100, 45));
    List<Vector2> cc = new List<Vector2>
    {
        // new Vector2(100, 200),
        // new Vector2(300, 100),
        // new Vector2(100, 100),
        // new Vector2(300, 000),
        new Vector2(117, 12),
        new Vector2(360, 18),
        new Vector2(119, 413),
        new Vector2(352, 413),
    };
    foreach (var vector2 in cc)
    {
        bitmap.WritePoint(vector2);
    }
    var bp = Algorithm.GetRectBy4Point(cc);
    bitmap.WriteRect(bp, Brushes.CornflowerBlue);
    bitmap.WritePoint(new Vector2((float)bp.Pos.X, (float)bp.Pos.Y), Brushes.Cornsilk);
    // foreach (Segment line in bp)
    // {
    //     bitmap.WriteASegment(line);
    //
    // }
    // bitmap.WriteRect(bp, Brushes.Cyan);

    // bitmap.Bitmap.Save($"让我康康{i}.png");

    foreach (var point in rpoints)
    {
        bitmap.WritePoint(point);
        var lines = pcSolver.GetFSXFormPoint(point);
        foreach (var line in lines)
        {
            bitmap.WriteALine(line);
            var cps = pcSolver.GetCrossPoint(line, point);
            Console.WriteLine(cps.Count);
            foreach (var cp in cps)
            {
                bitmap.WritePoint(cp);
            
            }
        }
        break;
    }
    bitmap.Bitmap.Save($"让我康康{i}.png");

    continue;

    bitmap.Bitmap.Save("让我康康.png");

}
