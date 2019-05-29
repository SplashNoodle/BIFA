#region System & Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

#region TextMeshPro
using TMPro;
#endregion

[RequireComponent(typeof(Rigidbody))]
public class Goal : MonoBehaviour
{

    /*
		Dans ce script, on définit le comportement des cages de but.
		Quand la balle entre dans la cage de but, elle respawne après deux secondes.
		Quand la balle entre dans la cage de but, le score associé augmente de 1.
	*/

    #region Private Variables
    [SerializeField]
    private TextMeshProUGUI _equipTxt;
	#endregion

	#region Public Variables
	[Range(0f,1f)]
	public float goalVolume;

	public AudioClip goalClip;

	public GlobalSettings settings;

    public enum Equipe
    {
        Equipe1,
        Equipe2
    }

    public Equipe equipe;
	#endregion

	#region Events
	public delegate void OnGoal();
	public static event OnGoal onGoal;
	public void RaiseOnGoal() {
		UnityEngine.Debug.Log("GAME START");
		onGoal?.Invoke();
	}
	#endregion

	#region Methods
	void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Ballon")||col.CompareTag("GoldenBall")) {
			RaiseOnGoal();
			SoundManager.sInst.PlayClip(GetComponent<AudioSource>(), goalClip, goalVolume*settings.masterVolume*settings.effectsVolume);
            if (equipe == Equipe.Equipe1) {
                ScoreManager.scoreInst.Score2++;
				if(col.CompareTag("GoldenBall"))
					ScoreManager.scoreInst.Score2++;
				ScoreManager.scoreInst.UpdateDisplay(_equipTxt, ScoreManager.scoreInst.Score2);
                ReputationManager.repInst.IncreaseRep(1, .25f);
            }
            if (equipe == Equipe.Equipe2) {
                ScoreManager.scoreInst.Score1++;
				if (col.CompareTag("GoldenBall"))
					ScoreManager.scoreInst.Score1++;
				ScoreManager.scoreInst.UpdateDisplay(_equipTxt, ScoreManager.scoreInst.Score1);
                ReputationManager.repInst.IncreaseRep(0, .1f);
            }
            _ = col.CompareTag("Ballon")?StartCoroutine(col.GetComponent<Ballon>().Respawn(0f)):StartCoroutine(col.GetComponent<Ballon>().RespawnGolden());
        }

        if(col.CompareTag("TempBallon"))
            StartCoroutine(col.GetComponent<Ballon>().Respawn(0f));
    }
    #endregion
}