using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavingCustomPony : MonoBehaviour {

    public ColorButton colButScript;
    public ButValue CMval;
    public ButValue frontHairVal;
    public ButValue backHairVal;
    public ButValue tailVal;
    public Image hairIm_1;
    public Image hairIm_2;
    public Image hairIm_3;
    public Image magic;
    public Image body;
    public Image eyes;
    public SkinnedMeshRenderer smr;
    public InputField Name_Input;

    private void Start()
    {
        Undo();
        SaveLoad.Load2();
        SaveLoad.Load3();
    }

    public void TaskOnClick()
    {
        StaticInfo.datScript.prop.CM = CMval.value;
        StaticInfo.datScript.prop.frontHair = frontHairVal.value;
        StaticInfo.datScript.prop.backHair = backHairVal.value;
        StaticInfo.datScript.prop.tail = tailVal.value;
        StaticInfo.datScript.prop.hairColor_1 = hairIm_1.color;
        StaticInfo.datScript.prop.hairColor_2 = hairIm_2.color;
        StaticInfo.datScript.prop.hairColor_3 = hairIm_3.color;
        StaticInfo.datScript.prop.magicColor = magic.color;
        StaticInfo.datScript.prop.bodyColor = body.color;
        StaticInfo.datScript.prop.eyesColor = eyes.color;

        if (smr.GetBlendShapeWeight(46) > 50)
            StaticInfo.datScript.prop.ifMale = true;
        else
            StaticInfo.datScript.prop.ifMale = false;

        StaticInfo.datScript.prop.Name = Name_Input.text;

        StaticInfo.datScript.FromNormalToSerialized();
        SaveLoad.Save();

        //Debug.Log(SaveLoad.savedGames.bodyColor);
    }

    public void Undo()
    {
        SaveLoad.Load();
        StaticInfo.datScript.FromSerializedToNormal();

        colButScript.but = null;

        CMval.value = StaticInfo.datScript.prop.CM;
        frontHairVal.value = StaticInfo.datScript.prop.frontHair;
        backHairVal.value = StaticInfo.datScript.prop.backHair;
        tailVal.value = StaticInfo.datScript.prop.tail;
        hairIm_1.color = StaticInfo.datScript.prop.hairColor_1;
        hairIm_2.color = StaticInfo.datScript.prop.hairColor_2;
        hairIm_3.color = StaticInfo.datScript.prop.hairColor_3;
        magic.color = StaticInfo.datScript.prop.magicColor;
        body.color = StaticInfo.datScript.prop.bodyColor;
        eyes.color = StaticInfo.datScript.prop.eyesColor;

        if (StaticInfo.datScript.prop.ifMale == true)
            smr.SetBlendShapeWeight(46, 100);
        else
            smr.SetBlendShapeWeight(46, 0);

        Name_Input.text = StaticInfo.datScript.prop.Name;
    }
}
