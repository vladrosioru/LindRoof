namespace GpcWrapper;

public struct Vertex(double x, double y)
{
	public double X = x;

	public double Y = y;

	public override string ToString()
	{
		return "(" + X + "," + Y + ")";
	}
}
