using System;

public class Triangle
{
    public Vector3 V1 { get; }
    public Vector3 V2 { get; }
    public Vector3 V3 { get; }

    public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        V1 = v1;
        V2 = v2;
        V3 = v3;
    }

    public Vector3 Centroid()
    {
        return new Vector3(
            (V1.X + V2.X + V3.X) / 3f,
            (V1.Y + V2.Y + V3.Y) / 3f,
            (V1.Z + V2.Z + V3.Z) / 3f
        );
    }

    public (Vector3 min, Vector3 max) GetBoundingBox()
    {
        float minX = Math.Min(V1.X, Math.Min(V2.X, V3.X));
        float minY = Math.Min(V1.Y, Math.Min(V2.Y, V3.Y));
        float minZ = Math.Min(V1.Z, Math.Min(V2.Z, V3.Z));

        float maxX = Math.Max(V1.X, Math.Max(V2.X, V3.X));
        float maxY = Math.Max(V1.Y, Math.Max(V2.Y, V3.Y));
        float maxZ = Math.Max(V1.Z, Math.Max(V2.Z, V3.Z));

        return (
            new Vector3(minX, minY, minZ),
            new Vector3(maxX, maxY, maxZ)
        );
    }

    public override string ToString()
    {
        return $"[{V1}, {V2}, {V3}]";
    }
}