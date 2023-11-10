using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchingStartedCommand
{
    public delegate bool IsPointerOverUIElement();
    public IsPointerOverUIElement _isPointerOverUIElement;

    public OnTouchingStartedCommand(IsPointerOverUIElement isPointerOverUIElement)
    {
        _isPointerOverUIElement = isPointerOverUIElement;
    }

    public void Execute(bool isTouching, bool isFirstTimeTouchTaken, Vector2? mousePosition, InputManager manager)
    {
        if (Input.GetMouseButtonDown(0) && !_isPointerOverUIElement.Invoke())
        {
            isTouching = true;
            manager.IsTouching = isTouching;
            InputSignals.Instance.onInputTaken?.Invoke();
            Debug.LogWarning("Executed ---> OnInputTaken");
            if (!isFirstTimeTouchTaken)
            {
                isFirstTimeTouchTaken = true;
                manager.IsFirstTimeTouchTaken = isFirstTimeTouchTaken;
                InputSignals.Instance.onFirstTimeTouchTaken?.Invoke();
                Debug.LogWarning("Executed ---> OnFirstTimeTouchTaken");
            }

            mousePosition = Input.mousePosition;
            manager.MousePosition = mousePosition;
        }
    }
}
