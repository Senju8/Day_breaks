using Player.Bullet;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// プレイヤーを定義
    /// </summary>
    public class PlayerBehaviour : MonoBehaviour, IDamageable
    {
        [Header("プレイヤーの体力")]
        [SerializeField] private int health = 100;

        [Header("プレイヤーの攻撃力")]
        [SerializeField] private int attackDamage = 50;

        [Header("プレイヤーの速度（通常）")]
        [SerializeField] private float movementSpeed = 1.0F;

        [Header("プレイヤーの速度（ダッシュ時）")]
        [SerializeField] private float sprintSpeed = 3.0F;

        [Header("シューター")]
        [SerializeField] private GameObject shooterObject;

        [Header("プレイヤーの弾")]
        [SerializeField] private GameObject bulletObject;

        [Header("弾の初速度")]
        [SerializeField] private float bulletVelocity = 5.0F;

        [Header("弾の寿命（秒）")]
        [SerializeField] private float bulletLifespan = 10.0F;

        [Header("弾のスポーン位置までの距離")]
        [SerializeField] private float bulletSpawnDistance = 1.0F;

        [Header("射撃後のクールダウン（秒）")]
        [SerializeField] private float shootingCooldown = 0.1F;

        [Header("長押し射撃モード")]
        [SerializeField] private bool holdingShootingMode = false;

        /// <summary>
        /// プレイヤーの体力（残り）
        /// </summary>
        private int remainingHealth;

        /// <summary>
        /// 射撃後のクールダウン（残り）
        /// </summary>
        private float remainingShootingCooldown;

        private InputAction move;
        private InputAction sprint;
        private InputAction use;
        private InputAction shoot;
        private InputAction cursor;

        private new Rigidbody2D rigidbody2D;

        public void Start()
        {
            // PlayerInputの取得
            InputActionMap playerActions = this.GetComponent<PlayerInput>()?.currentActionMap;

            if (playerActions != null)
            {
                this.move = playerActions.FindAction("Move");
                this.sprint = playerActions.FindAction("Sprint");
                this.use = playerActions.FindAction("Use");
                this.shoot = playerActions.FindAction("Shoot");
                this.cursor = playerActions.FindAction("Cursor");
            }

            // Rigidbody2Dの取得
            this.rigidbody2D = GetComponent<Rigidbody2D>();

            // ステータスの初期化
            this.remainingHealth = this.health;
            this.remainingShootingCooldown = 0.0F;
        }

        public void Update()
        {
            this.Shoot();
            this.Use();
        }

        public void FixedUpdate()
        {
            this.Move();
        }

        public void OnCollisionEnter2D(Collision2D collision2D)
        {
            switch (collision2D.gameObject.tag)
            {
            }
        }

        /// <summary>
        /// プレイヤーの移動
        /// </summary>
        private void Move()
        {
            // 入力を確認
            if (this.move == null || !this.move.IsPressed())
                return;

            // Rigidbody2Dが設定されているか確認
            if (this.rigidbody2D == null)
                return;

            // 速度を計算
            Vector2 direction = this.move.ReadValue<Vector2>();
            float finalSpeed = this.sprint != null && this.sprint.IsPressed() ? this.sprintSpeed : this.movementSpeed;

            // 速度を適用
            this.rigidbody2D.linearVelocity = finalSpeed * direction;
        }

        /// <summary>
        /// プレイヤーの射撃
        /// </summary>
        private void Shoot()
        {
            // クールダウンを減らす
            if (this.remainingShootingCooldown > 0.0F)
            {
                this.remainingShootingCooldown -= Time.deltaTime;
                return;
            }

            // 入力を確認
            if (this.cursor == null || this.shoot == null)
                return;

            if (this.holdingShootingMode)
            {
                if (!this.shoot.IsPressed())
                    return;
            }
            else
            {
                if (!this.shoot.WasPressedThisFrame())
                    return;
            }

            // 弾が設定されているか確認
            if (this.bulletObject == null)
            {
                Debug.LogWarning("プレイヤーの弾が設定されていません！");
                return;
            }

            // シューターを設定
            GameObject shooterObject = this.shooterObject == null ? this.gameObject : this.shooterObject;

            if (!shooterObject.activeInHierarchy)
            {
                Debug.LogWarning("シューターが非アクティブになっています！");
                return;
            }

            // 弾の位置と速度を計算
            Vector2 cursorPos = this.cursor.ReadValue<Vector2>();
            Vector2 shooterPos = shooterObject.transform.position;
            Vector2 aimPos = Camera.main ? Camera.main.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, 0.0F)) : shooterPos;
            Vector2 aimDir = Vector2.Normalize(new Vector2(aimPos.x, aimPos.y) - new Vector2(shooterPos.x, shooterPos.y));

            // 弾を射撃
            GameObject bulletObject = UnityEngine.Object.Instantiate(this.bulletObject, shooterPos + aimDir * this.bulletSpawnDistance, Quaternion.identity);
            Rigidbody2D rigidbody2D = bulletObject.GetComponent<Rigidbody2D>();
            BulletBehaviour bulletBehaviour = bulletObject.GetComponent<BulletBehaviour>();

            if (rigidbody2D != null)
            {
                rigidbody2D.linearVelocity = aimDir * this.bulletVelocity;
            }

            if (bulletBehaviour != null && bulletBehaviour.enabled)
            {
                bulletBehaviour.AttackDamage = this.attackDamage;
                bulletBehaviour.Lifespan = this.bulletLifespan;
            }

            // クールダウンを設定
            this.remainingShootingCooldown = this.shootingCooldown;
        }

        /// <summary>
        /// アイテムを使用
        /// </summary>
        private void Use()
        {
        }

        /// <summary>
        /// IDamageableより実装
        /// </summary>
        public void OnDamaged(int damageAmount)
        {
            this.remainingHealth = Mathf.Clamp(this.remainingHealth - damageAmount, 0, this.health);
        }
    }
}