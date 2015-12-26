using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using eegame;

namespace eegame{

	public class TableManager : Singleton<TableManager> 
	{
		private const string Data_End = ".dat";

		public TableManager()
		{

		}

		public List<T> GetTableWithList<T>()
		{
			string file_path = DataEdit.FormatFileName(typeof(T).Name,FileNameType.RESOURCE_TABLE)+Data_End;
			if (File.Exists (file_path))
			{
				using (var reader = new BinaryReader(File.OpenRead(file_path)))
				{
					var table = reader.ReadMarshalToList<T>();
					return table;
				}
				
			} 
			else {
				Debug.LogError("Data Not Exsit : "+file_path);
			}

			return null;
		}

		public Dictionary<uint,T> GetTableWithDic<T>() where T : IDicTable
		{
			string file_path = DataEdit.FormatFileName(typeof(T).Name,FileNameType.RESOURCE_TABLE)+Data_End;
			if (File.Exists (file_path))
			{
				using (var reader = new BinaryReader(File.OpenRead(file_path)))
				{
					var table = reader.ReadMarshalToDic<T>();
					return table;
				}
				
			} 
			else {
				Debug.LogError("Data Not Exsit : "+file_path);
			}

			return null;
		}

	}
}

