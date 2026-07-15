using UnityEngine;

namespace Player.Item
{
    public interface IPlayerItem
    {
        string Id { get; }

        string Name { get; }

        void Use(IPlayerContext playerContext);
    }
}
