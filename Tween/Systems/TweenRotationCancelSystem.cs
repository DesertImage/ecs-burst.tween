using DesertImage.ECS;

namespace Game.Tween
{
    public struct TweenRotationCancelSystem : IInitSystem, IExecuteSystem
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

        public unsafe void Execute(SystemsContext* context)
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