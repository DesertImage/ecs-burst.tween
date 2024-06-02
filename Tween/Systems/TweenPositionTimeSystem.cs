using DesertImage.ECS;

namespace Game.Tween
{
    public struct TweenPositionTimeSystem : IInitialize, IExecute
    {
        private EntitiesGroup _group;

        public void Initialize(in World world)
        {
            _group = Filter.Create(world)
                .With<TweenPosition>()
                .Find();
        }

        public void Execute(ref SystemsContext context)
        {
            var tweens = _group.GetComponents<TweenPosition>();
            for (var i = tweens.Length - 1; i >= 0; i--)
            {
                ref var tween = ref tweens.Get(i);

                tween.ElapsedTime += context.DeltaTime;

                if (tween.ElapsedTime < tween.Time) return;

                _group.GetEntity(i).Remove<TweenPosition>();
            }
        }
    }
}