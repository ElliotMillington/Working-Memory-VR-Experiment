using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PanelObject : MonoBehaviour
{

    public GameObject up;
    public GameObject down;

    public Transform panel;
    public string currentDimension;

    public Text panelTitle;
    public Text badgeText;
    public GameObject deleteSign;

    private void Start() {
        PanelGroup script = this.transform.GetComponentInParent<PanelGroup>();
    }

    public int getPanelIndex()
    {
        return panel.transform.GetSiblingIndex();
    }
    
    void moveDown()
    {
        if (this.panel.parent.GetChildCount() != this.panel.GetSiblingIndex()+2)
        {
            int currentIndex = this.panel.GetSiblingIndex();
            int nextIndex = currentIndex+1;

            PanelObject replacedSibling = this.panel.parent.GetChild(nextIndex).GetComponent<PanelObject>();

            this.panel.SetSiblingIndex(nextIndex);

            this.transform.parent.GetComponent<PanelGroup>().switchIndexes(currentIndex, nextIndex);

            this.checkButtons();
            replacedSibling.checkButtons();
        }
    }

    public void moveUp()
    {
        if (this.panel.GetSiblingIndex() != 0)
        {
            int currentIndex = this.panel.GetSiblingIndex();
            int nextIndex = currentIndex-1;

            PanelObject replacedSibling = this.panel.parent.GetChild(nextIndex).GetComponent<PanelObject>();

            this.panel.SetSiblingIndex(nextIndex);

            this.transform.parent.GetComponent<PanelGroup>().switchIndexes(currentIndex, nextIndex);

            this.checkButtons();
            replacedSibling.checkButtons();
        }
    }

    public void checkButtons()
    {
        //if top
        if(this.panel.GetSiblingIndex() == 0)
        {
            this.up.SetActive(false);
        }else
        {
            this.up.SetActive(true);
        }

        //if the bottom
        if(this.panel.parent.GetChildCount() == this.panel.GetSiblingIndex()+2)
        {
            this.down.SetActive(false);
        }else
        {
            this.down.SetActive(true);
        }
    }

    public void negateMovement()
    {
        up.SetActive(false);
        down.SetActive(false);
    }

    public void invertDimension()
    {
        if (currentDimension == "2D")
        {
            currentDimension = "3D";
            badgeText.text = currentDimension;
        } 
        else
        {
            currentDimension = "2D";
            badgeText.text = currentDimension;
        }
    }

    public string getDimension()
    {
        return currentDimension;
    }

    public void deletePanel()
    {
        PanelGroup script = this.transform.GetComponentInParent<PanelGroup>();
        script.removeIndex(getPanelIndex());
    }

    public void setDelete(bool value)
    {
        deleteSign.SetActive(value);
    }
}
