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

            //DOVirtual Dotweenin �zel bir class�d�r. Bu class�n i�inde �ok g�zel fonksiyon tipleri var. Bir tanesi asenkron bir �ekilde fonksiyon tetiklemeyi
            //sa�l�yor. Bulundu�unuz classtaki Invoke fonksiyonu gibi d���nebilirsin ama burda daha do�ru �ekilde �al���yor. Asenkronlu�u daha do�ru
            //�ekilde �al���yor. Bir de sadece bulundu�u class de�il, 
            //Sinyal tetikleyip ba�ka classlara eri�me �ans�m�z var. A��rl�kl� olarak DelayedCall fonksiyonu geliyor. Gecikmeli asenkron
            //fonksiyon �a��rmas�nda �ok iyi.
            //DelayedCall kullanmam�z�n sebebi gecikmeli bir �ekilde fonksiyon �a��rmak. Bunu neden yap�yoruz toplar havuza d��t�kten belli bir s�re sonra
            //o toplar�n adetine bak�p ona g�re i�lem yap�p havuzdan yok olmas�n� istiyoruz o y�zden.
            DOVirtual.DelayedCall(3, () =>
            {
                //Burada other dedi�i havuz oluyor collectible objelerinin topland��� yer.Onun parent gameobjectine eri�ip childobjeleri �zerinden
                //PoolController Scriptine eri�ip TakeResults fonksiyonunu �al��t�r�yoruz.
                //PoolControllerdan almam�z�n sebebi toplam toplanan obje say�s�n� ve toplanacak obje say�s�n� PoolController �zerinde tutuyoruz.
                PoolController poolController = other.transform.parent.GetComponentInChildren<PoolController>();
                bool result = poolController.TakeResults(manager.StageValue);

                if (result)
                {
                    //promise yap�s� kullan�larak veya await task particlellar bittikten sonra onStageAreaSuccessfull �al��t�r�lacak
                    //Bulundu�umuz stage de�erini g�nderiyoruz 2 seviye aras� 3 stage var hangisindeysek o gidiyor.
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
            //if(other.CompareTag(_stageArea)) burdaki iften ��k�yor a�a��daki �artlara bo� yere bakmas�n diye yaz�l�yor.
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
            //42.dkda anlat�yor hoca PlayerControllerde
            CoreGameSignals.Instance.onMiniGameAreaEntered?.Invoke(other.transform.parent.GetComponentInChildren<PoolController>().OnTakeCollectedTotalCountRate());
        }
    }

    //Bunu yazmam�z�n sebebi ForceBallsToPoolCommand �zerinde forcepos konumunu do�ru yazm��m�y�z collectiblelar ger�ekten orada m� toplan�yor
    //onun tespitini yapmak i�in yap�yoruz. Physics.OverlapSphere'�n testi i�in.
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
