using DesertImage.ECS;
using Unity.Mathematics;

namespace Game.Tween
{
    public struct TweenScaleSystem : IInitialize, IExecute
    {
        private EntitiesGroup _group;

        public void Initialize(in World world)
        {
            _group = Filter.Create(world)
                .With<Scale>()
                .With<TweenScale>()
                .Find();
        }

        public void Execute(ref SystemsContext context)
        {
            var scales = _group.GetComponents<Scale>();
            var tweenScales = _group.GetComponents<TweenScale>();

            foreach (var i in _group)
            {
                var tween = tweenScales.Read(i);

                scales.Get(i).Value = math.lerp
                (
                    tween.Start,
                    tween.End,
                    Easing.GetEase(tween.Ease, tween.ElapsedTime / tween.Time)
                );
            }
        }
    }
}