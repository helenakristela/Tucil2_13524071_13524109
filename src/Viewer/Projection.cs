using System.Drawing;

public static class Projection
{
    public static PointF Project(Vector3 v, int width, int height, float fov = 90f)
    {
        if (v.Z <= 0.001f) v = new Vector3(v.X, v.Y, 0.001f);

        float scale = 1f / MathF.Tan(fov * 0.5f * MathF.PI / 180f);

        float x = (v.X * scale) / v.Z;
        float y = (v.Y * scale) / v.Z;

        return new PointF(
            width / 2f + x * width / 2f,
            height / 2f - y * height / 2f
        );
    }
}