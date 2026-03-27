using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class ObjParser
{
    public static List<Triangle> Parse(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("File tidak ditemukan: " + path);

        var vertices = new List<Vector3>(8192);
        var triangles = new List<Triangle>(8192);

        int invalidVertexCount = 0;
        int invalidFaceCount = 0;

        foreach (var line in File.ReadLines(path))
        {
            var trimmed = line.Trim();

            if (string.IsNullOrEmpty(trimmed) || trimmed[0] == '#')
                continue;

            var parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            switch (parts[0])
            {
                case "v":
                    ParseVertex(parts, vertices, ref invalidVertexCount);
                    break;

                case "f":
                    if (!ParseFace(parts, vertices, triangles))
                        invalidFaceCount++;
                    break;

                default:
                    break;
            }
        }

        if (invalidVertexCount > 0)
        {
            Console.WriteLine($"Warning: {invalidVertexCount} invalid vertex skipped");
        }

        if (invalidFaceCount > 0)
        {
            Console.WriteLine($"Warning: {invalidFaceCount} invalid/unsupported faces skipped");
        }

        if (vertices.Count == 0)
        {
            Console.WriteLine("Warning: no valid vertices found");
        }

        return triangles;
    }

    private static void ParseVertex(string[] parts, List<Vector3> vertices, ref int invalidCount)
    {
        if (parts.Length < 4)
        {
            invalidCount++;
            return;
        }

        bool successX = float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float x);
        bool successY = float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float y);
        bool successZ = float.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out float z);

        if (!successX || !successY || !successZ)
        {
            invalidCount++;
            return;
        }

        vertices.Add(new Vector3(x, y, z));
    }

    private static bool ParseFace(string[] parts, List<Vector3> vertices, List<Triangle> triangles)
    {
        if (parts.Length != 4)
        {
            Console.WriteLine("Warning: non-triangle face ignored");
            return false;
        }

        if (!TryParseIndex(parts[1], vertices.Count, out int i1) ||
            !TryParseIndex(parts[2], vertices.Count, out int i2) ||
            !TryParseIndex(parts[3], vertices.Count, out int i3))
        {
            Console.WriteLine("Warning: invalid face index");
            return false;
        }

        if (!IsValidIndex(i1, vertices.Count) ||
            !IsValidIndex(i2, vertices.Count) ||
            !IsValidIndex(i3, vertices.Count))
        {
            Console.WriteLine("Warning: face index out of bounds");
            return false;
        }

        triangles.Add(new Triangle(
            vertices[i1],
            vertices[i2],
            vertices[i3]
        ));

        return true;
    }

    private static bool TryParseIndex(string token, int vertexCount, out int index)
    {
        index = -1;

        var split = token.Split('/');
        if (split.Length == 0) return false;

        if (!int.TryParse(split[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int rawIndex))
            return false;

        if (rawIndex <= 0)
            return false;

        index = rawIndex - 1;
        return true;
    }

    private static bool IsValidIndex(int i, int count)
    {
        return i >= 0 && i < count;
    }
}