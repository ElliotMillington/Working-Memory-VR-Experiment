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
            //Number of trials per block
            int trial_num = 3;

            //Please refer to the reference files for block contents.
            Block block_sc1 = session.CreateBlock(trial_num);
            Block block_sc2 = session.CreateBlock(trial_num);
            Block block_vs1 = session.CreateBlock(trial_num);
            Block block_vs2 = session.CreateBlock(trial_num);

            //Setting block types
            block_sc1.settings.SetValue("scene_type", "Shapes_Colours");
            block_sc2.settings.SetValue("scene_type", "Shapes_Colours");
            block_vs1.settings.SetValue("scene_type", "Visual_Search");
            block_vs2.settings.SetValue("scene_type", "Visual_Search");

            //Setting block scene names
            block_sc1.settings.SetValue("scene_name", "Shapes_Colours_3d");
            block_sc2.settings.SetValue("scene_name", "Shapes_Colours_2d");
            block_vs1.settings.SetValue("scene_name", "Visual_Search");
            block_vs2.settings.SetValue("scene_name", "Visual_Search");
        }

        public void SetUpTrial(Trial trial)
        {
            //If first trial in the block, load a new scene.
            if (trial.numberInBlock == 1)
            {
                string scenePath = trial.settings.GetString("scene_name");

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
        }

        public void CleanUpTrial(Trial trial)
        {
            FindObjectOfType<TrialManager>().TrialCleanUp(trial);
        }
    }
}