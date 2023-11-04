using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InputData
{
    //Parmak h�z� limitasyonu, ayn� zamanda hareket �arpan� HorizontalInputSpeed
    public float HorizontalInputSpeed;
    //Yatay kitleme, fiziksel limitasyon ClampValues
    public Vector2 ClampValues;
    //Yumu�atman�n ne kadar olaca��
    public float ClampSpeed;


}
