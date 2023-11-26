using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMeshController : MonoBehaviour
{
    [SerializeField]
    private new Renderer renderer;
    [SerializeField]
    private TextMeshPro scaleText;
    [SerializeField]
    private ParticleSystem confetti;
    [SerializeField]
    private ParticleSystem flameParticleLeft;
    [SerializeField]
    private ParticleSystem flameParticleRight;


    private PlayerMeshData _data;

    private void Awake()
    {
        scaleText.gameObject.SetActive(false);
    }

    //Manager taraf�ndan gelen data oldu�u i�in internal yapt�k.
    internal void SetData(PlayerMeshData data)
    {
        _data = data;
    }

    internal void ScaleUpPlayer()
    {
        renderer.gameObject.transform.DOScaleX(_data.ScaleCounter, 1f).SetEase(Ease.Flash);
    }

    internal void ShowUpText()
    {
        scaleText.gameObject.SetActive(true);
        //Text g�r�n�r hale geliyor
        scaleText.DOFade(1, 0).SetEase(Ease.Flash).OnComplete(() =>
        {
            //0.30f saniye sonra g�r�nmez hale geliyor text
            scaleText.DOFade(0, 0.30f).SetDelay(0.35f);
            //0.65 saniye sonra y ekseninde 1 birim yukar� ��k�yor.
            scaleText.rectTransform.DOAnchorPosY(1f,0.65f).SetEase(Ease.Linear);
        });
    }

    //Particle systemde partiklar�n alt�nda birka� tane daha particle bulunuyor ki particle daha kaliteli g�z�kebilsin.
    internal void PlayConfetti()
    {
        //Play fonksiyonu tetiklendi�in particel�n t�m childlar�da tetikleniyor. Play Garbage Allocation olu�turuyor.
        confetti.Play();
        //Play�n ��yle bir dezavantaji var. Burada Object Pooling dedi�imiz bir kavram var. Bu Object Pooling particlelar i�inde ge�erli asl�nda.
        //��nk� particlelar� s�rekli Playledi�inizde ya da Instantiate Destroy yapt���n�z zaman �ok ciddi bir Garbage Allocation al�yorsunuz 
        //Bu direk ekran kart�na bindirilen bir y�k. Dolay�s�yla iyi bir y�ntem de�il ama e�erki particlelar� Play yerine Instantiate Destroy yerine
        //Emit y�ntemi kullan�rsan�z, o Emittede referanslayarak asl�nda bir Pooling sistemi yazm�� olup �ok �ok optimize bir y�ntem ile gerekli particle�
        //sahnede Emit edip i�i bitince g�nderecek �ekilde y�netebilirsiniz.
        //confetti.Emit(new ParticleSystem.EmitParams() Emit ile Garbage Allocation� ortadan kald�r�p Pooling sistemi ile particle sistemi �al��t�r�yoruz. Daha iyi bir sistem.
        //{
        //    position = transform.position,
        //    rotation = transform.localRotation,
        //    velocity = Vector3.zero
        //});
    }

    internal void PlayFlameParticle()
    {
        flameParticleLeft.gameObject.SetActive(true);
        flameParticleRight.gameObject.SetActive(true);
        flameParticleLeft.Play();
        flameParticleRight.Play();
    }

    internal void SetDeActiveFlameParticle()
    {
        flameParticleLeft.gameObject.SetActive(false);
        flameParticleRight.gameObject.SetActive(false);
    }

    //Bu reset mini game'e ge�i� i�in yaz�ld�.
    internal void OnReset()
    {
        renderer.gameObject.transform.DOScaleX(1, 1).SetEase(Ease.Linear);
    }
}
