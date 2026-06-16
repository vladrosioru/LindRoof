using System;

namespace LindRoof.toolbar;

[Flags]
public enum AllowedDockLocation : byte
{
	Top = 1,
	Left = 2,
	Bottom = 4,
	Right = 8,
	Floating = 0x10
}
