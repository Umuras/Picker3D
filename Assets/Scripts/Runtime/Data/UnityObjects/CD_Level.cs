using Assets.Scripts.Data.ValueObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Burada hangi klas�r�n i�inde CD_Level'a t�klarsan�z orada CD_Level isminde bir ScriptableObject
//�retecektir.
[CreateAssetMenu(fileName = "CD_Level", menuName = "Picker3D/CD_Level", order = 0)]
public class CD_Level : ScriptableObject
{
    //Burada da ScriptableObject �zerinde kulland���m�z script LevelData listesi �zerinden PoolDatadaki
    //RequiredObjectCountlara eri�ebiliyoruz.
    public List<LevelData> Levels;
}
