using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tek bir g�revi olacak ve Monobehaviourdan t�remeyecek
public class OnLevelDestroyerCommand
{
    private Transform _levelHolder;

    public OnLevelDestroyerCommand(Transform levelHolder)
    {
       _levelHolder = levelHolder;
    }

    public void Execute()
    {
        if (_levelHolder.childCount <= 0)
        {
            return;
        }
        //Monobehaviourdan t�retmedi�imiz i�in direk Destroyu kullanam�yoruz.
        //Object. diyerek eri�iyoruz.
        Object.Destroy(_levelHolder.GetChild(0).gameObject);
    }
}
