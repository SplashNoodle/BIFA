using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ball", menuName = "BIFA2018/Ball Infos")]
public class BallInfos : ScriptableObject
{
    public enum BallType
    {
        Basket,
        Bowling,
        Foot
    }

    public BallType ballType;

    public float groundBouncingValue;
    public float borderbouncingValue;
    public float maxVelocity;
    public float playerShootingStrength;
}
