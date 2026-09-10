using GamePlay.Units;
using UnityEngine;

namespace GamePlay.Actions.Effects.ChangeColor
{
     public class AttackEffect : BaseEffect
    {
        [SerializeField] private Color32 newColor;
        [SerializeField] private string materialPropertyName = "_Color";
        public override ActionEffectRuntime EffectRuntime => new AttackEffectRuntime(newColor, materialPropertyName);
        
        public class AttackEffectRuntime : ActionEffectRuntime
        {
            private readonly Color32 newColor;
            private readonly string materialPropertyName;
            private IChangeColorEffector effector;
            private MaterialPropertyBlock propertyBlock;
            
            public AttackEffectRuntime(Color32 newColor, string materialPropertyName)
            {
                this.newColor = newColor;
                this.materialPropertyName = materialPropertyName;
                propertyBlock = new MaterialPropertyBlock();
            }

            public override void OnStart(Interaction interaction)
            {
                interaction.Initiator.Status.AddInteractionState();
                interaction.Target.Status.AddInteractionState();
                
                effector = interaction.Target.GetComponent<IChangeColorEffector>();
                effector.Renderer.GetPropertyBlock(propertyBlock);
            }

            public override void OnEnd(Interaction interaction)
            {
                interaction.Initiator.Status.RemoveInteractionState();
                interaction.Target.Status.RemoveInteractionState();
                interaction.Target.Status.ChangeState(State.Dead);
                
                propertyBlock.SetColor(materialPropertyName, newColor);
                effector.Renderer.SetPropertyBlock(propertyBlock);
            }
            
            public override void OnUpdate(Interaction interaction)
            {
                
            }
            
            public override void OnCancel(Interaction interaction)
            {
                interaction.Initiator.Status.RemoveInteractionState();
                interaction.Target.Status.RemoveInteractionState();
            }

            public override bool ShouldInterrupt(Interaction interaction)
            {
                return interaction.Target.Status.CurrentState == State.Dead
                       || interaction.Initiator.Status.CurrentState == State.Dead
                       || interaction.Target.Status.HasShieldState();
            }
        }
    }
}