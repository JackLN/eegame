using UnityEngine;

namespace eegame
{
    public abstract class Singleton<T> where T : class,new()
    {
		protected static T m_Instance = new T();
		
		protected Singleton()
		{
			if (null != m_Instance)
				throw new UnityException("This " + (typeof(T)).ToString() + " Singleton Instance is not null !!!");
		}
		
		public static T Instance
		{
			get { return m_Instance; }
		}
	}
}
