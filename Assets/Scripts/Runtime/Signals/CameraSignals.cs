using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraSignals : MonoSingleton<CameraSignals>
{
    #region Singleton
    //public static CameraSignals Instance;

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    Instance = this;
    //}


    #endregion

    public UnityAction onSetCameraTarget = delegate { };
}
