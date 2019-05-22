#region System & Unity
using System.Collections.Generic;
using UnityEngine;
#endregion

public class SupporterSpawn : MonoBehaviour
{

    /*
        Ce script sert au spawn des supporters dans le stade, ainsi qu'à l'attribution de leurs couleurs
    */

    #region Private Variables
    //private int previous = 0;

    private float empty;

    private List<GameObject> meshes = new List<GameObject>();
    private List<GameObject> supList = new List<GameObject>();
    #endregion

    #region Public Variables
    [Range(0, 1f)]
    public float emptyCount = .1f;
    [Range(0, 1f)]
    public float offsetMax = 0f;

	public Transform supParent;

    public SupCircle[] circles;

    public Supporter[] supporters;
    #endregion

    #region Methods
    void Awake() {
        SpawnIntoCircles();
    }

    void SpawnIntoCircles() {
        for (int i = 0; i < circles.Length; i++) {
            SpawnIntoCircle(i);
        }
    }

    void SpawnIntoCircle(int index) {
        //On spawne les supporters dans chaque ligne du cercle
        for (int i = 0; i < circles[index].circle.Length; i++) {
            //On regarde combien de places sont disponibles
            float absDist = Vector3.Distance(circles[index].circle[i].lineStart.position, circles[index].circle[i].lineEnd.position);
            int rDist = Mathf.RoundToInt(absDist);

            int places = 0;

            //On remplit la liste de supporters en fonctions des places qu'ils prennent
            for (places = 0; places < rDist;) {
                Supporter supporter;
                empty = Random.value;

                //Si le pourcentage de places vides est différent de 0
                if (emptyCount != 0f) {
                    //Si empty est supérieur au pourcentage de places vides
                    if (empty > emptyCount)
                        //On spawne un supporter
                        supporter = supporters[Random.Range(0, supporters.Length - 1)];
                    //Sinon
                    else
                        //On spawne une place vide
                        supporter = supporters[supporters.Length - 1];
                }
                //Sinon
                else
                    //On spawne un supporter
                    supporter = supporters[Random.Range(0, supporters.Length - 1)];


                //Si le nombres de places utilisé ajouté au nombre de places prises par le supporter est inférieur ou égal au nombre de places disponibles
                if (places + supporter.supporterSize <= rDist) {
                    SpawnSupporter(circles[index].circle[i].lineStart.position, circles[index].circle[i].lineEnd.position, supporter, places);

                    //On ajoute le nombres de places utilisées par le supporter
                    places += supporter.supporterSize;
                }
            }
        }
    }

    void SpawnSupporter(Vector3 start, Vector3 end, Supporter sup, int place) {
        Vector3 dir = end - start;

        Vector3 pos = start + dir.normalized * (place + Random.Range(-offsetMax, offsetMax));

        Vector3 targetOrientation;

        GameObject supporter;

        if (sup.prefab != null) {
            supporter = Instantiate(sup.prefab, pos, Quaternion.identity);

            targetOrientation = Vector3.Cross(dir.normalized, Vector3.up);

            supporter.transform.LookAt(supporter.transform.position - targetOrientation);

            if (supporter.GetComponent<Animator>())
                supporter.GetComponent<Animator>().SetFloat("AnimOffset", Random.Range(0f, 1f));

			supporter.transform.parent = supParent;

            supList.Add(supporter);
        }

    }


    #endregion

#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void OnDrawGizmos() {
		if (!Application.isPlaying) {
			for (int i = 0; i < circles.Length; i++) {
				for (int j = 0; j < circles[i].circle.Length; j++) {
					Gizmos.color = Color.blue;
					Gizmos.DrawLine(circles[i].circle[j].lineStart.position, circles[i].circle[j].lineEnd.position);
				}
			}
		}
    }
#endif    
}

[System.Serializable]
public class SupLines
{
    public Transform lineStart, lineEnd;
}

[System.Serializable]
public class SupCircle
{
    public SupLines[] circle;
}
