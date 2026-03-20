using System;

public class Vector3
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }

    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3 Add(Vector3 other)
    {
        return new Vector3(X + other.X, Y + other.Y, Z + other.Z);
    }

    public Vector3 Subtract(Vector3 other)
    {
        return new Vector3(X - other.X, Y - other.Y, Z - other.Z);
    }

    public Vector3 Multiply(float scalar)
    {
        return new Vector3(X * scalar, Y * scalar, Z * scalar);
    }

    public Vector3 Divide(float scalar)
    {
        if (scalar == 0)
            throw new DivideByZeroException("Scalar tidak boleh 0.");
        return new Vector3(X / scalar, Y / scalar, Z / scalar);
    }

    public float Dot(Vector3 other)
    {
        return X * other.X + Y * other.Y + Z * other.Z;
    }

    public Vector3 Cross(Vector3 other)
    {
        return new Vector3(
            Y * other.Z - Z * other.Y,
            Z * other.X - X * other.Z,
            X * other.Y - Y * other.X
        );
    }

    public float Length()
    {
        return MathF.Sqrt(X * X + Y * Y + Z * Z);
    }

    public Vector3 Normalize()
    {
        float len = Length();
        if (len == 0) return new Vector3(0, 0, 0);
        return Divide(len);
    }

    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return a.Add(b);
    }

    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return a.Subtract(b);
    }

    public static Vector3 operator *(Vector3 a, float scalar)
    {
        return a.Multiply(scalar);
    }

    public static Vector3 operator /(Vector3 a, float scalar)
    {
        return a.Divide(scalar);
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}