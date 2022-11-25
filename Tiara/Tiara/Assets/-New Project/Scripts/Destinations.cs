using System.Collections.Generic;
using UnityEngine;

namespace AV{
    public class Destinations : MonoBehaviour
    {
        public static List<Transform> destinations = new List<Transform>();
        public static Transform[] groups;

        private void Awake()
        {
            groups = new Transform[transform.childCount];
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i] = transform.GetChild(i);
            }
        }
    }
}
