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



    public void OnReset()
    {

    }
}
