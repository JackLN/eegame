using UnityEngine;

namespace eegame
{
    public abstract class Singleton<T> whert T : class
    {
	private static T _instance = null;
	public static T Instance
	{
	    get
	    {
		if(_instance == null)
		{
		    _instance = new T();
		    _instance.Init();
		}
		return _instance;
	    }
	}

	protected virtual void Init()
	{

	}

}
