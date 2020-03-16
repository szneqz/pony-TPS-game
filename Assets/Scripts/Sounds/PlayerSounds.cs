using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    public AudioSource source;
    public AudioSource source2;
    private Player2 playerScirpt;
    public List<AudioClip> clipsStep;
    public List<AudioClip> clipsTrot;
    public List<AudioClip> clipsRun;
    public List<AudioClip> clipsWings;
    public bool hstepping = true;
    public bool stepping = true;
    public bool trotting = true;
    public bool running = true;
    public bool flapping = true;


   void Start()
   {
        playerScirpt = GetComponent<Player2>();
   }

    private void Update()
    {
        if (playerScirpt.fly)
            source2.volume = (playerScirpt.actualSpeed / (playerScirpt.runSpeed * 2)) * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
        else
            source2.volume = 0.0f;
    }

    public IEnumerator stepSound()
    {
        if (stepping && hstepping)
        {
            stepping = false;
            source.volume = Random.Range(0.85f, 1.0f) * 0.15f * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
            source.pitch = Random.Range(0.95f, 1.1f);
            source.PlayOneShot(clipsStep[Random.Range(0, clipsStep.Count)]);
            yield return new WaitForSeconds(0.6f);
            source.PlayOneShot(clipsStep[Random.Range(0, clipsStep.Count)]);
            yield return new WaitForSeconds(0.3f);
            stepping = true;
        }
    }

    public IEnumerator trotSound()
    {
        if (trotting)
        {
            trotting = false;
            source.volume = Random.Range(0.9f, 1.0f) * 0.25f * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
            source.pitch = Random.Range(0.95f, 1.0f);
            source.PlayOneShot(clipsTrot[Random.Range(0, clipsTrot.Count)]);
            yield return new WaitForSeconds(0.55f);
            trotting = true;
        }
    }

    public IEnumerator runSound()
    {
        if (running)
        {
            running = false;
            source.volume = Random.Range(0.9f, 1.0f) * 0.25f * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
            source.pitch = Random.Range(0.95f, 1.0f);
            source.PlayOneShot(clipsRun[Random.Range(0, clipsRun.Count)]);
            yield return new WaitForSeconds(0.4f);
            running = true;
        }
    }

    public IEnumerator dropSound()
    {
        source.volume = Random.Range(0.85f, 1.0f) * 0.4f * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
        source.pitch = Random.Range(0.95f, 1.1f) * 0.9f;
        source.PlayOneShot(clipsStep[Random.Range(0, clipsStep.Count)]);
        yield return new WaitForSeconds(0.2f);
        source.PlayOneShot(clipsStep[Random.Range(0, clipsStep.Count)]);
    }

    public IEnumerator horizontalStepSound()
    {
        if (hstepping)
        {
            stepping = false;
            hstepping = false;
            source.volume = Random.Range(0.85f, 1.0f) * 0.1f * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
            source.pitch = Random.Range(0.95f, 1.1f) * 0.9f;
            source.PlayOneShot(clipsStep[Random.Range(0, clipsStep.Count)]);
            yield return new WaitForSeconds(0.4f);
            source.PlayOneShot(clipsStep[Random.Range(0, clipsStep.Count)]);
            yield return new WaitForSeconds(0.3f);
            hstepping = true;
        }
    }

    public IEnumerator wingSound(float time)
    {
        if (flapping)
        {
            flapping = false;
            source.volume = Random.Range(0.9f, 1.0f) * 0.4f * StaticInfo.datScript.op.VFXAudio * StaticInfo.datScript.op.genAudio;
            source.pitch = Random.Range(0.95f, 1.0f) * 1.4f;
            source.PlayOneShot(clipsWings[Random.Range(0, clipsWings.Count)]);
            yield return new WaitForSeconds(time);
            flapping = true;
        }
    }
}
