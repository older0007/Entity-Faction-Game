using GamePlay.Actions.Effects.ChangeColor;
using GamePlay.Units;
using UnityEngine;

namespace GamePlay.Actions.Effects.Resurrection
{
    public class ResurrectionEffect : BaseEffect
    {
        [SerializeField] private string materialPropertyName = "_Color";

        public override ActionEffectRuntime EffectRuntime => new ResurrectionEffectRuntime(materialPropertyName);
        
        public class ResurrectionEffectRuntime : ActionEffectRuntime
        {
            private Color32 newColor;
            private string materialPropertyName;
            private IChangeColorEffector effector;
            private MaterialPropertyBlock propertyBlock;
            
            public ResurrectionEffectRuntime(string materialPropertyName)
            {
                this.materialPropertyName = materialPropertyName;
                propertyBlock = new MaterialPropertyBlock();
            }
            
            public override void OnStart(Interaction interaction)
            {
                interaction.Initiator.Status.AddShieldState();
                
                effector = interaction.Target.GetComponent<IChangeColorEffector>();
                newColor = effector.BaseColor;
                effector.Renderer.GetPropertyBlock(propertyBlock);
            }
            
            public override void OnEnd(Interaction interaction)
            {
                interaction.Initiator.Status.RemoveShieldState();
                interaction.Target.Status.AddRevivedState();
                
                propertyBlock.SetColor(materialPropertyName, newColor);
                effector.Renderer.SetPropertyBlock(propertyBlock);
            }

            public override bool ShouldInterrupt(Interaction interaction)
            {
                return interaction.Initiator.Status.CurrentState == State.Dead;
            }
        }
    }
}