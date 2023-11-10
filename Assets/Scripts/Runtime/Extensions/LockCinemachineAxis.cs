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


[ExecuteInEditMode] //Editörde çalýþmasý için kullandýðýmýz attribute.
[SaveDuringPlay] //Runtimeda kitleme yapabilmemiz için yazýyoruz.
[AddComponentMenu("")] //Bu yazdýðýmýz scripti cinemachine kýsmýna ekleyebilmemiz için AddComponentMenuyu kullanýyoruz.
//AddComponentMenu içerisine isim girdiðin zaman Component Menu kýsmýnda özel olarak o script o isimle gözüküyor.

public class LockCinemachineAxis : CinemachineExtension
{
    [SerializeField]private CinemachineLockAxis lockAxis;

    //Tooltip deðiþken üzerine mouseu getirdiðinde çýkan bilgi kutusu oluyor.
    [Tooltip("Lock the Cinemachine Virtual Camera's X Axis position with this specific value")]
    public float XClampValue = 0; //X ekseninde kameranýn hareketini kitlemek için kullanýyoruz.

    //Pre öncesi Post sonrasý demek bize burada Post lazým olayýn sonrasýndan sonra iþlem yapacaðýz çünkü
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        //Switch yapýsýnýn olmasýnýn sebebi script üzerinde X,Y,Z hangisi seçili ise o konum kitlenecek.
        switch (lockAxis)
        {
            case CinemachineLockAxis.X:
                //Bodydeki kamera pozisyon deðerlerini müdahele etmek istiyoruz o yüzden Body ise diye þart ekledik.
                if (stage == CinemachineCore.Stage.Body)
                {
                    //state.RawPosition diyerek Bodynin pozisyonlarýna eriþtik
                    Vector3 pos = state.RawPosition;
                    //x eksenini 0'a eþitliyoruz
                    pos.x = XClampValue;
                    //bodynin pozisyonunu güncelliyoruz.
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
