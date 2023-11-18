using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
        CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
        CoreGameSignals.Instance.onLevelFailed += OnLevelFailed;
        CoreGameSignals.Instance.onReset += OnReset;
        CoreGameSignals.Instance.onStageAreSuccessful += OnStageAreaSuccessful;
    }

    private void OnLevelFailed()
    {
        //Fail panelini a��yoruz.
        CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Fail, 2);
    }

    private void OnLevelSuccessful()
    {
        //Win panelini a��yoruz.
        CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Win, 2);
    }

    private void OnLevelInitialize(byte levelValue)
    {
        //Level Paneli sahneye getiriliyor. ��nk� an itibari ile art�k sahnede level�m�z var.
        CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 0);
        //Burada da o anki level de�erimiz ne ise onu set ediyoruz.
        UISignals.Instance.onSetLevelValue?.Invoke((byte)CoreGameSignals.Instance.onGetLevelValue?.Invoke());
    }

    private void OnStageAreaSuccessful(byte stageValue)
    {
        UISignals.Instance.onSetStageColor?.Invoke(stageValue);
    }

    private void OnReset()
    {
        //T�m panelleri kapat�yoruz.
        CoreUISignals.Instance.onCloaseAllPanels?.Invoke();
        //Start panelini getiriyoruz.
        CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 1);
    }

    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
        CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
        CoreGameSignals.Instance.onLevelFailed -= OnLevelFailed;
        CoreGameSignals.Instance.onReset -= OnReset;
        CoreGameSignals.Instance.onStageAreSuccessful -= OnStageAreaSuccessful;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    public void Play()
    {
        //Oyunu ba�lat�yoruz.
        UISignals.Instance.onPlay?.Invoke();
        //Start panelini kapat�yoruz. Start Panelini 1.layerda a�m��t�k ��nk�
        CoreUISignals.Instance.onClosePanel?.Invoke(1);
        //Oyunumuzu ba�latt���m�z i�in Inputu aktif hale getiriyoruz.
        InputSignals.Instance.onEnableInput?.Invoke();
        CameraSignals.Instance.onSetCameraTarget?.Invoke();
    }

    public void NextLevel()
    {
        CoreGameSignals.Instance.onNextLevel?.Invoke();
    }

    public void RestartLevel()
    {
        CoreGameSignals.Instance.onRestartLevel?.Invoke();
    }
}
