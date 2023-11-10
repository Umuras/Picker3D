using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using System;

public class PoolController : MonoBehaviour
{
    //[SerializeField] 
    //private List<DOTweenAnimation> tweens = new List<DOTweenAnimation>();

    [SerializeField] 
    private TextMeshPro poolText;

    [SerializeField] 
    private byte stageID;

    [SerializeField]
    private new Renderer renderer;

    [ShowNonSerializedField]
    private PoolData _data;
    [ShowNonSerializedField]
    private byte _collectedCount;

    private readonly string _collectable = "Collectable";

    private void Awake()
    {
        _data = GetPoolData();
    }

    private PoolData GetPoolData()
    {
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
        if (stageValue != stageID)
        {
            return;
        }

        renderer.material.DOColor(new Color(0.1607842f, 0.6039216f, 01766218f), 1f).SetEase(Ease.Linear);
    }

    private void OnActiveTweens(byte stageValue)
    {
        if (stageValue != stageID)
        {
            return;
        }

        //foreach (var tween in tweens)
        //{
        //    tween.DOPlay();
        //}
    }

    private void Start()
    {
        SetRequiredAmountText();
    }

    private void SetRequiredAmountText()
    {
        poolText.text = $"0/{_data.RequiredObjectCount}";
    }

    public bool TakeResults(byte managerStageValue)
    {
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

        IncreaseCollectedAmount();
        SetCollectedAmountToPool();
    }

    private void IncreaseCollectedAmount()
    {
        _collectedCount++;
    }

    private void SetCollectedAmountToPool()
    {
        poolText.text = $"{_collectedCount}/{_data.RequiredObjectCount}";
    }
    //Decrease ve Increase iþlemi tek bir fonksiyon üzerinden yapýlabilir mi düþün
    private void DecreaseCollectedAmount()
    {
        _collectedCount--;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(_collectable))
        {
            return;
        }

        DecreaseCollectedAmount();
        SetCollectedAmountToPool();
    }

   
}
