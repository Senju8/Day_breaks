using Enemy;
using UnityEngine;

namespace Player.Bullet
{
    /// <summary>
    /// プレイヤーの弾
    /// </summary>
    public class BulletBehaviour : MonoBehaviour
    {
        [Header("弾の攻撃力")]
        [SerializeField] private int attackDamage = 50;

        [Header("弾の寿命（秒）")]
        [SerializeField] private float duration = 8.0F;

        public int AttackDamage
        {
            get { return this.attackDamage; }
            set { this.attackDamage = value; }
        }

        public float Duration
        {
            get { return this.duration; }
            set { this.duration = value; }
        }

        /// <summary>
        /// 弾の寿命（残り）
        /// </summary>
        private float remaningDuration;

        public void Start()
        {
            // ステータスの初期化
            this.remaningDuration = this.duration;
        }

        public void Update()
        {
            this.Collapse();
        }

        /// <summary>
        /// 寿命を減らす
        /// </summary>
        private void Collapse()
        {
            if (this.duration > 0.0F)
            {
                this.duration -= Time.deltaTime;
            }
            else
            {
                UnityEngine.Object.Destroy(this.gameObject);
            }
        }

        public void OnTriggerEnter2D(Collider2D collider2D)
        {
            this.Attack(collider2D.gameObject);
            this.Walls(collider2D.gameObject);
        }

        /// <summary>
        /// 衝突したEntityMovementにダメージを与える
        /// </summary>
        private void Attack(GameObject gameObject)
        {
            // HitboxMarkerとの衝突を検知
            HitboxMarker hitboxMarker = gameObject.GetComponent<HitboxMarker>();

            if (hitboxMarker != null && hitboxMarker.enabled)
            {
                EnemyMovement enemyMovement = gameObject.GetComponentInParent<EnemyMovement>();

                // HitboxMarkerがEnemyMovementの子であるかチェック
                if (enemyMovement != null)
                {
                    // EnemyMovementにダメージを与える
                    enemyMovement.OnDamagedByPlayer(this.attackDamage);

                    // 弾を消す
                    UnityEngine.Object.Destroy(this.gameObject);
                }
            }
        }

        /// <summary>
        ///  Wallsとの衝突
        /// </summary>
        private void Walls(GameObject gameObject, int level = 0)
        {
            if (level < 0)
                return;

            Transform parentTransform = gameObject.transform.parent;

            // 親Transformが"Walls"かどうか確認
            if (parentTransform != null && parentTransform.gameObject != null)
            {
                if (parentTransform.gameObject.name == "Walls")
                {
                    // 弾を消す
                    UnityEngine.Object.Destroy(this.gameObject);
                }
                else
                {
                    // さらに上の階層を確認
                    this.Walls(gameObject, level - 1);
                }
            }
        }
    }
}