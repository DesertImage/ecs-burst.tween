using DesertImage.ECS;
using Unity.Mathematics;

namespace Game.Tween
{
    public struct TweenRotationSystem : IInitSystem, IExecuteSystem
    {
        private EntitiesGroup _group;

        public void Initialize(in World world)
        {
            _group = Filter.Create(world)
                .With<Rotation>()
                .With<TweenRotation>()
                .Find();
        }

        public void Execute(ref SystemsContext context)
        {
            var rotations = _group.GetComponents<Rotation>();
            var tweenRotations = _group.GetComponents<TweenRotation>();

            for (var i = 0; i < rotations.Length; i++)
            {
                var tween = tweenRotations[i];

                rotations.Get(i).Value = math.lerp
                (
                    tween.Start,
                    tween.End,
                    Easing.GetEase(tween.Ease, tween.ElapsedTime / tween.Time)
                );
            }
        }
    }
}