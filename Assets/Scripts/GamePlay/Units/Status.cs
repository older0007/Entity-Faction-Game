using System;

namespace GamePlay.Units
{
    [Serializable] //for debugging purposes, to see the state in the inspector
    public class Status
    {
        public State CurrentState { get; private set; } = State.None;
        public StatusEffect CurrentStatusEffect { get; private set; } = StatusEffect.None;

        public event Action<State> OnStateChange
        {
            add => onStateChange += value;
            remove => onStateChange -= value;
        }

        private Action<State> onStateChange;
        
        public void ChangeState(State newState, bool force = false)
        {
            if(!force && CurrentState == State.Dead) return; // soft lock, once dead, cannot change state
            
            if (CurrentState == newState) return;
            CurrentState = newState;
            onStateChange?.Invoke(newState);
        }
        
        private void AddStatus(StatusEffect status)
        {
            if (CurrentStatusEffect.HasFlag(status)) return;
            CurrentStatusEffect |= status;
        }
        
        private void RemoveStatus(StatusEffect status)
        {
            if (!CurrentStatusEffect.HasFlag(status)) return;
            CurrentStatusEffect &= ~status;
            onStateChange?.Invoke(CurrentState);
        }
        
        public bool HasStatus(StatusEffect status)
        {
            return CurrentStatusEffect.HasFlag(status);
        }
        
        public void AddInteractionState()
        {
            AddStatus(StatusEffect.Interaction);
        }
        
        public void AddShieldState()
        {
            AddStatus(StatusEffect.Shielded);
        }
        
        public void AddInActionState()
        {
            AddStatus(StatusEffect.InAction);
        }
        
        public void AddRevivedState()
        {
            AddStatus(StatusEffect.Revived);

            ChangeState(State.Idle, true); // force change to idle state when revived
            CurrentStatusEffect = StatusEffect.None;
        }
        
        public void RemoveRevivedState()
        {
            RemoveStatus(StatusEffect.Revived);
        }
        
        public void RemoveInActionState()
        {
            RemoveStatus(StatusEffect.InAction);
        }
        
        public void RemoveShieldState()
        {
            RemoveStatus(StatusEffect.Shielded);    
        }
        
        public bool HasInteractionState()
        {
            return HasStatus(StatusEffect.Interaction);
        }
        
        public bool HasShieldState()
        {
            return HasStatus(StatusEffect.Shielded);
        }
        
        public void RemoveInteractionState()
        {
            RemoveStatus(StatusEffect.Interaction);
        }
    }

    [Serializable]
    public enum State
    {
        None = 0,
        Idle = 1,
        Dead = 2
    }
    
    [Flags]
    [Serializable]
    public enum StatusEffect
    {
        None = 0,
        Shielded = 1 << 0,
        Interaction = 1 << 1,
        Revived = 1 << 2,
        InAction = 1 << 3,
    }
}