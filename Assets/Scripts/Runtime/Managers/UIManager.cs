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
        //Fail panelini açýyoruz.
        CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Fail, 2);
    }

    private void OnLevelSuccessful()
    {
        //Win panelini açýyoruz.
        CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Win, 2);
    }

    private void OnLevelInitialize(byte levelValue)
    {
        //Level Paneli sahneye getiriliyor. Çünkü an itibari ile artýk sahnede levelýmýz var.
        CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 0);
        //Burada da o anki level deðerimiz ne ise onu set ediyoruz.
        UISignals.Instance.onSetLevelValue?.Invoke((byte)CoreGameSignals.Instance.onGetLevelValue?.Invoke());
    }

    private void OnStageAreaSuccessful(byte stageValue)
    {
        UISignals.Instance.onSetStageColor?.Invoke(stageValue);
    }

    private void OnReset()
    {
        //Tüm panelleri kapatýyoruz.
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
        //Oyunu baþlatýyoruz.
        UISignals.Instance.onPlay?.Invoke();
        //Start panelini kapatýyoruz. Start Panelini 1.layerda açmýþtýk çünkü
        CoreUISignals.Instance.onClosePanel?.Invoke(1);
        //Oyunumuzu baþlattýðýmýz için Inputu aktif hale getiriyoruz.
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
