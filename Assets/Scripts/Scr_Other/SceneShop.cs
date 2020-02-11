using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using TMPro;
using UniRx;

public class SceneShop : MonoBehaviour
{
    private PlayerController playerController;

    public ScriptableObjectsSettingsSceneShop scrObjsSettingsSceneShop;

    public TextMeshProUGUI LabelText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI PriceText;

    private ReactiveProperty<string> RecLabelText = new ReactiveProperty<string>();
    private ReactiveProperty<string> RecScoreText = new ReactiveProperty<string>();
    private ReactiveProperty<string> RecPriceText = new ReactiveProperty<string>();

    public Image Ico;

    public GameObject OpenPanel;
    public GameObject ClosePanel;

    private SceneName sceneName;

    private int ScoreScene;
    private int PriceScene;

    [SerializeField] private bool Bought;

    private void Awake()
    {
        RecLabelText.SubscribeToText(LabelText, i => $"Name: {sceneName}");
        RecScoreText.SubscribeToText(ScoreText, i => $"Score: {ScoreScene}");
        RecPriceText.SubscribeToText(PriceText, i => $"Price: {PriceScene}");
        playerController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
        if (File.Exists(Application.dataPath + $"/data/SceneData{scrObjsSettingsSceneShop.sceneName}.sav")) LoadComponent();
    }

    private void LoadComponent()
    {
        int[] data = SceneData.LoadSceneData(scrObjsSettingsSceneShop.sceneName);
        ScoreScene = data[0];
        Bought = true;
    }

    private void Start()
    {
        sceneName = scrObjsSettingsSceneShop.sceneName;
        PriceScene = scrObjsSettingsSceneShop.Price;
        Ico.sprite = scrObjsSettingsSceneShop.Ico;
        RecLabelText.Value = scrObjsSettingsSceneShop.sceneName.ToString();
        RecScoreText.Value = ScoreScene.ToString();
        RecPriceText.Value = PriceScene.ToString();
        if (Bought)
        {
            OpenPanel.SetActive(true);
            ClosePanel.SetActive(false);
        }
    }

    public void OnBuyScene()
    {
        var playerMoney = playerController.Money;
        if (playerMoney > PriceScene)
        {
            playerController.SetMoney(-Mathf.Abs(PriceScene));
            playerController.SavePlayerData();
            OpenPanel.SetActive(true);
            ClosePanel.SetActive(false);
            CreateFile();
        }
    }

    public void CreateFile()
    {
        playerController.SetSceneName(sceneName);
        playerController.SaveSceneData();
    }

    public void OnPlayScene()
    {
        if (sceneName == SceneName.BloodyHarvest) SceneManager.LoadScene(1);
        if (sceneName == SceneName.InfiniteSpace) SceneManager.LoadScene(2);
        if (sceneName == SceneName.FindingNemo) SceneManager.LoadScene(3);
    }
}