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
    //DotweenAnimation üzerinden objelere animasyon verilecek sonra o objelerin üzerinde bulunan DOTweenAnimation scriptleri bu listeye atýlacak.
    [SerializeField] 
    private List<DOTweenAnimation> tweens = new List<DOTweenAnimation>();

    [SerializeField] 
    private TextMeshPro poolText;
    //Birden fazla poolumuz yani collectiblelarýn toplanacaðý havuz olduðu için o havuzun hangi stagedeki havuz olduðunu anlamak için kullanýyoruz.
    [SerializeField] 
    private byte stageID;
    //Poolun rengini deðiþtirmek için kullanýyoruz.
    [SerializeField]
    private new Renderer renderer;

    [ShowNonSerializedField]
    private PoolData _data;
    //Toplanýlan objelerin adedini tutuyoruz.
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
        //Levels listesi içinde ilk baþta onGetLevelValue sinyali üzerinden o anki seviyeye eriþiyoruz, ardýndan bulunduðumuz seviye üzerinden
        //Pools listesinden stageID üzerinden o stagedeki requiredObjectCount deðerine eriþiyoruz.
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
        //Mevcut stage ile bu pool ayný mý onun kontrolü yapýlýyor, yani bulunduðumuz pool bulunduðumuz stage'a mi ait
        if (stageValue != stageID)
        {
            return;
        }
        //Havuzun rengini deðiþtiriyoruz.
        renderer.material.DOColor(new Color(poolAfterColor.x, poolAfterColor.y, poolAfterColor.z, 1), 0.5f).SetEase(Ease.Flash).SetRelative(false);
    }

    private void OnActiveTweens(byte stageValue)
    {
        //Mevcut stage ile bu pool ayný mý onun kontrolü yapýlýyor, yani bulunduðumuz pool bulunduðumuz stage'a mi ait
        if (stageValue != stageID)
        {
            return;
        }
        //Tüm DOTweenAnimationlarý çalýþtýrýyoruz.
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
        //PoolText üzerinde o Pool için gerekli olan toplanacak obje sayýsýný yazdýrýyoruz.
        poolText.text = $"0/{_data.RequiredObjectCount}";
    }

    public bool TakeResults(byte managerStageValue)
    {
        //Burada Poolun stageIDsi ile ManagerStageValuesý ayný ise yani ikisinide stage deðeri ayný ise
        //_collectedCount >= _data.RequiredObjectCount iþlemini yapacak.
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
    //Decrease ve Increase iþlemi tek bir fonksiyon üzerinden yapýlabilir mi düþün
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
