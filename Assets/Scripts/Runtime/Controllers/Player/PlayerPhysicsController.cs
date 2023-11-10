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

            //DOVirtual Dotweenin özel bir classýdýr. Bu classýn içinde çok güzel fonksiyonlar var. Bir tanesi asenkron bir þekilde fonksiyon tetiklemeyi
            //saðlýyor. Sinyal tetikleyip baþka classlara eriþme þansýmýz var. Aðýrlýklý olarak DelayedCall fonksiyonu geliyor. Gecikmeli asenkron
            //fonksiyon çaðýrmasýnda çok iyi.
            //DelayedCall kullanmamýzýn sebebi gecikmeli bir þekilde fonksiyon çaðýrmak. Bunu neden yapýyoruz toplar havuza düþtüken belli bir süre sonra
            //o toplarýn havuzdan yok olmasýný istiyoruz o yüzden.
            DOVirtual.DelayedCall(3, () =>
            {
                bool result = other.transform.parent.GetComponentInChildren<PoolController>()
                .TakeResults(manager.StageValue);

                if (result)
                {
                    CoreGameSignals.Instance.onStageAreSuccessful?.Invoke(manager.StageValue);
                    InputSignals.Instance.onEnableInput?.Invoke();
                }
                else
                {
                    CoreGameSignals.Instance.onLevelFailed?.Invoke();
                }
            });
            return;
        }

        if (other.CompareTag(_finish))
        {
            CoreGameSignals.Instance.onFinishAreaEntered?.Invoke();
            InputSignals.Instance.onDisableInput?.Invoke();
            CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
            return;
        }

        if (other.CompareTag(_miniGame))
        {
            //Write the MiniGame Mechanics
            //42.dkda anlatýyor hoca PlayerControllerde
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Transform transform1 = manager.transform;
        Vector3 position1 = transform1.position;

        Gizmos.DrawSphere(new Vector3(position1.x, position1.y + 1f, position1.z + 1f), 1.35f);
    }



    public void OnReset()
    {

    }
}
