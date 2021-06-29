using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UXF;

//Shape-Colour-Experiment
namespace SCE
{
    public class ShapeColourTask: MonoBehaviour
    {
        public void GenerateExperiment(Session session)
        {
            //Create a new block
            Block newBlock = session.CreateBlock(3);

            foreach (Trial trial in newBlock.trials)
            {
                
            }
        }

        public void CheckStatusOrStartNext(Trial trial)
        {
            // End if this is the last trial of the session
            bool endNow = trial == Session.instance.LastTrial;

            // if last trial in current block
            Block currentBlock = trial.block;
            if (trial == currentBlock.lastTrial && !endNow)
            {

            }
        }
    }
}
