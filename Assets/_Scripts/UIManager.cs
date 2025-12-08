using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class UIManager : SingletonBehaviour<UIManager>
{
    [SerializeField] TextMeshProUGUI ammoTxt;
    [SerializeField] TextMeshProUGUI modeTxt;
    public void ChangeAmmo(int count)
    {
        ammoTxt.text = "Ammo : " + count;
    }
    public void ChangeMode(string mode)
    {
        // I wanted to show the actuall button (context specific, like for gamepads too),
        // but unity input system doesn't have an easy solution for that, so I just give up on that for now
        modeTxt.text = "Mode (Press C): " + mode; 
    }
    public void DisplayAmmo(bool val)
    {
        ammoTxt.gameObject.SetActive(val);
    }
    public void DisplayMode(bool val)
    {
        modeTxt.gameObject.SetActive(val);
    }
}
