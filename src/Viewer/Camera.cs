using System;

public class Camera
{
    public float Yaw = 0;
    public float Pitch = 0;

    public float Distance = 8f;

    public Vector3 Transform(Vector3 v)
    {
        Vector3 p = new Vector3(v.X, v.Y, v.Z + Distance);

        float cosY = MathF.Cos(Yaw);
        float sinY = MathF.Sin(Yaw);
        float x1 = cosY * p.X + sinY * p.Z;
        float z1 = -sinY * p.X + cosY * p.Z;

        float cosP = MathF.Cos(Pitch);
        float sinP = MathF.Sin(Pitch);
        float y1 = cosP * p.Y - sinP * z1;
        float z2 = sinP * p.Y + cosP * z1;

        if (z2 < 0.1f)
            z2 = 0.1f;

        return new Vector3(x1, y1, z2);
    }

    public void Rotate(float dyaw, float dpitch)
    {
        Yaw += dyaw;
        Pitch += dpitch;

        float limit = MathF.PI / 2f - 0.01f;
        if (Pitch > limit) Pitch = limit;
        if (Pitch < -limit) Pitch = -limit;
    }

    public void Zoom(float amount)
    {
        Distance += amount;

        if (Distance < 1.0f)
            Distance = 1.0f;

        if (Distance > 100f)
            Distance = 100f;
    }
}