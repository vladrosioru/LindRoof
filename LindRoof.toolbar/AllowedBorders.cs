using System;

namespace LindRoof.toolbar;

[Flags]
public enum AllowedBorders
{
	None = 0,
	Top = 1,
	Left = 2,
	Bottom = 4,
	Right = 8,
	All = 0xF
}
