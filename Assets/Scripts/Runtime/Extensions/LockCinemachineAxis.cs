using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CinemachineLockAxis
{
    X,
    Y,
    Z
}


[ExecuteInEditMode] //Edit�rde �al��mas� i�in kulland���m�z attribute.
[SaveDuringPlay] //Runtimeda kitleme yapabilmemiz i�in yaz�yoruz.
[AddComponentMenu("")] //Bu yazd���m�z scripti cinemachine k�sm�na ekleyebilmemiz i�in AddComponentMenuyu kullan�yoruz.
//AddComponentMenu i�erisine isim girdi�in zaman Component Menu k�sm�nda �zel olarak o script o isimle g�z�k�yor.

public class LockCinemachineAxis : CinemachineExtension
{
    [SerializeField]private CinemachineLockAxis lockAxis;

    //Tooltip de�i�ken �zerine mouseu getirdi�inde ��kan bilgi kutusu oluyor.
    [Tooltip("Lock the Cinemachine Virtual Camera's X Axis position with this specific value")]
    public float XClampValue = 0; //X ekseninde kameran�n hareketini kitlemek i�in kullan�yoruz.

    //Pre �ncesi Post sonras� demek bize burada Post laz�m olay�n sonras�ndan sonra i�lem yapaca��z ��nk�
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        //Switch yap�s�n�n olmas�n�n sebebi script �zerinde X,Y,Z hangisi se�ili ise o konum kitlenecek.
        switch (lockAxis)
        {
            case CinemachineLockAxis.X:
                //Bodydeki kamera pozisyon de�erlerini m�dahele etmek istiyoruz o y�zden Body ise diye �art ekledik.
                if (stage == CinemachineCore.Stage.Body)
                {
                    //state.RawPosition diyerek Bodynin pozisyonlar�na eri�tik
                    Vector3 pos = state.RawPosition;
                    //x eksenini 0'a e�itliyoruz
                    pos.x = XClampValue;
                    //bodynin pozisyonunu g�ncelliyoruz.
                    state.RawPosition = pos;
                }
                break;
            case CinemachineLockAxis.Y:
                if (stage == CinemachineCore.Stage.Body)
                {
                    Vector3 pos = state.RawPosition;
                    pos.y = XClampValue;
                    state.RawPosition = pos;
                }
                break;
            case CinemachineLockAxis.Z:
                if (stage == CinemachineCore.Stage.Body)
                {
                    Vector3 pos = state.RawPosition;
                    pos.z = XClampValue;
                    state.RawPosition = pos;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}
