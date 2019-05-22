#region System & Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

#region Rewired
using Rewired;
#endregion

[System.Serializable]
public class InGamePlayer
{
    public int index;
}

public class LocalMultiManager : MonoBehaviour
{
    #region Public Variables
    public GameObject[] waitingForPlayerObj, charImages;

    public Transform[] charButtons;

    public List<InGamePlayer> pIndex = new List<InGamePlayer>();

    public PInfos[] pInfos;

	public CharAndTeeSelection[] charAndTeeSelectors;
    #endregion

    #region Public Static Variables
    public static LocalMultiManager localInst;
    #endregion

    #region Methods
    void Awake()
    {
        localInst = this;
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            ReInput.players.GetPlayer(i).isPlaying = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player p = ReInput.players.GetPlayer(i);
            if (p.GetButtonDown("Submit") && !p.isPlaying)
            {
                pIndex.Add(new InGamePlayer() { index = i });
                Debug.Log("Player " + pIndex.Count + " entered the game, and has index " + i + " !");
                p.isPlaying = true;

                //On désactive le message demandant de rejoindre le jeu
                waitingForPlayerObj[pIndex.Count - 1].SetActive(false);
                //On active l'image représentant le personnage sélectionné
                charImages[pIndex.Count - 1].SetActive(true);
                //On récupère l'index du joueur pour les inputs in game
                pInfos[pIndex.Count - 1].pIndex = i;
                //On définit l'équipe du joueur en fonction de son ordre d'arrivée dans le jeu
                pInfos[pIndex.Count - 1].equipe = ((pIndex.Count-1) % 2 == 0) ? 0 : 1;
                //On active le script de selection
                //selectors[pIndex.Count - 1].enabled = true;
				charAndTeeSelectors[pIndex.Count - 1].enabled = true;
            }
        }
    }
    #endregion
}
