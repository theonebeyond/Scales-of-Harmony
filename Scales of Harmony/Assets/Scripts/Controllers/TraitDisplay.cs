using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TraitDisplay : MonoBehaviour
{
    public UnityEngine.Object[] images;
    private int index = 0;
    // Start is called before the first frame update
    public void pushImage(Blessing data)
    {
        Image imageComponent = images[index].GetComponent<Image>() as Image;
        imageComponent.sprite = data.blessingSprite;
        index++;
    }
}
