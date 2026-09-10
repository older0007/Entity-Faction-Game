using UnityEngine;

namespace World
{
    public class TickConfig : ScriptableObject
    {
        [SerializeField] private float tickInterval = 1f;
        
        public float TickInterval => tickInterval;
    }
}