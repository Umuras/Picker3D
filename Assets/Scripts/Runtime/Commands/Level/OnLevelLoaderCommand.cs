using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tek bir g�revi olacak ve Monobehaviourdan t�remeyecek,
//G�revi levellar� sahneye getirmek
//Commandlerin Controllerlara g�re fark� �u Commandlerin spesifik tek bir g�revi var, hi�bir ek
//i�lem yapm�yorlar. Herhangi bir datalar� yok. �ok �zerk bir yap�ya pek sahip de�iller. �ivi
//�akmak gibi g�revi var, asl�nda. Bir araba in�aa etmeye �al��m�yorlar.
//Buras� tam CommandPattern de�ildir, Strateji Patterndir, Command Patternlerde i�lem kayd� var-
//d�r, i�lemi geri alma �zelli�i de vard�r.
public class OnLevelLoaderCommand
{
    private Transform _levelHolder;

    public OnLevelLoaderCommand(Transform levelHolder)
    {
        _levelHolder = levelHolder;
    }

    //CommandPatterne �zel adland�rma. Execute.
    public void Execute(byte levelIndex)
    {
        //Resources klas�r� i�indeki level prefab�na eri�ip sonrada onu instaniate ediyoruz.
        Object.Instantiate(Resources.Load<GameObject>($"Prefabs/LevelPrefabs/level {levelIndex}"), _levelHolder,true);
    }
}
