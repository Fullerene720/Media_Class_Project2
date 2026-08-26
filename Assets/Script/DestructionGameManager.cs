using UnityEngine;

public class DestructionGameManager : MonoBehaviour
{
    public static DestructionGameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameHUD gameHUD;

    public int TotalDamageCost { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        TotalDamageCost = 0;
    }

    private void Start()
    {
        if (gameHUD != null)
        {
            gameHUD.UpdateDamageCost(TotalDamageCost);
        }
    }

    /// <summary>
    /// çáåvëπäQäzÇ…ã‡äzÇâ¡éZÇ∑ÇÈÅB
    /// </summary>
    public void AddDamageCost(int damageCost)
    {
        if (damageCost <= 0)
        {
            return;
        }

        TotalDamageCost += damageCost;

        Debug.Log($"åªç›ÇÃëπäQäz: {TotalDamageCost}â~");

        if (gameHUD != null)
        {
            gameHUD.UpdateDamageCost(TotalDamageCost);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}