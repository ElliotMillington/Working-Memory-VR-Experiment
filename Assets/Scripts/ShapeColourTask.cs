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
        public List<GameObject> targets;

        //Array of shape option objects
        public List<GameObject> shapeOptions;

        private void Start()
        {
            targets = GameObject.Find("Stand").GetChildren();
            shapeOptions = GameObject.Find("Board").GetChildren();

            foreach (GameObject shape in targets)
            {
                shape.GetComponent<Shape>().clickable = false;
            }
        }

        /// <param name="session"></param>
        public void GenerateExperiment(Session session)
        {
            //Create a new block
            Block newBlock = session.CreateBlock(3);

            foreach (Trial trial in newBlock.trials)
            {
                //Randomising shape and colour option
                foreach (GameObject go in shapeOptions)
                {
                    go.GetComponent<MeshFilter>().mesh = possibleShapes[Random.Range(0, possibleShapes.Length)];
                    go.GetComponent<Renderer>().material = possibleColours[Random.Range(0, possibleColours.Length)];
                    go.transform.Rotate(new Vector3(Random.Range(-180, 180), 0, 0));
                    print(go.name);
                }

                //Selecting and modifying target shapes
                List<int> keyShapesIndices = HelperMethods.GenRandomInts(0, shapeOptions.Count - 1, targets.Count);
                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].GetComponent<MeshFilter>().mesh = shapeOptions[keyShapesIndices[i]].GetComponent<MeshFilter>().mesh;
                    targets[i].GetComponent<Renderer>().material = shapeOptions[keyShapesIndices[i]].GetComponent<Renderer>().material;
                }
            }
        }
    }
}
