using DesertImage.Collections;
using DesertImage.ECS;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace Game.Tween
{
    public struct TweenRotationSystem : IInitialize, IExecute
    {
        private EntitiesGroup _group;

        public void Initialize(in World world)
        {
            _group = Filter.Create(world)
                .With<Rotation>()
                .With<TweenRotation>()
                .Find();
        }

        public void Execute(ref SystemsContext context)
        {
            var job = new TweenRotationJob
            {
                Entitites = _group.Values,
                RotationList = _group.GetComponents<Rotation>(),
                TweenRotationList = _group.GetComponents<TweenRotation>()
            };

            context.Handle = job.Schedule(context.Handle);
        }

        [BurstCompile]
        private struct TweenRotationJob : IJob
        {
            public UnsafeReadOnlyArray<uint> Entitites;
            public UnsafeUintReadOnlySparseSet<Rotation> RotationList;
            public UnsafeUintReadOnlySparseSet<TweenRotation> TweenRotationList;

            public void Execute()
            {
                for (var i = 0; i < Entitites.Length; i++)
                {
                    var entityId = Entitites[i];

                    var tween = TweenRotationList.Read(entityId);
                    ref var rotation = ref RotationList.Get(entityId);

                    rotation.Value = quaternion.Euler
                    (
                        math.lerp
                        (
                            tween.Start,
                            tween.End,
                            Easing.GetEase(tween.Ease, tween.ElapsedTime / tween.Time)
                        )
                    );
                }
            }
        }
    }
}