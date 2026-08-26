using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    [Header("Object Settings")]
    [SerializeField] private int maxHp = 100;

    [Tooltip("‚±‚ÌƒIƒuƒWƒFƒNƒg‚ð”j‰ó‚µ‚½‚Æ‚«‚Ì‘¹ŠQŠz")]
    [SerializeField] private int damageValue = 10000;

    [Header("UI")]
    [SerializeField] private DestructibleUI destructibleUI;

    private int currentHp;
    private bool isDestroyed;

    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;
    public int DamageValue => damageValue;

    private void Awake()
    {
        currentHp = maxHp;

        if (destructibleUI == null)
        {
            destructibleUI =
                GetComponentInChildren<DestructibleUI>(true);
        }

        if (destructibleUI != null)
        {
            destructibleUI.Initialize(maxHp);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed)
        {
            return;
        }

        if (damage <= 0)
        {
            return;
        }

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);

        Debug.Log(
            $"{gameObject.name} : {currentHp} / {maxHp}"
        );

        if (destructibleUI != null)
        {
            destructibleUI.UpdateHp(currentHp, maxHp);
        }

        if (currentHp <= 0)
        {
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        if (isDestroyed)
        {
            return;
        }

        isDestroyed = true;

        // ‘¹ŠQŠz‚ðGameManager‚Ö‰ÁŽZ
        if (DestructionGameManager.Instance != null)
        {
            DestructionGameManager.Instance.AddDamageCost(damageValue);
        }
        else
        {
            Debug.LogWarning(
                "DestructionGameManager‚ªScene“à‚É‘¶Ý‚µ‚Ü‚¹‚ñB"
            );
        }

        Debug.Log(
            $"{gameObject.name} ‚ð”j‰ó‚µ‚Ü‚µ‚½B" +
            $" ‘¹ŠQŠz: {damageValue}‰~"
        );

        Destroy(gameObject);
    }
}