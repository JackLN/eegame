using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using eegame;

namespace eegame{
    
    public class AudioName
    {
	
    }

    public class AudioData
    {
	public AudioSource audio_source = null;
	public string sFile;
    }

    public class AudioManager : MonoSingleton<AudioManager>
    {
	const int MAX_AUDIO_SOURCE_COUNT = 20;
	
	AudioListener m_AudioListener = null;
	AudioSource m_MusicAudioSource = null;

	Queue<AudioData> m_QueAudioData = new Queue<AudioData>();
	
	private void Start()
	{
	    m_MusicAudioSource = gameObject.AddComponent<AudioSource>();
	}
    }

   
}
