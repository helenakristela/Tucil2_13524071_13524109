using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public static class ObjWriter
{
    public static (int vertexCount, int faceCount) Write(string path, List<Cube> voxels)
    {
        using StreamWriter writer = new StreamWriter(path);

        int vertexOffset = 1;
        int faceCount = 0;

        HashSet<string> voxelSet = new HashSet<string>();
        foreach (var v in voxels)
        {
            voxelSet.Add(GetKey(v));
        }

        foreach (Cube cube in voxels)
        {
            List<Vector3> vertices = GetCubeVertices(cube);

            foreach (var v in vertices)
            {
                writer.WriteLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "v {0} {1} {2}",
                    v.X, v.Y, v.Z
                ));
            }

            float size = cube.Width();

            bool[] faceVisible = new bool[6];

            faceVisible[0] = !voxelSet.Contains(GetNeighborKey(cube, -size, 0, 0)); // left
            faceVisible[1] = !voxelSet.Contains(GetNeighborKey(cube,  size, 0, 0)); // right
            faceVisible[2] = !voxelSet.Contains(GetNeighborKey(cube, 0, -size, 0)); // bottom
            faceVisible[3] = !voxelSet.Contains(GetNeighborKey(cube, 0,  size, 0)); // top
            faceVisible[4] = !voxelSet.Contains(GetNeighborKey(cube, 0, 0, -size)); // back
            faceVisible[5] = !voxelSet.Contains(GetNeighborKey(cube, 0, 0,  size)); // front

            int[][][] faces = new int[][][]
            {
                new int[][] { new int[]{0,3,7}, new int[]{0,7,4} }, // left
                new int[][] { new int[]{1,2,6}, new int[]{1,6,5} }, // right
                new int[][] { new int[]{0,1,5}, new int[]{0,5,4} }, // bottom
                new int[][] { new int[]{2,3,7}, new int[]{2,7,6} }, // top
                new int[][] { new int[]{0,1,2}, new int[]{0,2,3} }, // back
                new int[][] { new int[]{4,5,6}, new int[]{4,6,7} }  // front
            };

            for (int f = 0; f < 6; f++)
            {
                if (!faceVisible[f]) continue;

                foreach (var tri in faces[f])
                {
                    int a = tri[0] + vertexOffset;
                    int b = tri[1] + vertexOffset;
                    int c = tri[2] + vertexOffset;

                    writer.WriteLine($"f {a} {b} {c}");
                    faceCount++;
                }
            }

            vertexOffset += 8;
        }

        int vertexCount = voxels.Count * 8;
        return (vertexCount, faceCount);
    }

    private static string GetKey(Cube c)
    {
        var center = c.Center();
        return string.Format(
            CultureInfo.InvariantCulture,
            "{0:F6}_{1:F6}_{2:F6}",
            center.X, center.Y, center.Z
        );
    }

    private static string GetNeighborKey(Cube c, float dx, float dy, float dz)
    {
        var center = c.Center();
        return string.Format(
            CultureInfo.InvariantCulture,
            "{0:F6}_{1:F6}_{2:F6}",
            center.X + dx,
            center.Y + dy,
            center.Z + dz
        );
    }

    private static List<Vector3> GetCubeVertices(Cube c)
    {
        Vector3 min = c.Min;
        Vector3 max = c.Max;

        return new List<Vector3>
        {
            new Vector3(min.X, min.Y, min.Z),
            new Vector3(max.X, min.Y, min.Z),
            new Vector3(max.X, max.Y, min.Z),
            new Vector3(min.X, max.Y, min.Z),
            new Vector3(min.X, min.Y, max.Z),
            new Vector3(max.X, min.Y, max.Z),
            new Vector3(max.X, max.Y, max.Z),
            new Vector3(min.X, max.Y, max.Z)
        };
    }
}