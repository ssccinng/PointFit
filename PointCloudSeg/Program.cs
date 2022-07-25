// See https://aka.ms/new-console-template for more information

using PointCloudSeg;

var points = 
    // .GetPointsFromPointCloud(@"E:\WXWork\1688856549301066\Cache\File\2022-07\debug_result(1)\debug_result\00000001\point_w.ply");
    PointCloud.GetPointCloudFromFile(@"E:\WXWork\1688856549301066\Cache\File\2022-07\debug_result\00000002\point_w.ply");

points.GetBitmapFromPoints().Bitmap.Save("dani.png");    