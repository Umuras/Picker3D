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
            //Stage �n�ne geldi�i i�in �al���yor.
            CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
            //Burada da inputlar� devre d��� b�rak�yoruz, player hareket edemiyor.
            InputSignals.Instance.onDisableInput?.Invoke();

            //Stage Area Control Process
            //Burada belli s�re i�erisinde player�n toplad��� ve havuz i�indeki objelerin say�s�n� kontrol edece�iz e�er ba�ar�l� ise devam
            //edecek de�ilse fail ekran� gelecek.

            //DOVirtual Dotweenin �zel bir class�d�r. Bu class�n i�inde �ok g�zel fonksiyonlar var. Bir tanesi asenkron bir �ekilde fonksiyon tetiklemeyi
            //sa�l�yor. Sinyal tetikleyip ba�ka classlara eri�me �ans�m�z var. A��rl�kl� olarak DelayedCall fonksiyonu geliyor. Gecikmeli asenkron
            //fonksiyon �a��rmas�nda �ok iyi.
            //DelayedCall kullanmam�z�n sebebi gecikmeli bir �ekilde fonksiyon �a��rmak. Bunu neden yap�yoruz toplar havuza d��t�ken belli bir s�re sonra
            //o toplar�n havuzdan yok olmas�n� istiyoruz o y�zden.
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
            //42.dkda anlat�yor hoca PlayerControllerde
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
