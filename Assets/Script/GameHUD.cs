using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text damageCostText;

    /// <summary>
    /// 損害額の表示を更新する。
    /// </summary>
    public void UpdateDamageCost(int damageCost)
    {
        if (damageCostText == null)
        {
            return;
        }

        damageCostText.text = $"¥{damageCost:N0}";
    }
}