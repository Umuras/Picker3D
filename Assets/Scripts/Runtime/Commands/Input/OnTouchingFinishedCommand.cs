using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchingFinishedCommand
{
    public delegate bool IsPointerOverUIElement();
    public IsPointerOverUIElement _isPointerOverUIElement;

    public OnTouchingFinishedCommand(IsPointerOverUIElement isPointerOverUIElement)
    {
        _isPointerOverUIElement = isPointerOverUIElement;
    }

    public void Execute(bool isTouching, InputManager manager)
    {
        if (Input.GetMouseButtonUp(0) && !_isPointerOverUIElement.Invoke())
        {
            isTouching = false;
            manager.IsTouching = isTouching;
            InputSignals.Instance.onInputReleased?.Invoke();
            Debug.LogWarning("Executed ---> OnInputReleased");
        }
    }
}
