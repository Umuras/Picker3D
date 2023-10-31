using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PoolData structýný Editör tarafýnda görünür halde kullanmak istediðimizden Serializable 
//yapýyoruz. Editörde bir scriptable altýnda bu deðerleri kullanýp veri girebilmek istiyorsak
//Serializable yapmalýyýz. Serializefield bir class içerisinde Serializable System Namespacei 
//içerisindedir. Bir deðiþken için Serializefield, bir çatý yapý için Serializable kullanýlýr
[Serializable]
public struct PoolData
{
    //int 4 bytelýk bir deðiþken tipi ve daha az yer kaplamasý için int yerine byte kullanýyoruz.
    //byte 1 byte yer kaplýyor.
    //long intin 8 byte hali double da floatýn 8 byte halidir. Bizim requiredobjectcount deðerimiz
    //çok büyük olmayacaðýndan dolayý intte tutmamýza gerek yok o yüzden byte kullanýyoruz. byte
    //0-255 arasýnda bu sayý deðeride bizim için yeterlidir.
    //RequiredObjectCount bize oyunda havuza düþürmemiz gereken kaç adet obje olmasý lazým onu belirliyor.
    public byte RequiredObjectCount;
}
