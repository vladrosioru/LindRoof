using System.Collections;

namespace LindRoof;

public class EdgeType
{
	public string name;

	public double length;

	public int count;

	public ArrayList elemente;

	public EdgeType(string nam)
	{
		name = nam;
		elemente = new ArrayList();
	}
}
