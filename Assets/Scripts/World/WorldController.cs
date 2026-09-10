using System;
using System.Collections.Generic;
using GamePlay.Actions;
using GamePlay.Units;
using UnityEngine;

namespace World
{
    public class WorldController : MonoBehaviour
    {
        [SerializeField] private TickConfig tickConfig;
        [SerializeField] private FractionController fractionController;
        [SerializeField] private InteractionController interactionController;
        
        private TickSystem tickSystem;
        
        private void Awake()
        {
            tickSystem = new TickSystem();
            tickSystem.Initialize(tickConfig.TickInterval);
            tickSystem.Start();
        }

        private void OnEnable()
        {
            tickSystem.OnTick += OnTick;
        }

        private void OnDisable()
        {
            tickSystem.OnTick -= OnTick;
        }

        public void Update()
        {
            tickSystem.OnUpdate();
            interactionController.OnUpdate();
        }
        
        private void OnTick()
        {
            fractionController.OnTick();
        }
        
        private void ActionAllToAll()
        {
            for (var i = 0; i < fractionController.Units.Count; i++)
            {
                var from = fractionController.Units[i];
                if (!from) continue;
                if (Selection.TryPickBest(from, fractionController.Units, out var best))
                {
                    interactionController.TryStart(best.Action, from, best.Target);
                }
            }
        }
    }
}