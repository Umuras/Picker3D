using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PoolData struct�n� Edit�r taraf�nda g�r�n�r halde kullanmak istedi�imizden Serializable 
//yap�yoruz. Edit�rde bir scriptable alt�nda bu de�erleri kullan�p veri girebilmek istiyorsak
//Serializable yapmal�y�z. Serializefield bir class i�erisinde Serializable System Namespacei 
//i�erisindedir. Bir de�i�ken i�in Serializefield, bir �at� yap� i�in Serializable kullan�l�r
[Serializable]
public struct PoolData
{
    //int 4 bytel�k bir de�i�ken tipi ve daha az yer kaplamas� i�in int yerine byte kullan�yoruz.
    //byte 1 byte yer kapl�yor.
    //long intin 8 byte hali double da float�n 8 byte halidir. Bizim requiredobjectcount de�erimiz
    //�ok b�y�k olmayaca��ndan dolay� intte tutmam�za gerek yok o y�zden byte kullan�yoruz. byte
    //0-255 aras�nda bu say� de�eride bizim i�in yeterlidir.
    //RequiredObjectCount bize oyunda havuza d���rmemiz gereken ka� adet obje olmas� laz�m onu belirliyor.
    public byte RequiredObjectCount;
}
