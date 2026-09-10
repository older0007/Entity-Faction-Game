using UnityEngine;

namespace GamePlay.Actions.Effects.ChangeColor
{
    public interface IChangeColorEffector
    {
        public Color32 BaseColor { get; }
        public Renderer Renderer { get; }
    }
}