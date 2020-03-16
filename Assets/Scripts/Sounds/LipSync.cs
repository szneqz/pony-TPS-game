using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LipSync : MonoBehaviour {

    private SkinnedMeshRenderer characterWings;
    private Transform mainTransform;
    private AudioSourceGetSpectrumData audioScript;
    private float[] shape = {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};

    void Start ()
    {
        mainTransform = transform;
        characterWings = mainTransform.Find("Character_Wings").GetComponent<SkinnedMeshRenderer>();
        audioScript = GetComponent<AudioSourceGetSpectrumData>();
    }
	
	void Update ()
    {
		for(int i = 0; i < shape.Length; i++)
        {
            if (i == 0 || i == 2)
            {
                if (i == audioScript.whatSync)
                    shape[i] += Time.deltaTime * 400.0f;
                else
                    shape[i] -= Time.deltaTime * 200.0f;
            }
            else
            {
                if (i == audioScript.whatSync)
                    shape[i] += Time.deltaTime * 1000.0f;
                else
                    shape[i] -= Time.deltaTime * 500.0f;
            }

            shape[i] = Mathf.Clamp(shape[i], 0.0f, 100.0f);

            characterWings.SetBlendShapeWeight(32 + i, shape[i]);
            characterWings.SetBlendShapeWeight(0, audioScript.dbValue * 0.7f);
        }
	}
}
