using System;
using System.Collections.Generic;

public class Voxelizer
{
    public static void Build(OctreeNode node, List<Triangle> triangles, int maxDepth)
    {
        List<Triangle> intersecting = new List<Triangle>();

        foreach (var tri in triangles)
        {
            if (Intersection.Intersects(tri, node.Bounds))
            {
                intersecting.Add(tri);
            }
        }

        node.Triangles = intersecting;

        Console.WriteLine($"Depth {node.Depth}, Triangles: {intersecting.Count}");

        if (node.Depth >= maxDepth)
            return;

        if (intersecting.Count == 0)
            return;

        node.Split();

        if (node.Children == null)
            return;

        foreach (var child in node.Children)
        {
            Build(child, intersecting, maxDepth);
        }
    }
}