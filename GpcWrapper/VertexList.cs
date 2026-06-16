using System.Drawing;
using System.Drawing.Drawing2D;

namespace GpcWrapper;

public class VertexList
{
	public int NofVertices;

	public Vertex[] Vertex;

	public VertexList()
	{
	}

	public VertexList(PointF[] p)
	{
		NofVertices = p.Length;
		Vertex = new Vertex[NofVertices];
		for (int i = 0; i < p.Length; i++)
		{
			ref Vertex reference = ref Vertex[i];
			reference = new Vertex(p[i].X, p[i].Y);
		}
	}

	public GraphicsPath ToGraphicsPath()
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		graphicsPath.AddLines(ToPoints());
		return graphicsPath;
	}

	public PointF[] ToPoints()
	{
		PointF[] array = new PointF[NofVertices];
		for (int i = 0; i < NofVertices; i++)
		{
			ref PointF reference = ref array[i];
			reference = new PointF((float)Vertex[i].X, (float)Vertex[i].Y);
		}
		return array;
	}

	public GraphicsPath TristripToGraphicsPath()
	{
		GraphicsPath graphicsPath = new GraphicsPath();
		for (int i = 0; i < NofVertices - 2; i++)
		{
			graphicsPath.AddPolygon(new PointF[3]
			{
				new PointF((float)Vertex[i].X, (float)Vertex[i].Y),
				new PointF((float)Vertex[i + 1].X, (float)Vertex[i + 1].Y),
				new PointF((float)Vertex[i + 2].X, (float)Vertex[i + 2].Y)
			});
		}
		return graphicsPath;
	}

	public override string ToString()
	{
		string text = "Polygon with " + NofVertices + " vertices: ";
		for (int i = 0; i < NofVertices; i++)
		{
			text += Vertex[i].ToString();
			if (i != NofVertices - 1)
			{
				text += ",";
			}
		}
		return text;
	}
}
