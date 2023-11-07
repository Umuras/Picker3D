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

    //PlayerMovementData Input taraf�nda konu�ulan�n birebir ayn�s�. �ift a�amal� taraf�n�n 2. aya��.
    [Serializable]
    public struct PlayerMovementData
    {
        //ForwordSpeed ileri y�nl� sabit h�zla giden parametre.
        public float ForwardSpeed;
        //SidewaySpeed yatay y�nl� sabit h�zla giden parametre. Player taraf�ndaki data k�sm� sa�a sola hareket i�in.
        public float SidewaySpeed;
    }

    [Serializable]
    public struct PlayerMeshData
    {
        //Her ba�ar�l� stage sonras�nda player�n boyutunu b�y�tmek i�in kullanaca��z. 
        public float ScaleCounter;
    }

    [Serializable]
    public struct PlayerForceData
    {
        //Kuvvetin hangi y�ne do�ru olaca��n� belirten de�i�ken.
        public float3 ForceParameters;
    }

