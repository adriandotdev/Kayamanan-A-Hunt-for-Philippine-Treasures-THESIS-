using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OpenAllLights : MonoBehaviour
{
    private void OnEnable()
    {
        TimeManager.OpenLights += OpenLight;
    }

    private void OnDisable()
    {
        TimeManager.OpenLights -= OpenLight;
    }

    public void OpenLight(bool isOpen)
    {

        gameObject.GetComponent<Light2D>().enabled = isOpen;
    }
}
