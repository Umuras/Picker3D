using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CoreGameSignals : MonoSingleton<CoreGameSignals>
{
    #region Singleton
    //public static CoreGameSignals Instance;

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

    public UnityAction<byte> onLevelInitialize = delegate { };
    public UnityAction onClearActiveLevel = delegate { };
    public UnityAction onLevelSuccessful = delegate { };
    public UnityAction onLevelFailed = delegate { };
    public UnityAction onNextLevel = delegate { };
    public UnityAction onRestartLevel = delegate { };
    public UnityAction onReset = delegate { };
    public Func<byte> onGetLevelValue = delegate { return 0; };
    public UnityAction onStageAreaEntered = delegate { };
    public UnityAction<byte> onStageAreaSuccessful = delegate { };
    public UnityAction onFinishAreaEntered = delegate { }; //Bu k�sm� hoca yapmayacak biz yapaca��z.
    public UnityAction onDestroyCollectibleParticles = delegate { };
    public Func<float> onTakeCollectedTotalCountRate = delegate { return 0; };
    public UnityAction<float> onMiniGameAreaEntered = delegate { };
}
