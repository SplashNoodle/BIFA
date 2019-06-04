#region System & Unity
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

#region Rewired
using Rewired;
#endregion

public class GameManager : MonoBehaviour
{
	#region Private Variables
	private bool opStart = false;

	private GameObject _launch;

	private VerifierInfos _verifs;
	#endregion

	#region Public Variables
	public PInfos[] pInfos;

	public enum GameMode
	{
		Selection,
		Menu,
		Game
	}

	public GameMode mode = GameMode.Selection;
	#endregion

	#region Public Static Variables
	public static GameManager gmInst;
	#endregion

	#region Events
	public delegate void OnGameReload();
	public static event OnGameReload onGameReload;
	public void RaiseOnGameReload(Scene scene, LoadSceneMode mode) {
		Debug.Log("GAME RELOAD");
		onGameReload?.Invoke();
		SceneManager.sceneLoaded -= RaiseOnGameReload;
	}

	public delegate void OnGamePause();
	public static event OnGamePause onGamePause;
	public void RaiseOnGamePause() {
		Debug.Log("PAUSE");
		onGamePause?.Invoke();
	}

	public delegate void OnGameResume();
	public static event OnGameResume onGameResume;
	public void RaiseOnResume() {
		Debug.Log("RESUME");
		onGameResume?.Invoke();
	}
	#endregion

	#region Methods
	void Awake() {
		if (gmInst == null) {
			gmInst = this;
		}
		else if (gmInst != this) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(this.gameObject);

		foreach (PInfos item in pInfos) {
			item.pseudo = "";
			item.equipe = 0;
			item.pIndex = 0;
			item.characterIndex = 0;
			item.clothesIndex = 0;
		}
	}

	void Update() {
		if ((SceneManager.GetActiveScene().name == "Selection1V1" || SceneManager.GetActiveScene().name == "Selection2V2") && _verifs == null) {
			mode = GameMode.Selection;

			_verifs = FindObjectOfType<VerifierInfos>();

			_launch = _verifs.launch;
		}

		if ((SceneManager.GetActiveScene().name == "Stade1V1" || SceneManager.GetActiveScene().name == "Stade2V2") && mode == GameMode.Selection)
			mode = GameMode.Menu;

		switch (mode) {
			case GameMode.Selection:
				Selection();
				break;
			case GameMode.Menu:
				Menu();
				break;
			case GameMode.Game:
				PauseGame();
				break;
		}

	}

	void Selection() {

		if (CharAndTeeSelection.readyCount == CharAndTeeSelection.maxReadyCount && CharAndTeeSelection.maxReadyCount > 0) {
			if (!_launch.activeSelf)
				_launch.SetActive(true);
			for (int i = 0; i < pInfos.Length; i++) {
				if (ReInput.players.GetPlayer(pInfos[i].pIndex).GetButtonDown("Submit") && !opStart) {
					opStart = true;
					//SceneManager.LoadScene(1);
					if (SceneManager.GetActiveScene().name == "Selection1V1")
						LoadingUI.loadInst.Show(SceneManager.LoadSceneAsync("Stade1V1"));
					if (SceneManager.GetActiveScene().name == "Selection2V2")
						LoadingUI.loadInst.Show(SceneManager.LoadSceneAsync("Stade2V2"));
				}
			}
		}
		else {
			if (_launch.activeSelf)
				_launch.SetActive(false);
		}
	}

	void Menu() {
		//Si le joueur quitte le mode pause
		//On relance le jeu
	}

	void PauseGame() {
		//Si un joueur appuie sur le bouton de pause
		for (int i = 0; i < pInfos.Length; i++) {
			if (ReInput.players.GetPlayer(pInfos[i].pIndex).GetButtonDown("Pause")) {
				//On met le jeu en pause
				RaiseOnGamePause();
			}
		}
	}
	#endregion

	#region Public Methods
	public void Pause() {
		Time.timeScale = 0;
	}

	public void Resume() {
		Time.timeScale = 1;
		RaiseOnResume();
	}

	public void Quit() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}

	public void Load1V1() {
		LoadingUI.loadInst.Show(SceneManager.LoadSceneAsync("Selection1V1"));
	}

	public void Load2V2() {
		LoadingUI.loadInst.Show(SceneManager.LoadSceneAsync("Selection2V2"));
	}

	public void ReloadSelection() {
		Debug.Log("RELOAD CHARACTER SELECTION");
		CharAndTeeSelection.readyCount = 0;
		if (SceneManager.GetActiveScene().name == "Stade1V1")
			LoadingUI.loadInst.Show(SceneManager.LoadSceneAsync("Selection1V1"));
		if (SceneManager.GetActiveScene().name == "Stade2V2")
			LoadingUI.loadInst.Show(SceneManager.LoadSceneAsync("Selection2V2"));
	}

	public void ReloadGame() {
		Debug.Log("RELOAD GAME");
		if (SceneManager.GetActiveScene().name == "Stade1V1")
			LoadingUI.loadInst.Show(SceneManager.LoadSceneAsync("Stade1V1"));
		if (SceneManager.GetActiveScene().name == "Stade2V2")
			LoadingUI.loadInst.Show(SceneManager.LoadSceneAsync("Stade2V2"));
		SceneManager.sceneLoaded += RaiseOnGameReload;
	}
	#endregion
}
