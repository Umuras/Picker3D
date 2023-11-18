using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data.ValueObjects
{
    //Editör tarafında görünür olmasını istediğinimiz için Serializable yapıyoruz.
    [Serializable]
    public struct LevelData
    {
        //Structta bir değişkeni newleyemiyorsun bu şekilde yazman yanlış. Ama class olsaydı
        //yapabilirdik. Struct Class'a göre daha basittir, kapsamı daha küçüktür. Burada struct
        //kullanarak int yerine byte kullandığımız gibi optimizasyon yapıyoruz.
        //Eğer bunu newlenmiş halde kullanmak istiyorsan constructor oluşturmalısın.
        //public List<PoolData> Pools = new List<PoolData>();
        //public LevelData(List<PoolData> pools)
        //{
        // Bu şekilde newlenmiş poolsu Pools değişkenine gönderebilirsin. Ama biz editörde 
        //yapacağımız için gerek yok.
        //    Pools = pools;
        //}
        //Burada da her seviye için poolsdata bilgisini tutuyoruz.
        public List<PoolData> Pools;
        //ScriptableObject üzerinden bu değeri dolduracaksın o leveldaki toplam toplanacak toplanabilir sayısını
        //sonra topladığın obje sayısı ile oranlayıp yüzdelik değeri bulacaksın, yüzde kaçını topladığını
        public short TotalSpawnedCollectableCount;

    }
}
