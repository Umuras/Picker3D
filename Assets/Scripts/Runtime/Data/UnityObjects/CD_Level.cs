using Assets.Scripts.Data.ValueObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Burada hangi klasörün içinde CD_Level'a týklarsanýz orada CD_Level isminde bir ScriptableObject
//üretecektir.
[CreateAssetMenu(fileName = "CD_Level", menuName = "Picker3D/CD_Level", order = 0)]
public class CD_Level : ScriptableObject
{
    //Burada da ScriptableObject üzerinde kullandýðýmýz script LevelData listesi üzerinden PoolDatadaki
    //RequiredObjectCountlara eriþebiliyoruz.
    public List<LevelData> Levels;
}
