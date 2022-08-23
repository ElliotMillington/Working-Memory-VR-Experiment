using System.Collections.Generic;
using UnityEngine;
using UXF;
using UnityEngine.SceneManagement;

/*

    This script is assigned to the Session Manager which is a obejct which is always present in any given scene.

    Manages escaping scenes, creating blocks for a session, set up and clean up of trials, saving trial information, and scene navigation

*/


public class SessionManager : MonoBehaviour
{

    public GameObject exitPanel;

    // Start is called before the first frame update.
    void Awake()
    {
        //This means that the object will persist between scenes.
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        // do nothing if escape key not pressed
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        //check which scenes we are in

        // if current scene is 2d or 3d then end UXF session and return to GUI scene
        if (SceneManager.GetActiveScene().name == "Shapes_Colours_2d" || SceneManager.GetActiveScene().name == "Shapes_Colours_3d")
        {
            // end seesion if exists
            if (Session.instance != null)
            {
                Session.instance.settings.SetValue("Escape", "esc");
                Session.instance.End();
            }

            // move to GUI Scene
            moveToGUI(GameObject.Find("TrialManager"), GameObject.Find("[UXF_Rig]"));
            return;
        }

        if (SceneManager.GetActiveScene().name == "EntryScene")
        {
            // in entry scene

            //find all panels in the scene


            //load and save panel
            GameObject save_panel = GameObject.FindGameObjectWithTag("Save_Panel");

            //UXF panel
            GameObject uxf_panel = GameObject.FindGameObjectWithTag("UXF_Panel");

            //exit panel is passed in editor

            // if any of the panels are open i.e not null then close all
            if (save_panel != null || uxf_panel != null || exitPanel.active)
            {
                if (save_panel !=null) save_panel.SetActive(false);
                if (uxf_panel != null) uxf_panel.SetActive(false);
                exitPanel.SetActive(false);
                return;
            }
            else
            {
                // else open the exit panel
                exitPanel.SetActive(true);
            }

        }

        
        

        //scene not found
        return;
    }

    // Called for when session starts.
    public void Generate(Session session)
    {
        // get PanelGroup script
        PanelGroup groupScript = GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>();
        
        

        foreach (PanelObject obj in groupScript.panelGroup)
        {
            PanelData dataObj = obj.gameObject.GetComponent<PanelData>();

            int trial_num = dataObj.numberOfTrials;
            int target_num = dataObj.targetNum;
            float shape_display_time = dataObj.shapeDisplayTime;
            float delay_time = dataObj.targetToDisplayDelay;

            string option_distro;
            string scene_type;
            int option_num;
            string scene_name;

            bool confirm_start = dataObj.confirmStart;
            bool display_random;
            bool target_random;

            List<(Color, string)> selectedColours = dataObj.selectedColours;
            List<Material> selectedMaterials = dataObj.selectedMaterials;

            List<Mesh> selectedMeshes = dataObj.selectedMeshes;
            List<Texture> selectedTextures = dataObj.selectedTextures;


            if (dataObj.dimension == 2)
            {
                option_num = dataObj.twoDisplayNum;
                scene_type = "Two_Dimensional";
                scene_name = "Shapes_Colours_2d";

                //not relevant for 2D
                option_distro = null; 
                display_random = false; //false but not user for 2d
                target_random = false;
            }else{
                option_num = dataObj.threeDisplayNum;
                option_distro = dataObj.optionDistro;
                scene_type = "Three_Dimensional";
                scene_name = "Shapes_Colours_3d";
                display_random = dataObj.displayRand; 
                target_random = dataObj.targetRand;
            }
            makeBlock(session, trial_num, scene_type, scene_name, option_num, target_num, option_distro, shape_display_time, delay_time, selectedColours, selectedMaterials, selectedMeshes, selectedTextures, confirm_start, display_random, target_random);
        }
    }

    private void makeBlock(Session session, int trial_num, string scene_type, string scene_name, int option_num, int target_num, string option_distro, float shape_display_time, float delay_time, List<(Color,string)> selectedColours, List<Material> selectedMaterials, List<Mesh> selectedMeshes, List<Texture> selectedTextures, bool confirm_start, bool display_random, bool target_random)
    {
        Block block = session.CreateBlock(trial_num);

        block.settings.SetValue("scene_type", scene_type);
        block.settings.SetValue("scene_name", scene_name);

        block.settings.SetValue("option_num", option_num);
        block.settings.SetValue("target_num", target_num);
        block.settings.SetValue("shape_display_time", shape_display_time);
        block.settings.SetValue("delay_time", delay_time);

        //delay_time;

        block.settings.SetValue("confirm_start", confirm_start);
        
        if (scene_type == "Three_Dimensional")
        {
            block.settings.SetValue("option_distro", option_distro);
            block.settings.SetValue("display_random", display_random);
            block.settings.SetValue("target_random", target_random);

            //pass in approriate meshes/textures or colour/material depening on scene
            //Mesh and Materical
            block.settings.SetValue("selected_meshes", selectedMeshes);
            block.settings.SetValue("selected_materials", selectedMaterials);

        }else{
            //Texture and Color
            block.settings.SetValue("selected_colours", selectedColours);
            block.settings.SetValue("selected_textures", selectedTextures);
        }
    }

    public void SetUpTrial(Trial trial)
    {
        string scenePath = trial.settings.GetString("scene_name");
        //If first trial in the block, load a new scene.
        if (trial.numberInBlock == 1)
        {
            AsyncOperation loadScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scenePath);
            loadScene.completed += (op) => { SceneSpecificSetup(trial); };
        } else
        {
            SceneSpecificSetup(trial);
        }
    }

    void SceneSpecificSetup(Trial trial)
    {
        TrialManager manager = FindObjectOfType<TrialManager>();
        if (trial.numberInBlock == 1)
        {
            manager.BlockSetUp(trial.block);
        }
        manager.TrialSetUp(trial);
        if(trial.numberInBlock > 1)
        {
            GameObject startButton = GameObject.FindGameObjectWithTag("startButton");
            GameObject startText = GameObject.FindGameObjectWithTag("textHolder");
            if (startButton != null) startButton.SetActive(false);
            if (startText != null) startText.SetActive(false);
        }

    }

    public void SceneSpecificClean(Trial trial)
    {

    }

    public void CleanUpTrial(Trial trial)
    {
        List<UXFDataRow> responses;
        // trial was escaped and ended
        if (Session.instance.settings.ContainsKey("Escape"))
        {
            // save whateever data was collected in that time
            if (trial.block.settings.ContainsKey("response_list"))
            {
                responses = (List<UXFDataRow>)trial.block.settings.GetObject("response_list");
                var headers = new string[] { "Block_Number", "Trial_Number", "Dimension", "Layout", "Total_User_Time_Milliseconds", "Target_Shapes", "Participant_Selected_Shapes", "Correctly_Selected_Shapes", "Incorrectly_Selected_Shapes", "All_Correct" };
                var surveyData = new UXF.UXFDataTable(headers);
                foreach (UXFDataRow row in responses)
                {
                    surveyData.AddCompleteRow(row);
                }
                UXF.Session.instance.SaveDataTable(surveyData, "block_results" + trial.block.number);
            }
                
         
            return;
        }

        print("End of trial");

        if (trial.numberInBlock == 1)
        {
            responses = new List<UXFDataRow>();
        }else
        {
            responses = (List<UXFDataRow>)trial.block.settings.GetObject("response_list");
        }

        
        UXFDataRow surveyResponse = new UXF.UXFDataRow();
        surveyResponse.Add(("Total_User_Time_Milliseconds", trial.settings.GetDouble("total_time")));
        surveyResponse.Add(("Target_Shapes", trial.settings.GetString("target_shapes")));
        surveyResponse.Add(("Participant_Selected_Shapes", trial.settings.GetString("selected_shapes")));
        surveyResponse.Add(("Correctly_Selected_Shapes", trial.settings.GetString("correct_shapes")));
        surveyResponse.Add(("Incorrectly_Selected_Shapes", trial.settings.GetString("incorrect_shapes")));  
        surveyResponse.Add(("All_Correct", trial.settings.GetBool("all_correct")));   

        surveyResponse.Add(("Block_Number", trial.block.number));
        surveyResponse.Add(("Trial_Number", trial.numberInBlock));
        surveyResponse.Add(("Dimension", trial.settings.GetInt("dimension")));
        surveyResponse.Add(("Layout", trial.settings.GetString("layout")));
    

        responses.Add(surveyResponse);
        trial.block.settings.SetValue("response_list", responses);


        // save output
        if (trial.block.lastTrial == trial)
        {
            var headers = new string[]{ "Block_Number", "Trial_Number", "Dimension", "Layout", "Total_User_Time_Milliseconds", "Target_Shapes","Participant_Selected_Shapes", "Correctly_Selected_Shapes", "Incorrectly_Selected_Shapes", "All_Correct"};
            var surveyData = new UXF.UXFDataTable(headers); 
            foreach(UXFDataRow row in responses)
            {
                surveyData.AddCompleteRow(row);
            }
            UXF.Session.instance.SaveDataTable(surveyData, "block_results" + trial.block.number);  
        } 
    }

    [SerializeField]
    public void moveToGUI(GameObject trialManager, GameObject rig)
    {
        AsyncOperation loadScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("EntryScene");
            loadScene.completed += (op) => 
            { 
                Destroy(trialManager);
                Destroy(rig);
                Destroy(this.gameObject);
            };
    }

}

