using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorkingMemory
{
    public static class HelperMethods
    {
        public static List<GameObject> GetChildren(this GameObject go)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform tran in go.transform)
            {
                children.Add(tran.gameObject);
            }
            return children;
        }

        public static List<int> GenRandomInts(int min, int max, int num)
        {
            List<int> oldList = Enumerable.Range(0, num).ToList();

            // this is the list we're going to move items to
            List<int> newList = new List<int>();

            // make sure tmpList isn't already empty
            while (newList.Count < num)
            {
                int index = Random.Range(0, oldList.Count);
                newList.Add(oldList[index]);
                oldList.RemoveAt(index);
            }

            return newList;
        }

        public static float[] Seq(int len, float start, float end)
        {
            float by = (end - start) / len;
            float[] seq = new float[len];

            seq[0] = start;
            for (int i = 1; i < len; i++)
            {
                seq[i] = seq[i - 1] + by;
            }
            return seq;
        }
    }
}
