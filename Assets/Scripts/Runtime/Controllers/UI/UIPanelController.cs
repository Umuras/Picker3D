using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    //Canvas içerisindeki layer 0,1,2 gameobjectlirini bu listenin içine yerleþtirip bunlarýn içine panelleri yükleyeceðiz.
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
        //Burada da tüm layerlar dolaþýlýp eðer içinde panel var ise yok ediliyor.
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
        //Her yeni panel açýldýðýnda o layerda bulunan paneli kaldýrýp istenen paneli eklemek için ilk baþta OnClosePaneli çalýþtýrdýk.
        OnClosePanel(value);
        //Burada ise verilen konumdan Panelin ismi girilerek hangi layerýn child objesi olacaðý belirlenerek orada oluþmasý saðlanýyor.
        Instantiate(Resources.Load<GameObject>($"Screens/{panelType}Panel"), layers[value]);
    }

    private void OnClosePanel(int value)
    {
        //value deðerindeki layer gameobjecti içinde panel var mý yok mu kontrol ediliyor yok ise return ediliyor.
        if (layers[value].childCount <= 0)
        {
            return;
        }
        //Editörde kullanýlýrken DestroyImmediate ile yok ediliyor panel Destroy çalýþmýyor
        //Ama oyun build alýndýðýnda ve Runtimeda DestroyImmediate çalýþmadýðý için Destroy ile yok edilyor panel.
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
