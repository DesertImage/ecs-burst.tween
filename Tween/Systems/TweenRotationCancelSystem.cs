using DesertImage.ECS;

namespace Game.Tween
{
    public struct TweenRotationCancelSystem : IInitialize, IExecute
    {
        private EntitiesGroup _cancelAllGroup;
        private EntitiesGroup _cancelRotationGroup;

        public void Initialize(in World world)
        {
            _cancelAllGroup = Filter.Create(world)
                .With<TweenRotation>()
                .With<TweenCancelAll>()
                .Find();

            _cancelRotationGroup = Filter.Create(world)
                .With<TweenRotation>()
                .With<TweenRotationCancel>()
                .Find();
        }

        public void Execute(ref SystemsContext context)
        {
            foreach (var entity in _cancelAllGroup)
            {
                entity.Remove<TweenRotation>();
            }

            foreach (var entity in _cancelRotationGroup)
            {
                entity.Remove<TweenRotation>();
            }
        }
    }
}