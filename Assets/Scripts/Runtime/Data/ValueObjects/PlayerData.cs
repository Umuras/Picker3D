using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

    [Serializable]
    public struct PlayerData
    {
        public PlayerMovementData MovementData;
        public PlayerMeshData MeshData;
        public PlayerForceData ForceData;
    }

    //PlayerMovementData Input tarafýnda konuþulanýn birebir aynýsý. Çift aþamalý tarafýnýn 2. ayaðý.
    [Serializable]
    public struct PlayerMovementData
    {
        //ForwordSpeed ileri yönlü sabit hýzla giden parametre.
        public float ForwardSpeed;
        //SidewaySpeed yatay yönlü sabit hýzla giden parametre. Player tarafýndaki data kýsmý saða sola hareket için.
        public float SidewaySpeed;
    }

    [Serializable]
    public struct PlayerMeshData
    {
        //Her baþarýlý stage sonrasýnda playerýn boyutunu büyütmek için kullanacaðýz. 
        public float ScaleCounter;
    }

    [Serializable]
    public struct PlayerForceData
    {
        //Kuvvetin hangi yöne doðru olacaðýný belirten deðiþken.
        public float3 ForceParameters;
    }

