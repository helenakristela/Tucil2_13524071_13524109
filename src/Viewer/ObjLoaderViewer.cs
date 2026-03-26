using System;
using System.IO;
using System.Globalization;

public static class ObjLoaderViewer
{
    public static Model Load(string path)
    {
        Model model = new Model();

        foreach (var line in File.ReadLines(path))
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            if (parts[0] == "v")
            {
                float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                model.Vertices.Add(new Vector3(x, y, z));
            }
            else if (parts[0] == "f" && parts.Length >= 4)
            {
                int a = int.Parse(parts[1].Split('/')[0]) - 1;
                int b = int.Parse(parts[2].Split('/')[0]) - 1;
                int c = int.Parse(parts[3].Split('/')[0]) - 1;
                model.Faces.Add((a, b, c));
            }
        }

        return model;
    }
}