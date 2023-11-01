using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputSignals : MonoBehaviour
{
    public static InputSignals Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //Burada bo� bir delegate olmas�n�n sebebi bu sinyal tetiklendi�inde, dinleyicisi yok ise hata vermesin, oyunu dondurmas�n diye
    //bo�a tetiklensin diyoruz asl�nda burada.
    public UnityAction onFirstTimeTouchTaken = delegate { };
    public UnityAction onEnableInput = delegate { };
    public UnityAction onDisableInput = delegate { };
    //public UnityAction<bool> onInputStateChanged = delegate { };
    public UnityAction onInputTaken = delegate { };
    public UnityAction onInputReleased = delegate { };
    public UnityAction<HorizontalInputParams> onInputDragged = delegate { };

    //Params ifadesi parametrenin k�saltmas�d�r. Bunun bir veri paslay�c�s� oldu�unu temsil ediyor.

}
