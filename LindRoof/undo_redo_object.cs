namespace LindRoof;

public class undo_redo_object
{
	public string operation;

	public GraphicObject obj;

	public PointD coords1;

	public PointD coords2;

	public undo_redo_object(string op, GraphicObject ob)
	{
		operation = op;
		obj = ob;
		coords1 = (PointD)ob.startPoint.Clone();
		coords2 = (PointD)ob.stopPoint.Clone();
	}
}
