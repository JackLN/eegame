using UnityEngine;
using System;
using System.Collections;

using eegame;
using cctween;

public abstract class UIBaseWindow : MonoBehaviour
{
	public System.Action OnClose;	
	public System.Action OnFinshShow;
	
	[HideInInspector]
	public UIGrayLayer m_GrayLayer = null;

	// 窗口是否处于正在关闭的过程中
	private bool m_isClosing = false;

	public bool IsClosing()
	{
		return m_isClosing;
	}
	
	public string GetClassName()
	{
		string className = this.GetType().FullName;
		return className;
	}

	public int getMaxDepth()
	{
		if (this == null || this.gameObject == null)
			return 0;
		
		return UIManager.getMaxDepth(this.gameObject);
	}

	public int getMinDepth()
	{
		if (this == null || this.gameObject == null)
			return 0;
		
		return UIManager.getMinDepth(this.gameObject);
	}
	
	public void addDepth(int depth)
	{
		UIManager.addDepth(this.gameObject, depth);
	}
	
	public void setDepth(int depth)
	{
		UIManager.setDepth(this.gameObject, depth);
	}
	
	public bool isVisible()
	{
		if (gameObject != null && gameObject.activeInHierarchy)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public void addGrayLayer(Action onClick = null)
	{
		int depth = getMinDepth() - 1;
		m_GrayLayer = UIManager.getGrayLayer ();
		if (m_GrayLayer != null)
		{
			m_GrayLayer.setDepth(depth);
			m_GrayLayer.show();
		}

		if (onClick != null && m_GrayLayer != null)
		{
			m_GrayLayer.OnClick = onClick;
		}
	}
	
	public void SetGrayLayerAtive(bool active)
	{
		if (m_GrayLayer != null)
		{
			m_GrayLayer.gameObject.SetActive(active);
		}
	}
	
	public void bringTop()
	{
		UIRoot root = UIManager.getUIRoot();
		int maxDepth = UIManager.getMaxDepth(root.gameObject);
		int minDepth = getMinDepth();
		int newDepth = maxDepth + (0 - minDepth) + 2;
		addDepth(newDepth);
		
		if (m_GrayLayer != null)
		{
			m_GrayLayer.setDepth(newDepth - 1);
		}
	}
	
	public virtual void show()
	{
		gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		gameObject.SetActive(true);
		if (m_GrayLayer != null)
		{
			m_GrayLayer.show();
		}
		
		if (OnFinshShow != null)
		{
			OnFinshShow();
			OnFinshShow = null;
		}
		
	}
	
	public virtual void showWithScale(float time = 0.15f)
	{
		gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		gameObject.SetActive(true);
		
		CCAction action = CCSequence.create(
			CCScaleTo.create(time * 0.8f, 1.1f),
			CCScaleTo.create(time * 0.2f, 1.0f)
			);
		CCTween.runAction(gameObject, action);
		
		if (OnFinshShow != null)
		{
			OnFinshShow();
			OnFinshShow = null;
		}
	}
	
	public virtual void hide()
	{
		if (m_GrayLayer != null)
		{
			m_GrayLayer.hide();
		}
		if (gameObject.activeInHierarchy)
		{
			gameObject.SetActive(false);
			
			if (OnClose != null)
			{
				OnClose();
				OnClose = null;
			}
		}        
	}
	
	public virtual void hideWithScale()
	{
		CCAction action = CCSequence.create(
			CCScaleTo.create(0.08f, 1.1f),
			CCDelayTime.create(0.04f),
			CCScaleTo.create(0.12f, 0.1f),
			CCCallback.create(0, () =>
		                  {
			hide();
		}));
		CCTween.runAction(gameObject, action);
		
		m_isClosing = true;
	}
	
	public virtual void destroy()
	{
		if (m_GrayLayer != null)
		{
			UIManager.destoryWindow<UIGrayLayer>(m_GrayLayer.gameObject);
			m_GrayLayer = null;
		}

		if (OnClose != null)
		{
			OnClose();
			OnClose = null;
		}

		UIManager.destoryWindow(GetClassName(), gameObject);

		Resources.UnloadUnusedAssets();
		System.GC.Collect();
	}
	
	public virtual void destroyWithScale()
	{
		CCAction action = CCSequence.create(
			CCScaleTo.create(0.08f, 1.1f),
			CCDelayTime.create(0.04f),
			CCScaleTo.create(0.12f, 0.1f),
			CCCallback.create(0, () =>
		                  {
			destroy();
		}));
		CCTween.runAction(gameObject, action);
		
		m_isClosing = true;
	}
	
	
	void OnDestroy()
	{
		if (m_GrayLayer != null)
		{
			UIManager.destoryWindow<UIGrayLayer>(m_GrayLayer.gameObject);
			m_GrayLayer = null;
		}     
	}
	
	void InvokeShow()
	{
		gameObject.SetActive(true);
		if (m_GrayLayer != null)
		{
			m_GrayLayer.show();
		}
	}       
}
