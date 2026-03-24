using System;
using System.Collections.Generic;
using System.IO;

public static class ObjWriter
{
    public static void Write(string path, List<Cube> voxels)
    {
        using StreamWriter writer = new StreamWriter(path);

        int vertexOffset = 1;

        foreach (Cube cube in voxels)
        {
            List<Vector3> vertices = GetCubeVertices(cube);

            foreach (var v in vertices)
            {
                writer.WriteLine($"v {v.X} {v.Y} {v.Z}");
            }

            int[,] faces = new int[,]
            {
                {0,1,2}, {0,2,3}, 
                {4,5,6}, {4,6,7}, 
                {0,1,5}, {0,5,4}, 
                {2,3,7}, {2,7,6}, 
                {1,2,6}, {1,6,5}, 
                {0,3,7}, {0,7,4}  
            };

            for (int i = 0; i < faces.GetLength(0); i++)
            {
                int a = faces[i, 0] + vertexOffset;
                int b = faces[i, 1] + vertexOffset;
                int c = faces[i, 2] + vertexOffset;

                writer.WriteLine($"f {a} {b} {c}");
            }

            vertexOffset += 8;
        }
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