using System;

public static class Intersection
{
    private const float EPS = 1e-8f;

    public static bool Intersects(Triangle tri, Cube cube)
    {
        var (triMin, triMax) = tri.GetBoundingBox();

        if (!AABBOverlap(triMin, triMax, cube.Min, cube.Max))
            return false;

        return TriangleAABB(tri, cube);
    }

    private static bool AABBOverlap(Vector3 min1, Vector3 max1, Vector3 min2, Vector3 max2)
    {
        return (min1.X <= max2.X + EPS && max1.X >= min2.X - EPS) &&
               (min1.Y <= max2.Y + EPS && max1.Y >= min2.Y - EPS) &&
               (min1.Z <= max2.Z + EPS && max1.Z >= min2.Z - EPS);
    }

    private static bool TriangleAABB(Triangle tri, Cube cube)
    {
        Vector3 c = cube.Center();
        Vector3 h = (cube.Max - cube.Min) * 0.5f;

        Vector3 v0 = tri.V1 - c;
        Vector3 v1 = tri.V2 - c;
        Vector3 v2 = tri.V3 - c;

        Vector3 e0 = v1 - v0;
        Vector3 e1 = v2 - v1;
        Vector3 e2 = v0 - v2;

        Vector3 normal = e0.Cross(e1);
        float normalLenSq = normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z;
        if (normalLenSq < EPS)
            return true;

        if (!TestAxis(e0, v0, v1, v2, h)) return false;
        if (!TestAxis(e1, v0, v1, v2, h)) return false;
        if (!TestAxis(e2, v0, v1, v2, h)) return false;

        if (!AxisOverlap(v0.X, v1.X, v2.X, h.X)) return false;
        if (!AxisOverlap(v0.Y, v1.Y, v2.Y, h.Y)) return false;
        if (!AxisOverlap(v0.Z, v1.Z, v2.Z, h.Z)) return false;

        if (!PlaneBoxOverlap(normal, v0, h)) return false;

        return true;
    }

    private static bool AxisOverlap(float v0, float v1, float v2, float h)
    {
        float min = MathF.Min(v0, MathF.Min(v1, v2));
        float max = MathF.Max(v0, MathF.Max(v1, v2));

        return !(min > h || max < -h);
    }

    private static bool TestAxis(Vector3 edge, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 h)
    {
        float lenSq = edge.X * edge.X + edge.Y * edge.Y + edge.Z * edge.Z;
        if (lenSq < EPS)
            return true;

        float p0, p1, p2, r;

        p0 = edge.Z * v0.Y - edge.Y * v0.Z;
        p1 = edge.Z * v1.Y - edge.Y * v1.Z;
        p2 = edge.Z * v2.Y - edge.Y * v2.Z;
        r = MathF.Abs(edge.Z) * h.Y + MathF.Abs(edge.Y) * h.Z;
        if (!Overlap(p0, p1, p2, r)) return false;

        p0 = edge.X * v0.Z - edge.Z * v0.X;
        p1 = edge.X * v1.Z - edge.Z * v1.X;
        p2 = edge.X * v2.Z - edge.Z * v2.X;
        r = MathF.Abs(edge.X) * h.Z + MathF.Abs(edge.Z) * h.X;
        if (!Overlap(p0, p1, p2, r)) return false;

        p0 = edge.Y * v0.X - edge.X * v0.Y;
        p1 = edge.Y * v1.X - edge.X * v1.Y;
        p2 = edge.Y * v2.X - edge.X * v2.Y;
        r = MathF.Abs(edge.Y) * h.X + MathF.Abs(edge.X) * h.Y;
        if (!Overlap(p0, p1, p2, r)) return false;

        return true;
    }

    private static bool Overlap(float p0, float p1, float p2, float r)
    {
        float min = MathF.Min(p0, MathF.Min(p1, p2));
        float max = MathF.Max(p0, MathF.Max(p1, p2));

        return !(min > r || max < -r);
    }

    private static bool PlaneBoxOverlap(Vector3 normal, Vector3 vert, Vector3 maxBox)
    {
        Vector3 vmin = new Vector3(
            normal.X > 0 ? -maxBox.X : maxBox.X,
            normal.Y > 0 ? -maxBox.Y : maxBox.Y,
            normal.Z > 0 ? -maxBox.Z : maxBox.Z
        );

        Vector3 vmax = new Vector3(
            normal.X > 0 ? maxBox.X : -maxBox.X,
            normal.Y > 0 ? maxBox.Y : -maxBox.Y,
            normal.Z > 0 ? maxBox.Z : -maxBox.Z
        );

        float dotMin = normal.Dot(vmin + vert);
        float dotMax = normal.Dot(vmax + vert);

        if (dotMin > 0) return false;
        if (dotMax >= 0) return true;

        return false;
    }
}