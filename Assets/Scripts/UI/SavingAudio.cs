using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavingAudio : MonoBehaviour {

    public Slider general;
    public Slider music;
    public Slider VFX;
    public Toggle shadows;

    private void Start()
    {
        Undo2();
    }

    public void TaskOnClick2()
    {
        StaticInfo.datScript.op.genAudio = general.value;
        StaticInfo.datScript.op.musicAudio = music.value;
        StaticInfo.datScript.op.VFXAudio = VFX.value;
        StaticInfo.datScript.op.realTimeS = shadows.isOn;
        SaveLoad.Save2();
    }

    public void Undo2()
    {
        SaveLoad.Load2();
        general.value = StaticInfo.datScript.op.genAudio;
        music.value = StaticInfo.datScript.op.musicAudio;
        VFX.value = StaticInfo.datScript.op.VFXAudio;
        shadows.isOn = StaticInfo.datScript.op.realTimeS;
    }
}
