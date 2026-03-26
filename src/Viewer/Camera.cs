using System;

public class Camera
{
    public float Yaw = 0;
    public float Pitch = 0;
    public float Distance = 3f;

    public Vector3 Transform(Vector3 v)
    {
        // Translate (move object away from camera)
        Vector3 p = new Vector3(v.X, v.Y, v.Z + Distance);

        // Rotate Y (Yaw)
        float cosY = MathF.Cos(Yaw);
        float sinY = MathF.Sin(Yaw);
        float x1 = cosY * p.X + sinY * p.Z;
        float z1 = -sinY * p.X + cosY * p.Z;

        // Rotate X (Pitch)
        float cosP = MathF.Cos(Pitch);
        float sinP = MathF.Sin(Pitch);
        float y1 = cosP * p.Y - sinP * z1;
        float z2 = sinP * p.Y + cosP * z1;

        return new Vector3(x1, y1, z2);
    }

    public void Rotate(float dyaw, float dpitch)
    {
        Yaw += dyaw;
        Pitch += dpitch;
    }

    public void Zoom(float amount)
    {
        Distance += amount;
        if (Distance < 0.5f) Distance = 0.5f;
    }
}