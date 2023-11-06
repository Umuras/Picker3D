using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelController : MonoBehaviour
{
    //Level paneldeki Imagelere eriþmek için bu listeyi kullanýyoruz.
    [SerializeField]
    private List<Image> stageImages = new List<Image>();

    //Level paneldeki Textlere eriþmek için bu listeyi kullanýyoruz.
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
        //0.5 saniye süreyle bu renge geçiþ yapýyoruz.
        //Dotweeni sakýn Updatede kullanma tehlikeli
        //Renk deðerlerini 0 ile 1 arasýnda giriyorsun burada
        stageImages[stageValue].DOColor(new Color(0.9960785f, 0.4196079f, 0.07843138f), 0.5f);
    }

    private void OnSetLevelValue(byte levelValue)
    {
        //Matematikte ve bilgisayarda ilk deðer 0 dan baþladýðý için oyuncuya ilk bölümü 0.bölüm deðil 1.bölüm olarak göstermek için
        //++levelValue diyerek 1.bölümü gösteriyoruz. sol tarafa ++ diyince önce artýp sonra additionalValue deðiþkenine aktarým yapýlýyor.
        //Sol taraf mevcut seviyemizi temsil ediyor.
        byte additionalValue = ++levelValue;
        levelTexts[0].text = additionalValue.ToString();
        additionalValue++;
        //Burada ise bir sonraki seviyeyi gösteriyoruz.
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
