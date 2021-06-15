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

        //Array of target objects
        private List<GameObject> targets;

        //Array of shape option objects
        private List<GameObject> shapeOptions;

        private void Start()
        {
            targets = GameObject.Find("Stand").GetChildren();
            shapeOptions = GameObject.Find("Board").GetChildren();
        }

        public void GenerateExperiment(Session session)
        {
            //Create a new block
            Block newBlock = session.CreateBlock(3);

            foreach (Trial trial in newBlock.trials)
            {
                //Randomising shape and colour option
                foreach (GameObject go in shapeOptions)
                {
                    go.GetComponent<MeshFilter>().mesh = possibleShapes[Random.Range(0, possibleShapes.Length - 1)];
                    go.GetComponent<Renderer>().material = possibleColours[Random.Range(0, possibleColours.Length - 1)];
                }
                //TODO Randomise which shapes and colours are visible
                //TODO Randomise their orientation?
                //TODO Randomise which of these are selected as the key shapes
            }
        }
    }
}
