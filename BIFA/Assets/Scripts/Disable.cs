using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Disable : MonoBehaviour
{
	ParticleSystem pS;

	AudioSource src;

	public float defVol;

	public AudioClip[] clips;

	void OnEnable() {
		src = GetComponent<AudioSource>();
		src.volume = defVol * SoundManager.sInst.settings.effectsVolume * SoundManager.sInst.settings.masterVolume;
		src.clip = clips[Random.Range(0, clips.Length)];
		src.Play();
	}

	void Start() {
		pS = GetComponent<ParticleSystem>();
	}
	void Update() {
		if (pS.isStopped)
			gameObject.SetActive(false);
	}
}
