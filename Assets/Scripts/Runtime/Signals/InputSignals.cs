using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class InputSignals : MonoSingleton<InputSignals>
{
    //Bu �ekilde MonoSingletondan miras ald��� zaman art�k singleton olu�turmam�za gerek kalm�yor otomatik olarak singleton yap�s�na sahip oluyor
    //Tek k�t� yan� e�er birden fazla InputSignals var ise oyunda onlar� silemiyor.
    //public static InputSignals Instance;

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    Instance = this;
    //}

    //Burada bo� bir delegate olmas�n�n sebebi bu sinyal tetiklendi�inde, dinleyicisi yok ise hata vermesin, oyunu dondurmas�n diye
    //bo�a tetiklensin diyoruz asl�nda burada.
    public UnityAction onFirstTimeTouchTaken = delegate { };
    public UnityAction onEnableInput = delegate { };
    public UnityAction onDisableInput = delegate { };
    //public UnityAction<bool> onInputStateChanged = delegate { };
    public UnityAction onInputTaken = delegate { };
    public UnityAction onInputReleased = delegate { };
    //Params ifadesi parametrenin k�saltmas�d�r. Bunun bir veri paslay�c�s� oldu�unu temsil ediyor.
    public UnityAction<HorizontalInputParams> onInputDragged = delegate { };
    public UnityAction<bool, InputManager> onTouchingFinished = delegate { };
    public UnityAction<bool, bool, Vector2?, InputManager> onTouchingStarted = delegate { };
    public Action<bool, Vector2?, InputData, float3, float, InputManager> onTouchingContinues = delegate { };
}
