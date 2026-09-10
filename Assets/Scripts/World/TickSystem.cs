using System;
using UnityEngine;

namespace World
{
    public class TickSystem
    {
        public event Action OnTick;
        
        private float timeInterval;
        private DateTime lastTickTime;

        public void Initialize(float timeInterval)
        {
            this.timeInterval = timeInterval;
        }
        
        public void Start()
        {
            lastTickTime = DateTime.Now.AddSeconds(timeInterval);
        }

        public void OnUpdate()
        {
            if(lastTickTime <= DateTime.Now)
            {
                Tick();
                lastTickTime = DateTime.Now.AddSeconds(timeInterval);
            }
        }

        private void Tick()
        {
            OnTick?.Invoke();
        }
    }
}