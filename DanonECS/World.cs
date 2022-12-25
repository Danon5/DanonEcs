using DanonEcs.Internal;
using DanonEcs.Internal.DataStructures;

namespace DanonEcs
{
    public sealed class World
    {
        public SparseSet<Entity> test;

        internal World()
        {
            EcsData.EnsureCreated();
        }
        
        internal void Destroy()
        {
            EcsData.EnsureDeletedIfNoWorldsExist();
        }

        public static World Create() => new World();
        public static void Destroy(in World world) => world.Destroy();
    }
}