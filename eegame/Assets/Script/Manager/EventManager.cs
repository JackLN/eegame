using System;
using System.Collections.Generic;

using eegame;

namespace eegame
{
    public enum EventType
    {
		EventMax,
    }

    public class EventManager : Singleton<EventManager>
    {
		public delegate void EventHandle(EventType evt,System.Object data);
		private EventHandle[] m_ArrayHandle;
		
		public EventManager()
		{
		    int array_num = (int)EventType.EventMax;
		    m_ArrayHandle = new EventHandle[array_num];
		}
		
		public void Resist(EventType evt,EventHandle handle)
		{
		    if(evt >= EventType.EventMax)
		    {
				return;
		    }
		    m_ArrayHandle[(int)evt] += handle;
		}
		
		public void UnResist(EventType evt,EventHandle handle)
		{
		    if(evt >= EventType.EventMax)
		    {
				return;
		    }
		    if(m_ArrayHandle[(int)evt] != null)
		    {
				m_ArrayHandle[(int)evt] -= handle;
		    }
		}

		public void Dispatch(EventType evt,System.Object data)
		{
		    if(evt >= EventType.EventMax)
		    {
				return;
		    }
		    if(m_ArrayHandle[(int)evt] != null)
		    {
				m_ArrayHandle[(int)evt](evt,data);
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
