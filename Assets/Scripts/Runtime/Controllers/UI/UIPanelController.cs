using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> layers = new List<Transform>();

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreUISignals.Instance.onClosePanel += OnClosePanel;
        CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
        CoreUISignals.Instance.onCloaseAllPanels += OnCloseAllPanels;
    }

    private void OnCloseAllPanels()
    {
        foreach (Transform layer in layers)
        {
            if (layer.childCount <= 0)
            {
                return;
            }

#if UNITY_EDITOR
            DestroyImmediate(layer.GetChild(0).gameObject);
#else
            Destroy(layer.GetChild(0).gameObject);
#endif
        }
    }

    private void OnOpenPanel(UIPanelTypes panelType, int value)
    {
        OnClosePanel(value);
        Instantiate(Resources.Load<GameObject>($"Screens/{panelType}Panel"), layers[value]);
    }

    private void OnClosePanel(int value)
    {
        if (layers[value].childCount <= 0)
        {
            return;
        }

#if UNITY_EDITOR
        DestroyImmediate(layers[value].GetChild(0).gameObject);
#else
        Destroy(layers[value].GetChild(0).gameObject);
#endif
    }

    private void UnSubscribeEvents()
    {
        CoreUISignals.Instance.onClosePanel -= OnClosePanel;
        CoreUISignals.Instance.onOpenPanel -= OnOpenPanel;
        CoreUISignals.Instance.onCloaseAllPanels -= OnCloseAllPanels;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
