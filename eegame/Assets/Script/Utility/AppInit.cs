using UnityEngine;
using System.Collections;
using eegame;

public class AppInit : MonoBehaviour {

	private static bool m_IsInit = false;

	private void Awake()
	{
		if (!m_IsInit)
		{
			InitApp();
		}
	}

	private void InitApp()
	{
		UIManager.Instance.Init ();
		UIManager.getGrayLayer ().show ();
	}

}
