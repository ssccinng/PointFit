// See https://aka.ms/new-console-template for more information

using PointFit;

var points = PointCloudHelper
    .GetPointsFromPointCloud(@"E:\WXWork\1688856549301066\Cache\File\2022-07\debug_result(1)\debug_result\00000001\point_w.ply");
    
Console.WriteLine(points.Count);

var bitmap = PointCloudHelper.GetBitmapFromPoints(points).Bitmap;
bitmap.Save("dani1.png");