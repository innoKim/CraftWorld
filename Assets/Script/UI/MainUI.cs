using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {
    
    enum eMainUI
    {
        Charactor,
        Item,
        Craft,
        None
    }
    
    public List<Toggle> toggles;
    public List<GameObject> pannels;
    public GameObject mainPannel;
    eMainUI curUI;
    
    void Start()
    {
        mainPannel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) OnOff(eMainUI.Item);
        else if (Input.GetKeyDown(KeyCode.O)) OnOff(eMainUI.Craft);
        else if (Input.GetKeyDown(KeyCode.U)) OnOff(eMainUI.Charactor);
    }

    void OnOff(eMainUI uiSort)
    {
        if (!mainPannel.active)
        {
            mainPannel.SetActive(true);
            for (int i = 0; i < toggles.Count; i++)
            {
                toggles[i].isOn = (i == (int)uiSort);
                pannels[i].SetActive(i == (int)uiSort);
                toggles[i].gameObject.GetComponent<ToggleSpriteColor>().ColorChanger();
            }

            curUI = uiSort;
        }

        else if (curUI == uiSort)
        {
            mainPannel.SetActive(false);
        }

        else
        {
            mainPannel.SetActive(false);
            OnOff(uiSort);
        }
    }    
}
