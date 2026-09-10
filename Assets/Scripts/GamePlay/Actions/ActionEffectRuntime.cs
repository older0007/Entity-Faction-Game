using System;
using GamePlay.Units;

namespace GamePlay.Actions
{
    public class ActionEffectRuntime
    {
        public static readonly ActionEffectRuntime None = new ActionEffectRuntime();
        public virtual void OnStart(Interaction interaction) { }
        public virtual void OnUpdate(Interaction interaction) { }
        public virtual void OnCancel(Interaction interaction) { }
        public virtual void OnEnd(Interaction interaction) { }
        public virtual bool ShouldInterrupt(Interaction interaction) => false;
        private sealed class NoEffect : ActionEffectRuntime { }
    }
}