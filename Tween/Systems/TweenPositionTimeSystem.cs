using DesertImage.Collections;
using DesertImage.ECS;
using Unity.Burst;
using Unity.Jobs;

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
            var job = new TweenPositionTimeJob
            {
                Entitites = _group.Values,
                TweenPositionList = _group.GetComponents<TweenPosition>(),
                World = context.World,
                DeltaTime = context.DeltaTime
            };

            context.Handle = job.Schedule(context.Handle);
        }

        [BurstCompile]
        private struct TweenPositionTimeJob : IJob
        {
            public UnsafeReadOnlyArray<uint> Entitites;
            public UnsafeUintReadOnlySparseSet<TweenPosition> TweenPositionList;

            public World World;
            public float DeltaTime;

            public void Execute()
            {
                for (var i = 0; i < Entitites.Length; i++)
                {
                    var entityId = Entitites[i];

                    ref var tween = ref TweenPositionList.Get(entityId);

                    tween.ElapsedTime += DeltaTime;

                    if (tween.ElapsedTime < tween.Time) continue;

                    World.GetEntity(entityId).Remove<TweenPosition>();
                }
            }
        }
    }
}