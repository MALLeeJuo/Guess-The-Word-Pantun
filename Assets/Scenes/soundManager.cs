using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
	[SerializeField]
	private AudioClip menuMusic;

	[SerializeField]
	private AudioClip levelMusic;

	[SerializeField]
	private AudioSource source;

	static private soundManager instance;

	protected virtual void Awake() {

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(this);
		} else {
			Destroy(this);
			return;
		}
	}

	protected virtual void Start() {
		PlayMenuMusic();
	}
	
	static public void PlayMenuMusic ()
	{
		if (instance != null) {
			if (instance.source != null) {
				instance.source.Stop();
				instance.source.clip = instance.menuMusic;
				instance.source.Play();
			}
		} else {
			Debug.LogError("Unavailable MusicPlayer component");
		}
	}
	
	static public void PlayLevelMusic ()
	{
		if (instance != null) {
			if (instance.source != null) {
				instance.source.Stop();
				instance.source.clip = instance.levelMusic;
				instance.source.Play();
			}
		} else {
			Debug.LogError("Unavailable MusicPlayer component");
		}
	}

    static public void PauseMusic()
	{
        instance.source.Pause();
	}
}
