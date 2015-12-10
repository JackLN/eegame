using UnityEngine;
using System.Collections;

using eegame;

namespace eegame
{
	public enum FileNameType
	{
		RESOURCE_NAME,
		RESOURCE_MUSIC_NAME,
		NONE,
	}

	public class DataEdit : Singleton<DataEdit>
	{
		public DataEdit()
		{

		}

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
			return strFileName;			
		}

		private static string getPathWithType(FileNameType type)
		{
			string str = "";
			switch (type) 
			{
			case FileNameType.RESOURCE_NAME:
				str = "Resource/";
				break;
			case FileNameType.NONE:
				str = "Resource/";
				break;
			case FileNameType.RESOURCE_MUSIC_NAME:
				str = "Resource/Music/";
				break;
			}
			return str;
		}
	}
}
