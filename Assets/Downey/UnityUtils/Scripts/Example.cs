using System;
using UnityEngine;

namespace UnityUtils
{
    public class Example : MonoBehaviour
    {
        public GameObject objectExample;
        public string stringExample;
        private void Awake()
        {
            //DateExample();
            //StringExample();
            //objectExample.HideInHierarchy();
            Debug.Log(objectExample.Path());
        }

        public void DateExample()
        {
            DateTime now = DateTime.Now;
            DateTime temp = now.WithDate();
            Debug.Log(now);
            Debug.Log(temp);
        }

        public void StringExample()
        {
            string temp = stringExample.ConvertToAlphanumeric();
            Debug.Log(stringExample);
            Debug.Log(temp);
        }
    }
}
