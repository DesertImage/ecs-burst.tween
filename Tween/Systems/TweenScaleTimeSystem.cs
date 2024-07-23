using DesertImage.Collections;
using DesertImage.ECS;
using Unity.Burst;
using Unity.Jobs;

namespace Game.Tween
{
    public struct TweenScaleTimeSystem : IInitialize, IExecute
    {
        private EntitiesGroup _group;

        public void Initialize(in World world)
        {
            _group = Filter.Create(world)
                .With<TweenScale>()
                .Find();
        }

        public void Execute(ref SystemsContext context)
        {
            var job = new TweenScaleTimeJob
            {
                Entitites = _group.Values,
                TweenScaleList = _group.GetComponents<TweenScale>(),
                World = context.World,
                DeltaTime = context.DeltaTime
            };

            context.Handle = job.Schedule(context.Handle);
        }

        [BurstCompile]
        private struct TweenScaleTimeJob : IJob
        {
            public UnsafeReadOnlyArray<uint> Entitites;
            public UnsafeUintReadOnlySparseSet<TweenScale> TweenScaleList;

            public World World;
            public float DeltaTime;

            public void Execute()
            {
                for (var i = 0; i < Entitites.Length; i++)
                {
                    var entityId = Entitites[i];

                    ref var tween = ref TweenScaleList.Get(entityId);

                    tween.ElapsedTime += DeltaTime;

                    if (tween.ElapsedTime < tween.Time) continue;

                    World.GetEntity(entityId).Remove<TweenScale>();
                }
            }
        }
    }
}