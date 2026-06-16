using System;
using System.Collections;
using Microsoft.Win32;

namespace LindRoof;

public class FileAssociation
{
	private FileType FileInfo;

	public string ProperName
	{
		get
		{
			return FileInfo.ProperName;
		}
		set
		{
			FileInfo.ProperName = value;
		}
	}

	public string FullName
	{
		get
		{
			return FileInfo.FullName;
		}
		set
		{
			FileInfo.FullName = value;
		}
	}

	public string ContentType
	{
		get
		{
			return FileInfo.ContentType;
		}
		set
		{
			FileInfo.ContentType = value;
		}
	}

	public string Extension
	{
		get
		{
			return FileInfo.Extension;
		}
		set
		{
			if (value.Substring(0, 1) != ".")
			{
				value = "." + value;
			}
			FileInfo.Extension = value;
		}
	}

	public short IconIndex
	{
		get
		{
			return FileInfo.IconIndex;
		}
		set
		{
			FileInfo.IconIndex = value;
		}
	}

	public string IconPath
	{
		get
		{
			return FileInfo.IconPath;
		}
		set
		{
			FileInfo.IconPath = value;
		}
	}

	public FileAssociation()
	{
		FileInfo = default(FileType);
		FileInfo.Commands.Captions = new ArrayList();
		FileInfo.Commands.Commands = new ArrayList();
	}

	public void AddCommand(string Caption, string Command)
	{
		if (Caption == null || Command == null)
		{
			throw new ArgumentNullException();
		}
		FileInfo.Commands.Captions.Add(Caption);
		FileInfo.Commands.Commands.Add(Command);
	}

	public void Create()
	{
		try
		{
			Remove();
		}
		catch (ArgumentException)
		{
		}
		if (Extension == "" || ProperName == "")
		{
			throw new ArgumentException();
		}
		try
		{
			RegistryKey registryKey = Registry.ClassesRoot.CreateSubKey(Extension);
			registryKey.SetValue("", ProperName);
			if (ContentType != null && ContentType != "")
			{
				registryKey.SetValue("Content Type", ContentType);
			}
			registryKey.Close();
			registryKey = Registry.ClassesRoot.CreateSubKey(ProperName);
			registryKey.SetValue("", FullName);
			registryKey.Close();
			if (IconPath != "")
			{
				registryKey = Registry.ClassesRoot.CreateSubKey(ProperName + "\\DefaultIcon");
				registryKey.SetValue("", IconPath + "," + IconIndex);
				registryKey.Close();
			}
			for (int i = 0; i < FileInfo.Commands.Captions.Count; i++)
			{
				registryKey = Registry.ClassesRoot.CreateSubKey(ProperName + "\\Shell\\" + (string)FileInfo.Commands.Captions[i]);
				registryKey = registryKey.CreateSubKey("Command");
				registryKey.SetValue("", FileInfo.Commands.Commands[i]);
				registryKey.Close();
			}
		}
		catch
		{
		}
	}

	public void Remove()
	{
		if (Extension == null || ProperName == null)
		{
			throw new ArgumentNullException();
		}
		if (Extension == "" || ProperName == "")
		{
			throw new ArgumentException();
		}
		try
		{
			Registry.ClassesRoot.DeleteSubKeyTree(Extension);
			Registry.ClassesRoot.DeleteSubKeyTree(ProperName);
		}
		catch
		{
		}
	}
}
