using System;
using System.Collections.Generic;
using Reflex.Attributes;
using Reflex.Extensions;
using UnityEngine;

namespace Downey.Reflex
{
    public class Greeter : MonoBehaviour
    {
        [Inject]private readonly IEnumerable<string> strings;
        //[Inject] ILogger logger;
        [Inject] IEnumerable<ILogger> loggerSet;

        [Inject]
        void Inject(IEnumerable<ILogger> loggers)
        {
            int i = 1;
            foreach (var logger in loggerSet)
            {
                logger.Log($"[Method-injected] Logger #{i++}");
            }
        }
        private void Start()
        {
            //Debug.Log(string.Join(" ", strings));
            // logger.Log("From ILogger");
            // int i = 1;
            // foreach (var log in loggerSet)
            // {
            //     logger.Log($"From IEnumerable<ILogger> binding #{i++}");
            // }
            var logger = gameObject.scene.GetSceneContainer().Resolve<ILogger>();
            logger.Log("Resolved aat runtime via scene");
        }
    }
}