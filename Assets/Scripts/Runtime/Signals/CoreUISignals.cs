using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoreUISignals : MonoBehaviour
{
    #region Singleton

    public static CoreUISignals Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    #endregion

    //onOpenPaneldeki int parametresi layer kontrol� yap�yor, layer 2 en �nde g�z�ken panel olacak layer 0 ise en arkada g�r�nen panel olacak.
    //TryGetComponent, GetComponent, gerektik�ede FindWithTag kullanabilirsin, di�erlerini kullanma.
    public UnityAction<UIPanelTypes, int> onOpenPanel = delegate { };
    public UnityAction<int> onClosePanel = delegate { };
    public UnityAction onCloaseAllPanels = delegate { };
}
