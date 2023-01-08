using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DanonEcs;

namespace DanonEcsBenchmarks
{
    public class AllocBenchmarks
    {
        private World m_world;
        
        [GlobalSetup]
        public void Setup()
        {
            m_world = World.Create();
        }
        
        [Benchmark]
        public void AllocEntities()
        {
            
        }

        [IterationCleanup]
        public void Cleanup()
        {
            
        }
    }
    
    internal static class Benchmarks
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<AllocBenchmarks>();
        }
    }
}