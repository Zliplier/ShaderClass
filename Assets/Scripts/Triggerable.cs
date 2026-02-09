using System;
using UnityEngine;
using UnityEngine.Events;

namespace Samarnggoon.GameDev4.EventDrivenArch
{
    public class Triggerable : MonoBehaviour
    {
        public UnityEvent onTriggerEnter;
        
        public UnityEvent onTriggerExit;
        
        private void OnTriggerEnter(Collider other)
        {
            if(onTriggerEnter != null)
                onTriggerEnter.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if(onTriggerExit != null)
                onTriggerExit.Invoke();
        }
    }
}