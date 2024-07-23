using DesertImage.Collections;
using DesertImage.ECS;
using Unity.Burst;
using Unity.Jobs;
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
            var job = new TweenPositionJob
            {
                Entitites = _group.Values,
                PositionList = _group.GetComponents<Position>(),
                TweenPositionList = _group.GetComponents<TweenPosition>()
            };

            context.Handle = job.Schedule(context.Handle);
        }

        [BurstCompile]
        private struct TweenPositionJob : IJob
        {
            public UnsafeReadOnlyArray<uint> Entitites;
            public UnsafeUintReadOnlySparseSet<Position> PositionList;
            public UnsafeUintReadOnlySparseSet<TweenPosition> TweenPositionList;

            public void Execute()
            {
                for (var i = 0; i < Entitites.Length; i++)
                {
                    var entityId = Entitites[i];

                    var tween = TweenPositionList.Read(entityId);
                    ref var position = ref PositionList.Get(entityId);

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
}