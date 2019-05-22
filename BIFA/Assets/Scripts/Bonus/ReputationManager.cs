using System.Diagnostics;
using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    /*
        Ce script gère la réputation de chaque équipe.
        Lorsqu'un joueur marque un but, il augmente la réputation de son équipe.
        Lorsqu'un joueur effectue une action de popularité, il augmente la réputation de son équipe.
        Les bonus apparaissent aléatoirement sur le terrain. Plus la popularité d'une équipe est haute, plus la probabilité qu'un bonus spawne du côté du but de l'autre équipe est haute.
    */

    #region Private Variables
    [Range(0f, 1f)] private float _Rep = .5f;
    [SerializeField, Range(15, 20)] private int _minCoolDown = 20;
    [SerializeField, Range(30, 50)] private int _maxCoolDown = 50;
    private float _bonusSpawnCoolDown;

    [SerializeField] private Transform[] _boundsX = new Transform[4], _boundsZ = new Transform[2];

    private GameObject _bonus;
	[SerializeField]
	private GameObject _goldenBonus;

    [SerializeField] private GameObject[] _allBonus;

    private Stopwatch _bonusSpawnTimer = new Stopwatch();
    #endregion

    #region Public Static Variables
    public static ReputationManager repInst;
    #endregion

    #region Methods
	void OnEnable() {
		ScoreManager.onGameOver += DisableObjects;
	}

	void OnDisable() {
		ScoreManager.onGameOver -= DisableObjects;
	}

    void Awake() {
        if (repInst == null) {
            repInst = this;
        }
        else if (repInst != this) {
            Destroy(gameObject);
            return;
        }
    }

    void Start() {
        _bonusSpawnCoolDown = Random.Range(_minCoolDown, _maxCoolDown);
        _bonusSpawnTimer.Start();
    }

    void Update() {
        if (_bonusSpawnTimer.Elapsed.TotalSeconds >= _bonusSpawnCoolDown) {
            SpawnBonus();
            _bonusSpawnCoolDown = Random.Range(_minCoolDown, _maxCoolDown);
            _bonusSpawnTimer.Reset();
            _bonusSpawnTimer.Start();
        }
    }

    void SpawnBonus() {
		float golden = Random.value;
		if (golden <= .95f)
			_bonus = _allBonus[Random.Range(0, _allBonus.Length)];
		else
			_bonus = _goldenBonus;

        float proba = Random.value, posX = 0, posZ = 0;

        //On définit la position en x en fonction de la probabilité et de la réputation
        //Si la réputation vaut 0
        if (Rep == 0)
            //On spawne le bonus à droite
            posX = Random.Range(_boundsX[2].position.x, _boundsX[3].position.x);
        //Sinon, si la réputation vaut 1
        else if (Rep == 1)
            //On spawne le bonus à gauche
            posX = Random.Range(_boundsX[0].position.x, _boundsX[1].position.x);
        //Sinon
        else if (Rep > 0 && Rep < 1) {
            //Si la réputation est inférieure à 0.5
            if (Rep < .5f) {
                //Si la probabilité est au-dessus de la réputation
                if (proba > Rep)
                    //On spawne le bonus à droite
                    posX = Random.Range(_boundsX[2].position.x, _boundsX[3].position.x);
                //Sinon
                else
                    //On spawne le bonus au milieu
                    posX = Random.Range(_boundsX[1].position.x, _boundsX[2].position.x);
            }
            //Si la réputation est à 0.5
            if (Rep == .5f)
                //On spawne le bonus au milieu
                posX = Random.Range(_boundsX[1].position.x, _boundsX[2].position.x);
            //Si la réputation est supérieure à 0.5
            if (Rep > .5f) {
                //Si la probabilité est en-dessous de la réputation
                if (proba < Rep)
                    //On spawne le bonus à gauche
                    posX = Random.Range(_boundsX[0].position.x, _boundsX[1].position.x);
                //Sinon
                else
                    //On spawne le bonus au milieu
                    posX = Random.Range(_boundsX[1].position.x, _boundsX[2].position.x);
            }
        }

        posZ = Random.Range(_boundsZ[0].position.z, _boundsX[1].position.z);

        _bonus.transform.position = new Vector3(posX, _bonus.transform.position.y, posZ);

        _bonus.SetActive(true);
    }
    #endregion

    #region Public Methods
    public void IncreaseRep(int id, float val) {
        switch (id) {
            case 0:
                Rep -= val;
                break;
            case 1:
                Rep += val;
                break;
        }
    }

	public void DisableObjects() {
		enabled = false;
	}
    #endregion

    #region Public Properties
    public float Rep { get => _Rep; set => _Rep = value; }
    #endregion
}
