using GamePlay.Actions;
using GamePlay.Fractions;
using UnityEngine;

namespace GamePlay.Units
{
    [CreateAssetMenu(fileName = "NewUnit", menuName = "GameTool/Create/UnitConfig")]
    public class UnitConfig : ScriptableObject
    {
        [SerializeField] private FractionConfig fractionConfig;
        [SerializeField] private Color32 baseColor;
        [SerializeField] private string unitName;
        [SerializeField] private ActionConfig[] actions;

        public FractionConfig FractionConfig => fractionConfig;
        public Color32 BaseColor => baseColor;
        public string UnitName => unitName;
        public ActionConfig[] Actions => actions;
        
        public override string ToString() => $"{unitName} ({fractionConfig.FractionName})";
    }
}