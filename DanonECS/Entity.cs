namespace DanonEcs
{
    public readonly struct Entity
    {
        public readonly int id;
        public readonly int version;

        internal Entity(int id, int version)
        {
            this.id = id;
            this.version = version;
        }
    }
}