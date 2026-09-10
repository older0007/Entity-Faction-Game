using System.Collections.Generic;
using GamePlay.Units;

namespace GamePlay.Actions
{
    public readonly struct Candidate
    {
        public readonly ActionConfig Action;
        public readonly Unit Target;
        public Candidate(ActionConfig action, Unit target)
        {
            Action = action;
            Target = target;
        }
    }
    public static class Selection
    {
        public static bool IsAvailable(ActionConfig action, Unit from, Unit to)
        {
            if (!action || !from || !to) return false;
            if (action.IsSelfCast)
            {
                if (from != to) return false;
            }
            else if (from == to) return false;
            
            if (from.Status.CurrentState != State.Idle) return false;
            
            var targetDead = to.Status.CurrentState == State.Dead;
            
            if (action.CanCastOnDead != targetDead) return false;
            if (!Offers(from, action)) return false;
            if (action.IsSelfCast) return true;
            
            return action.Allows(action, from.Config.FractionConfig, to.Config.FractionConfig);
        }
        
        public static bool TryPickBest(Unit from, IReadOnlyList<Unit> all, out Candidate best)
        {
            best = default;
            var found = false;
            var actions = from.Config.Actions;
            if (actions == null) return false;
            for (var a = 0; a < actions.Length; a++)
            {
                var action = actions[a];
                if (!action) continue;
                for (var i = 0; i < all.Count; i++)
                {
                    var to = all[i];
                    if (!IsAvailable(action, from, to)) continue;
                    var candidate = new Candidate(action, to);
                    if (!found || Compare(candidate, best) < 0)
                    {
                        best = candidate;
                        found = true;
                    }
                }
            }
            return found;
        }

        private static int Compare(Candidate a, Candidate b)
        {
            var c = b.Action.Priority.CompareTo(a.Action.Priority);
            if (c != 0) return c;
            return string.CompareOrdinal(a.Action.Id, b.Action.Id);
        }

        private static bool Offers(Unit unit, ActionConfig action)
        {
            var offers = unit.Config.Actions;
            if (offers == null) return false;
            for (var i = 0; i < offers.Length; i++)
            {
                if (offers[i] == action)
                {
                    return true;
                }
            }
            return false;
        }
    }
}