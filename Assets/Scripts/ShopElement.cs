using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    private RoofPrefabInfo roofPrefabInfo;

    public RoofPrefabInfo RoofPrefab
    {
        get
        {
            return roofPrefabInfo;
        }
        set
        {
            roofPrefabInfo = value;
            UpdateTexture(roofPrefabInfo.menuImage);
        }
    }
    [SerializeField]
    RawImage image;
    [SerializeField]
    RectTransform imageTransform;
    public delegate void SelectElement(ShopElement shopElement);
    public event SelectElement ThisElementSelect;

    public void UpdateTexture(Texture texture)
    {
        image.texture = texture;
    }

    public void OnTapElement()
    {
        ThisElementSelect?.Invoke(this);
    }

    public void MarkAsSelected()
    {
        imageTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void MarkAsUnselected()
    {
        imageTransform.localScale = new Vector3(1, 1, 1);
    }
}
