using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Item
{
    /// <summary>
    /// アイテムが登録されるレジストリ
    /// </summary>
    public class PlayerItemRegistry
    {
        /// <summary>
        /// このクラスのインスタンス
        /// </summary>
        public static readonly PlayerItemRegistry INSTANCE = new();

        /// <summary>
        /// 登録されたアイテムホルダー
        /// </summary>
        private readonly Dictionary<string, PlayerItemHolder> playerItems = new();

        private PlayerItemRegistry() { }

        /// <summary>
        /// 新しいアイテムを登録する
        /// </summary>
        public bool Register(IPlayerItem playerItem)
        {
            if (playerItem == null)
                return false;

            try
            {
                this.playerItems.Add(playerItem.Id, new PlayerItemHolder(playerItem));

                return true;
            }
            catch (ArgumentException)
            {
                Debug.LogWarning($"アイテム（ID: {playerItem.Id}）はすでに登録されています！");
            }

            return false;
        }

        /// <summary>
        /// 登録されたアイテムを削除する
        /// </summary>
        public bool Delete(string id)
        { 
            return this.playerItems.Remove(id);
        }

        /// <summary>
        /// 登録されたアイテムを使用する
        /// </summary>
        public bool Use(PlayerItemState playerItemState, GameObject playerObject)
        {
            if (this.playerItems.ContainsKey(playerItemState.Id))
            {
                IPlayerItem playerItem = this.playerItems[playerItemState.Id]?.playerItem;

                // アイテムを使用できるか確認
                if (playerItem != null && playerItemState.Count >= playerItem.Cost && playerItem.CanUse(playerItemState, playerObject))
                {
                    // アイテム数を減らす
                    playerItemState.Count -= playerItem.Cost;

                    // アイテムを使用
                    playerItem.DoUse(playerItemState, playerObject);

                    return true;
                }

                Debug.LogError($"アイテム（ID: {playerItemState.Id}）がnullです！");
            }
            else
            {
                Debug.LogWarning($"アイテム（ID: {playerItemState.Id}）は登録されていません！");
            }

            return false;
        }

        /// <summary>
        /// アイテムホルダー
        /// </summary>
        private class PlayerItemHolder
        {
            /// <summary>
            /// アイテム
            /// </summary>
            public IPlayerItem playerItem;

            public PlayerItemHolder(IPlayerItem playerItem)  
            {
                this.playerItem = playerItem;
            }
        }
    }
}