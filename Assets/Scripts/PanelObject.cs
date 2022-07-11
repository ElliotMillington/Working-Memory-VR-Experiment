using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace WorkingMemory
{
public class PanelObject : MonoBehaviour
{

    public Toggle colourToggle;

    public Toggle colourPanelToggle;

    public Toggle[] colourToggles;

    // used to make relevant objects invisible
    public GameObject colourElement;
    public GameObject colourPanel;




    public Toggle shapeToggle;

    public Toggle shapePanelToggle;

    public Toggle[] shapeToggles;

    // used to make relevant objects invisible
    public GameObject shapeElement;
    public GameObject shapePanel;



    public GameObject up;
    public GameObject down;

    public Transform panel;
    public string currentDimension;

    public Text panelTitle;
    public Text badgeText;
    public GameObject deleteSign;

    public Text sliderValue;

    public Slider slider;

    public PanelData dataObject;

    private void Start() {
        PanelGroup script = this.transform.GetComponentInParent<PanelGroup>();

        colourToggle.onValueChanged.AddListener(delegate{
            togglePanel(colourToggle, "colour");
            });

        colourPanelToggle.onValueChanged.AddListener(delegate{
            togglePanel(colourPanelToggle, "colourPanel");
            });
        
        foreach (Toggle toggle in colourToggles)
        {
            toggle.onValueChanged.AddListener(delegate{
            this.gameObject.GetComponent<PanelData>().updateColour(toggle, toggle.isOn);
            });
        }
        
        shapeToggle.onValueChanged.AddListener(delegate{
            togglePanel(shapeToggle, "shape");
            });
        
        shapePanelToggle.onValueChanged.AddListener(delegate{
            togglePanel(shapePanelToggle, "shapePanel");
            });
        
        foreach (Toggle toggle in shapeToggles)
        {
            toggle.onValueChanged.AddListener(delegate{
            this.gameObject.GetComponent<PanelData>().updateShape(toggle, toggle.isOn);
            });
        }
    }

    void togglePanel(Toggle toggleObject, string name)
    {
        if (name == "colour" || name == "colourPanel")
        {
            colourToggle.isOn = toggleObject.isOn;
            colourPanelToggle.isOn = toggleObject.isOn;

            if (name == "colourPanel")
            {
                colourElement.SetActive(true);
                colourPanel.SetActive(false);

                //reset to the colours
                this.gameObject.GetComponent<PanelData>().colourReset();

                //re-check the boxes
                foreach (Toggle panelToggle in colourToggles)
                {
                    panelToggle.isOn = true;
                }

            }else
            {
                colourElement.SetActive(false);
                colourPanel.SetActive(true);
            }
        } else if (name == "shape" || name == "shapePanel")
        {
            shapeToggle.isOn = toggleObject.isOn;
            shapePanelToggle.isOn = toggleObject.isOn;

            if (name == "shapePanel")
            {
                shapeElement.SetActive(true);
                shapePanel.SetActive(false);

                //reset to the colours
                this.gameObject.GetComponent<PanelData>().shapeReset();

                //re-check the boxes
                foreach (Toggle panelToggle in shapeToggles)
                {
                    panelToggle.isOn = true;
                }

            }else
            {
                shapeElement.SetActive(false);
                shapePanel.SetActive(true);
            }
        }
    }

    public void deselectShapes()
    {
        foreach (Toggle panelToggle in shapeToggles)
        {
            panelToggle.isOn = false;
        }
    }

    public void deselectColours()
    {
        foreach (Toggle panelToggle in colourToggles)
        {
            panelToggle.isOn = false;
        }
    }

    public void updateSliderValue()
    {
        sliderValue.text = slider.value.ToString();
    }

    public int getPanelIndex()
    {
        return panel.transform.GetSiblingIndex();
    }
    
    void moveDown()
    {
        if (this.panel.parent.childCount != this.panel.GetSiblingIndex()+2)
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
        if(this.panel.parent.childCount == this.panel.GetSiblingIndex()+2)
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
}
