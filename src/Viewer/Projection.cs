using System.Drawing;

public static class Projection
{
    public static PointF Project(Vector3 v, int width, int height, float fov = 90f)
    {
        if (v.Z <= 0.1f)
            return new PointF(float.NaN, float.NaN);

        float fovRad = fov * MathF.PI / 180f;
        float scale = 1f / MathF.Tan(fovRad * 0.5f);

        float aspectRatio = (float)width / height;

        float invZ = 1f / v.Z;

        float x = v.X * scale * invZ / aspectRatio;
        float y = v.Y * scale * invZ;

        float screenX = (x + 1f) * 0.5f * width;
        float screenY = (1f - (y + 1f) * 0.5f) * height;

        return new PointF(screenX, screenY);
    }
}