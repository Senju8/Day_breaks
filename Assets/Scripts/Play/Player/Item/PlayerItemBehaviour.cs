using UnityEngine;

namespace Player.Item
{
    /// <summary>
    /// プレイヤーのアイテム
    /// </summary>
    public abstract class PlayerItemBehaviour : MonoBehaviour, IPlayerItem
    {
        public abstract string Id {
     get;
        }

        public void DoUse(PlayerItemState playerItemState, GameObject playerObject) { }

        /// <summary>
        /// このアイテムをを登録する
        /// </summary>
        public void Awake()
        {
            PlayerItemRegistry.INSTANCE.Register(this);
        }
    }
}