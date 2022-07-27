using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UXF;
using UnityEngine.SceneManagement;

namespace WorkingMemory
{
    public class SessionManager : MonoBehaviour
    {

        // Start is called before the first frame update.
        void Awake()
        {
            //This means that the object will persist between scenes.
            DontDestroyOnLoad(gameObject);
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
                float delay_time = dataObj.delay_time;

                string option_distro;
                string scene_type;
                int option_num;
                string scene_name;

                bool confirm_start = dataObj.confirmStart;
                bool display_random;
                bool target_random;

                List<Color> selectedColours = dataObj.selectedColours;
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
                makeBlock(session, trial_num, scene_type, scene_name, option_num, target_num, option_distro, delay_time, selectedColours, selectedMaterials, selectedMeshes, selectedTextures, confirm_start, display_random, target_random);
            }
        }

        private void makeBlock(Session session, int trial_num, string scene_type, string scene_name, int option_num, int target_num, string option_distro, float delay_time, List<Color> selectedColours, List<Material> selectedMaterials, List<Mesh> selectedMeshes, List<Texture> selectedTextures, bool confirm_start, bool display_random, bool target_random)
        {
            Block block = session.CreateBlock(trial_num);

            block.settings.SetValue("scene_type", scene_type);
            block.settings.SetValue("scene_name", scene_name);

            block.settings.SetValue("option_num", option_num);
            block.settings.SetValue("target_num", target_num);
            block.settings.SetValue("delay_time", delay_time);

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

            string scene_type = trial.settings.GetString("scene_type");

            if (scene_type != "Two_Dimensional")
            {
                string option_string = trial.settings.GetString("option_distro").ToLower();
                
                if(option_string != "grid")
                {
                    GameObject wallObj = GameObject.FindGameObjectWithTag("wall");
                    wallObj.SetActive(false);
                }
            }
            
        }

        public void SceneSpecificClean(Trial trial)
        {

        }

        public void CleanUpTrial(Trial trial)
        {



            //Record results


            /*
            // ~ example show questions and get responses via the UI ~
            string exampleQuestion1 = "How difficult was the task";
            string exampleResponse1 = "Easy!";

            string exampleQuestion2 = "How are you feeling today on a scale of 1-7";
            int exampleResponse2 = 6;

            // ~ example show questions and get responses via the UI ~
            

            // questions are headers
            var headers = new string[]{ exampleQuestion1,  exampleQuestion2 };
            var surveyData = new UXF.UXFDataTable(headers); 

            // one row for the response (only 1 participant here!)
            var surveyResponse = new UXF.UXFDataRow();
            surveyResponse.Add((exampleQuestion1, exampleResponse1));
            surveyResponse.Add((exampleQuestion2, exampleResponse2));

            surveyData.AddCompleteRow(surveyResponse);

            // save output
            UXF.Session.instance.SaveDataTable(surveyData, "survey");
        }
            */
        }

        [SerializeField]
        private void moveToGUI()
        {
            SceneManager.LoadScene("EntryScene");
        }

    }
}
