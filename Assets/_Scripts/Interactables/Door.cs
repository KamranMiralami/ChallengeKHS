using SFXSystem;
using TMPro;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] TextMeshProUGUI interactionTxt;
    bool isOpen;
    public override void OnHighlight()
    {
        if(indicator != null)
        {
            indicator.SetActive(true);
        }
    }
    public override void OnInteract()
    {
        if (isOpen)
        {
            animator.SetBool("Open", false);
            isOpen = false;
        }
        else
        {
            animator.SetBool("Open", true);
            isOpen = true;
        }
        interactionTxt.text = "Press E to " + (isOpen ? "Close" : "Open");
        SoundSystemManager.Instance.PlaySFX("door");
    }

    public override void OnUnHighlight()
    {
        if (indicator != null)
        {
            indicator.SetActive(false);
        }
    }
    void LateUpdate()
    {
        Vector3 camDir = Camera.main.transform.position - transform.position;
        camDir.y = 0;
        if (Vector3.Dot(transform.forward, camDir) < 0)
        {
            indicator.transform.rotation = 
                Quaternion.Euler(indicator.transform.rotation.eulerAngles.x, 180, indicator.transform.rotation.eulerAngles.z);
        }
        else
        {
            indicator.transform.rotation =
                Quaternion.Euler(indicator.transform.rotation.eulerAngles.x, 0, indicator.transform.rotation.eulerAngles.z);
        }
    }
}
