using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Mute button variables")]
    public RawImage muteButtonImage;
    public Texture mutedTexture;
    public Texture unmutedTexture;
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject tutorialPanel;
    public GameObject cashShopPanel;
    public GameObject shopPanel;
    [Header("Cash shop variables")]
    public Text cashShopCashText;
    [Header("Main panel variables")]
    public Text mainPanelCashText;
    [Header("Shop variables")]
    public Text shopCashText;
    public ShopManager shopManager;

    FileDataController fileDataController;
    [SerializeField]
    List<GameObject> previousPanelsList;
    GameObject currentPanel;

    private void Start()
    {
        fileDataController = GetComponent<FileDataController>();
        previousPanelsList = new List<GameObject>();
        currentPanel = mainPanel;
        shopManager.FileSynchroRequired += SynchronizeDataWithFileStorage;
        shopManager.UICurrencySynchroRequired += SynchronizeShopPanelUICashWithStorageData;
        shopManager.SetFileDataController(fileDataController);
    }

    void SynchronizeDataWithFileStorage()
    {
        fileDataController.SynchronizeRoofPrefabsContainerWithDataStotage();
    }

    public void GoToTutorialPanel()
    {
        tutorialPanel.SetActive(true);
        currentPanel.SetActive(false);
        previousPanelsList.Add(currentPanel);
        currentPanel = tutorialPanel;
    }

    public void GoToCashShopPanel()
    {
        cashShopPanel.SetActive(true);
        currentPanel.SetActive(false);
        previousPanelsList.Add(currentPanel);
        currentPanel = cashShopPanel;
    }

    public void GoToShopPanel()
    {
        shopPanel.SetActive(true);
        currentPanel.SetActive(false);
        previousPanelsList.Add(currentPanel);
        fileDataController.InitRoofPrefabsContainer();
        SynchronizeShopPanelUICashWithStorageData();
        shopManager.InitShop();
        currentPanel = shopPanel;
    }

    public void GoBack()
    {
        currentPanel.SetActive(false);
        currentPanel = previousPanelsList[previousPanelsList.Count - 1];
        currentPanel.SetActive(true);
        previousPanelsList.RemoveAt(previousPanelsList.Count - 1);
        SynchronizeCashShopPanelUICashWithStorageData();
        SynchronizeShopPanelUICashWithStorageData();
        SynchronizeMainPanelUICashWithStorageData();
    }

    public void UpdateSoundState()
    {
        if (AudioListener.volume == 1f)
        {
            AudioListener.volume = 0f;
            muteButtonImage.texture = mutedTexture;
            return;
        }
        if (AudioListener.volume == 0f)
        {
            AudioListener.volume = 1f;
            muteButtonImage.texture = unmutedTexture;
            return;
        }
    }

    public void SynchronizeCashShopPanelUICashWithStorageData()
    {
        cashShopCashText.text = fileDataController.ReadCurrencyAmount().ToString();
    }

    public void SynchronizeShopPanelUICashWithStorageData()
    {
        shopCashText.text = fileDataController.ReadCurrencyAmount().ToString();
    }

    public void SynchronizeMainPanelUICashWithStorageData()
    {
        mainPanelCashText.text = fileDataController.ReadCurrencyAmount().ToString();
    }
}
