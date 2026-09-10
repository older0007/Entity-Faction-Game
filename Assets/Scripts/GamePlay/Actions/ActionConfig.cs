using System;
using GamePlay.Actions.Effects;
using GamePlay.Fractions;
using UnityEngine;

namespace GamePlay.Actions
{
    [CreateAssetMenu(fileName = "NewAction", menuName = "GameTool/Create/ActionConfig")]
    public class ActionConfig : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string actionName;
        [SerializeField, TextArea] private string description;
        [SerializeField] private ActionType actionType;
        [SerializeField] private bool canCastOnDead;
        [SerializeField] private float actionDuration;
        [SerializeField] private int priority;
        [SerializeField] private bool isSelfCast;
        [SerializeField] private FractionPair[] fractionPairs;
        [SerializeField] private BaseEffect effect;
        
        public string Id => id;
        public string ActionName => actionName;
        public string Description => description;
        public ActionType Type => actionType;
        public float Duration => actionDuration;
        public int Priority => priority;
        public FractionPair[] FractionPairs => fractionPairs;
        public BaseEffect Effect => effect;
        public bool CanCastOnDead => canCastOnDead;
        public bool IsSelfCast => isSelfCast;
        
        public bool Allows(ActionConfig action, FractionConfig from, FractionConfig to)
        {
            if (!from || !to || fractionPairs == null) return false;
            for (var i = 0; i < fractionPairs.Length; i++)
            {
                if (fractionPairs[i] != null && fractionPairs[i].Allows(from, to))
                {
                    return true;
                }
            }
            
            Debug.Log($"Rejected {action.ActionName}: {from.name} -> {to.name}");
            return false;
        }
        
        private void OnValidate()
        {
            id = GetEntityId().ToString();
        }

        public enum ActionType
        {
            None,
            Immediate,
            Delayed,
            //AutoComplete //for actions that should complete by any condition, like a buff or debuff that has a duration and then ends automatically
        }
        
        [Serializable]
        public class FractionPair
        {
            [SerializeField] private FractionConfig initiator;
            [SerializeField] private FractionConfig target;
            
            public bool Allows(FractionConfig from, FractionConfig to) =>
                initiator == from && target == to;
        }
    }
}