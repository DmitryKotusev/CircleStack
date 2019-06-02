using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofContainer : MonoBehaviour
{
    [SerializeField]
    Transform roofHolder;

    public event System.Action ClipPlayed;

    public void SetRootPrefab(GameObject roofPrefab)
    {
        GameObject roofInstance = Instantiate(roofPrefab, roofHolder.position, roofHolder.rotation);
        if (roofHolder.childCount > 0)
        {
            foreach (Transform child in roofHolder)
            {
                Destroy(child.gameObject);
            }
        }
        roofInstance.transform.parent = roofHolder;
    }

    public void ClipPlayedNotify()
    {
        ClipPlayed?.Invoke();
    }
}
