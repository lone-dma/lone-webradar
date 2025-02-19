using MessagePack;

namespace AncientMountain.Managed.Data
{
    [MessagePackObject]
    public sealed class WebRadarUpdate
    {
        /// <summary>
        /// Update version (used for ordering).
        /// </summary>
        [Key(0)]
        public uint Version { get; init; }
        /// <summary>
        /// True if In-Game, otherwise False.
        /// </summary>
        [Key(1)]
        public bool InGame { get; init; }
        /// <summary>
        /// Contains the Map ID of the current map.
        /// </summary>
        [Key(2)]
        public string MapID { get; init; }
        /// <summary>
        /// All Players currently on the map.
        /// </summary>
        [Key(3)]
        public List<WebRadarPlayer> Players { get; init; }
    }
}
