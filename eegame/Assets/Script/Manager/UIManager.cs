using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

using cctween;
using eegame;

namespace eegame
{
	public class UIManager : MonoSingleton<UIManager>
	{
		public static UIRoot m_Root;
		public static Camera m_Camera;
		public static UICamera m_UICamera;
		public static GameObject m_RootPanel;
		public static UIGrayLayer m_GrayLayer = null;

		public class UIResInfo
		{
			public string name;
			public string path;
			public uint uiMark;
			public UnityEngine.Object prefab = null;
			public List<GameObject> objList = new List<GameObject>();
		}

		private static Dictionary<string, UIResInfo> m_resInfoDict = new Dictionary<string, UIResInfo>();

		private void registAll()
		{
			registerResPath<UIGrayLayer>("UIPrefabs/UIGrayLayer", 1000);
		}

		private void Start()
		{
			DontDestroyOnLoad(gameObject);
		}

		public void Init()
		{
			m_Root = gameObject.GetComponent<UIRoot>();
			m_Camera = transform.Find("Camera").GetComponent<Camera>();
			m_UICamera = m_Camera.GetComponent<UICamera>();
			m_RootPanel = transform.Find("Camera/Panel").gameObject;
			registAll ();
		}

		public static void registerResPath<T>(string path, uint mark) where T : UIBaseWindow
		{
			string className = typeof(T).ToString();
			if (m_resInfoDict.ContainsKey(className))
			{
				return;
			}
			UIResInfo resInfo = new UIResInfo();
			resInfo.name = className;
			resInfo.uiMark = mark;
			resInfo.path = path;
			resInfo.prefab = null;
			m_resInfoDict[className] = resInfo;
		}

		public static UIRoot getUIRoot()
		{
			return m_Root;
		}

		public static UIGrayLayer getGrayLayer()
		{
			if (m_GrayLayer == null) 
			{
				m_GrayLayer = createWindow<UIGrayLayer>();
			}
			return m_GrayLayer;
		}
		
		public static GameObject getRootPanle()
		{
			return m_RootPanel;
		}
		
		public static void addCameraCullingMask(string layerName)
		{
			m_Camera.cullingMask |= (1 << LayerMask.NameToLayer(layerName));
		}
		
		public static void delCameraCullingMask(string layerName)
		{
			m_Camera.cullingMask &= ~(1 << LayerMask.NameToLayer(layerName));
		}
		
		public static void SetUILayer()
		{
			m_UICamera.eventReceiverMask = (1 << (int)LayerEnum.UILayer);
		}
		
		public static void SetUnUILayer()
		{

		}
		
		// 获取屏幕高
		public static int getHeight()
		{
			float s = (float)m_Root.activeHeight / Screen.height;
			int height = Mathf.CeilToInt(Screen.height * s);
			return height;
		}
		
		// 获取屏幕宽
		public static int getWidth()
		{
			float s = (float)m_Root.activeHeight / Screen.height;
			int width = Mathf.CeilToInt(Screen.width * s);
			return width;
		}
		
		// 获取最深的子节点
		public static int getMaxDepth(GameObject obj)
		{
			int depth = int.MinValue;
			
			UIPanel[] panels = obj.GetComponentsInChildren<UIPanel>(false);
			for (int i = 0; i < panels.Length; ++i)
			{
				UIPanel p = panels[i];
				depth = Mathf.Max(depth, p.depth);
			}
			
			if (depth == int.MinValue)
			{
				depth = 0;
			}
			
			return depth;
		}
		
		// 获取最浅的子节点
		public static int getMinDepth(GameObject obj)
		{
			int depth = int.MaxValue;
			
			UIPanel[] panels = obj.GetComponentsInChildren<UIPanel>(true);
			for (int i = 0; i < panels.Length; ++i)
			{
				UIPanel p = panels[i];
				depth = Mathf.Min(depth, p.depth);
			}
			
			if (depth == int.MaxValue)
			{
				depth = 0;
			}
			
			return depth;
		}
		
		// 每个节点都加上这个深度,可以是负值
		public static void addDepth(GameObject obj, int depth)
		{
			UIPanel[] panels = obj.GetComponentsInChildren<UIPanel>(true);
			for (int i = 0; i < panels.Length; ++i)
			{
				UIPanel p = panels[i];
				p.depth += depth;
			}
		}
		
		public static void setDepth(GameObject obj, int depth)
		{
			int minDepth = getMinDepth(obj);
			int subDepth = depth - minDepth;
			addDepth(obj, subDepth);
		}

		public static Dictionary<uint, GameObject> CurrentOpenUI()
		{
			Dictionary<uint, GameObject> OpenUI = new Dictionary<uint, GameObject>();
			foreach(KeyValuePair<string,UIResInfo> main in m_resInfoDict)
			{
				if(main.Value.objList.Count>0)
				{
					for(int i=0;i<main.Value.objList.Count;i++)
					{
						if(main.Value.objList[i].activeInHierarchy==true)
						{
							OpenUI[main.Value.uiMark] = main.Value.objList[i].gameObject;
							continue;
						}
					}
				}
			}
			return OpenUI;
		}
		
		// 加载预设
		public static UnityEngine.Object loadWindowRes<T>()
		{
			string className = typeof(T).ToString();
			
			UIResInfo resInfo;
			if (!m_resInfoDict.TryGetValue(className, out resInfo))
			{
				Debug.LogError("can't find the res path of " + className + ", please register res path frist !");
				return null;
			}
			
			if (resInfo.prefab == null)
			{
				resInfo.prefab = Resources.Load(resInfo.path);
				if (resInfo.prefab == null)
				{
					Debug.LogError("can't load the res form '" + resInfo.path + "'");
					return null;
				}
			}
			return resInfo.prefab;
		}
		
		public static T findWindow<T>() where T : UIBaseWindow
		{
			return findWindow<T>(0);
		}
		
		public static T findWindow<T>(int index) where T : UIBaseWindow
		{
			string name = typeof(T).ToString();
			
			UIResInfo resInfo;
			if (!m_resInfoDict.TryGetValue(name, out resInfo))
			{
				return null;
			}
			
			if (resInfo.objList.Count > index)
			{
				return resInfo.objList[index].GetComponent<T>();
			}
			
			return null;
		}
		
		// 创建
		public static T createWindow<T>() where T : UIBaseWindow
		{
			GameObject parent = getRootPanle();
			return createWindow<T>(parent, true);
		}
		
		// 创建
		public static T createWindow<T>(bool bSingle) where T : UIBaseWindow
		{
			GameObject parent = getRootPanle();
			return createWindow<T>(parent, bSingle);
		}
		
		// 创建
		public static T createWindow<T>(GameObject parent, bool bSingle) where T : UIBaseWindow
		{
			if (bSingle)
			{
				T window = findWindow<T>();
				if (window != null)
				{
					return window;
				}
			}
			
			GameObject res = loadWindowRes<T>() as GameObject;
			if (res == null)
			{
				return null;
			}
			
			GameObject uiGo = NGUITools.AddChild(parent, res);
			UIAnchor[] Anchor = uiGo.GetComponentsInChildren<UIAnchor>(true);
			for (int i = 0; i < Anchor.Length; ++i)
			{
				if (Anchor[i].uiCamera == null)
				{
					Anchor[i].uiCamera = UITools.FindInParent<Camera>(uiGo);
				}
			}
			uiGo.SetActive(false);
			
			T t = uiGo.AddMissingComponent<T>();
			
			string className = typeof(T).ToString();
			UIResInfo resInfo = m_resInfoDict[className];
			if (resInfo != null)
			{
				resInfo.objList.Add(uiGo);
			}

			
			return t;
		}
		
		public static void destoryWindow<T>(GameObject uiGo) where T : UIBaseWindow
		{
			string className = typeof(T).ToString();
			
			destoryWindow(className, uiGo);
		}

		public static void destoryWindow(string className, GameObject uiGo)
		{
			UIResInfo resInfo;
			if (m_resInfoDict.TryGetValue(className, out resInfo))
			{
				for (int i = 0; i < resInfo.objList.Count; i++)
				{
					if (uiGo == resInfo.objList[i])
					{
						NGUITools.Destroy(uiGo);
						resInfo.objList[i] = null;
						resInfo.objList.RemoveAt(i);
					}
				}
			}
		}
		
		public static void destoryWindow<T>()
		{
			string name = typeof(T).ToString();
			
			UIResInfo resInfo;
			if (m_resInfoDict.TryGetValue(name, out resInfo))
			{
				for (int i = 0; i < resInfo.objList.Count; i++)
				{
					if (resInfo.objList[i] != null)
					{
						NGUITools.Destroy(resInfo.objList[i]);
						resInfo.objList[i] = null;
					}
				}
				resInfo.objList.Clear();
			}
		}
		
		public static void destoryAllWindows()
		{
			foreach (KeyValuePair<string, UIResInfo> pair in m_resInfoDict)
			{
				UIResInfo resInfo = pair.Value;
				for (int i = 0; i < resInfo.objList.Count; i++)
				{
					if (resInfo.objList[i] != null)
					{
						NGUITools.Destroy(resInfo.objList[i]);
						resInfo.objList[i] = null;
					}
				}
				resInfo.objList.Clear();
				resInfo.prefab = null;
			}
			
			Resources.UnloadUnusedAssets();
		}
		

		public static void bringTop(GameObject obj)
		{
			UIRoot root = getUIRoot();
			int maxDepth = getMaxDepth(root.gameObject);
			int minDepth = getMinDepth(obj);
			addDepth(obj, maxDepth + (0 - minDepth) + 2);
		}

	}


}

