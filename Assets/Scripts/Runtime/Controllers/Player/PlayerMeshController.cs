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

    //Manager tarafýndan gelen data olduðu için internal yaptýk.
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
        //Text görünür hale geliyor
        scaleText.DOFade(1, 0).SetEase(Ease.Flash).OnComplete(() =>
        {
            //0.30f saniye sonra görünmez hale geliyor text
            scaleText.DOFade(0, 0.30f).SetDelay(0.35f);
            //0.65 saniye sonra y ekseninde 1 birim yukarý çýkýyor.
            scaleText.rectTransform.DOAnchorPosY(1f,0.65f).SetEase(Ease.Linear);
        });
    }

    //Particle systemde partiklarýn altýnda birkaç tane daha particle bulunuyor ki particle daha kaliteli gözükebilsin.
    internal void PlayConfetti()
    {
        //Play fonksiyonu tetiklendiðin particelýn tüm childlarýda tetikleniyor. Play Garbage Allocation oluþturuyor.
        confetti.Play();
        //Playýn þöyle bir dezavantaji var. Burada Object Pooling dediðimiz bir kavram var. Bu Object Pooling particlelar içinde geçerli aslýnda.
        //Çünkü particlelarý sürekli Playlediðinizde ya da Instantiate Destroy yaptýðýnýz zaman çok ciddi bir Garbage Allocation alýyorsunuz 
        //Bu direk ekran kartýna bindirilen bir yük. Dolayýsýyla iyi bir yöntem deðil ama eðerki particlelarý Play yerine Instantiate Destroy yerine
        //Emit yöntemi kullanýrsanýz, o Emittede referanslayarak aslýnda bir Pooling sistemi yazmýþ olup çok çok optimize bir yöntem ile gerekli particleý
        //sahnede Emit edip iþi bitince gönderecek þekilde yönetebilirsiniz.
        //confetti.Emit(new ParticleSystem.EmitParams() Emit ile Garbage Allocationý ortadan kaldýrýp Pooling sistemi ile particle sistemi çalýþtýrýyoruz. Daha iyi bir sistem.
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

    //Bu reset mini game'e geçiþ için yazýldý.
    internal void OnReset()
    {
        renderer.gameObject.transform.DOScaleX(1, 1).SetEase(Ease.Linear);
    }
}
