using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAndText : MonoBehaviour {

    public Image panelImage;
    public Text panelText;
    [TextArea(3, 10)]
    public string text;
    public Texture2D image;
    public PassDataScript dataScript;
    public int levelToLoad = 0;

    private void Start()
    {
        image = transform.GetComponent<Image>().overrideSprite.texture;
    }

    public void changing()
    {
        panelImage.overrideSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        panelText.text = text;
        dataScript.prop.level = levelToLoad;
    }

}
