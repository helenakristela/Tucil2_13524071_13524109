using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: dotnet run -- <input.obj> <maxDepth> [output.obj] [parallel]");
            return;
        }

        string inputPath = args[0];

        if (!int.TryParse(args[1], out int maxDepth) || maxDepth < 0)
        {
            Console.WriteLine("Error: maxDepth harus bilangan bulat >= 0.");
            return;
        }

        string outputPath = args.Length >= 3 ? args[2] : "output.obj";
        bool useParallel = args.Length >= 4 && args[3].ToLower() == "parallel";

        try
        {
            Timer timer = new Timer();
            timer.Start();

            List<Triangle> triangles = ObjParser.Parse(inputPath);

            Console.WriteLine($"File       : {inputPath}");
            Console.WriteLine($"Max Depth  : {maxDepth}");
            Console.WriteLine($"Triangles  : {triangles.Count}");
            Console.WriteLine($"Mode       : {(useParallel ? "Parallel" : "Sequential")}");

            if (triangles.Count == 0)
            {
                Console.WriteLine("Tidak ada triangle valid dari file .obj.");
                return;
            }

            Cube rootCube = BuildRootCube(triangles);
            Console.WriteLine($"Root Cube  : {rootCube}");

            OctreeNode root = new OctreeNode(rootCube, 0);

            if (useParallel)
            {
                Voxelizer.BuildParallel(root, triangles, maxDepth);
            }
            else
            {
                Voxelizer.Build(root, triangles, maxDepth);
            }

            List<Cube> voxels = Voxelizer.CollectLeafVoxels(root);

            ObjWriter.Write(outputPath, voxels);

            timer.Stop();

            Console.WriteLine();
            Console.WriteLine("STATISTICS");

            Console.WriteLine($"Root Triangles        : {root.Triangles.Count}");
            Console.WriteLine($"Total Nodes           : {root.CountNodes()}");
            Console.WriteLine($"Leaf Nodes            : {root.CountLeaves()}");
            Console.WriteLine($"Occupied Nodes        : {Statistics.CountOccupiedNodes(root)}");
            Console.WriteLine($"Occupied Leaf Nodes   : {Statistics.CountOccupiedLeaves(root)}");
            Console.WriteLine($"Empty Leaf Nodes      : {Statistics.CountEmptyLeaves(root)}");
            Console.WriteLine($"Max Reached Depth     : {Statistics.GetMaxReachedDepth(root)}");
            Console.WriteLine($"Voxel Count           : {voxels.Count}");
            Console.WriteLine($"Occupancy Ratio       : {Statistics.ComputeOccupancyRatio(root):P2}");

            Console.WriteLine();
            Console.WriteLine("NODES PER DEPTH");

            Dictionary<int, int> stats = root.CountNodesPerDepth();
            foreach (var kvp in stats.OrderBy(x => x.Key))
            {
                Console.WriteLine($"Depth {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine();
            Console.WriteLine("OCCUPIED LEAF PER DEPTH");

            Dictionary<int, int> occupiedLeafStats = Statistics.CountOccupiedLeavesPerDepth(root);
            foreach (var kvp in occupiedLeafStats.OrderBy(x => x.Key))
            {
                Console.WriteLine($"Depth {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine();
            Console.WriteLine("PERFORMANCE");
            Console.WriteLine($"Execution Time (ms): {timer.ElapsedMilliseconds()}");
            Console.WriteLine($"Execution Time (s) : {timer.ElapsedSeconds():F4}");

            Console.WriteLine();
            Console.WriteLine($"Output .obj saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private static Cube BuildRootCube(List<Triangle> triangles)
    {
        Vector3 globalMin = triangles[0].GetBoundingBox().min;
        Vector3 globalMax = triangles[0].GetBoundingBox().max;

        foreach (Triangle triangle in triangles)
        {
            var (min, max) = triangle.GetBoundingBox();

            globalMin = new Vector3(
                MathF.Min(globalMin.X, min.X),
                MathF.Min(globalMin.Y, min.Y),
                MathF.Min(globalMin.Z, min.Z)
            );

            globalMax = new Vector3(
                MathF.Max(globalMax.X, max.X),
                MathF.Max(globalMax.Y, max.Y),
                MathF.Max(globalMax.Z, max.Z)
            );
        }

        float sizeX = globalMax.X - globalMin.X;
        float sizeY = globalMax.Y - globalMin.Y;
        float sizeZ = globalMax.Z - globalMin.Z;
        float maxSize = MathF.Max(sizeX, MathF.Max(sizeY, sizeZ));

        if (maxSize < 1e-6f)
        {
            maxSize = 1.0f;
        }

        float padding = 1e-4f;
        globalMin -= new Vector3(padding, padding, padding);
        globalMax += new Vector3(padding, padding, padding);

        Vector3 center = new Vector3(
            (globalMin.X + globalMax.X) / 2f,
            (globalMin.Y + globalMax.Y) / 2f,
            (globalMin.Z + globalMax.Z) / 2f
        );

        Vector3 half = new Vector3(maxSize / 2f, maxSize / 2f, maxSize / 2f);

        return new Cube(center - half, center + half);
    }
}