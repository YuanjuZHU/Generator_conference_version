using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapHandAttachments : MonoBehaviour
{
    private static GameObject currentAttachment;

    /// <summary>
    /// hide the current hand attachment and display the "help" menu
    /// </summary>
    /// <param name="helpMenuAttachment"></param>
    public void SwapWithHelpMenu(GameObject helpMenuAttachment)
    {
        GetCurrentAttachment();
        currentAttachment.SetActive(false);
        helpMenuAttachment.SetActive(true);
        Debug.Log("Swaping 0: " + currentAttachment);
        Debug.Log("Swaping 1: " + helpMenuAttachment);
    }

    /// <summary>
    /// hide the "help" menu and display the hidden menu
    /// </summary>
    /// <param name="helpMenuAttachment"></param>
    public void SetBackAttachment(GameObject helpMenuAttachment)
    {
        helpMenuAttachment.SetActive(false);
        currentAttachment.SetActive(true);
    }

    /// <summary>
    /// before displaying the "help" menu, the current menu should be noted
    /// </summary>
    public void GetCurrentAttachment()
    {
        var transform = GameObject.Find("Attachments hands").transform;
        var attachmentCount = transform.childCount;
        Debug.Log("attachmentCount: " + attachmentCount);
        for (int i = 0; i < attachmentCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                currentAttachment = transform.GetChild(i).gameObject;
            } 
        }
    }
}
