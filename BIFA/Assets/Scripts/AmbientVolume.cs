using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientVolume : MonoBehaviour
{
	public AudioSource[] srcs;

	float[] defaultVolumes = new float[4];

	void OnEnable() {
		srcs = GetComponents<AudioSource>();
	}

	void Start() {
		for (int i = 0; i < srcs.Length; i++) {
			if (srcs[i] != null)
				defaultVolumes[i] = srcs[i].volume;
			else
				Debug.Log("Srcs " + i + " does not exist.");
		}
	}

    void Update()
    {
		for (int i = 0; i < srcs.Length; i++) {
			if(srcs[i]!=null)
			srcs[i].volume = defaultVolumes[i]*SoundManager.sInst.settings.masterVolume * SoundManager.sInst.settings.ambientVolume;
			else
				Debug.Log("Srcs " + i + " does not exist.");
		}
    }
}
