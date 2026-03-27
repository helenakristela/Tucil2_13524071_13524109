using System.Collections.Generic;

public static class Statistics
{
    public static int CountOccupiedLeaves(OctreeNode node)
    {
        if (node.IsLeaf)
        {
            return node.Triangles.Count > 0 ? 1 : 0;
        }

        int total = 0;

        if (node.Children != null)
        {
            foreach (OctreeNode child in node.Children)
            {
                total += CountOccupiedLeaves(child);
            }
        }

        return total;
    }

    public static int CountEmptyLeaves(OctreeNode node)
    {
        if (node.IsLeaf)
        {
            return node.Triangles.Count == 0 ? 1 : 0;
        }

        int total = 0;

        if (node.Children != null)
        {
            foreach (OctreeNode child in node.Children)
            {
                total += CountEmptyLeaves(child);
            }
        }

        return total;
    }

    public static int CountOccupiedNodes(OctreeNode node)
    {
        int total = node.Triangles.Count > 0 ? 1 : 0;

        if (node.Children != null)
        {
            foreach (OctreeNode child in node.Children)
            {
                total += CountOccupiedNodes(child);
            }
        }

        return total;
    }

    public static int GetMaxReachedDepth(OctreeNode node)
    {
        int maxDepth = node.Depth;

        if (node.Children != null)
        {
            foreach (OctreeNode child in node.Children)
            {
                int childDepth = GetMaxReachedDepth(child);
                if (childDepth > maxDepth)
                {
                    maxDepth = childDepth;
                }
            }
        }

        return maxDepth;
    }

    public static Dictionary<int, int> CountOccupiedLeavesPerDepth(OctreeNode node)
    {
        Dictionary<int, int> result = new Dictionary<int, int>();
        FillOccupiedLeavesPerDepth(node, result);
        return result;
    }

    private static void FillOccupiedLeavesPerDepth(OctreeNode node, Dictionary<int, int> stats)
    {
        if (node.IsLeaf && node.Triangles.Count > 0)
        {
            if (!stats.ContainsKey(node.Depth))
                stats[node.Depth] = 0;

            stats[node.Depth]++;
        }

        if (node.Children != null)
        {
            foreach (OctreeNode child in node.Children)
            {
                FillOccupiedLeavesPerDepth(child, stats);
            }
        }
    }

    public static float ComputeOccupancyRatio(OctreeNode root)
    {
        int occupied = CountOccupiedLeaves(root);
        int total = root.CountLeaves();

        if (total == 0) return 0;
        return (float)occupied / total;
    }

    public static Dictionary<int, int> CountPrunedNodesPerDepth(OctreeNode node)
    {
        Dictionary<int, int> result = new Dictionary<int, int>();
        FillPrunedNodes(node, result);
        return result;
    }

    private static void FillPrunedNodes(OctreeNode node, Dictionary<int, int> stats)
    {
        if (node.IsPruned)
        {
            if (!stats.ContainsKey(node.Depth))
                stats[node.Depth] = 0;

            stats[node.Depth]++;
        }

        if (node.Children != null)
        {
            foreach (OctreeNode child in node.Children)
            {
                FillPrunedNodes(child, stats);
            }
        }
    }
}