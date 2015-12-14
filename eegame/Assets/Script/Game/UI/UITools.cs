using UnityEngine;
using System.Collections.Generic;


//Layer层枚举
public enum LayerEnum
{
	// 主摄像机开启层
	Default = 0,
	TransparentFX = 1, // 透明层 主摄像机不显示
	IgnoreRaycast = 2,
	Water = 4,
	
	UILayer = 12, //UI
}

namespace eegame
{
	public class UITools
	{
		static public void setLayer(GameObject go, int layer)
		{
			go.layer = layer;
			foreach (Transform t in go.transform)
			{
				UITools.setLayer(t.gameObject, layer);
			}
		}
		
		static public void setParent(GameObject go, GameObject parent)
		{
			if (go == null || parent == null)
			{
				return;
			}
			
			if (go.transform.parent == parent)
			{
				return;
			}

			Transform t = go.transform;
			Vector3 localScale = t.localScale;
			Vector3 localPosition = t.localPosition;
			Quaternion localRotation = t.localRotation;
			
			t.parent = parent.transform;
			t.localPosition = localPosition;
			t.localRotation = localRotation;
			t.localScale = localScale;
		}
		
		static public T FindInParent<T>(GameObject go) where T : Component
		{
			if (go == null)
			{
				return null;
			}
			
			object[] comp = go.GetComponents<T>();
			
			if (comp.Length == 0)
			{
				Transform t = go.transform.parent;
				
				while (t != null && comp.Length == 0)
				{
					comp = t.gameObject.GetComponents<T>();
					t = t.parent;
				}
			}
			
			if (comp.Length == 0)
			{
				return null;
			}
			
			return (T)comp[0];
		}
		
		//创建一张图片
		static public Texture2D CreatTexture(Color color, Vector2 size)
		{
			Texture2D tex = new Texture2D((int)size.x, (int)size.y);
			for (int y = 0; y < tex.height; ++y)
			{
				for (int x = 0; x < tex.width; ++x)
				{
					tex.SetPixel(x, y, color);
				}
			}
			return tex;
		}
		
		//创建一张图片
		static public Texture2D CreatTexture(Color color, Vector2 size, TextureFormat fmt)
		{
			Texture2D tex = new Texture2D((int)size.x, (int)size.y, fmt, false);
			for (int y = 0; y < tex.height; ++y)
			{
				for (int x = 0; x < tex.width; ++x)
				{
					tex.SetPixel(x, y, color);
				}
			}
			return tex;
		}
		
		public static int SetBitValueTrue(int c, int b)
		{
			return c |= 1 << b;
		}
		
		static public T AddChild<T>(GameObject parent, GameObject prefab) where T : Component
		{
			T ts = prefab.GetComponent<T>();
			if (ts == null)
			{
				return null;
			}
			
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			#if UNITY_EDITOR
			UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
			#endif
			if (go != null && parent != null)
			{
				Transform t = go.transform;
				t.parent = parent.transform;
				t.localPosition = Vector3.zero;
				t.localRotation = Quaternion.identity;
				t.localScale = Vector3.one;
				go.layer = parent.layer;
			}
			T tsGO = go.GetComponent<T>();
			return tsGO;
		}
		
		// 获取屏幕宽
		public static int GetUIWidth(UIRoot root)
		{
			float s = (float)root.activeHeight / Screen.height;
			int width = Mathf.CeilToInt(Screen.width * s);
			return width;
		}
		
		// 获取屏幕高
		public static int GetUIHeight(UIRoot root)
		{
			float s = (float)root.activeHeight / Screen.height;
			int height = Mathf.CeilToInt(Screen.height * s);
			return height;
		}
		
	}

}
