using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UXF;

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
            
            makeBlock(session, 3, "Three_Dimensional", "Shapes_Colours_3d", 9, 3, "grid", 2.0f);
            //makeBlock(session, 3, "Two_Dimensional", "Shapes_Colours_2d", 9, 3, null, 2.0f);
            

        }

        private void makeBlock(Session session, int trial_num, string scene_type, string scene_name, int option_num, int target_num, string option_distro, float delay_time)
        {
            Block block = session.CreateBlock(trial_num);
            block.settings.SetValue("scene_type", scene_type);
            block.settings.SetValue("scene_name", scene_name);

            block.settings.SetValue("option_num", option_num);
            block.settings.SetValue("target_num", target_num);
            block.settings.SetValue("delay_time", delay_time);

            if (scene_type == "Three_Dimensional")
            {
                block.settings.SetValue("option_distro", option_distro);
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

            string option_string = trial.settings.GetObject("option_distro").ToString();
            GameObject wallObj = GameObject.Find("Wall");
            if(option_string != "grid")
            {
                wallObj.SetActive(false);
            }
        }

        public void CleanUpTrial(Trial trial)
        {
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
    }
}
