using System;
using GamePlay.Actions;
using GamePlay.Actions.Effects;
using GamePlay.Actions.Effects.ChangeColor;
using UnityEngine;

namespace GamePlay.Units
{
    public class Unit : MonoBehaviour, IChangeColorEffector
    {
        [SerializeField] private Renderer renderer;
        
        public override string ToString() => $"{unitConfig.UnitName} ({unitConfig.FractionConfig.FractionName})";
        public Color32 BaseColor => unitConfig.BaseColor;
        public Renderer Renderer => renderer;
        public Status Status { get; private set; } = new Status();
        public UnitConfig Config => unitConfig;
        public Interaction Current { get; set; }
        public bool IsBusy => Current != null;

        private UnitConfig unitConfig;
        
        public void InitConfig(UnitConfig config)
        {
            unitConfig = config;
            renderer.material.color = unitConfig.BaseColor;
            Status.ChangeState(State.Idle);
        }
    }
}