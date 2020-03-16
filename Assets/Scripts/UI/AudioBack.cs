using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioBack : MonoBehaviour {

    public Slider master;
    public Slider music;
    public Slider VFX;
    public Toggle shadow;

	public void BackToSaved()
    {
        master.value = StaticInfo.datScript.op.genAudio;
        music.value = StaticInfo.datScript.op.musicAudio;
        VFX.value = StaticInfo.datScript.op.VFXAudio;
        shadow.isOn = StaticInfo.datScript.op.realTimeS;
    }
}
