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

    //Burada boþ bir delegate olmasýnýn sebebi bu sinyal tetiklendiðinde, dinleyicisi yok ise hata vermesin, oyunu dondurmasýn diye
    //boþa tetiklensin diyoruz aslýnda burada.
    public UnityAction onFirstTimeTouchTaken = delegate { };
    public UnityAction onEnableInput = delegate { };
    public UnityAction onDisableInput = delegate { };
    //public UnityAction<bool> onInputStateChanged = delegate { };
    public UnityAction onInputTaken = delegate { };
    public UnityAction onInputReleased = delegate { };
    public UnityAction<HorizontalInputParams> onInputDragged = delegate { };

    //Params ifadesi parametrenin kýsaltmasýdýr. Bunun bir veri paslayýcýsý olduðunu temsil ediyor.

}
