using System.Drawing;
using LindRoof;

public struct CaracteristiciPanou
{
	public double streasina = 0.0;

	public double pasOndula = 0.0;

	public double latimeFoaie = 0.0;

	public double petrecereFoi = 0.0;

	public string tipTigla = "";

	public bool optimizareStandard = true;

	public bool optimizareAjustabile = true;

	public bool variatorPasOndula = false;

	public int nrMinimOndule = 0;

	public int nrMaximOndule = 0;

	public double offsetMozaic = 0.0;

	public double decalajVecin = 0.0;

	public string observatii = "";

	public Layer panelLayer = new Layer("Panou NORDIC 400", Color.Red);

	public Layer onduleLayer = new Layer("OnduleLine", Color.Gray);

	public Layer selectionLayer = new Layer("Selection", Color.Green);

	public Layer borderMoveLayer = new Layer("MoveBorder", Color.Green);

	public StringFormat sf = new StringFormat();

	public Font txtFont = new Font("Arial Narrow", 10f);

	public CaracteristiciPanou(bool nimic)
	{
		sf.Alignment = StringAlignment.Near;
		sf.LineAlignment = StringAlignment.Far;
	}
}
