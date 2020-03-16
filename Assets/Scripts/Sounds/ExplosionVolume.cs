using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVolume : MonoBehaviour {

    private AudioSource source;
    private float startVol;

	void Start ()
    {
        source = GetComponent<AudioSource>();
        startVol = source.volume;
        upSound();
    }

    public void upSound()
    {
        source.volume = startVol * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
    }
}
