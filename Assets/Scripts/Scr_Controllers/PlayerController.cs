using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using TMPro;
using UniRx;

public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI MoneyText;
    public Image LifeImage;

    private ReactiveProperty<int> RecScoreText = new ReactiveProperty<int>();
    private ReactiveProperty<int> RecMoneyText = new ReactiveProperty<int>();

    [SerializeField] private SceneName sceneName;

    private int OldScore;
    private int Reward;
    public int Score { get; set; }
    public int Money { get; set; }

    private float RewardTime;
    private float CurrentTime;

    [SerializeField] private bool ActivePlayer;

    public Transform SpawnPoint;
    public GameObject PrePlayer;

    private void Awake()
    {
        if (ActivePlayer)
        {
            PoolManager.Instance.AddPool(PoolType.Pool_ShipPlayer);
            if (File.Exists(Application.dataPath + $"/data/SceneData{sceneName}.sav")) LoadPlayerData();
            RecScoreText.SubscribeToText(ScoreText, i => $"Score: {Score}");
        }
        RecMoneyText.SubscribeToText(MoneyText, i => $"Money: {Money}");
        if (File.Exists(Application.dataPath + "/data/PlayerData.sav")) LoadPlayerData();
    }

    private void Start()
    {
        if (ActivePlayer)
        {
            Reward = 1;
            RewardTime = 3;
            SpawnPlayer();
            TextScore();
        }
        TextMoney();
    }

    private void SpawnPlayer()
    {
        var player = PoolManager.Instance.Spawn(PoolType.Pool_ShipPlayer, PrePlayer, SpawnPoint.position, SpawnPoint.rotation, SpawnPoint);
        player.GetComponent<PlayerView>().OnUpdateLife += UpdateLifeImage;
    }

    private void FixedUpdate()
    {
        if (ActivePlayer)
        {
            if (CurrentTime < RewardTime) CurrentTime += Time.deltaTime;
            else
            {
                SetScore(Reward);
                CurrentTime = 0;
            }
        }
    }

    public void SetScore(int score)
    {
        Score += score;
        TextScore();
    }

    private void TextScore()
    {
        RecScoreText.Value = Score;
    }

    public void SetMoney(int money)
    {
        Money += money;
        TextMoney();
    }

    private void TextMoney()
    {
        RecMoneyText.Value = Money;
    }

    public void UpdateLifeImage(float value)
    {
        LifeImage.fillAmount = value;
    }

    public void SavePlayerData()
    {
        PlayerData.SavePlayerData(this);
    }

    public void SaveSceneData()
    {
        if (Score < OldScore) Score = OldScore;
        SceneData.SaveSceneData(this);
    }

    public SceneName GetSceneName()
    {
        return sceneName;
    }

    public void BackToMenu()
    {
        var singl = GameObject.FindGameObjectWithTag("Singleton").gameObject;
        if (singl != null) Destroy(singl);
        SavePlayerData();
        SaveSceneData();
        SceneManager.LoadScene(0);
    }

    public void SetSceneName(SceneName type)
    {
        sceneName = type;
    }

    public void LoadPlayerData()
    {
        int[] data = PlayerData.LoadPlayerData();
        Money = data[0];
    }

    public void LoadSceneData()
    {
        int[] data = SceneData.LoadSceneData(sceneName);
        OldScore = data[0];
    }

    public int GetScore()
    {
        return Score;
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
        if (ActivePlayer) SaveSceneData();
    }
}