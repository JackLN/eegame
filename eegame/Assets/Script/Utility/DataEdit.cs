using UnityEngine;
using System.Collections;

using eegame;

namespace eegame
{
	public enum FileNameType
	{
		RESOURCE_NAME,
		RESOURCE_MUSIC,
		RESOURCE_TABLE,
		RESOURCE_PREFABS,
		NONE,
	}

	public class DataEdit
	{
		/// <summary>
		/// Formats the name of the file.
		/// </summary>
		public static string FormatFileName(string strFileName, FileNameType type)
		{
			string strResourcePath = getPathWithType (type);

			int index = strFileName.LastIndexOf('.');
			if (index >= 0)
			{
				strFileName = strFileName.Substring(0, index);
			}
						
			strFileName = strFileName.Replace('\\', '/');
			return strResourcePath+strFileName;			
		}

		private static string getPathWithType(FileNameType type)
		{
			string str = "";
			switch (type) 
			{
			case FileNameType.RESOURCE_NAME:

				break;
			case FileNameType.NONE:

				break;
			case FileNameType.RESOURCE_MUSIC:

				break;
			case FileNameType.RESOURCE_TABLE:
				str = System.Environment.CurrentDirectory+"/Data/";
				break;
			}
			return str;
		}
	}
}
