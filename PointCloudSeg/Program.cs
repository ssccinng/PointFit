// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Numerics;
using PointCloudSeg;

int add = 70;
List<Vector2> rrplist = new();
for (int i = 0; i < 10000; i++)
{
    rrplist.Add(new Vector2(Random.Shared.Next(2000) + 500,
        Random.Shared.Next(2000) + 500));
}


for (int k = 0; k < 70; ++k)
{
    


DirectBitmap bbmp = new(3000, 3000);
var rect = new Rect(new Vector2(1500, 1500), 2000, 2000, 0);
bbmp.WriteRect(rect);


for (int i = 0; i < 10000; i++)
{
    var np = rrplist[i];
    // var np = new Vector2(Random.Shared.Next(2000) + 500,
    //     Random.Shared.Next(2000) + 500);
    bbmp.WritePoint(np);
    var lines = PCSolver.GetFSXFormPoint(np, k, add);
    List<Vector2> acp = new();
    foreach (Line line in lines)
    {
        line.Start = line.GetVector2FromX(0);
        line.End = line.GetVector2FromX(3000);
        foreach (Line line1 in rect.Lines)
        {
            // Console.WriteLine(line.K);
            // var p = Algorithm.CalcCrossPoint(line, line1).Value;
            
            var p = Algorithm.CalcSegmentCrossPoint(line, line1);
            if (p.HasValue)
            acp.Add(p.Value);
            // bbmp.WritePoint(acp.Last());
        }
        // bbmp.WriteALine(line);

    }

    for (int j = 1; j <= 10; j++)
    {
        var cc = new SolidBrush(Color.FromArgb(255 - j * 20, j * 20, 0));
        var lp = np;
        np = new Vector2(acp.Average(s => s.X), acp.Average(s => s.Y));
        bbmp.WriteASegment(new Segment(lp, np), cc);
        // bbmp.WritePoint(np, Brushes.Beige);
        bbmp.WritePoint(np, cc);
        lines = PCSolver.GetFSXFormPoint(np, k, add);
        acp = new();
        foreach (Line line in lines)
        {
            line.Start = line.GetVector2FromX(0);
            line.End = line.GetVector2FromX(3000);
            foreach (Line line1 in rect.Lines)
            {
                // Console.WriteLine(line.K);
                // var p = Algorithm.CalcCrossPoint(line, line1).Value;
            
                var p = Algorithm.CalcSegmentCrossPoint(line, line1);
                if (p.HasValue)
                    acp.Add(p.Value);
                // bbmp.WritePoint(acp.Last());
            }
        }
    }
    
}
bbmp.Bitmap.Save($"dadadani{k}.png");
bbmp.Dispose();
}
return;





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
        // bitmap.WritePoint(vector2);
    }
    var bp = Algorithm.GetRectBy4Point(cc);
    // bitmap.WriteRect(bp, Brushes.CornflowerBlue);
    // bitmap.WritePoint(new Vector2((float)bp.Pos.X, (float)bp.Pos.Y), Brushes.Cornsilk);
    // foreach (Segment line in bp)
    // {
    //     bitmap.WriteASegment(line);
    //
    // }
    // bitmap.WriteRect(bp, Brushes.Cyan);

    // bitmap.Bitmap.Save($"让我康康{i}.png");

    foreach (var point in rpoints)
    {
        bitmap.WritePoint(point, Brushes.Cornsilk);
        var lines = PCSolver.GetFSXFormPoint(point);
        List<Point> rwkk = new();
        foreach (var line in lines)
        {
            // bitmap.WriteALine(line);
            // 过滤过远的点
            var cps = pcSolver.GetCrossPoint(line, point);
            rwkk.AddRange(cps);
            Console.WriteLine(cps.Count);
            // foreach (var cp in cps)
            // {
            //     bitmap.WritePoint(cp);
            //
            // }
        }

        var xx = rwkk.Average(s => s.X);
        var yy = rwkk.Average(s => s.Y);
        var np = new Point((int)xx, (int)yy);
        lines = PCSolver.GetFSXFormPoint(np);
        bitmap.WritePoint(np, Brushes.Yellow);


        foreach (var line in lines)
        {
            // bitmap.WriteALine(line, Brushes.Aqua);
            // 过滤过远的点
            var cps = pcSolver.GetCrossPoint(line, np);
            rwkk.AddRange(cps);
            Console.WriteLine(cps.Count);
            foreach (var cp in cps)
            {
                // bitmap.WritePoint(cp);
            
            }
        }
        xx = rwkk.Average(s => s.X);
        yy = rwkk.Average(s => s.Y);
        np = new Point((int)xx, (int)yy);
        lines = PCSolver.GetFSXFormPoint(np);
        bitmap.WritePoint(np, Brushes.Cyan);
        // bitmap.Bitmap.Save($"让我康康{i}.png");

        foreach (var line in lines)
        {
            bitmap.WriteALine(line, Brushes.Aqua);
            // 过滤过远的点
            var cps = pcSolver.GetCrossPoint(line, np);
            rwkk.AddRange(cps);
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
