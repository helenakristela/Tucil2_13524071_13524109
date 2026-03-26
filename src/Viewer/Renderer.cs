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

            var p1 = Projection.Project(v1, width, height);
            var p2 = Projection.Project(v2, width, height);
            var p3 = Projection.Project(v3, width, height);

            g.DrawLine(Pens.Black, p1, p2);
            g.DrawLine(Pens.Black, p2, p3);
            g.DrawLine(Pens.Black, p3, p1);
        }
    }
}