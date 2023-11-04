using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tek bir görevi olacak ve Monobehaviourdan türemeyecek,
//Görevi levellarý sahneye getirmek
//Commandlerin Controllerlara göre farký þu Commandlerin spesifik tek bir görevi var, hiçbir ek
//iþlem yapmýyorlar. Herhangi bir datalarý yok. Çok özerk bir yapýya pek sahip deðiller. Çivi
//çakmak gibi görevi var, aslýnda. Bir araba inþaa etmeye çalýþmýyorlar.
//Burasý tam CommandPattern deðildir, Strateji Patterndir, Command Patternlerde iþlem kaydý var-
//dýr, iþlemi geri alma özelliði de vardýr.
public class OnLevelLoaderCommand
{
    private Transform _levelHolder;

    public OnLevelLoaderCommand(Transform levelHolder)
    {
        _levelHolder = levelHolder;
    }

    //CommandPatterne özel adlandýrma. Execute.
    public void Execute(byte levelIndex)
    {
        //Resources klasörü içindeki level prefabýna eriþip sonrada onu instaniate ediyoruz.
        Object.Instantiate(Resources.Load<GameObject>($"Prefabs/LevelPrefabs/level {levelIndex}"), _levelHolder,true);
    }
}
