using System;
using System.Collections.Generic;
using GamePlay.Actions;
using GamePlay.Fractions;
using GamePlay.Units;
using UnityEngine;

namespace World
{
    public class FractionController : MonoBehaviour
    {
        [SerializeField] private Fraction[] fractions;
        [SerializeField] private InteractionController interactionController;

        public Fraction[] Fractions => fractions;

        public IReadOnlyList<Unit> Units => allUnits.AsReadOnly();
        
        private List<Unit> allUnits = new();
        
        private void OnEnable()
        {
            foreach (var fraction in fractions)
            {
                fraction.OnUnitCreated += OnUnitCreated;
            }
        }

        private void OnDisable()
        {
            foreach (var fraction in fractions)
            {
                fraction.OnUnitCreated -= OnUnitCreated;
            }
        }

        private void OnUnitCreated(Unit unit)
        {
            allUnits.Add(unit);
        }

        public void OnTick()
        {
            for (var index = 0; index < fractions.Length; index++)
            {
                var fraction = fractions[index];
                fraction.OnUpdate();
                
                for (var i = 0; i < fraction.Units.Count; i++)
                {
                    var from = fraction.Units[i];
                    if (!from) continue;
                    if (Selection.TryPickBest(from, Units, out var best))
                    {
                        interactionController.TryStart(best.Action, from, best.Target);
                    }
                }
            }
        }
    }
}