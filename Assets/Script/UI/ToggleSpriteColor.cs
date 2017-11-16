using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSpriteColor:MonoBehaviour
{
    public void ColorChanger()
    {
        Toggle toggle = GetComponent<Toggle>();

        if (toggle.isOn)
        {
            Color newColor = new Color(1, 1, 1, 1);
            toggle.image.color = newColor;
        }
        else
        {
            Color newColor = new Color(0.5f, 0.5f, 0.5f, 1);
            toggle.image.color = newColor;
        }
    }
}
