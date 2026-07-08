using UnityEngine;

/// <summary>
/// 敵の攻撃判定、距離計算に用いるコリダーであることを示すマーカー
/// 
/// 罠砦の有効化範囲などの判定用コリダーと区別するために使用
/// </summary>
public class HitboxMarker : MonoBehaviour
{
    private Collider2D hitCollider;

    public Collider2D HitCollider => hitCollider != null ? hitCollider:(hitCollider = GetComponent<Collider2D>());
}