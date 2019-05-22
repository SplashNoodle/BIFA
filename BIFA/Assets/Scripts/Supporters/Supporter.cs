using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Supporter", menuName = "BIFA2018/Supporter")]
public class Supporter : ScriptableObject
{

	//BIFA_TODO Quand textures dispo, supprimer le tableau de matériaux

    public string supporterName;
    public GameObject prefab;

    public int supporterSize;

	public Texture2D normalMap;
	public Texture2D[] textures = new Texture2D[2];
    public Color[] colors = new Color[2];
}
