using System.Drawing;

public static class Renderer
{
    public static void DrawModel(Graphics g, Model model, Camera cam, int width, int height)
    {
        foreach (var face in model.Faces)
        {
            Vector3 v1 = cam.Transform(model.Vertices[face.Item1]);
            Vector3 v2 = cam.Transform(model.Vertices[face.Item2]);
            Vector3 v3 = cam.Transform(model.Vertices[face.Item3]);

            if (v1.Z < 0.1f || v2.Z < 0.1f || v3.Z < 0.1f)
                continue;

            Vector3 edge1 = v2 - v1;
            Vector3 edge2 = v3 - v1;
            Vector3 normal = edge1.Cross(edge2);

            // Vector3 viewDir = new Vector3(0, 0, 1);
            // if (normal.Dot(viewDir) >= 0)
                // continue;

            var p1 = Projection.Project(v1, width, height);
            var p2 = Projection.Project(v2, width, height);
            var p3 = Projection.Project(v3, width, height);

            if (!IsValid(p1) || !IsValid(p2) || !IsValid(p3))
                continue;

            g.DrawLine(Pens.Black, p1, p2);
            g.DrawLine(Pens.Black, p2, p3);
            g.DrawLine(Pens.Black, p3, p1);
        }
    }

    private static bool IsValid(PointF p)
    {
        return !(float.IsNaN(p.X) || float.IsNaN(p.Y) ||
                 float.IsInfinity(p.X) || float.IsInfinity(p.Y) ||
                 p.X < -1000 || p.X > 10000 ||
                 p.Y < -1000 || p.Y > 10000);
    }
}