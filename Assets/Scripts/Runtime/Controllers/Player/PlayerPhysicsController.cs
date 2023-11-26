using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    [SerializeField]
    private PlayerManager manager;
    [SerializeField]
    private new Collider collider;
    [SerializeField]
    private new Rigidbody rigidbody;

    private readonly string _stageArea = "StageArea";
    private readonly string _finish = "FinishArea";
    private readonly string _miniGame = "MiniGameArea";

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_stageArea))
        {
            manager.ForceCommand.Execute();
            //Stage önüne geldiði için çalýþýyor.
            CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
            //Burada da inputlarý devre dýþý býrakýyoruz, player hareket edemiyor.
            InputSignals.Instance.onDisableInput?.Invoke();

            //Stage Area Control Process
            //Burada belli süre içerisinde playerýn topladýðý ve havuz içindeki objelerin sayýsýný kontrol edeceðiz eðer baþarýlý ise devam
            //edecek deðilse fail ekraný gelecek.

            //DOVirtual Dotweenin özel bir classýdýr. Bu classýn içinde çok güzel fonksiyon tipleri var. Bir tanesi asenkron bir þekilde fonksiyon tetiklemeyi
            //saðlýyor. Bulunduðunuz classtaki Invoke fonksiyonu gibi düþünebilirsin ama burda daha doðru þekilde çalýþýyor. Asenkronluðu daha doðru
            //þekilde çalýþýyor. Bir de sadece bulunduðu class deðil, 
            //Sinyal tetikleyip baþka classlara eriþme þansýmýz var. Aðýrlýklý olarak DelayedCall fonksiyonu geliyor. Gecikmeli asenkron
            //fonksiyon çaðýrmasýnda çok iyi.
            //DelayedCall kullanmamýzýn sebebi gecikmeli bir þekilde fonksiyon çaðýrmak. Bunu neden yapýyoruz toplar havuza düþtükten belli bir süre sonra
            //o toplarýn adetine bakýp ona göre iþlem yapýp havuzdan yok olmasýný istiyoruz o yüzden.
            DOVirtual.DelayedCall(3, () =>
            {
                //Burada other dediði havuz oluyor collectible objelerinin toplandýðý yer.Onun parent gameobjectine eriþip childobjeleri üzerinden
                //PoolController Scriptine eriþip TakeResults fonksiyonunu çalýþtýrýyoruz.
                //PoolControllerdan almamýzýn sebebi toplam toplanan obje sayýsýný ve toplanacak obje sayýsýný PoolController üzerinde tutuyoruz.
                PoolController poolController = other.transform.parent.GetComponentInChildren<PoolController>();
                bool result = poolController.TakeResults(manager.StageValue);

                if (result)
                {
                    //promise yapýsý kullanýlarak veya await task particlellar bittikten sonra onStageAreaSuccessfull çalýþtýrýlacak
                    //Bulunduðumuz stage deðerini gönderiyoruz 2 seviye arasý 3 stage var hangisindeysek o gidiyor.
                    CoreGameSignals.Instance.onStageAreaSuccessful?.Invoke(manager.StageValue);
                    float rateValue = poolController.OnTakeCollectedTotalCountRate();
                    UISignals.Instance.onSetTotalCollectableRate?.Invoke(rateValue);
                    //Player tekrardan hareket edilebilir hale geliyor.
                    InputSignals.Instance.onEnableInput?.Invoke();
                    //if (manager.StageValue == 2)
                    //{
                    //    CoreGameSignals.Instance.onDestroyCollectibleParticles?.Invoke();
                    //}
                }
                else
                {
                    //Karakter durduruluyor ve Fail Panelini oyuna getiriyoruz.
                    CoreGameSignals.Instance.onLevelFailed?.Invoke();
                }
            });
            //if(other.CompareTag(_stageArea)) burdaki iften çýkýyor aþaðýdaki þartlara boþ yere bakmasýn diye yazýlýyor.
            return;
        }

        if (other.CompareTag(_finish))
        {
            CoreGameSignals.Instance.onFinishAreaEntered?.Invoke();
            InputSignals.Instance.onDisableInput?.Invoke();
            return;
        }

        if (other.CompareTag(_miniGame))
        {
            //Write the MiniGame Mechanics
            //42.dkda anlatýyor hoca PlayerControllerde
            CoreGameSignals.Instance.onMiniGameAreaEntered?.Invoke(other.transform.parent.GetComponentInChildren<PoolController>().OnTakeCollectedTotalCountRate());
        }
    }

    //Bunu yazmamýzýn sebebi ForceBallsToPoolCommand üzerinde forcepos konumunu doðru yazmýþmýyýz collectiblelar gerçekten orada mý toplanýyor
    //onun tespitini yapmak için yapýyoruz. Physics.OverlapSphere'ýn testi için.
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Transform transform1 = manager.transform;
    //    Vector3 position1 = transform1.position;

    //    Gizmos.DrawSphere(new Vector3(position1.x, position1.y - 1f, position1.z + 0.9f), 1.7f);
    //}



    public void OnReset()
    {

    }
}
