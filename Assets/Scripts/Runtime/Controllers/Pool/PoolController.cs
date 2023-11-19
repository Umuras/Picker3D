using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using System;
using Unity.Mathematics;

public class PoolController : MonoBehaviour
{
    //DotweenAnimation �zerinden objelere animasyon verilecek sonra o objelerin �zerinde bulunan DOTweenAnimation scriptleri bu listeye at�lacak.
    [SerializeField] 
    private List<DOTweenAnimation> tweens = new List<DOTweenAnimation>();

    [SerializeField] 
    private TextMeshPro poolText;
    //Birden fazla poolumuz yani collectiblelar�n toplanaca�� havuz oldu�u i�in o havuzun hangi stagedeki havuz oldu�unu anlamak i�in kullan�yoruz.
    [SerializeField] 
    private byte stageID;
    //Poolun rengini de�i�tirmek i�in kullan�yoruz.
    [SerializeField]
    private new Renderer renderer;

    [ShowNonSerializedField]
    private PoolData _data;
    //Toplan�lan objelerin adedini tutuyoruz.
    [ShowNonSerializedField]
    private byte _collectedCount;
    private float3 poolAfterColor = new float3(0.2078432f, 0.3058824f, 0.5294118f);

    private readonly string _collectable = "Collectable";


    private void Awake()
    {
        _data = GetPoolData();
    }

    private PoolData GetPoolData()
    {
        //Levels listesi i�inde ilk ba�ta onGetLevelValue sinyali �zerinden o anki seviyeye eri�iyoruz, ard�ndan bulundu�umuz seviye �zerinden
        //Pools listesinden stageID �zerinden o stagedeki requiredObjectCount de�erine eri�iyoruz.
        return Resources.Load<CD_Level>("Data/CD_Level").Levels[(int)CoreGameSignals.Instance.onGetLevelValue?.Invoke()].Pools[stageID];
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onStageAreSuccessful += OnActiveTweens;
        CoreGameSignals.Instance.onStageAreSuccessful += OnChangePoolColor;
    }

    private void OnChangePoolColor(byte stageValue)
    {
        //Mevcut stage ile bu pool ayn� m� onun kontrol� yap�l�yor, yani bulundu�umuz pool bulundu�umuz stage'a mi ait
        if (stageValue != stageID)
        {
            return;
        }
        //Havuzun rengini de�i�tiriyoruz.
        renderer.material.DOColor(new Color(poolAfterColor.x, poolAfterColor.y, poolAfterColor.z, 1), 0.5f).SetEase(Ease.Flash).SetRelative(false);
    }

    private void OnActiveTweens(byte stageValue)
    {
        //Mevcut stage ile bu pool ayn� m� onun kontrol� yap�l�yor, yani bulundu�umuz pool bulundu�umuz stage'a mi ait
        if (stageValue != stageID)
        {
            return;
        }
        //T�m DOTweenAnimationlar� �al��t�r�yoruz.
        foreach (DOTweenAnimation tween in tweens)
        {
            tween.DOPlay();
        }
    }

    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.onStageAreSuccessful -= OnActiveTweens;
        CoreGameSignals.Instance.onStageAreSuccessful -= OnChangePoolColor;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void Start()
    {
        SetRequiredAmountText();
    }

    private void SetRequiredAmountText()
    {
        //PoolText �zerinde o Pool i�in gerekli olan toplanacak obje say�s�n� yazd�r�yoruz.
        poolText.text = $"0/{_data.RequiredObjectCount}";
    }

    public bool TakeResults(byte managerStageValue)
    {
        //Burada Poolun stageIDsi ile ManagerStageValues� ayn� ise yani ikisinide stage de�eri ayn� ise
        //_collectedCount >= _data.RequiredObjectCount i�lemini yapacak.
        if (stageID == managerStageValue)
        {
            return _collectedCount >= _data.RequiredObjectCount;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_collectable))
        {
            return;
        }

        IncreaseOrDecreaseCollectedAmount(ChangingCollectedAmount.Increase);
        SetCollectedAmountToPool();
    }

    private void IncreaseOrDecreaseCollectedAmount(ChangingCollectedAmount operation)
    {
        switch (operation)
        {
            case ChangingCollectedAmount.Increase:
                _collectedCount++;
                break;
            case ChangingCollectedAmount.Decrase:
                _collectedCount--;
                break;
        }
    }

    //private void IncreaseCollectedAmount()
    //{
    //    _collectedCount++;
    //}

    private void SetCollectedAmountToPool()
    {
        poolText.text = $"{_collectedCount}/{_data.RequiredObjectCount}";
    }
    //Decrease ve Increase i�lemi tek bir fonksiyon �zerinden yap�labilir mi d���n
    //private void DecreaseCollectedAmount()
    //{
    //    _collectedCount--;
    //}

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(_collectable))
        {
            return;
        }

        IncreaseOrDecreaseCollectedAmount(ChangingCollectedAmount.Decrase);
        SetCollectedAmountToPool();
    }

   private enum ChangingCollectedAmount
    {
        Increase,
        Decrase
    }
}
