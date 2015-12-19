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

		float m_MusicVolume = 0.5f;        
		float m_SoundVolume = 0.6f;

		bool soundOn = true;
		bool musicOn = true;

		string m_MusicFile = "";
		float m_CheckDelayTime = 0.3f;
		float m_CheckTime = 0.0f;

		Queue<AudioData> m_QueAudioData = new Queue<AudioData>();
		Dictionary<int, AudioData> m_DicPlayingAudioSource = new Dictionary<int, AudioData>();
		Dictionary<string, int> m_DicPlayingSoundFile = new Dictionary<string, int>();
		List<int> m_DeleteSounds = new List<int>();

		int m_iSoundUniqueId = 1;          
		int SoundUniqueId
		{
			get
			{
				if ( m_iSoundUniqueId == 0 )
				{
					m_iSoundUniqueId++;
				}
				return m_iSoundUniqueId++;
			}
		}

		/// <summary>
		/// set sound on/off
		/// </summary>
		public bool SoundOn
		{
			get
			{
				return soundOn;
			}
			set
			{
				soundOn = value;
				if (!value)
				{
					StopSounds();
				}
			}
		}
		
		/// <summary>
		/// set music on/off
		/// </summary>
		public bool MusicOn
		{
			get
			{
				return musicOn;
			}
			set
			{
				musicOn = value;
				if (value)
				{
					if ( !m_MusicAudioSource.isPlaying && m_MusicFile != null )
					{
						PlayMusic(m_MusicFile);
					}
					m_MusicAudioSource.volume = m_MusicVolume;
				}
				else
				{
					m_MusicAudioSource.volume = 0.0f;
				}
			}
		}

		private void Start()
		{
		    m_MusicAudioSource = gameObject.AddComponent<AudioSource>();
			m_MusicAudioSource.volume = m_MusicVolume;
			m_MusicAudioSource.rolloffMode = AudioRolloffMode.Custom;

			for (int i = 0; i < MAX_AUDIO_SOURCE_COUNT; i++) 
			{			
				AudioData soundData = new AudioData();
				soundData.audio_source = gameObject.AddComponent<AudioSource>();
				soundData.audio_source.volume = m_SoundVolume;
				soundData.audio_source.rolloffMode = AudioRolloffMode.Custom;
				m_QueAudioData.Enqueue(soundData);
			}
		}

		/// <summary>
		/// Play the sound.
		/// </summary>
		private int PlaySound(string sFile)
		{
			if (!SoundOn)
			{
				return 0;
			}
			
			int m_id = 0;
			if (m_DicPlayingSoundFile.TryGetValue(sFile, out m_id))
			{
				AudioData data = null;
				if ( m_DicPlayingAudioSource.TryGetValue(m_id, out data) && data.audio_source.isPlaying )               
					return 0;
			}
			
			if (m_QueAudioData.Count == 0)
			{
				return 0;
			}
			AudioData soundData = m_QueAudioData.Dequeue();
			AudioSource audio_source = soundData.audio_source;
			AudioClip clip = getAudioClip(sFile);
			if (clip == null)
			{
				m_QueAudioData.Enqueue(soundData);
				return 0;
			}
			soundData.sFile = sFile;
			audio_source.clip = clip;
			audio_source.volume = m_SoundVolume;
			audio_source.loop = false;
			audio_source.Play();
			int id = SoundUniqueId;
			m_DicPlayingAudioSource[id] = soundData;
			m_DicPlayingSoundFile[sFile] = id;
			return id;
		}

		/// <summary>
		/// Play the music.
		/// </summary>
		public bool PlayMusic(string sFile)
		{
			if (m_MusicFile == sFile)
			{
				return true;
			}
			m_MusicFile = sFile;
			if (!MusicOn)
			{
				return true;
			}
			AudioClip clip = getAudioClip (sFile);
			if (clip == null)
			{
				return false;
			}
			
			if (m_MusicAudioSource.isPlaying)
			{
				m_MusicAudioSource.Stop();
				Resources.UnloadAsset(m_MusicAudioSource.clip);
				m_MusicAudioSource.clip = null;
			}
			
			m_MusicAudioSource.clip = clip;
			m_MusicAudioSource.loop = true;
			m_MusicAudioSource.Play();
			
			return true;
		}

		/// <summary>
		/// Replay the music.
		/// </summary>
		public void ReplayMusic()
		{
			if (musicOn)
			{
				m_MusicAudioSource.volume = m_MusicVolume;
			}
		}

		/// <summary>
		/// Stops the music.
		/// </summary>
		/// <returns><c>true</c>, if music was stoped, <c>false</c> otherwise.</returns>
		public bool StopMusic()
		{
			if (m_MusicAudioSource != null && m_MusicAudioSource.isPlaying)
			{
				m_MusicAudioSource.Stop();
				Resources.UnloadAsset(m_MusicAudioSource.clip);
				m_MusicAudioSource.clip = null;
			}
			return true;
		}

		/// <summary>
		/// Stops the sounds.
		/// </summary>
		public void StopSounds()
		{
			foreach (AudioData data in m_QueAudioData)
			{
				data.audio_source.clip = null;
			}
			
			Dictionary<int, AudioData>.Enumerator iter = m_DicPlayingAudioSource.GetEnumerator();
			while( iter.MoveNext() )
			{
				KeyValuePair<int, AudioData> pair = iter.Current;
				if (pair.Value.audio_source.isPlaying)
				{
					pair.Value.audio_source.Stop();
				}
				m_QueAudioData.Enqueue(pair.Value);
				pair.Value.audio_source.clip = null;
			}
			m_DicPlayingAudioSource.Clear();
			m_DicPlayingSoundFile.Clear();
		}

		private void Update()
		{
			m_CheckTime += Time.deltaTime;
			if(m_CheckTime > m_CheckDelayTime)
			{
				CheckPlayingSounds();
				m_CheckTime = 0.0f;
			}
		}

		/// <summary>
		/// Remove music which has played.
		/// </summary>
		private void CheckPlayingSounds()
		{
			m_DeleteSounds.Clear();
			
			Dictionary<int, AudioData>.Enumerator iter = m_DicPlayingAudioSource.GetEnumerator();
			while( iter.MoveNext() )
			{
				KeyValuePair<int, AudioData> pair = iter.Current;
				if ( !pair.Value.audio_source.isPlaying )
				{
					m_DeleteSounds.Add(pair.Key);
					m_QueAudioData.Enqueue(pair.Value);
					m_DicPlayingSoundFile.Remove(pair.Value.sFile);
					pair.Value.audio_source.clip = null;
				}
			}
			
			for(int i=0; i<m_DeleteSounds.Count; ++i)
			{
				m_DicPlayingAudioSource.Remove(m_DeleteSounds[i]);
			}
		}

		/// <summary>
		/// Get the audio clip with file name.
		/// </summary>
		private AudioClip getAudioClip(string sFile)
		{
			string sRealFile = DataEdit.FormatFileName(sFile, FileNameType.RESOURCE_MUSIC);
			AudioClip clip = Resources.Load(sRealFile) as AudioClip;
			return clip;
		}
    }


}
