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
        //Monobehaviourdan türetmediðimiz için direk Destroyu kullanamýyoruz.
        //Object. diyerek eriþiyoruz.
        Object.Destroy(_levelHolder.GetChild(0).gameObject);
    }
}
