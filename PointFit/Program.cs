// See https://aka.ms/new-console-template for more information

using PointFit;

var points = PointCloudHelper
    // .GetPointsFromPointCloud(@"E:\WXWork\1688856549301066\Cache\File\2022-07\debug_result(1)\debug_result\00000001\point_w.ply");
    .GetPointsFromPointCloud(@"E:\WXWork\1688856549301066\Cache\File\2022-07\debug_result\00000000\point_w.ply");
    
Console.WriteLine(points.Count);
PointCloud plc = new PointCloud(points);

PointSolve pointSolve = new PointSolve(plc);
var bitmap = PointCloudHelper.GetBitmapFromPoints(points);
var ps = pointSolve.SolveTest();
// PointCloudHelper.WriteALine(bitmap, new Line{End = (1, 1), Start = (1000, 1000)});
Console.WriteLine($"有{ps.Count}个点命中");
Console.WriteLine(ps.Count);
for (int i = 0; i < ps.Count; i++)
{
    PointCloudHelper.WritePoint(bitmap, ps[i]);
    PointCloudHelper.WriteALine(bitmap, new Line((ps[i].X, ps[i].Y), 3));
    PointCloudHelper.WriteALine(bitmap, new Line((ps[i].X, ps[i].Y), 2));

    foreach (var point in pointSolve.SolveTest2(new Line((ps[i].X, ps[i].Y), 2), ps[i]))
    {
        PointCloudHelper.WritePoint(bitmap, point);

    }
    foreach (var point in pointSolve.SolveTest2(new Line((ps[i].X, ps[i].Y), 3), ps[i]))
    {
        PointCloudHelper.WritePoint(bitmap, point);

    }
}

bitmap.Bitmap.Save("dani1.png");