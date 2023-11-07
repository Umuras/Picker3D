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



    public void OnReset()
    {

    }
}
