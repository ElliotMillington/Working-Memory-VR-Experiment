using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

/*

    This script is assigned to each GUI panel object and facilitates the function of that panel.

*/

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

    public Color invertEnabledColour;
    public GameObject deleteSign;

    // reference to script holding trial data
    public PanelData dataObject;

    // the following are references information about the trial
    public InputField trialInputField;

    public Text dimensionBadgeText;

    public GameObject dimensionBadgeObj;

    public Text dimensionBadgeTitle;

    public Dropdown targetNumDrop;
    public Dropdown threeDisplayDrop;
    public Dropdown twoDisplayDrop;
    public Dropdown threeLayout;

    public Dropdown optionDistroDrop;

    public Slider displayTimeSlider;

    public Slider displayDelaySlider;

    public Text displaySliderValue;

    public Text delaySliderValue;


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

    public GameObject vrReason;

    // elements visable when 3d option
    public GameObject threeDisplayObj;
    public GameObject threeLayoutObj;

    // elements visable when 2d option
    public GameObject twoDisplayObj;

    [HideInInspector]
    public PanelGroup groupScript;

    public GameObject duplicateButton;


    public Toggle confirmStartToggle;
    public Toggle targetRandToggle;

    public Toggle displayRandToggle;



    private void Start() {
        groupScript = this.transform.GetComponentInParent<PanelGroup>();

        // creates bunch of listeners for toggle and slider components
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


        confirmStartToggle.onValueChanged.AddListener(delegate{
            simpleToggle(confirmStartToggle, "confirm");
            });
        
        displayRandToggle.onValueChanged.AddListener(delegate{
            simpleToggle(displayRandToggle, "displayRand");
            });
        
        targetRandToggle.onValueChanged.AddListener(delegate{
            simpleToggle(targetRandToggle, "targetRand");
            });

        if (dataObject.dimension == 2)
        {   
            threeDisplayObj.SetActive(false);
            threeLayoutObj.SetActive(false);

            twoDisplayObj.SetActive(true);
        } 
        else
        {
            twoDisplayObj.SetActive(false);

            threeDisplayObj.SetActive(true);
            threeLayoutObj.SetActive(true);
        }
    }

    public void duplicateMouseOver()
    {
        duplicateButton.GetComponent<RawImage>().color = invertEnabledColour;
    }

    public void duplicateMouseExit()
    {
        duplicateButton.GetComponent<RawImage>().color = new Color(1,1,1,1);
    }

    public void mouseOverBadge()
    {
        if (dataObject.dimension == 3)
        {
            dimensionBadgeObj.GetComponent<RawImage>().color = invertEnabledColour;
            dimensionBadgeTitle.text = (groupScript.headsetActive ? "Invert" : "Invert for Validity");
            dimensionBadgeTitle.fontSize = (groupScript.headsetActive ? 28 : 24);

            dimensionBadgeTitle.gameObject.SetActive(true);
        }else{
            // if VR is enbaled then change to purple, set dimension text
            if (groupScript.headsetActive)
            {
                dimensionBadgeObj.GetComponent<RawImage>().color = invertEnabledColour;
                dimensionBadgeTitle.text = "Invert";
                dimensionBadgeTitle.fontSize = 28;

                dimensionBadgeTitle.gameObject.SetActive(true);
            }else
            {
                dimensionBadgeObj.GetComponent<RawImage>().color = invalidTrialColour;
                dimensionBadgeTitle.text = "Connect VR to enable 3D.";
                dimensionBadgeTitle.fontSize = 20;

                dimensionBadgeTitle.gameObject.SetActive(true);
            }
        }
    }

    public void mouseDownBadge()
    {
        if (dataObject.dimension == 3)
        {
            invertDimension();
            if(!groupScript.headsetActive)
            {
                dimensionBadgeObj.GetComponent<RawImage>().color = invalidTrialColour;
                dimensionBadgeTitle.text = "Connect VR to enable 3D.";
                dimensionBadgeTitle.fontSize = 20;
            }
        }else{
            if (groupScript.headsetActive)
            {
                invertDimension();
            }
        }
    }

    public void mouseExitBadge()
    {
        dimensionBadgeObj.GetComponent<RawImage>().color = new Color(1,1,1,1);
        dimensionBadgeTitle.gameObject.SetActive(false);
    }

    // ensures that the trial is valid ***
    public void checkValidity(string source)
    {
        if (groupScript == null) return;
        // check if cardinality of (Shapes X Colours) > Number of Display
        // check that all fields have a value (mainly input field for number of trials)
        int cardinality = (dataObject.selectedColours.Count * dataObject.selectedTextures.Count);

        int dimensionalDisplay = (dataObject.dimension == 2 ? dataObject.twoDisplayNum : dataObject.threeDisplayNum);

        bool isDimenaionCompatible = (dataObject.dimension == 3 && !groupScript.headsetActive ? false : true);

        isValid = true;
        
        if (!(cardinality >= dimensionalDisplay))
        {
            isValid = false;
            shapeAndColourReason.SetActive(true);
        }else{
            shapeAndColourReason.SetActive(false);
        }

        if (!(dataObject.numberOfTrials > 0))
        {
            isValid = false;
            trialNumReason.SetActive(true);
        }else{
            trialNumReason.SetActive(false);
        }

        if(!isDimenaionCompatible)
        {
            isValid = false;
            vrReason.SetActive(true);
        }else{
            vrReason.SetActive(false);
        }   

        if(isValid)
        {
            validIcon.GetComponent<RawImage>().color = validTrialColour;
            validIcon.GetComponent<RawImage>().texture = validTrialTexture;

            validText.GetComponent<Text>().text = "Valid Trial";
        }
        else
        {
            validIcon.GetComponent<RawImage>().color = invalidTrialColour;
            validIcon.GetComponent<RawImage>().texture = invalidTrialTexture;
            validText.GetComponent<Text>().text = "Invalid Trial";
        }
        
        validPanel.SetActive(false);

        if (source != "groupScript") groupScript.allValid = groupScript.checkAllValid();
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
        checkValidity("panelObjectScript");
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
            dimensionBadgeText.text = "3D";

            threeDisplayObj.SetActive(true);
            threeLayoutObj.SetActive(true);

            twoDisplayObj.SetActive(false);
        } 
        else
        {
            dataObject.dimension = 2;
            dimensionBadgeText.text = "2D";

            twoDisplayObj.SetActive(true);

            threeDisplayObj.SetActive(false);
            threeLayoutObj.SetActive(false);
        }

        if (groupScript != null) checkValidity("panelObjectScript");
    }

    public void setDimension(int newDimension)
    {
        if (newDimension == 3)
        {
            dataObject.dimension = 3;
            dimensionBadgeText.text = "3D";

            threeDisplayObj.SetActive(true);
            threeLayoutObj.SetActive(true);
        
            twoDisplayObj.SetActive(false);
        } 
        else
        {
            dataObject.dimension = 2;
            dimensionBadgeText.text = "2D";

            twoDisplayObj.SetActive(true);

            threeDisplayObj.SetActive(false);
            threeLayoutObj.SetActive(false);
        }

    }

    public void deletePanel(GameObject deleteObj)
    {
        groupScript.removeIndex(deleteObj.transform.GetSiblingIndex());
    }

    public void setDelete(bool value)
    {
        deleteSign.SetActive(value);
    }

    public void setDuplicate(bool value)
    {
        duplicateButton.SetActive(value);
    }

    public void duplicate()
    {
        GameObject newObj = (GameObject) Instantiate(this.groupScript.PanelPrefab, this.gameObject.transform.parent);
        newObj.GetComponent<PanelData>().populateNew();
        newObj.transform.SetSiblingIndex(this.gameObject.transform.GetSiblingIndex()+1);
        newObj.transform.localScale = new Vector3(1,1,1);

        PanelData baseScript = this.gameObject.GetComponent<PanelData>();

        newObj.GetComponent<PanelObject>().groupScript = this.transform.GetComponentInParent<PanelGroup>();
        newObj.GetComponent<PanelObject>().duplicateMouseExit();
        newObj.GetComponent<PanelObject>().groupScript.enforceMove("enforce");
        groupScript.duplicationAtIndex(this.gameObject.transform.GetSiblingIndex(), newObj.GetComponent<PanelObject>());

        StartCoroutine(newObj.GetComponent<PanelObject>().duplicateScript(baseScript));
    }

    public IEnumerator duplicateScript(PanelData newDataScript)
    {
        // change all values in the created scipt to reflect the passed script
        // also select those values in the appropriate part of the panel

        yield return new WaitForSeconds(0.5f);

        //setting dimension
        setDimension(newDataScript.dimension);

        //ensure correct options are displayed suitable to the dimension
        if (dataObject.dimension == 2)
        {   
            threeDisplayObj.SetActive(false);
            threeLayoutObj.SetActive(false);

            twoDisplayObj.SetActive(true);
        } 
        else
        {
            twoDisplayObj.SetActive(false);

            threeDisplayObj.SetActive(true);
            threeLayoutObj.SetActive(true);
        }

        //settting trial number
        trialInputField.SetTextWithoutNotify(newDataScript.numberOfTrials.ToString());
        dataObject.numberOfTrials = newDataScript.numberOfTrials;

        //setting target shape dropdown
        selectDropdownValue(targetNumDrop, newDataScript.targetNum.ToString());

        //setting target shape dropdown
        selectDropdownValue(twoDisplayDrop, newDataScript.twoDisplayNum.ToString());

        //setting target shape dropdown
        selectDropdownValue(threeDisplayDrop, newDataScript.threeDisplayNum.ToString());

        //setting optionDistro dropdown
        selectDropdownValue(optionDistroDrop, newDataScript.optionDistro.ToString());

        // setting colours/Materials
        if (newDataScript.selectedColours.Count < newDataScript.allColours.Count)
        {
            colourToggle.isOn = false;
            foreach (Toggle toggle in colourToggles)
            {
                toggle.gameObject.GetComponent<ColourToggle>().correctToggle(newDataScript.selectedColours);
            }
            togglePanel(colourToggle, "colour");
        }


        // setting shapes
        if (newDataScript.selectedTextures.Count < newDataScript.allTextures.Count)
        {
            shapeToggle.isOn = false;
            foreach (Toggle toggle in shapeToggles)
            {
                toggle.gameObject.GetComponent<ShapeToggle>().correctToggle(newDataScript.selectedTextures);
            }
            togglePanel(shapeToggle, "shape");
        }

        // set display time slider
        displayTimeSlider.value = newDataScript.shapeDisplayTime;

        // set display time slider
        displayDelaySlider.value = newDataScript.targetToDisplayDelay;

        //set confirm start toggle
        confirmStartToggle.isOn = newDataScript.confirmStart;

        //set tagetRand toggle
        targetRandToggle.isOn = newDataScript.targetRand;

        //set displayRand toggle
        displayRandToggle.isOn = newDataScript.displayRand;

        newDataScript.gameObject.GetComponent<PanelObject>().checkValidity("panelObjectScript");
    }

    public IEnumerator swapScripts(int dimension, int numberOfTrials, int targetNum, int twoDisplayNum, int threeDisplayNum, string optionDistro, List<(Color, string)> selectedColours, List<Texture> selectedTextures, float shapeDisplayTime, float targetToDisplayDelay, bool confirmStart, bool targetRand, bool displayRand)
    {
        // change all values in the created scipt to reflect the passed script
        // also select those values in the appropriate part of the panel

        //setting dimension
        setDimension(dimension);

        //ensure correct options are displayed suitable to the dimension
        if (dataObject.dimension == 2)
        {   
            threeDisplayObj.SetActive(false);
            threeLayoutObj.SetActive(false);

            twoDisplayObj.SetActive(true);
        } 
        else
        {
            twoDisplayObj.SetActive(false);

            threeDisplayObj.SetActive(true);
            threeLayoutObj.SetActive(true);
        }

        //settting trial number
        trialInputField.SetTextWithoutNotify(numberOfTrials.ToString());
        dataObject.numberOfTrials = numberOfTrials;

        //setting target shape dropdown
        selectDropdownValue(targetNumDrop, targetNum.ToString());

        //setting target shape dropdown
        selectDropdownValue(twoDisplayDrop, twoDisplayNum.ToString());

        //setting target shape dropdown
        selectDropdownValue(threeDisplayDrop, threeDisplayNum.ToString());

        //setting optionDistro dropdown
        selectDropdownValue(optionDistroDrop, optionDistro.ToString());

        yield return new WaitForSeconds(0.5f);

        // setting colours/Materials
        if (selectedColours.Count < dataObject.allColours.Count)
        {

            colourToggle.isOn = false;
            foreach (Toggle toggle in colourToggles)
            {
                toggle.gameObject.GetComponent<ColourToggle>().correctToggle(selectedColours);
            }
            togglePanel(colourToggle, "colour");
        }

        
        // setting shapes
        if (selectedTextures.Count < dataObject.allTextures.Count)
        {
            shapeToggle.isOn = false;
            foreach (Toggle toggle in shapeToggles)
            {
                toggle.gameObject.GetComponent<ShapeToggle>().correctToggle(selectedTextures);
            }
            togglePanel(shapeToggle, "shape");
        }

        // set display time slider
        displayTimeSlider.value = shapeDisplayTime;

        // set display time slider
        displayDelaySlider.value = targetToDisplayDelay;

        //set confirm start toggle
        confirmStartToggle.isOn = confirmStart;

        //set tagetRand toggle
        targetRandToggle.isOn = targetRand;

        //set displayRand toggle
        displayRandToggle.isOn = displayRand;

        this.checkValidity("panelObjectScript");
    }

    public void selectDropdownValue(Dropdown dropdownToChange, string valueToFind)
    {
        // find the option with the given value
        int index = 0;
        for(int x = 0; x < dropdownToChange.options.Count; x++)
        {
            if (dropdownToChange.options[x].text == valueToFind)
            {
                index = x;
            }
        }

        //now select the cell in that dropdown
        dropdownToChange.value = index;
    }

    public void simpleToggle(Toggle baseToggle, string modifier)
    {
        switch (modifier)
        {
            case "confirm":
                dataObject.confirmStart = baseToggle.isOn;
                break;

            case "targetRand":
                dataObject.targetRand = baseToggle.isOn;
                break;
            
            case "displayRand":
                dataObject.displayRand = baseToggle.isOn;
                break;
        }
    }


}

