using UnityEngine;
using System;
using System.Collections.Generic;
using eegame;

public class UIGrayLayer : UIBaseWindow
{
	public Action OnClick = null;

	void Awake()
	{
		refresh();
		
		UIEventListener.Get(gameObject).onClick = OnClickBox;
	}
	
	public void OnEnable()
	{
		OnClick = null;
	}
	
	private void OnDisable()
	{
		OnClick = null;
	}
	
	void OnClickBox(GameObject go)
	{
		if (OnClick != null)
		{
			OnClick();
		}
	}
	
	void Update()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			refresh();
		}
		#endif
	}
	
	private UITexture m_texture;
	private BoxCollider m_collider;
	void refresh()
	{
		if (m_texture != null)
		{
			m_texture.width = UIManager.getWidth() + 100;
			m_texture.height = UIManager.getHeight() + 100;
			m_collider.size = new Vector2(m_texture.width, m_texture.height);
			return;
		}
		
		GameObject textureGo = NGUITools.AddChild(gameObject);
		textureGo.name = "texture";
		m_texture = NGUITools.AddMissingComponent<UITexture>(textureGo);
		m_texture.mainTexture = UITools.CreatTexture(Color.black, new Vector2(2, 2));
		m_texture.width = UIManager.getWidth() + 100;
		m_texture.height = UIManager.getHeight() + 100;
		m_texture.color = Color.black;
		m_texture.alpha = 0.8f;
		
		m_collider = gameObject.GetComponent<BoxCollider>();
		if (m_collider == null)
		{
			m_collider = gameObject.AddComponent<BoxCollider>();
		}
		m_collider.size = new Vector2(m_texture.width, m_texture.height);
	}
}
