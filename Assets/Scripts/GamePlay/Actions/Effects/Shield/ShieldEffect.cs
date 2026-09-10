using System;
using System.Collections.Generic;
using GamePlay.Units;
using UnityEngine;

namespace GamePlay.Actions.Effects.Wait
{
    public class ShieldEffect : BaseEffect
    {
        [SerializeField] private GameObject effectPrefab;
        
        public override ActionEffectRuntime EffectRuntime => new ShieldEffectRuntime(effectPrefab);
        
        public class ShieldEffectRuntime : ActionEffectRuntime
        {
            private readonly GameObject effectPrefab;
            private Dictionary<Unit, GameObject> spawnedEffects = new();
            
            public ShieldEffectRuntime(GameObject effectPrefab)
            {
                this.effectPrefab = effectPrefab;
            }

            public override void OnStart(Interaction interaction)
            {
                interaction.Initiator.Status.AddShieldState();
                interaction.Target.Status.AddShieldState();
                
                SpawnShield(interaction.Target);
                SpawnShield(interaction.Initiator);
            }

            public override void OnEnd(Interaction interaction)
            {
                interaction.Initiator.Status.RemoveShieldState();
                interaction.Target.Status.RemoveShieldState();
                
                DestroyEffects();
            }

            //safe point
            public override void OnCancel(Interaction interaction)
            {
                interaction.Initiator.Status.RemoveShieldState();
                interaction.Target.Status.RemoveShieldState();
                
                DestroyEffects();
            }

            private void SpawnShield(Unit target)
            {
                if(spawnedEffects.ContainsKey(target)) return;
                
                var effect = Instantiate(effectPrefab, target.transform.position, Quaternion.identity);
                
                effect.transform.SetParent(target.transform);
                effect.SetActive(true);
                
                spawnedEffects[target] = effect;
            }
            
            private void DestroyEffects()
            {
                foreach (var effect in spawnedEffects.Values)
                { 
                    Destroy(effect);
                }

                spawnedEffects.Clear();
            }
        }
    }
}