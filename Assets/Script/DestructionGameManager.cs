using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestructionGameManager : MonoBehaviour
{
    public static DestructionGameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameHUD gameHUD;
    [SerializeField] private PlayerAttack playerAttack;

    // FirstPersonControllerをここに設定
    [SerializeField] private Behaviour playerController;

    [Header("Game Settings")]
    [SerializeField] private float gameTime = 60f;

    [Header("Item Drop")]
    [SerializeField] private AttackPowerItem attackPowerItemPrefab;

    [Range(0f, 1f)]
    [SerializeField] private float attackPowerItemDropRate = 0.15f;

    public int TotalDamageCost { get; private set; }
    public float RemainingTime { get; private set; }
    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        TotalDamageCost = 0;
        RemainingTime = gameTime;
        IsPlaying = false;
    }

    private void Start()
    {
        // 初期UI
        if (gameHUD != null)
        {
            gameHUD.UpdateDamageCost(TotalDamageCost);
            gameHUD.UpdateTimer(RemainingTime);
            gameHUD.HideResult();

            if (playerAttack != null)
            {
                gameHUD.UpdateAttackPower(
                    playerAttack.AttackPower
                );
            }
        }

        // カウントダウン中は操作禁止
        SetPlayerControl(false);

        // FPSゲームなので開始時はカーソルをロック
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(GameSequence());
    }

    /// <summary>
    /// ゲーム開始までのカウントダウン。
    /// </summary>
    private IEnumerator GameSequence()
    {
        if (gameHUD != null)
        {
            gameHUD.ShowCountdown("3");
        }

        yield return new WaitForSeconds(1f);

        if (gameHUD != null)
        {
            gameHUD.ShowCountdown("2");
        }

        yield return new WaitForSeconds(1f);

        if (gameHUD != null)
        {
            gameHUD.ShowCountdown("1");
        }

        yield return new WaitForSeconds(1f);

        if (gameHUD != null)
        {
            gameHUD.ShowCountdown("START!");
        }

        // ここからゲーム開始
        IsPlaying = true;
        SetPlayerControl(true);

        // タイマーも同時に開始
        StartCoroutine(GameTimer());

        // START!は少しだけ残す
        yield return new WaitForSeconds(0.7f);

        if (gameHUD != null)
        {
            gameHUD.HideCountdown();
        }
    }

    /// <summary>
    /// 制限時間をカウントする。
    /// </summary>
    private IEnumerator GameTimer()
    {
        RemainingTime = gameTime;

        while (IsPlaying && RemainingTime > 0f)
        {
            RemainingTime -= Time.deltaTime;
            RemainingTime = Mathf.Max(RemainingTime, 0f);

            if (gameHUD != null)
            {
                gameHUD.UpdateTimer(RemainingTime);
            }

            yield return null;
        }

        if (IsPlaying)
        {
            FinishGame();
        }
    }

    /// <summary>
    /// 損害額を加算する。
    /// </summary>
    public void AddDamageCost(int damageCost)
    {
        if (!IsPlaying)
        {
            return;
        }

        if (damageCost <= 0)
        {
            return;
        }

        TotalDamageCost += damageCost;

        if (gameHUD != null)
        {
            gameHUD.UpdateDamageCost(TotalDamageCost);
        }
    }

    /// <summary>
    /// 攻撃力上昇時。
    /// </summary>
    public void OnAttackPowerIncreased(
        int increaseAmount,
        int currentAttackPower)
    {
        if (!IsPlaying)
        {
            return;
        }

        if (gameHUD == null)
        {
            return;
        }

        gameHUD.UpdateAttackPower(currentAttackPower);
        gameHUD.ShowAttackPowerIncrease(increaseAmount);
    }

    /// <summary>
    /// 攻撃力アイテムのドロップ抽選。
    /// </summary>
    public void TryDropAttackPowerItem(Vector3 position)
    {
        if (!IsPlaying)
        {
            return;
        }

        if (attackPowerItemPrefab == null)
        {
            return;
        }

        if (Random.value > attackPowerItemDropRate)
        {
            return;
        }

        Vector3 spawnPosition =
            position + Vector3.up * 0.5f;

        Instantiate(
            attackPowerItemPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }

    public void OnAutoFireUnlocked()
    {
        if (gameHUD == null)
        {
            return;
        }

        gameHUD.ShowAutoFireUnlocked();
    }

    /// <summary>
    /// ゲーム終了。
    /// </summary>
    private void FinishGame()
    {
        IsPlaying = false;
        RemainingTime = 0f;

        SetPlayerControl(false);

        if (gameHUD != null)
        {
            gameHUD.UpdateTimer(0f);
            gameHUD.ShowResult(TotalDamageCost);
        }

        // Result画面のボタンを押せるようにする
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void SetPlayerControl(bool enabled)
    {
        if (playerController != null)
        {
            playerController.enabled = enabled;
        }

        if (playerAttack != null)
        {
            playerAttack.enabled = enabled;
        }
    }

    /// <summary>
    /// 現在のステージをやり直す。
    /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    /// <summary>
    /// MainMenuへ戻る。
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}