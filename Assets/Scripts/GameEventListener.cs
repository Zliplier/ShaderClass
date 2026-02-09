using System;
using UnityEngine;
using UnityEngine.Events;

namespace Samarnggoon.GameDev4.EventDrivenArch
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent gameEvent;
        public UnityEvent response;

        public void OnEventRaised() => response?.Invoke();

        private void OnEnable()
        {
            if (gameEvent == null)
                return;
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (gameEvent == null)
                return;
            gameEvent.UnregisterListener(this);
        }

    }
}