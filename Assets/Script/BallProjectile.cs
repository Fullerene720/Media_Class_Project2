using UnityEngine;

public class BallProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float lifeTime = 5f;

    [Header("Visual")]
    [SerializeField] private Renderer ballRenderer;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material autoFireMaterial;

    [Header("Effects")]
    [SerializeField] private ParticleSystem hitEffectPrefab;

    private int attackPower;
    private bool hasHit;

    public void Initialize(
        int damage,
        bool isAutoFire)
    {
        attackPower = damage;

        if (ballRenderer != null)
        {
            if (isAutoFire && autoFireMaterial != null)
            {
                ballRenderer.sharedMaterial =
                    autoFireMaterial;
            }
            else if (normalMaterial != null)
            {
                ballRenderer.sharedMaterial =
                    normalMaterial;
            }
        }

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit)
        {
            return;
        }

        hasHit = true;

        if (DestructionGameManager.Instance != null &&
            !DestructionGameManager.Instance.IsPlaying)
        {
            Destroy(gameObject);
            return;
        }

        // 衝突位置にエフェクトを生成
        if (hitEffectPrefab != null &&
            collision.contactCount > 0)
        {
            ContactPoint contact =
                collision.GetContact(0);

            Instantiate(
                hitEffectPrefab,
                contact.point,
                Quaternion.LookRotation(contact.normal)
            );
        }

        IDamageable damageable =
            collision.collider
                .GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(attackPower);
        }

        Destroy(gameObject);
    }
}