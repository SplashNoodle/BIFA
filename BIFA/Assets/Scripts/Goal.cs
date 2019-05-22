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
    public enum Equipe
    {
        Equipe1,
        Equipe2
    }

    public Equipe equipe;
    #endregion

    #region Methods
    void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Ballon")) {
            if (equipe == Equipe.Equipe1) {
                ScoreManager.scoreInst.Score2++;
                ScoreManager.scoreInst.UpdateDisplay(_equipTxt, ScoreManager.scoreInst.Score2);
                ReputationManager.repInst.IncreaseRep(1, .25f);
            }
            if (equipe == Equipe.Equipe2) {
                ScoreManager.scoreInst.Score1++;
                ScoreManager.scoreInst.UpdateDisplay(_equipTxt, ScoreManager.scoreInst.Score1);
                ReputationManager.repInst.IncreaseRep(0, .1f);
            }
            StartCoroutine(col.GetComponent<Ballon>().Respawn(0f));
        }

        if(col.CompareTag("TempBallon"))
            StartCoroutine(col.GetComponent<Ballon>().Respawn(0f));
    }
    #endregion

    #region Coroutines
    IEnumerator Ola() {
        //OLA
        yield return new WaitForSeconds(0.3f);
    }
    #endregion
}