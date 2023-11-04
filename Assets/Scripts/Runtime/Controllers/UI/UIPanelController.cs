using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    //Canvas i�erisindeki layer 0,1,2 gameobjectlirini bu listenin i�ine yerle�tirip bunlar�n i�ine panelleri y�kleyece�iz.
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
        //Burada da t�m layerlar dola��l�p e�er i�inde panel var ise yok ediliyor.
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
        //Her yeni panel a��ld���nda o layerda bulunan paneli kald�r�p istenen paneli eklemek i�in ilk ba�ta OnClosePaneli �al��t�rd�k.
        OnClosePanel(value);
        //Burada ise verilen konumdan Panelin ismi girilerek hangi layer�n child objesi olaca�� belirlenerek orada olu�mas� sa�lan�yor.
        Instantiate(Resources.Load<GameObject>($"Screens/{panelType}Panel"), layers[value]);
    }

    private void OnClosePanel(int value)
    {
        //value de�erindeki layer gameobjecti i�inde panel var m� yok mu kontrol ediliyor yok ise return ediliyor.
        if (layers[value].childCount <= 0)
        {
            return;
        }
        //Edit�rde kullan�l�rken DestroyImmediate ile yok ediliyor panel Destroy �al��m�yor
        //Ama oyun build al�nd���nda ve Runtimeda DestroyImmediate �al��mad��� i�in Destroy ile yok edilyor panel.
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
