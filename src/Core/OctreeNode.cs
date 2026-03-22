using System.Collections.Generic;

public class OctreeNode
{
    public Cube Bounds { get; }
    public int Depth { get; }
    public OctreeNode[]? Children { get; private set; }

    public List<Triangle> Triangles { get; set; }

    public bool IsLeaf => Children == null;

    public OctreeNode(Cube bounds, int depth)
    {
        Bounds = bounds;
        Depth = depth;
        Children = null;

        Triangles = new List<Triangle>();
    }

    public void Split()
    {
        if (!IsLeaf) return;

        List<Cube> cubes = Bounds.SplitInto8();
        Children = new OctreeNode[8];

        for (int i = 0; i < 8; i++)
        {
            Children[i] = new OctreeNode(cubes[i], Depth + 1);
        }
    }

    public void BuildToDepth(int maxDepth)
    {
        if (Depth >= maxDepth) return;

        Split();

        if (Children == null) return;

        foreach (OctreeNode child in Children)
        {
            child.BuildToDepth(maxDepth);
        }
    }

    public bool ShouldSplit(int maxDepth, int minTriangles = 1)
    {
        return Depth < maxDepth && Triangles.Count > minTriangles;
    }

    public int CountNodes()
    {
        int total = 1;

        if (Children != null)
        {
            foreach (OctreeNode child in Children)
            {
                total += child.CountNodes();
            }
        }

        return total;
    }

    public int CountLeaves()
    {
        if (IsLeaf) return 1;

        int total = 0;
        foreach (OctreeNode child in Children!)
        {
            total += child.CountLeaves();
        }

        return total;
    }

    public Dictionary<int, int> CountNodesPerDepth()
    {
        var result = new Dictionary<int, int>();
        FillDepthStats(result);
        return result;
    }

    private void FillDepthStats(Dictionary<int, int> stats)
    {
        if (!stats.ContainsKey(Depth))
            stats[Depth] = 0;

        stats[Depth]++;

        if (Children != null)
        {
            foreach (OctreeNode child in Children)
            {
                child.FillDepthStats(stats);
            }
        }
    }
}