using System;
using System.Globalization;

namespace LindRoof;

[Serializable]
public class PointD : IDisposable, ICloneable
{
	protected bool disposed;

	private double _x;

	private double _y;

	public double X
	{
		get
		{
			return _x;
		}
		set
		{
			_x = value;
		}
	}

	public double Y
	{
		get
		{
			return _y;
		}
		set
		{
			_y = value;
		}
	}

	public PointD()
	{
	}

	public PointD(double x, double y)
	{
		_x = x;
		_y = y;
	}

	public override string ToString()
	{
		return " {X=" + _x + " Y=" + _y + "}";
	}

	public string ToString(CultureInfo culture)
	{
		return " {X=" + _x.ToString(culture) + " Y=" + _y.ToString(culture) + "}";
	}

	~PointD()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			disposed = true;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
