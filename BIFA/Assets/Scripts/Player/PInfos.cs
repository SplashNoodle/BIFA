using UnityEngine;

[CreateAssetMenu(fileName = "New Player Infos", menuName = "BIFA2018/Player Infos")]
public class PInfos : ScriptableObject
{
    public string pseudo;

    public int equipe;
    public int pIndex;

    public int selectorIndex;
    public int characterIndex;
    public int clothesIndex;

	public Sprite uiSprite;
}
