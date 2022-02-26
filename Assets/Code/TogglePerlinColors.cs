using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePerlinColors : MonoBehaviour
{
    public Toggle toggle;
    public GameObject pColors;
    public GameObject strictColors;
        public void ToggleColors()
    {
        pColors.SetActive(!toggle.isOn);
        strictColors.SetActive(toggle.isOn);
    }
}
