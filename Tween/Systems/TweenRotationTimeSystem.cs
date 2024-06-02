using DesertImage.ECS;

namespace Game.Tween
{
    public struct TweenRotationTimeSystem : IInitialize, IExecute
    {
        private EntitiesGroup _group;

        public void Initialize(in World world)
        {
            _group = Filter.Create(world)
                .With<TweenRotation>()
                .Find();
        }

        public void Execute(ref SystemsContext context)
        {
            var tweens = _group.GetComponents<TweenRotation>();

            foreach (var entityId in _group)
            {
                ref var tween = ref tweens.Get(entityId);

                tween.ElapsedTime += context.DeltaTime;

                if (tween.ElapsedTime < tween.Time) return;

                _group.GetEntity(entityId).Remove<TweenRotation>();
            }
        }
    }
}