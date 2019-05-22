#region System & Unity
using UnityEngine;
#endregion

public class Bonus : MonoBehaviour
{
    #region Private Variables
    [SerializeField]
    private BonusType bonusT = BonusType.Undefined;
    #endregion

    #region Public Variables
    public GameObject[] magnets;

    public enum BonusType
    {
        Aimant,
        Eclair,
        Flocon,
        Grappin,
		Golden,
        Undefined
    }
    #endregion

    #region Methods
    void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player")) {
            switch (BonusT) {
                case BonusType.Aimant:
                    if (col.GetComponent<GamePlayer>().PlayerInfos.equipe == 0)
                        Magnetize(1, col.GetComponent<GamePlayer>());
                    else
                        Magnetize(0, col.GetComponent<GamePlayer>());
                    break;
                case BonusType.Eclair:
                    ShowLightningParts(col.gameObject);
                    break;
                case BonusType.Flocon:
                    AllowFreeze(col.gameObject);
                    break;
                case BonusType.Grappin:
                    BringCloser(col.gameObject);
                    break;
				case BonusType.Golden:
					GoldenBall();
					break;
                case BonusType.Undefined:
                    break;
            }
            gameObject.SetActive(false);
        }
    }

    void Magnetize(int equipeID, GamePlayer p) {
        //On active l'aimant correspondant
        magnets[equipeID].SetActive(true);
		magnets[equipeID].GetComponent<Magnet>().player = p;

	}

    void ShowLightningParts(GameObject player) {
        //On active le script de stun
        player.GetComponent<StunPlayer>().enabled = true;
    }

    void AllowFreeze(GameObject player) {
		//BIFA_TODO Remplacer par mur de glace devant le but.
        //On active le script qui permet de geler la balle
        player.GetComponent<IceWall>().enabled = true;
    }

    void BringCloser(GameObject player) {
        //On active le grappin du joueur
        player.GetComponent<GamePlayer>().hook.SetActive(true);
    }

	void GoldenBall() {

	}
    #endregion

    #region Public Getters
    public BonusType BonusT { get => bonusT; set => bonusT = value; }
    #endregion
}
