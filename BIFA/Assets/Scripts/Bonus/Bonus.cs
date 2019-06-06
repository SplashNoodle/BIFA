#region System & Unity
using UnityEngine;
#endregion

[RequireComponent(typeof(AudioSource))]
public class Bonus : MonoBehaviour
{
    #region Private Variables
    [SerializeField]
    private BonusType bonusT = BonusType.Undefined;
	#endregion

	#region Public Variables
	public float defaultVolume = .7f;

	public AudioSource bonusSoundMaster;

    public GameObject[] magnets;

	public AudioClip takeBonus;

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
	void OnEnable() {
		if (bonusSoundMaster == null)
			bonusSoundMaster = GameObject.FindGameObjectWithTag("BSMaster").GetComponent<AudioSource>();
		bonusSoundMaster.volume = defaultVolume * SoundManager.sInst.settings.effectsVolume;
	}

    void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player")) {
			SoundManager.sInst.PlayClip(bonusSoundMaster, takeBonus, defaultVolume * SoundManager.sInst.settings.effectsVolume);
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
		ReputationManager.repInst.goldenBall.SetActive(true);
	}
    #endregion

    #region Public Getters
    public BonusType BonusT { get => bonusT; set => bonusT = value; }
    #endregion
}
