#region System & Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

#region TextMeshPro
using TMPro;
#endregion

public class MusicMaster : MonoBehaviour {
	#region Private Variables
	private int index;

	private bool showTitle = true;

	private float volume = .5f;

	private AudioSource source;
	#endregion

	#region Public Variables
	public string[] titles;

	public AudioClip[] musics;

	public TextMeshProUGUI title;
	#endregion

	#region Methods
	private void Start() {
		source = GetComponent<AudioSource>();
		source.volume = volume;
		index = Random.Range(0, musics.Length);
		source.clip = musics[index];
		source.Play();
		StartCoroutine(ShowTitle());
		StartCoroutine(PlayMusic());
	}

	private void Update() {
		title.text = titles[index];
		if (showTitle) {
			if (title.alpha < 0.99f)
				title.alpha = Mathf.Lerp(title.alpha, 1f, Time.deltaTime*5f);
			else
				title.alpha = 1f;
		}
		else {
			if (title.alpha > 0.01f)
				title.alpha = Mathf.Lerp(title.alpha, 0, Time.deltaTime*5f);
			else
				title.alpha = 0f;
		}

	}
	#endregion

	#region Coroutines
	IEnumerator PlayMusic() {
		yield return new WaitForSeconds(source.clip.length);
		index++;
		index = (int)Mathf.Repeat(index, musics.Length);
		source.clip = musics[index];
		if (!source.isPlaying)
			source.Play();
		StartCoroutine(ShowTitle());
		StartCoroutine(PlayMusic());
		
	}

	IEnumerator ShowTitle() {
		showTitle = true;
		yield return new WaitForSeconds(10f);
		showTitle = false;
	}
	#endregion
}
