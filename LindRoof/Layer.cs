using System;
using System.Drawing;

namespace LindRoof;

public class Layer : IDisposable, ICloneable
{
	public string nameOfLayer;

	public Color colorOfLayer;

	public Layer(string name, Color color)
	{
		nameOfLayer = name;
		colorOfLayer = color;
	}

	public void Dispose()
	{
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
