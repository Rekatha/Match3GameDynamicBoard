using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
	public int x;
	public int y;

	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	
	public void Multiply(int multiplier)
	{
		x *= multiplier;
		y *= multiplier;
	}

	public void Add(Point adder)
	{
		x += adder.x;
		y += adder.y;
	}

	public Vector2 ToVector()
	{
		return new Vector2(this.x, this.y);
	}

	public bool IsEquals(Point secondPoint)
	{
		return (this.x == secondPoint.x && this.y == secondPoint.y);
	}

	public static Point FromVector(Vector2 vector)
	{
		return new Point((int)vector.x, (int)vector.y);
	}

	public static Point FromVector(Vector3 vector)
	{
		return new Point((int)vector.x, (int)vector.y);
	}

	public static Point Multiply(Point point, int multiplier)
	{
		return new Point(point.x * multiplier, point.y * multiplier);
	}

	public static Point Add(Point firstPoint, Point secondPoint)
	{
		return new Point(firstPoint.x + secondPoint.x, firstPoint.y + secondPoint.y);
	}

	public static Point Clone(Point clonedPoint)
	{
		return new Point(clonedPoint.x, clonedPoint.y);
	}

	public static Point Zero
	{
		get { return new Point(0, 0); }
	}

	public static Point One
	{
		get { return new Point(1, 1); }
	}
	public static Point Up
	{
		get { return new Point(0, 1); }
	}

	public static Point Down
	{
		get { return new Point(0, -1); }
	}

	public static Point Right
	{
		get { return new Point(1, 0); }
	}

	public static Point Left
	{
		get { return new Point(-1, 0); }
	}
}
