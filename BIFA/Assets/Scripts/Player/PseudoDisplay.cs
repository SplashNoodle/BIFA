using UnityEngine;

using TMPro;

public class PseudoDisplay : MonoBehaviour
{
    public TextMeshProUGUI pDisplay;

    public PInfos[] infos;

    private void Awake()
    {
        pDisplay.text = "";
        for (int i = 0; i < infos.Length; i++)
        {
            pDisplay.text += infos[i].pseudo + '\n';
        }
    }
}
