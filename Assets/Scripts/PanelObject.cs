using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Text panelTitle;
    public Text badgeText;
    public GameObject deleteSign;

    // reference to script holding trial data
    public PanelData dataObject;

    // the following are references information about the trial
    public InputField trialInputField;

    public Text dimensionBadgeText;

    public Dropdown targetNumDrop;
    public Dropdown threeDisplayDrop;
    public Dropdown twoDisplayDrop;
    public Dropdown threeLayout;

    public Slider delaySlider;

    public Text sliderValue;

    // The following variables indicate if trial information is valid 
    public bool isValid;

    public GameObject validIcon;
    public GameObject validText;

    public GameObject validQuestionMark;
    public Color validTrialColour;
    public Color invalidTrialColour;

    public Texture validTrialTexture;
    public Texture invalidTrialTexture;

    public GameObject validPanel;

    public GameObject shapeAndColourReason;
    public GameObject trialNumReason;

    // elements visable when 3d option
    public GameObject threeDisplayObj;
    public GameObject threeLayoutObj;

    // elements visable when 2d option
    public GameObject twoDisplayObj;

    public PanelGroup groupScript;


    private void Start() {
        groupScript = this.transform.GetComponentInParent<PanelGroup>();

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

        checkValidity();

        if (dataObject.dimension == 2)
        {   
            threeDisplayObj.SetActive(true);
            threeLayoutObj.SetActive(true);

            twoDisplayObj.SetActive(false);
        } 
        else
        {
            twoDisplayObj.SetActive(true);

            threeDisplayObj.SetActive(false);
            threeLayoutObj.SetActive(false);
        }
    }

    public void checkValidity()
    {
        // check if cardinality of (Shapes X Colours) > Number of Display
        // check that all fields have a value (mainly input field for number of trials)
        int cardinality = (dataObject.selectedColours.Count * dataObject.selectedTextures.Count);

        int dimensionalDisplay = (dataObject.dimension == 2 ? dataObject.twoDisplayNum : dataObject.threeDisplayNum);

        if (cardinality >= dimensionalDisplay && dataObject.numberOfTrials > 0)
        {
            isValid = true;
            validIcon.GetComponent<RawImage>().color = validTrialColour;
            validIcon.GetComponent<RawImage>().texture = validTrialTexture;

            validText.GetComponent<Text>().text = "Valid Trial";
            validPanel.SetActive(false);
        }else{
            isValid = false;
            validIcon.GetComponent<RawImage>().color = invalidTrialColour;
            validIcon.GetComponent<RawImage>().texture = invalidTrialTexture;

            validText.GetComponent<Text>().text = "Invalid Trial";

            if (!(cardinality >= dimensionalDisplay))
            {
                shapeAndColourReason.SetActive(true);
            }else{
                shapeAndColourReason.SetActive(false);
            }

            if (!(dataObject.numberOfTrials > 0))
            {
                trialNumReason.SetActive(true);
            }else{
                trialNumReason.SetActive(false);
            }
        }

        groupScript.allValid = groupScript.checkAllValid();

    }

    public void validMouseOver()
    {
        
        if (isValid)
        {
            // if valid then only show the text
            validText.SetActive(true);
        }else{
            // if invalid then show text and hide icon and show question mark
            validText.SetActive(true);
            validIcon.SetActive(false);
            validQuestionMark.SetActive(true);
        }
    }

    public void validMouseLeave()
    {
        if (isValid)
        {
            // if valid then hide the text
            validText.SetActive(false);
        }else{
            // if invalid then hide text and show icon and hide question mark
            validText.SetActive(false);
            validIcon.SetActive(true);
            validQuestionMark.SetActive(false);
        }
    }

    public void validMouseDown()
    {
        if (!isValid)
        {
            validPanel.SetActive(true);
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
        if (dataObject.dimension == 2)
        {
            dataObject.dimension = 3;
            badgeText.text = "3D";

            threeDisplayObj.SetActive(true);
            threeLayoutObj.SetActive(true);

            twoDisplayObj.SetActive(false);
        } 
        else
        {
            dataObject.dimension = 2;
            badgeText.text = "2D";

            twoDisplayObj.SetActive(true);

            threeDisplayObj.SetActive(false);
            threeLayoutObj.SetActive(false);
        }
    }

    public void deletePanel()
    {
        groupScript.removeIndex(getPanelIndex());
    }

    public void setDelete(bool value)
    {
        deleteSign.SetActive(value);
    }
}
}
