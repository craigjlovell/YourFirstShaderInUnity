using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    Canvas canvas = null;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvas = GameObject.FindGameObjectWithTag("Game").GetComponent<Canvas>();
            if (canvas.enabled == true)
            {
                canvas = null;
                Time.timeScale = 0;
                canvas = GetComponentInParent<Canvas>();
                canvas.enabled = false;
                canvas = GameObject.FindGameObjectWithTag("Pause").GetComponent<Canvas>();
                canvas.enabled = true;
            }
            else
            {
                Time.timeScale = 1;
                canvas = GameObject.FindGameObjectWithTag("Pause").GetComponent<Canvas>();
                canvas.enabled = false;
                canvas = GameObject.FindGameObjectWithTag("Game").GetComponent<Canvas>();
                canvas.enabled = true;
            }
        }
    }
}