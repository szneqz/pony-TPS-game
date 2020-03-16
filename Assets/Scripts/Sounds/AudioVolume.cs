using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour {

    public bool ifMusic;
    private AudioSource source;
    private float startVol;

	void Start ()
    {
        source = GetComponent<AudioSource>();
        startVol = source.volume;
	}
	
	void Update ()
    {
        if(ifMusic)
		    source.volume = startVol * StaticInfo.datScript.op.musicAudio * StaticInfo.datScript.op.genAudio;
        else
            source.volume = startVol * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
    }
}
