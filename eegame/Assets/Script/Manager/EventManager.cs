using System;
using System.Collections.Generic;

namespace eegame
{
    public enum EventType
    {
	EventMax,
    }

    public class EventManager : MonoSingleton<EventManager>
    {
	public delegate void EventHandle(EventType event,System.Object data);
	private EventHandle[] m_ArrayHandle;
	
	private void Start()
	{
	    int array_num = (int)EventType.EventMax;
	    m_ArrayHandle = new EventHandle[array_num];
	}
	
	public void Resist(EventType event,EventHandle handle)
	{
	    if(event >= EventType.EventMax)
	    {
		return;
	    }
	    m_ArrayHandle[(int)evnet] += handle;
	}
	
	public void UnResist(EventType event,EventHandle handle)
	{
	    if(event >= EventType.EventMax)
	    {
		return;
	    }
	    if(m_ArrayHandle[(int)event] != null)
	    {
		m_ArrayHandle[(int)event] -= handle;
	    }
	}

	public void Dispatch(EventType event,System.Object data)
	{
	    if(evnet >= EventType.EventMax)
	    {
		return;
	    }
	    if(m_ArrayHandle[(int)event] != null)
	    {
		m_ArrayHandle[(int)event](event,data);
	    }
	}

	public void CleanAllEvent()
	{
	    for(int i = 0;i < m_ArrayHandle.Length; ++i)
	    {
		m_ArrayHandle[i] = null;
	    }
	}
    }
	    		
}
