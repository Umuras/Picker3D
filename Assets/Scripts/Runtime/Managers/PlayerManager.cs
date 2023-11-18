using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Ge�ti�im stagei tutup ona g�re aksiyon almam�z i�in olu�turuyoruz.
    //Level say�lar�n�n ortas�nda olan 3 adet k�s�m stageleri belirtiyor.
    public byte StageValue;

    internal ForceBallsToPoolCommand ForceCommand;

    [SerializeField]
    private PlayerMovementController movementController;

    [SerializeField]
    private PlayerMeshController meshController;

    [SerializeField]
    private PlayerPhysicsController physicsController;

    private PlayerData _data;

    private void Awake()
    {
        _data = GetPlayerData();
        SendDataToControllers();
        Init();
    }


    private PlayerData GetPlayerData()
    {
        return Resources.Load<CD_Player>("Data/CD_Player").Data;
    }

    //PlayerManagerin i� yapmas�n� istemiyoruz, b�t�n rolleri par�alamam�z laz�m.
    //PlayerData datalar� olu�tururken par�alanmas�n�n sebebi PlayerMovement PlayerMesh gibi Manager nezninde bu datalar�n direk
    //gerekli g�revcilere da��t�labiliyor olmas�n� sa�lamak.
    private void SendDataToControllers()
    {
        movementController.SetData(_data.MovementData);
        meshController.SetData(_data.MeshData);
    }

    //Burada da PlayerManager scriptini ve Forcedatay� ForceBalssToPoolCommande g�nderiyoruz.
    private void Init()
    {
        ForceCommand = new ForceBallsToPoolCommand(this, _data.ForceData);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        InputSignals.Instance.onInputTaken += OnInputTaken; //OnInputTaken yerine () => movementController.IsReadyToMove(true); bu �ekilde de yaz�labilir,
        //fonksiyon i�erisinde tek bir i�lem oldu�u i�in hoca g�sterme ama�l� yaz�yor ama tavsiye etmiyor.
        InputSignals.Instance.onInputReleased += OnInputReleased;
        InputSignals.Instance.onInputDragged += OnInputDragged;
        UISignals.Instance.onPlay += OnPlay;
        CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
        CoreGameSignals.Instance.onLevelFailed += OnLevelFailed;
        CoreGameSignals.Instance.onStageAreaEntered += OnStageAreaEntered;
        CoreGameSignals.Instance.onStageAreSuccessful += OnStageAreaSuccessful;
        CoreGameSignals.Instance.onFinishAreaEntered += OnFinishAreaEntered;
        CoreGameSignals.Instance.onReset += OnReset;
    }

    private void OnPlay()
    {
        //IsReadyToPlayde ileri y�nl� ve yatay hareket kitlenip a��l�yor.
        movementController.IsReadyToPlay(true);
    }

    private void OnInputTaken()
    {
        //IsReadyToMove da yatay hareket kilitlenip a��l�yor.
        movementController.IsReadyToMove(true);
    }

    private void OnInputDragged(HorizontalInputParams inputParams)
    {
        //Input parametleri her mouse hareket etti�inde inputParamsdan gelen parametreler movementControllera gidecek ki
        //movementController i� yaps�n. UpdateInputParams arac� i�levi g�r�yor.
        movementController.UpdateInputParams(inputParams);
    }

    private void OnInputReleased()
    {
        movementController.IsReadyToMove(false);
    }
    private void OnStageAreaEntered()
    {
        movementController.IsReadyToPlay(false);
    }

    private void OnStageAreaSuccessful(byte value)
    {
        //Stage tamamland���nda yeni stage de�erine ge�i� sa�lan�yor 1 artt�r�larak.
        StageValue = (byte)++value;
        movementController.IsReadyToPlay(true);
        meshController.ScaleUpPlayer();
        meshController.PlayConfetti();
        meshController.ShowUpText();
    }

    private void OnFinishAreaEntered()
    {
        CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
        //Mini Game Yazilmali
    }

    private void OnLevelFailed()
    {
        movementController.IsReadyToPlay(false);
    }

    private void OnLevelSuccessful()
    {
        movementController.IsReadyToPlay(false);
    }

    private void OnReset()
    {
        StageValue = 0;
        //Normal Reset yazmam�s�n�n sebebi fonksiyon ismine Monobehaviourdaki Reset fonksiyonu ile kar��mamas� i�in.
        movementController.OnReset();
        physicsController.OnReset();
        meshController.OnReset();
    }

    private void UnSubscribeEvents()
    {
        InputSignals.Instance.onInputTaken -= OnInputTaken; //movementController.IsReadyToMove(false) bu �ekilde yazarak oluyor mu diye bak
        InputSignals.Instance.onInputReleased -= OnInputReleased;
        InputSignals.Instance.onInputDragged -= OnInputDragged;
        UISignals.Instance.onPlay -= OnPlay;
        CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
        CoreGameSignals.Instance.onLevelFailed -= OnLevelFailed;
        CoreGameSignals.Instance.onStageAreaEntered -= OnStageAreaEntered;
        CoreGameSignals.Instance.onStageAreSuccessful -= OnStageAreaSuccessful;
        CoreGameSignals.Instance.onFinishAreaEntered -= OnFinishAreaEntered;
        CoreGameSignals.Instance.onReset -= OnReset;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
