using Enemy;
using UnityEngine;

namespace Player.Bullet
{
    /// <summary>
    /// プレイヤーの弾を定義
    /// </summary>
    public class BulletBehaviour : MonoBehaviour
    {
        [Header("弾の攻撃力")]
        [SerializeField] private int attackDamage = 50;

        [Header("弾の寿命（秒）")]
        [SerializeField] private float lifespan = 10.0F;

        public int AttackDamage
        {
            get { return this.attackDamage; }
            set { this.attackDamage = value; }
        }

        public float Lifespan
        {
            get { return this.lifespan; }
            set { this.lifespan = value; }
        }

        public void Update()
        {
            // 寿命を減らす
            if (this.lifespan > 0.0F)
            {
                this.lifespan -= Time.deltaTime;
            }
            else
            {
                UnityEngine.Object.Destroy(this.gameObject);
            }
        }

        public void OnCollisionEnter2D(Collision2D collision2D)
        {
            switch (collision2D.gameObject.tag)
            {
                case "Enemy":
                    this.Attack(collision2D.gameObject);
                    break;
            }
        }

        /// <summary>
        /// EnemyMovementにダメージを与える
        /// </summary>
        private void Attack(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            EnemyMovement enemyMovement = gameObject.GetComponent<EnemyMovement>();

            if (enemyMovement != null && enemyMovement.enabled)
            {
                //enemyMovement.OnDamagedByPlayer(this.attackDamage);

                // 弾を消す
                UnityEngine.Object.Destroy(this.gameObject);
            }
        }
    }
}