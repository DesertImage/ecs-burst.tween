using DesertImage.ECS;
using Unity.Mathematics;

namespace Game.Tween
{
    public struct TweenPositionSystem : IInitialize, IExecute
    {
        private EntitiesGroup _group;

        public void Initialize(in World world)
        {
            _group = Filter.Create(world)
                .With<Position>()
                .With<TweenPosition>()
                .Find();
        }

        public void Execute(ref SystemsContext context)
        {
            var positions = _group.GetComponents<Position>();
            var tweenPositions = _group.GetComponents<TweenPosition>();

            for (var i = 0; i < positions.Length; i++)
            {
                var tween = tweenPositions[i];

                positions.Get(i).Value = math.lerp
                (
                    tween.Start,
                    tween.End,
                    Easing.GetEase(tween.Ease, tween.ElapsedTime / tween.Time)
                );
            }
        }
    }
}