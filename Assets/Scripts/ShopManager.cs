using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    ShopElement currentlySelectedElement;
    [SerializeField]
    Text buyButtonText;
    [SerializeField]
    RectTransform shopContent;
    [SerializeField]
    GameObject shopElementPrefab;
    [SerializeField]
    GameObject equipButton;
    [SerializeField]
    GameObject buyButton;

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
        adjustButtons();
    }

    public void InitShop()
    {
        FillShopContent();
        adjustButtons();
    }

    public void adjustButtons()
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
        }
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
        Debug.Log("Equip button clicked");
    }

    public void BuyRoof()
    {
        Debug.Log("Buy button clicked");
    }
}
