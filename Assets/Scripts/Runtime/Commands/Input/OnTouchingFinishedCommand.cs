using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchingFinishedCommand : MonoBehaviour
{
    public delegate bool IsPointerOverUIElement();
    public IsPointerOverUIElement _isPointerOverUIElement;

    public OnTouchingFinishedCommand(IsPointerOverUIElement isPointerOverUIElement)
    {
        _isPointerOverUIElement = isPointerOverUIElement;
    }

    public void Execute(bool isTouching)
    {
        if (Input.GetMouseButtonUp(0) && !_isPointerOverUIElement.Invoke())
        {
            isTouching = false;
            InputSignals.Instance.onInputReleased?.Invoke();
            Debug.LogWarning("Executed ---> OnInputReleased");
        }
    }
}
