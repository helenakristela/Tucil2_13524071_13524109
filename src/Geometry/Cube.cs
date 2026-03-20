using System;
using System.Collections.Generic;

public class Cube
{
    public Vector3 Min { get; }
    public Vector3 Max { get; }

    public Cube(Vector3 min, Vector3 max)
    {
        if (min.X > max.X || min.Y > max.Y || min.Z > max.Z)
            throw new ArgumentException("Min harus lebih kecil atau sama dengan Max pada semua sumbu!");

        Min = min;
        Max = max;
    }

    public Vector3 Center()
    {
        return new Vector3(
            (Min.X + Max.X) / 2f,
            (Min.Y + Max.Y) / 2f,
            (Min.Z + Max.Z) / 2f
        );
    }

    public float Width()
    {
        return Max.X - Min.X;
    }

    public float Height()
    {
        return Max.Y - Min.Y;
    }

    public float Depth()
    {
        return Max.Z - Min.Z;
    }

    public List<Cube> SplitInto8()
    {
        Vector3 c = Center();

        return new List<Cube>
        {
            new Cube(
                new Vector3(Min.X, Min.Y, Min.Z),
                new Vector3(c.X, c.Y, c.Z)
            ),
            new Cube(
                new Vector3(c.X, Min.Y, Min.Z),
                new Vector3(Max.X, c.Y, c.Z)
            ),
            new Cube(
                new Vector3(Min.X, c.Y, Min.Z),
                new Vector3(c.X, Max.Y, c.Z)
            ),
            new Cube(
                new Vector3(c.X, c.Y, Min.Z),
                new Vector3(Max.X, Max.Y, c.Z)
            ),
            new Cube(
                new Vector3(Min.X, Min.Y, c.Z),
                new Vector3(c.X, c.Y, Max.Z)
            ),
            new Cube(
                new Vector3(c.X, Min.Y, c.Z),
                new Vector3(Max.X, c.Y, Max.Z)
            ),
            new Cube(
                new Vector3(Min.X, c.Y, c.Z),
                new Vector3(c.X, Max.Y, Max.Z)
            ),
            new Cube(
                new Vector3(c.X, c.Y, c.Z),
                new Vector3(Max.X, Max.Y, Max.Z)
            )
        };
    }

    public override string ToString()
    {
        return $"Cube(Min={Min}, Max={Max})";
    }
}