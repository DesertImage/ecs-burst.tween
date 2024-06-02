using DesertImage.ECS;

namespace Game.Tween
{
    public struct TweenScaleCancelSystem : IInitialize, IExecute
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

        public void Execute(ref SystemsContext context)
        {
            foreach (var entityId in _cancelAllGroup)
            {
                _cancelAllGroup.GetEntity(entityId).Remove<TweenScale>();
            }

            foreach (var entityId in _cancelScaleGroup)
            {
                _cancelAllGroup.GetEntity(entityId).Remove<TweenScale>();
            }
        }
    }
}