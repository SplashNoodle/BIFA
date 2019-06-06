#region System & Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class SoundManager : MonoBehaviour
{
	#region Public Variables
	public GlobalSettings settings;
	#endregion

	#region Public Static Variables
	public static SoundManager sInst;
	#endregion

	#region Methods
	void Awake() {
		if (sInst == null)
			sInst = this;
		else if (sInst != this) {
			Destroy(gameObject);
			return;
		}
	}
	#endregion

	#region Public Methods
	/// <summary>
	/// Play audio clip on audio source with default volume.
	/// </summary>
	/// <param name="src">The audio source.</param>
	/// <param name="clip">The audio clip.</param>
	public void PlayClip(AudioSource src, AudioClip clip) {
		if (src.clip != clip)
			src.clip = clip;
		src.Play();
	}

	/// <summary>
	/// Play audio clip on audio source with custom volume.
	/// </summary>
	/// <param name="src">The audio source.</param>
	/// <param name="clip">The audio clip.</param>
	/// <param name="v">The volume.</param>
	public void PlayClip(AudioSource src, AudioClip clip, float v) {
		if (src.clip != clip)
			src.clip = clip;
		src.volume = v*settings.masterVolume;
		src.Play();
	}
	#endregion
}
