namespace TheQuestionReborn.Settings
{
    public class Settings
    {
        public Settings()
        {
            IsTileShow = false;
            TimeTileUpdate = 30;
        }

        public uint TimeTileUpdate { get; set; }

        public bool IsTileShow { get; set; }
    }
}
