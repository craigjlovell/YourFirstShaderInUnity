using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicCurserControl : MonoBehaviour
{
    InputManagerTPS inputMan = null;
    bool locked = false;
    float timer = 0;
    float timerReset = 0;
    void Start()
    {
        inputMan = GetComponent<InputManagerTPS>();
    }

    void Update()
    {
        if (timer <= 0f)
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                locked = !locked;
                inputMan.cursorLocked = !inputMan.cursorLocked;
                inputMan.cursorInputLook = !inputMan.cursorInputLook;

                if (locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                    timer = timerReset;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    timer = timerReset;
                }
            }
        }
        if (timer < 0f)
            timer -= 1f * Time.deltaTime;
    }
}
