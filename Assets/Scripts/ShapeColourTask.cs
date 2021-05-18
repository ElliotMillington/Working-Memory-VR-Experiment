using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UXF;

//Shape-Colour-Experiment
namespace SCE
{
    public class ShapeColourTask: MonoBehaviour
    {
        //Array of potential shape meshes
        public Mesh[] possibleShapes;

        //Array of potential colours
        public Material[] possibleColours;

        public void GenerateExperiment(Session session)
        {
            //Create a new block
            Block newBlock = session.CreateBlock(3);

            foreach (Trial trial in newBlock.trials)
            {
                //TODO Randomise which shapes and colours are visible
                //TODO Randomise their orientation?
                //TODO Randomise which of these are selected as the key shapes
                possibleShapes[0] = null;
                possibleColours[0] = null;
            }
        }
    }
}
