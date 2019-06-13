using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    ShopElement currentlySelectedElement;
    ShopElement currentlyEquipedElement;
    [SerializeField]
    Text buyButtonText;
    [SerializeField]
    RectTransform shopContent;
    [SerializeField]
    ScrollRect scrollRect;
    [SerializeField]
    GameObject shopElementPrefab;
    [SerializeField]
    GameObject equipButton;
    [SerializeField]
    GameObject buyButton;

    private Animator buyButtonAnimator;
    private FileDataController fileDataController;

    public delegate void EquipNewRoof(RoofPrefabInfo roofPrefabInfo);
    public event EquipNewRoof EquipNewRoofEvent;
    public event Action FileSynchroRequired;
    public event Action UICurrencySynchroRequired;

    private void Start()
    {
        buyButtonAnimator = buyButton.GetComponent<Animator>();
    }

    public void SetFileDataController(FileDataController fileDataController)
    {
        this.fileDataController = fileDataController;
    }

    public void OnSelectElement(ShopElement shopElement)
    {
        // UI change of previosly selected element
        if (currentlySelectedElement != null)
        {
            currentlySelectedElement.MarkAsUnselected();
        }

        currentlySelectedElement = shopElement;

        // UI change of newly selected element
        currentlySelectedElement.MarkAsSelected();

        buyButtonText.text = currentlySelectedElement.RoofPrefab.price.ToString();
        AdjustButtons();
    }

    public void InitShop()
    {
        FillShopContent();
        AdjustButtons();
    }

    public void AdjustButtons()
    {
        if (currentlySelectedElement == null)
        {
            equipButton.SetActive(false);
            buyButton.SetActive(false);
            return;
        }
        if (currentlySelectedElement.RoofPrefab.isBought)
        {
            buyButton.SetActive(false);
            if (currentlySelectedElement.RoofPrefab.isEquiped)
            {
                equipButton.SetActive(false);
            }
            else
            {
                equipButton.SetActive(true);
            }
        }
        else
        {
            buyButton.SetActive(true);
            equipButton.SetActive(false);
        }
    }

    public void FillShopContent()
    {
        if (shopContent.childCount > 0)
        {
            ClearShopContent();
        }
        RoofsPrefabsContainer roofsPrefabsContainer =
            GameObject.FindGameObjectWithTag("RoofPrefabsContainer").GetComponent<RoofsPrefabsContainer>();
        roofsPrefabsContainer.roofPrefabInfos.Sort((first, second) =>
        {
            return first.arrayIndex.CompareTo(second.arrayIndex);
        });
        foreach (RoofPrefabInfo roofPrefabInfo in roofsPrefabsContainer.roofPrefabInfos)
        {
            GameObject shopElement = Instantiate(shopElementPrefab, shopContent);
            ShopElement shopElementData = shopElement.GetComponent<ShopElement>();
            shopElementData.RoofPrefab = roofPrefabInfo;
            shopElementData.ThisElementSelect += OnSelectElement;
            if (shopElementData.RoofPrefab.isEquiped)
            {
                shopElementData.MarkAsEquiped();
                currentlyEquipedElement = shopElementData;
            }
            else
            {
                shopElementData.MarkAsUnequiped();
            }
        }

        // Move content to top
        scrollRect.verticalNormalizedPosition = 1;
    }

    private void ClearShopContent()
    {
        foreach (RectTransform child in shopContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void EquipRoof()
    {
        RoofsPrefabsContainer roofsPrefabsContainer =
            GameObject.FindGameObjectWithTag("RoofPrefabsContainer").GetComponent<RoofsPrefabsContainer>();
        // Equip roof
        EquipNewRoofEvent?.Invoke(currentlySelectedElement.RoofPrefab);
        // Mark old as unequiped
        RoofPrefabInfo roofPrefabInfo = roofsPrefabsContainer.roofPrefabInfos.Find((roofInfo) =>
        {
            return roofInfo.roofsName == currentlyEquipedElement.RoofPrefab.roofsName;
        });
        roofPrefabInfo.isEquiped = false;
        currentlyEquipedElement.MarkAsUnequiped();
        ////////////////////////
        ///// Mark as equiped
        currentlySelectedElement.MarkAsEquiped();
        currentlyEquipedElement = currentlySelectedElement;
        roofPrefabInfo = roofsPrefabsContainer.roofPrefabInfos.Find((roofInfo) =>
        {
            return roofInfo.roofsName == currentlyEquipedElement.RoofPrefab.roofsName;
        });
        roofPrefabInfo.isEquiped = true;
        // Synchronize with file storage!!
        FileSynchroRequired?.Invoke();
        AdjustButtons();
        Debug.Log("Equip button clicked");
    }

    public void BuyRoof()
    {
        float price = currentlySelectedElement.RoofPrefab.price;
        if (!fileDataController.SpendCurrencyAmount(price))
        {
            buyButtonAnimator.SetTrigger("Drag");
            // play deny sound
            return;
        }
        RoofsPrefabsContainer roofsPrefabsContainer =
            GameObject.FindGameObjectWithTag("RoofPrefabsContainer").GetComponent<RoofsPrefabsContainer>();
        RoofPrefabInfo roofPrefabInfo = roofsPrefabsContainer.roofPrefabInfos.Find((roofInfo) =>
        {
            return roofInfo.roofsName == currentlySelectedElement.RoofPrefab.roofsName;
        });
        roofPrefabInfo.isBought = true;
        FileSynchroRequired?.Invoke();
        UICurrencySynchroRequired?.Invoke();
        // play accept sound
        // May be some change some UI after buying
        AdjustButtons();
        Debug.Log("Buy button clicked");
    }
}
