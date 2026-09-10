using System.Collections.Generic;
using GamePlay.Units;
using UnityEngine;

namespace GamePlay.Actions
{
    public class InteractionController : MonoBehaviour
    {
        private static InteractionController Instance { get; set; }
        private readonly List<Interaction> active = new List<Interaction>();
        
        private void Awake() => Instance = this;
        
        public void OnUpdate()
        {
            for (var i = active.Count - 1; i >= 0; i--)
            {
                var interaction = active[i];
                interaction.Effect.OnUpdate(interaction);
                
                if(interaction.Effect.ShouldInterrupt(interaction))
                {
                    Cancel(interaction, CancelReason.InterruptedByEffect);
                    continue;
                }
                
                if (interaction.IsCompleted)
                {
                    Complete(interaction);
                }
            }
        }

        /*private void OnDrawGizmos()
        {
            if (Instance == null) return;
            for (var index = 0; index < Instance.active.Count; index++)
            {
                var interaction = Instance.active[index];
                Gizmos.color = Color.rebeccaPurple;
                Gizmos.DrawLine(interaction.Initiator.transform.position, interaction.Target.transform.position);
            }
        }*/

        public bool TryStart(ActionConfig action, Unit from, Unit to)
        {
            if (!Selection.IsAvailable(action, from, to))
            {
                return false;
            }
            var running = from.Current;
            if (running != null)
            {
                if (action.Priority <= running.ActionConfig.Priority)
                {
                    return false;
                }
                
                Cancel(running, CancelReason.NewPriorityAction);
            }
            
            var interaction = new Interaction(action, from, to);
            StartInteraction(interaction);
            
            if (!interaction.IsTimed)
            {
                EndInteraction(interaction);
                return true;
            }

            from.Current = interaction;
            Instance.active.Add(interaction);
            return true;
        }

        private void StartInteraction(Interaction interaction)
        {
            interaction.Effect.OnStart(interaction);
            OnStart(interaction);
            
            interaction.Initiator.Status.AddInActionState();
        }

        private void EndInteraction(Interaction interaction)
        {
            interaction.Effect.OnEnd(interaction);
            OnEnd(interaction);
            
            interaction.Initiator.Status.RemoveInActionState();
        }

        public void Cancel(Interaction interaction, CancelReason reason = CancelReason.None)
        {
            interaction.Effect.OnCancel(interaction);
            Instance.Detach(interaction);
            OnCancel(interaction, reason);
            
            interaction.Initiator.Status.RemoveInActionState();
        }
        
        private void Complete(Interaction interaction)
        {
            interaction.Effect.OnEnd(interaction);
            Detach(interaction);
            OnComplete(interaction);
            
            interaction.Initiator.Status.RemoveInActionState();
        }
        
        private void Detach(Interaction interaction)
        {
            if (interaction.Initiator.Current == interaction)
            {
                interaction.Initiator.Current = null;
                if (interaction.Initiator.Status.CurrentState != State.Dead)
                {
                    interaction.Initiator.Status.ChangeState(State.Idle);
                }
            }
            active.Remove(interaction);
        }

        private void OnStart(Interaction interaction)
        {
            if (interaction.ActionConfig.Type == ActionConfig.ActionType.Delayed)
            {
                DrawDebugLine(interaction, Color.cyan, interaction.ActionConfig.Duration);
            }
            else
            {
                DrawDebugLine(interaction, Color.green, 1f);
            }
         
            Debug.Log($"Interaction started: {interaction.ActionConfig.ActionName} from {interaction.Initiator.name} to {interaction.Target.name}");
        }

        private void OnEnd(Interaction interaction)
        {
            DrawDebugLine(interaction, Color.red, 1f);
            
            Debug.Log($"Interaction ended: {interaction.ActionConfig.ActionName} from {interaction.Initiator.name} to {interaction.Target.name}");
        }

        private void OnCancel(Interaction interaction, CancelReason reason)
        {
            DrawDebugLine(interaction, Color.yellow, 1f);
            
            Debug.Log($"Interaction canceled: {interaction.ActionConfig.ActionName} from {interaction.Initiator.name} to {interaction.Target.name} Reason: {reason}");
        }
        
        private void OnComplete(Interaction interaction)
        {
            DrawDebugLine(interaction, Color.blue, 1f);
            
            Debug.Log($"Interaction completed: {interaction.ActionConfig.ActionName} from {interaction.Initiator.name} to {interaction.Target.name}");
        }
        
        private void DrawDebugLine(Interaction interaction, Color color, float duration)
        {
#if UNITY_EDITOR
            if(interaction.Initiator == null || interaction.Target == null) return;

            if (interaction.ActionConfig.IsSelfCast)
            {
                return;
            }
            
            Debug.DrawLine(interaction.Initiator.transform.position, interaction.Target.transform.position, color, duration);
#endif
        }
        
        public enum CancelReason
        {
            None,
            NewPriorityAction,
            InterruptedByEffect,
        }
    }
}