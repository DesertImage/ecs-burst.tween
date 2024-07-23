using DesertImage.Collections;
using DesertImage.ECS;
using Unity.Burst;
using Unity.Jobs;
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
            var job = new TweenScaleJob
            {
                Entitites = _group.Values,
                ScaleList = _group.GetComponents<Scale>(),
                TweenScaleList = _group.GetComponents<TweenScale>()
            };

            context.Handle = job.Schedule(context.Handle);
        }

        [BurstCompile]
        private struct TweenScaleJob : IJob
        {
            public UnsafeReadOnlyArray<uint> Entitites;
            public UnsafeUintReadOnlySparseSet<Scale> ScaleList;
            public UnsafeUintReadOnlySparseSet<TweenScale> TweenScaleList;

            public void Execute()
            {
                for (var i = 0; i < Entitites.Length; i++)
                {
                    var entityId = Entitites[i];

                    var tween = TweenScaleList.Read(entityId);
                    ref var scale = ref ScaleList.Get(entityId);

                    scale.Value = math.lerp
                    (
                        tween.Start,
                        tween.End,
                        Easing.GetEase(tween.Ease, tween.ElapsedTime / tween.Time)
                    );
                }
            }
        }
    }
}