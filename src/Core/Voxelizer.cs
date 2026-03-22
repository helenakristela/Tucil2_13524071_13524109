using System;
using System.Collections.Generic;

public class Voxelizer
{
    public static void Build(OctreeNode node, List<Triangle> triangles, int maxDepth)
    {
        List<Triangle> intersecting = GetIntersectingTriangles(node.Bounds, triangles);
        node.Triangles = intersecting;

        if (ShouldStop(node, maxDepth))
            return;

        node.Split();

        if (node.Children == null)
            return;

        foreach (OctreeNode child in node.Children)
        {
            Build(child, intersecting, maxDepth);
        }
    }

    private static List<Triangle> GetIntersectingTriangles(Cube cube, List<Triangle> triangles)
    {
        List<Triangle> intersecting = new List<Triangle>();

        foreach (Triangle tri in triangles)
        {
            if (Intersection.Intersects(tri, cube))
            {
                intersecting.Add(tri);
            }
        }

        return intersecting;
    }

    private static bool ShouldStop(OctreeNode node, int maxDepth)
    {
        if (node.Depth >= maxDepth)
            return true;

        if (node.Triangles.Count == 0)
            return true;

        if (IsCubeTooSmall(node.Bounds))
            return true;

        return false;
    }

    private static bool IsCubeTooSmall(Cube cube, float minSize = 1e-4f)
    {
        return cube.Width() <= minSize ||
               cube.Height() <= minSize ||
               cube.Depth() <= minSize;
    }

    public static List<Cube> CollectLeafVoxels(OctreeNode root)
    {
        List<Cube> voxels = new List<Cube>();
        CollectLeafVoxelsRecursive(root, voxels);
        return voxels;
    }

    private static void CollectLeafVoxelsRecursive(OctreeNode node, List<Cube> voxels)
    {
        if (node.IsLeaf)
        {
            if (node.Triangles.Count > 0)
            {
                voxels.Add(node.Bounds);
            }
            return;
        }

        if (node.Children != null)
        {
            foreach (OctreeNode child in node.Children)
            {
                CollectLeafVoxelsRecursive(child, voxels);
            }
        }
    }

    public static List<OctreeNode> CollectLeafNodes(OctreeNode root)
    {
        List<OctreeNode> leaves = new List<OctreeNode>();
        CollectLeafNodesRecursive(root, leaves);
        return leaves;
    }

    private static void CollectLeafNodesRecursive(OctreeNode node, List<OctreeNode> leaves)
    {
        if (node.IsLeaf)
        {
            leaves.Add(node);
            return;
        }

        if (node.Children != null)
        {
            foreach (OctreeNode child in node.Children)
            {
                CollectLeafNodesRecursive(child, leaves);
            }
        }
    }

    public static List<OctreeNode> CollectOccupiedLeafNodes(OctreeNode root)
    {
        List<OctreeNode> leaves = new List<OctreeNode>();
        CollectOccupiedLeafNodesRecursive(root, leaves);
        return leaves;
    }

    private static void CollectOccupiedLeafNodesRecursive(OctreeNode node, List<OctreeNode> leaves)
    {
        if (node.IsLeaf)
        {
            if (node.Triangles.Count > 0)
            {
                leaves.Add(node);
            }
            return;
        }

        if (node.Children != null)
        {
            foreach (OctreeNode child in node.Children)
            {
                CollectOccupiedLeafNodesRecursive(child, leaves);
            }
        }
    }
}