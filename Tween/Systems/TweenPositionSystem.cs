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

            foreach (var entityId in _group)
            {
                var tween = tweenPositions.Read(entityId);

                ref var position = ref positions.Get(entityId);

                position.Value = math.lerp
                (
                    tween.Start,
                    tween.End,
                    Easing.GetEase(tween.Ease, tween.ElapsedTime / tween.Time)
                );
            }
        }
    }
}