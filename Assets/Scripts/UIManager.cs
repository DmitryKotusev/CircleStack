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
    [Header("Cash shop variables")]
    public Text cashShopCashText;

    FileDataController fileDataController;

    private void Start()
    {
        fileDataController = GetComponent<FileDataController>();
    }

    public void GoToTutorialPanel()
    {
        tutorialPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void GoBackToMainPanelFromTutorialPanel()
    {
        tutorialPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void GoToCashShopPanel()
    {
        cashShopPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void GoBackToMainPanelFromCashShopPanel()
    {
        cashShopPanel.SetActive(false);
        mainPanel.SetActive(true);
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

    public void SynchronizeShopPanelUICashWithStorageData()
    {
        cashShopCashText.text = fileDataController.ReadCurrencyAmount().ToString();
    }
}
