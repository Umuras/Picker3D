using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventSubscriber : MonoBehaviour
{
    [SerializeField]
    private UIEventSubscriptionTypes type;
    [SerializeField]
    private Button _button;
    private UIManager _manager;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        //Burada UIManagerdaki fonksiyonlarý kullanmak için eriþiyoruz ama solid prensiplerine uygun deðil.
        _manager = FindObjectOfType<UIManager>();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        switch (type)
        {
            case UIEventSubscriptionTypes.OnPlay:
                _button.onClick.AddListener(_manager.Play);
                break;
            case UIEventSubscriptionTypes.OnNextLevel:
                _button.onClick.AddListener(_manager.NextLevel);
                break;
            case UIEventSubscriptionTypes.OnRestartLevel:
                _button.onClick.AddListener(_manager.RestartLevel);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UnSubscribeEvents()
    {
        switch (type)
        {
            case UIEventSubscriptionTypes.OnPlay:
                _button.onClick.RemoveListener(_manager.Play);
                break;
            case UIEventSubscriptionTypes.OnNextLevel:
                _button.onClick.RemoveListener(_manager.NextLevel);
                break;
            case UIEventSubscriptionTypes.OnRestartLevel:
                _button.onClick.RemoveListener(_manager.RestartLevel);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
