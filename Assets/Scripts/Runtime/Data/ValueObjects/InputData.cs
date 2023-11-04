using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InputData
{
    //Parmak hýzý limitasyonu, ayný zamanda hareket çarpaný HorizontalInputSpeed
    public float HorizontalInputSpeed;
    //Yatay kitleme, fiziksel limitasyon ClampValues
    public Vector2 ClampValues;
    //Yumuþatmanýn ne kadar olacaðý
    public float ClampSpeed;


}
