using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    [Header("Object Settings")]
    [SerializeField] private int maxHp = 100;

    [Tooltip("このオブジェクトを破壊したときの損害額")]
    [SerializeField] private int damageValue = 10000;

    [Header("UI")]
    [SerializeField] private DestructibleUI destructibleUI;


    [Tooltip("必要に応じて追加するアイテムのドロップ位置")]
    [SerializeField] private Transform itemDropPoint;

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
            destructibleUI = GetComponentInChildren<DestructibleUI>(true);
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

        Debug.Log(  $"{gameObject.name} : {currentHp} / {maxHp}" );

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

        if (DestructionGameManager.Instance != null)
        {
            DestructionGameManager.Instance.AddDamageCost(damageValue);

            DestructionGameManager.Instance.TryDropAttackPowerItem(  GetDropPosition() );
        }

        Destroy(gameObject);
    }

    private Vector3 GetDropPosition()
    {
        if (itemDropPoint != null)
        {
            return itemDropPoint.position;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();

        if (colliders.Length == 0)
        {
            return transform.position;
        }

        Bounds bounds = colliders[0].bounds;

        for (int i = 1; i < colliders.Length; i++)
        {
            bounds.Encapsulate(colliders[i].bounds);
        }

        return bounds.center;
    }
}