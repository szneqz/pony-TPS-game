using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merge : MonoBehaviour {

    private Color bodyColor;
    private Color HairBGColor;
    private Color HairOneColor;
    private Color HairTwoColor;
    private Color eyesColor;
    public Color magicColor;

    private Texture2D cm;
    private Texture2D tail;
    private Texture2D backhair;
    private Texture2D fronthair;
    public Texture2D bodyAndEyes;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    //private bool bot;


    void propblock(MaterialPropertyBlock _propBlock, Renderer _renderer)
    {
        _renderer.GetPropertyBlock(_propBlock);

        _propBlock.SetColor("_Color1", HairBGColor);
        _propBlock.SetColor("_Color2", HairOneColor);
        _propBlock.SetColor("_Color3", HairTwoColor);
        _propBlock.SetColor("_Color4", bodyColor);
        _propBlock.SetColor("_Color5", eyesColor);
        _propBlock.SetTexture("_CMTex", cm);
        _propBlock.SetTexture("_MaskTex_1a", tail);
        _propBlock.SetTexture("_MaskTex_1b", backhair);
        _propBlock.SetTexture("_MaskTex_1c", fronthair);
        _propBlock.SetTexture("_MaskTex_2", bodyAndEyes);

        _renderer.SetPropertyBlock(_propBlock);
    }

    public void MergeThat (Color body, Color HC1, Color HC2, Color HC3, Color eyes, Color magic, int cutie, int tai, int bHair, int fHair)
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();

        //bot = transform.parent.GetComponent<ContrMovem>().ifBot;    //jezeli jest botem to
        //if(bot)
        //{
        //    bodyColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //    HairBGColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //    HairOneColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //    HairTwoColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //    eyesColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //    magicColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        //    cm = StaticInfo.datScript.cmTexList[Random.Range(0, 2)];
        //    tail = StaticInfo.datScript.tailTexList[transform.GetComponent<AddingObjects>().hairRandom[0]];
        //    backhair = StaticInfo.datScript.backHairTexList[transform.GetComponent<AddingObjects>().hairRandom[1]];
        //    fronthair = StaticInfo.datScript.frontHairTexList[transform.GetComponent<AddingObjects>().hairRandom[2]];
        //}
        //else
        //{
            bodyColor = body;
            HairBGColor = HC1;
            HairOneColor = HC2;
            HairTwoColor = HC3;
            eyesColor = eyes;
            magicColor = magic;
            cm = StaticInfo.datScript.cmTexList[cutie];
            tail = StaticInfo.datScript.tailTexList[tai];
            backhair = StaticInfo.datScript.backHairTexList[bHair];
            fronthair = StaticInfo.datScript.frontHairTexList[fHair];

        //bodyColor = StaticInfo.datScript.prop.bodyColor;
        //HairBGColor = StaticInfo.datScript.prop.hairColor_1;
        //HairOneColor = StaticInfo.datScript.prop.hairColor_2;
        //HairTwoColor = StaticInfo.datScript.prop.hairColor_3;
        //eyesColor = StaticInfo.datScript.prop.eyesColor;
        //magicColor = StaticInfo.datScript.prop.magicColor;
        //cm = StaticInfo.datScript.cmTexList[StaticInfo.datScript.prop.CM];
        //tail = StaticInfo.datScript.tailTexList[StaticInfo.datScript.prop.tail];
        //backhair = StaticInfo.datScript.backHairTexList[StaticInfo.datScript.prop.backHair];
        //fronthair = StaticInfo.datScript.frontHairTexList[StaticInfo.datScript.prop.frontHair];
        // }

        //material podstawowy (nie usuwac go)
        //Material mainMaterial = transform.parent.Find("Character_Wings").GetComponent<Renderer>().sharedMaterial;
        //hairbg, one two body eyes
        //cm, tail, backhari, fronthair, body&eyes
        propblock(_propBlock, _renderer);

        _propBlock = new MaterialPropertyBlock();
        _renderer = transform.parent.Find("Character_Wings").GetComponent<Renderer>();

        propblock(_propBlock, _renderer);

        transform.parent.GetComponent<Animator>().Rebind(); //naprawia po calym zabiegu nasza aniamcje
    }
}
