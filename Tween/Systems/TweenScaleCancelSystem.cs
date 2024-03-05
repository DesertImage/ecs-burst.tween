using DesertImage.ECS;

namespace Game.Tween
{
    public struct TweenScaleCancelSystem  : IInitSystem, IExecuteSystem
    {
        private EntitiesGroup _cancelAllGroup;
        private EntitiesGroup _cancelScaleGroup;

        public void Initialize(in World world)
        {
            _cancelAllGroup = Filter.Create(world)
                .With<TweenScale>()
                .With<TweenCancelAll>()
                .Find();

            _cancelScaleGroup = Filter.Create(world)
                .With<TweenScale>()
                .With<TweenScaleCancel>()
                .Find();
        }

        public unsafe void Execute(SystemsContext* context)
        {
            foreach (var entity in _cancelAllGroup)
            {
                entity.Remove<TweenScale>();
            }

            foreach (var entity in _cancelScaleGroup)
            {
                entity.Remove<TweenScale>();
            }
        }
    }
}