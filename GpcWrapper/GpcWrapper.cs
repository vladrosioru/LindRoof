using System;
using System.Runtime.InteropServices;

namespace GpcWrapper;

public class GpcWrapper
{
	private enum gpc_op
	{
		GPC_DIFF,
		GPC_INT,
		GPC_XOR,
		GPC_UNION
	}

	private struct gpc_vertex
	{
		public double x;

		public double y;
	}

	private struct gpc_vertex_list
	{
		public int num_vertices;

		public IntPtr vertex;
	}

	private struct gpc_polygon
	{
		public int num_contours;

		public IntPtr hole;

		public IntPtr contour;
	}

	private struct gpc_tristrip
	{
		public int num_strips;

		public IntPtr strip;
	}

	public static Tristrip PolygonToTristrip(Polygon polygon)
	{
		gpc_tristrip tristrip = default(gpc_tristrip);
		gpc_polygon polygon2 = PolygonTo_gpc_polygon(polygon);
		gpc_polygon_to_tristrip(ref polygon2, ref tristrip);
		Tristrip result = gpc_strip_ToTristrip(tristrip);
		Free_gpc_polygon(polygon2);
		gpc_free_tristrip(ref tristrip);
		return result;
	}

	public static Tristrip ClipToTristrip(GpcOperation operation, Polygon subject_polygon, Polygon clip_polygon)
	{
		gpc_tristrip result_tristrip = default(gpc_tristrip);
		gpc_polygon subject_polygon2 = PolygonTo_gpc_polygon(subject_polygon);
		gpc_polygon clip_polygon2 = PolygonTo_gpc_polygon(clip_polygon);
		gpc_tristrip_clip(operation, ref subject_polygon2, ref clip_polygon2, ref result_tristrip);
		Tristrip result = gpc_strip_ToTristrip(result_tristrip);
		Free_gpc_polygon(subject_polygon2);
		Free_gpc_polygon(clip_polygon2);
		gpc_free_tristrip(ref result_tristrip);
		return result;
	}

	public static Polygon Clip(GpcOperation operation, Polygon subject_polygon, Polygon clip_polygon)
	{
		gpc_polygon result_polygon = default(gpc_polygon);
		gpc_polygon subject_polygon2 = PolygonTo_gpc_polygon(subject_polygon);
		gpc_polygon clip_polygon2 = PolygonTo_gpc_polygon(clip_polygon);
		gpc_polygon_clip(operation, ref subject_polygon2, ref clip_polygon2, ref result_polygon);
		Polygon result = gpc_polygon_ToPolygon(result_polygon);
		Free_gpc_polygon(subject_polygon2);
		Free_gpc_polygon(clip_polygon2);
		gpc_free_polygon(ref result_polygon);
		return result;
	}

	public static void SavePolygon(string filename, bool writeHoleFlags, Polygon polygon)
	{
		gpc_polygon polygon2 = PolygonTo_gpc_polygon(polygon);
		IntPtr fp = fopen(filename, "wb");
		gpc_write_polygon(fp, writeHoleFlags ? 1 : 0, ref polygon2);
		fclose(fp);
		Free_gpc_polygon(polygon2);
	}

	public static Polygon ReadPolygon(string filename, bool readHoleFlags)
	{
		gpc_polygon polygon = default(gpc_polygon);
		IntPtr fp = fopen(filename, "rb");
		gpc_read_polygon(fp, readHoleFlags ? 1 : 0, ref polygon);
		Polygon result = gpc_polygon_ToPolygon(polygon);
		gpc_free_polygon(ref polygon);
		fclose(fp);
		return result;
	}

	private static gpc_polygon PolygonTo_gpc_polygon(Polygon polygon)
	{
		gpc_polygon result = new gpc_polygon
		{
			num_contours = polygon.NofContours
		};
		int[] array = new int[polygon.NofContours];
		for (int i = 0; i < polygon.NofContours; i++)
		{
			array[i] = (polygon.ContourIsHole[i] ? 1 : 0);
		}
		result.hole = Marshal.AllocCoTaskMem(polygon.NofContours * Marshal.SizeOf((object)array[0]));
		if (polygon.NofContours > 0)
		{
			Marshal.Copy(array, 0, result.hole, polygon.NofContours);
			result.contour = Marshal.AllocCoTaskMem(polygon.NofContours * Marshal.SizeOf((object)default(gpc_vertex_list)));
		}
		IntPtr intPtr = result.contour;
		for (int j = 0; j < polygon.NofContours; j++)
		{
			gpc_vertex_list gpc_vertex_list2 = new gpc_vertex_list
			{
				num_vertices = polygon.Contour[j].NofVertices,
				vertex = Marshal.AllocCoTaskMem(polygon.Contour[j].NofVertices * Marshal.SizeOf((object)default(gpc_vertex)))
			};
			IntPtr intPtr2 = gpc_vertex_list2.vertex;
			for (int k = 0; k < polygon.Contour[j].NofVertices; k++)
			{
				gpc_vertex gpc_vertex2 = new gpc_vertex
				{
					x = polygon.Contour[j].Vertex[k].X,
					y = polygon.Contour[j].Vertex[k].Y
				};
				Marshal.StructureToPtr((object)gpc_vertex2, intPtr2, false);
				intPtr2 = (IntPtr)((int)intPtr2 + Marshal.SizeOf((object)gpc_vertex2));
			}
			Marshal.StructureToPtr((object)gpc_vertex_list2, intPtr, false);
			intPtr = (IntPtr)((int)intPtr + Marshal.SizeOf((object)gpc_vertex_list2));
		}
		return result;
	}

	private static Polygon gpc_polygon_ToPolygon(gpc_polygon gpc_polygon)
	{
		Polygon polygon = new Polygon();
		polygon.NofContours = gpc_polygon.num_contours;
		polygon.ContourIsHole = new bool[polygon.NofContours];
		polygon.Contour = new VertexList[polygon.NofContours];
		int[] array = new int[polygon.NofContours];
		IntPtr hole = gpc_polygon.hole;
		if (polygon.NofContours > 0)
		{
			Marshal.Copy(gpc_polygon.hole, array, 0, polygon.NofContours);
		}
		for (int i = 0; i < polygon.NofContours; i++)
		{
			polygon.ContourIsHole[i] = array[i] != 0;
		}
		hole = gpc_polygon.contour;
		for (int j = 0; j < polygon.NofContours; j++)
		{
			gpc_vertex_list gpc_vertex_list2 = (gpc_vertex_list)Marshal.PtrToStructure(hole, typeof(gpc_vertex_list));
			polygon.Contour[j] = new VertexList();
			polygon.Contour[j].NofVertices = gpc_vertex_list2.num_vertices;
			polygon.Contour[j].Vertex = new Vertex[polygon.Contour[j].NofVertices];
			IntPtr intPtr = gpc_vertex_list2.vertex;
			for (int k = 0; k < polygon.Contour[j].NofVertices; k++)
			{
				gpc_vertex gpc_vertex2 = (gpc_vertex)Marshal.PtrToStructure(intPtr, typeof(gpc_vertex));
				polygon.Contour[j].Vertex[k].X = gpc_vertex2.x;
				polygon.Contour[j].Vertex[k].Y = gpc_vertex2.y;
				intPtr = (IntPtr)((int)intPtr + Marshal.SizeOf((object)gpc_vertex2));
			}
			hole = (IntPtr)((int)hole + Marshal.SizeOf((object)gpc_vertex_list2));
		}
		return polygon;
	}

	private static Tristrip gpc_strip_ToTristrip(gpc_tristrip gpc_strip)
	{
		Tristrip tristrip = new Tristrip();
		tristrip.NofStrips = gpc_strip.num_strips;
		tristrip.Strip = new VertexList[tristrip.NofStrips];
		IntPtr intPtr = gpc_strip.strip;
		for (int i = 0; i < tristrip.NofStrips; i++)
		{
			tristrip.Strip[i] = new VertexList();
			gpc_vertex_list gpc_vertex_list2 = (gpc_vertex_list)Marshal.PtrToStructure(intPtr, typeof(gpc_vertex_list));
			tristrip.Strip[i].NofVertices = gpc_vertex_list2.num_vertices;
			tristrip.Strip[i].Vertex = new Vertex[tristrip.Strip[i].NofVertices];
			IntPtr intPtr2 = gpc_vertex_list2.vertex;
			for (int j = 0; j < tristrip.Strip[i].NofVertices; j++)
			{
				gpc_vertex gpc_vertex2 = (gpc_vertex)Marshal.PtrToStructure(intPtr2, typeof(gpc_vertex));
				tristrip.Strip[i].Vertex[j].X = gpc_vertex2.x;
				tristrip.Strip[i].Vertex[j].Y = gpc_vertex2.y;
				intPtr2 = (IntPtr)((int)intPtr2 + Marshal.SizeOf((object)gpc_vertex2));
			}
			intPtr = (IntPtr)((int)intPtr + Marshal.SizeOf((object)gpc_vertex_list2));
		}
		return tristrip;
	}

	private static void Free_gpc_polygon(gpc_polygon gpc_pol)
	{
		Marshal.FreeCoTaskMem(gpc_pol.hole);
		IntPtr intPtr = gpc_pol.contour;
		for (int i = 0; i < gpc_pol.num_contours; i++)
		{
			gpc_vertex_list gpc_vertex_list2 = (gpc_vertex_list)Marshal.PtrToStructure(intPtr, typeof(gpc_vertex_list));
			Marshal.FreeCoTaskMem(gpc_vertex_list2.vertex);
			intPtr = (IntPtr)((int)intPtr + Marshal.SizeOf((object)gpc_vertex_list2));
		}
		Marshal.FreeCoTaskMem(gpc_pol.contour);
	}

	[DllImport("gpc.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void gpc_polygon_to_tristrip([In] ref gpc_polygon polygon, [In][Out] ref gpc_tristrip tristrip);

	[DllImport("gpc.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void gpc_polygon_clip([In] GpcOperation set_operation, [In] ref gpc_polygon subject_polygon, [In] ref gpc_polygon clip_polygon, [In][Out] ref gpc_polygon result_polygon);

	[DllImport("gpc.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void gpc_tristrip_clip([In] GpcOperation set_operation, [In] ref gpc_polygon subject_polygon, [In] ref gpc_polygon clip_polygon, [In][Out] ref gpc_tristrip result_tristrip);

	[DllImport("gpc.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void gpc_free_tristrip([In] ref gpc_tristrip tristrip);

	[DllImport("gpc.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void gpc_free_polygon([In] ref gpc_polygon polygon);

	[DllImport("gpc.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void gpc_read_polygon([In] IntPtr fp, [In] int read_hole_flags, [In][Out] ref gpc_polygon polygon);

	[DllImport("gpc.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void gpc_write_polygon([In] IntPtr fp, [In] int write_hole_flags, [In] ref gpc_polygon polygon);

	[DllImport("msvcr71.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern IntPtr fopen([In] string filename, [In] string mode);

	[DllImport("msvcr71.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern void fclose([In] IntPtr fp);

	[DllImport("msvcr71.dll", CallingConvention = CallingConvention.Cdecl)]
	private static extern int fputc([In] int c, [In] IntPtr fp);
}
