using DesertImage.ECS;

namespace Game.Tween
{
    public struct TweenPositionCancelSystem : IInitialize, IExecute
    {
        private EntitiesGroup _cancelAllGroup;
        private EntitiesGroup _cancelPositionGroup;

        public void Initialize(in World world)
        {
            _cancelAllGroup = Filter.Create(world)
                .With<TweenPosition>()
                .With<TweenCancelAll>()
                .Find();

            _cancelPositionGroup = Filter.Create(world)
                .With<TweenPosition>()
                .With<TweenPositionCancel>()
                .Find();
        }

        public void Execute(ref SystemsContext context)
        {
            foreach (var entity in _cancelAllGroup)
            {
                entity.Remove<TweenPosition>();
            }

            foreach (var entity in _cancelPositionGroup)
            {
                entity.Remove<TweenPosition>();
            }
        }
    }
}