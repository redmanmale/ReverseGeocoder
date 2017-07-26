namespace ReverseGeocoding.Model
{
    public struct OptStats
    {
        public long RecordsIn { get; }
        public long RecordsOut { get; }

        public OptStats(long In, long Out)
        {
            RecordsIn = In;
            RecordsOut = Out;
        }
    }
}
