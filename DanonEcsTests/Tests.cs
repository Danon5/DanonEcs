using DanonEcs;
using DanonEcs.Internal.DataStructures;
using NUnit.Framework;

namespace DanonEcsTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var world = World.Create();
            world.test = new SparseSet<Entity>();

            for (var i = 0; i < 500; i++) 
                world.test.Add(i, new Entity());
            
            Assert.True(world.test.Count == 500);
            
            for (var i = 0; i < 5; i++)
                world.test.Remove(i);
            
            Assert.True(world.test.Count == 495);
        }
    }
}