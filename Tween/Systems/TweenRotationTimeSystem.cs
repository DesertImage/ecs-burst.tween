using DesertImage.Collections;
using DesertImage.ECS;
using Unity.Burst;
using Unity.Jobs;

namespace Game.Tween
{
    public struct TweenRotationTimeSystem : IInitialize, IExecute
    {
        private EntitiesGroup _group;

        public void Initialize(in World world)
        {
            _group = Filter.Create(world)
                .With<TweenRotation>()
                .Find();
        }

        public void Execute(ref SystemsContext context)
        {
            var job = new TweenRotationTimeJob
            {
                Entitites = _group.Values,
                TweenRotationList = _group.GetComponents<TweenRotation>(),
                World = context.World,
                DeltaTime = context.DeltaTime
            };

            context.Handle = job.Schedule(context.Handle);
        }

        [BurstCompile]
        private struct TweenRotationTimeJob : IJob
        {
            public UnsafeReadOnlyArray<uint> Entitites;
            public UnsafeUintReadOnlySparseSet<TweenRotation> TweenRotationList;

            public World World;
            public float DeltaTime;

            public void Execute()
            {
                for (var i = 0; i < Entitites.Length; i++)
                {
                    var entityId = Entitites[i];

                    ref var tween = ref TweenRotationList.Get(entityId);

                    tween.ElapsedTime += DeltaTime;

                    if (tween.ElapsedTime < tween.Time) continue;

                    World.GetEntity(entityId).Remove<TweenRotation>();
                }
            }
        }
    }
}