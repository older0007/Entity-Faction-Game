using System;
using GamePlay.Units;
using UnityEngine;

namespace GamePlay.Fractions
{
    [CreateAssetMenu(fileName = "NewFraction", menuName = "GameTool/Create/FractionConfig")]
    public class FractionConfig : ScriptableObject
    {
        [SerializeField] private string fractionName;
        [SerializeField] private string description;
        [SerializeField] private Color fractionColor;
        [SerializeField] private UnitData[] units;
        [SerializeField] private Unit basePrefab;
        
        public string FractionName => fractionName;
        public string Description => description;
        public Color FractionColor => fractionColor;
        public UnitData[] Units => units;
        public Unit BasePrefab => basePrefab;
        
        [Serializable]
        public class UnitData
        {
            [SerializeField] private UnitConfig unitConfig;
            [SerializeField] private Unit overriddenPrefab;
            
            public UnitConfig UnitConfig => unitConfig;
            public Unit OverriddenPrefab => overriddenPrefab;
        }
    }
}