using System;
using UnityEngine;

namespace GamePlay.Actions.Effects
{
    [Serializable]
    public abstract class BaseEffect : ScriptableObject
    {
        public abstract ActionEffectRuntime EffectRuntime { get; }
    }
}