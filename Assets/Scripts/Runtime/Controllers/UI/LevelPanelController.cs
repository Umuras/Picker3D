using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelController : MonoBehaviour
{
    //Level paneldeki Imagelere eri�mek i�in bu listeyi kullan�yoruz.
    [SerializeField]
    private List<Image> stageImages = new List<Image>();

    //Level paneldeki Textlere eri�mek i�in bu listeyi kullan�yoruz.
    [SerializeField]
    private List<TextMeshProUGUI> levelTexts = new List<TextMeshProUGUI>();

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        UISignals.Instance.onSetLevelValue += OnSetLevelValue;
        UISignals.Instance.onSetStageColor += OnSetStageColor;
    }

    private void OnSetStageColor(byte stageValue)
    {
        //0.5 saniye s�reyle bu renge ge�i� yap�yoruz.
        //Dotweeni sak�n Updatede kullanma tehlikeli
        //Renk de�erlerini 0 ile 1 aras�nda giriyorsun burada
        stageImages[stageValue].DOColor(new Color(0.9960785f, 0.4196079f, 0.07843138f), 0.5f);
    }

    private void OnSetLevelValue(byte levelValue)
    {
        //Matematikte ve bilgisayarda ilk de�er 0 dan ba�lad��� i�in oyuncuya ilk b�l�m� 0.b�l�m de�il 1.b�l�m olarak g�stermek i�in
        //++levelValue diyerek 1.b�l�m� g�steriyoruz. sol tarafa ++ diyince �nce art�p sonra additionalValue de�i�kenine aktar�m yap�l�yor.
        //Sol taraf mevcut seviyemizi temsil ediyor.
        byte additionalValue = ++levelValue;
        levelTexts[0].text = additionalValue.ToString();
        additionalValue++;
        //Burada ise bir sonraki seviyeyi g�steriyoruz.
        levelTexts[1].text = additionalValue.ToString();
    }

    private void UnSubscribeEvents()
    {
        UISignals.Instance.onSetLevelValue -= OnSetLevelValue;
        UISignals.Instance.onSetStageColor -= OnSetStageColor;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
