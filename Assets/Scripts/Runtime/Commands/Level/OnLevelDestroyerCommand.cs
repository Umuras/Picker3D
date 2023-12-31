using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tek bir görevi olacak ve Monobehaviourdan türemeyecek
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
        //Monobehaviourdan türetmediğimiz için direk Destroyu kullanamıyoruz.
        //Object. diyerek erişiyoruz.
        Object.Destroy(_levelHolder.GetChild(0).gameObject);
    }
}
