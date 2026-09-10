using System;
using GamePlay.Units;

namespace GamePlay.Actions
{
    public class Interaction
    {
        public ActionConfig ActionConfig { get; private set; }
        public Unit Initiator { get; private set; }
        public Unit Target { get; private set; }
        public ActionEffectRuntime Effect { get; private set; }
        public ActionConfig.ActionType Type => ActionConfig.Type;
        public bool IsTimed => ActionConfig.Type is ActionConfig.ActionType.Delayed;
        public DateTime startTime;

        public bool IsCompleted
        {
            get
            {
                if (!IsTimed) return true;
                var elapsedTime = (DateTime.Now - startTime).TotalSeconds;
                return elapsedTime >= ActionConfig.Duration;
            }
        }

        public Interaction(ActionConfig actionConfig, Unit initiator, Unit target)
        {
            ActionConfig = actionConfig;
            Initiator = initiator;
            Target = target;
            startTime = DateTime.Now;
            Effect = actionConfig.Effect?.EffectRuntime ?? ActionEffectRuntime.None;
        }

        public override string ToString() => $"[Interaction] {ActionConfig} from {Initiator} to {Target}";
    }
}