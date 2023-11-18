using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Geçtiðim stagei tutup ona göre aksiyon almamýz için oluþturuyoruz.
    //Level sayýlarýnýn ortasýnda olan 3 adet kýsým stageleri belirtiyor.
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

    //PlayerManagerin iþ yapmasýný istemiyoruz, bütün rolleri parçalamamýz lazým.
    //PlayerData datalarý oluþtururken parçalanmasýnýn sebebi PlayerMovement PlayerMesh gibi Manager nezninde bu datalarýn direk
    //gerekli görevcilere daðýtýlabiliyor olmasýný saðlamak.
    private void SendDataToControllers()
    {
        movementController.SetData(_data.MovementData);
        meshController.SetData(_data.MeshData);
    }

    //Burada da PlayerManager scriptini ve Forcedatayý ForceBalssToPoolCommande gönderiyoruz.
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
        InputSignals.Instance.onInputTaken += OnInputTaken; //OnInputTaken yerine () => movementController.IsReadyToMove(true); bu þekilde de yazýlabilir,
        //fonksiyon içerisinde tek bir iþlem olduðu için hoca gösterme amaçlý yazýyor ama tavsiye etmiyor.
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
        //IsReadyToPlayde ileri yönlü ve yatay hareket kitlenip açýlýyor.
        movementController.IsReadyToPlay(true);
    }

    private void OnInputTaken()
    {
        //IsReadyToMove da yatay hareket kilitlenip açýlýyor.
        movementController.IsReadyToMove(true);
    }

    private void OnInputDragged(HorizontalInputParams inputParams)
    {
        //Input parametleri her mouse hareket ettiðinde inputParamsdan gelen parametreler movementControllera gidecek ki
        //movementController iþ yapsýn. UpdateInputParams aracý iþlevi görüyor.
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
        //Stage tamamlandýðýnda yeni stage deðerine geçiþ saðlanýyor 1 arttýrýlarak.
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
        //Normal Reset yazmamýsýnýn sebebi fonksiyon ismine Monobehaviourdaki Reset fonksiyonu ile karýþmamasý için.
        movementController.OnReset();
        physicsController.OnReset();
        meshController.OnReset();
    }

    private void UnSubscribeEvents()
    {
        InputSignals.Instance.onInputTaken -= OnInputTaken; //movementController.IsReadyToMove(false) bu þekilde yazarak oluyor mu diye bak
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
