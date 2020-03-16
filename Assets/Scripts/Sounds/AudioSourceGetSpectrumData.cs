using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioSourceGetSpectrumData : MonoBehaviour {

    public bool pl = false;
    private static List<List<float>> series = new List<List<float>>();
    public AudioSource aud;
    public float dbValue = 0.0f;
    public int whatSync = 6;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.clip = Microphone.Start(null, true, 10, 44100);
        // ';..;' while (!(Microphone.GetPosition(null) > 0)) { } ';..;'  DEMONICZNY WHILE KTÓRY FREEZUJE MI GRE I ZJADL MI ZE 2 GODZINY ROBOTY!
        aud.Play();

        series.Add(new List<float>() { 3.56f, 6.62f, 7.02f, 5.99f, 6.88f, 6.77f, 6.88f, 7.04f, 7.62f, 7.01f, 5.58f, 5.85f, 5.54f, 5.85f, 5.56f, 5.39f, 5.26f, 5.15f, 5.05f, 4.96f, 4.89f});
        series.Add(new List<float>() { 3.48f, 6.46f, 5.41f, 5.31f, 3.26f, 2.2f, 1.63f, 1.23f, 1.04f, 0.82f, 0.72f, 0.61f, 0.34f, 0.29f, 0.23f, 0.09f, 0.05f, 0.02f, 0.04f, -0.07f, -0.22f});
        series.Add(new List<float>() { 9.26f, 8.09f, 8.04f, 7.06f, 7.35f, 4.97f, 6.07f, 5.6f, 5.38f, 5.01f, 4.79f, 4.61f, 4.48f, 4.37f, 4.27f, 4.19f, 4.11f, 4.04f, 3.97f, 3.91f, 3.85f});
        series.Add(new List<float>() { 2.31f, 3.43f, 6.84f, 6.65f, 5.18f, 3.47f, 2.99f, 2.78f, 2.6f, 2.42f, 2.36f, 2.21f, 2.12f, 2.02f, 1.93f, 1.85f, 1.78f, 1.68f, 1.64f, 1.6f, 1.53f});
        series.Add(new List<float>() { 7.67f, 7.45f, 6.62f, 5.09f, 4.97f, 4.31f, 4.02f, 3.82f, 3.67f, 3.55f, 3.44f, 3.35f, 3.26f, 3.19f, 3.11f, 3.05f, 2.99f, 2.92f, 2.85f, 2.81f, 2.76f});
        series.Add(new List<float>() { 5.94f, 7.74f, 7.72f, 5.37f, 2.77f, 2.94f, 2.75f, 2.58f, 2.36f, 2.23f, 2.05f, 2.07f, 1.93f, 1.84f, 1.73f, 1.64f, 1.55f, 1.43f, 1.32f, 1.22f, 1.04f});
    }

    void Volume()
    {
        float[] samples = new float[256];
        aud.GetOutputData(samples, 0); // fill array with samples
        float sum = 0.0f;
        for (int i = 0; i < 256; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        dbValue = Mathf.Clamp(20 * Mathf.Log10(Mathf.Sqrt(sum / 256) / 0.1f) + 70.0f, 0.0f, 80.0f);    //tak sobie sklampowalem
        //Debug.Log(dbValue);
    }

    void Update()
    {
        float[] spectrum = new float[256];
        float avSpec = 0.0f;
        whatSync = 6;   //6 oznacza brak synchornizacji
        float bestDif = 3.0f;

        if(pl)
        {
            aud.Stop();
            aud.clip = null;
            aud.clip = Microphone.Start(null, true, 10, 44100);
            while (!(Microphone.GetPosition(null) > 0)) { }
            aud.Play();
            pl = false;
        }

        aud.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        //for (int i = 1; i < spectrum.Length - 1; i++)
        //{
        //    Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
        //}

        for(int i = 1; i <= 21; i++)
        {
            spectrum[i] = Mathf.Round((Mathf.Log(spectrum[i]) + 10) * 100.0f) / 100.0f;
            avSpec += spectrum[i];
        }

        avSpec = avSpec / 21.0f;

        for (int i = 0; i < 6; i++)
        {
            float avDif = 0.0f;
            // float avSer = 0.0f;
            // for (int j = 0; j < 21; j++)
            //  {
            //      avSer += series[i][j];
            //  }
            //  avSer = avSer / 21.0f;
            for (int k = 0; k < 3; k++) //sprawdzam jeszcze lekko różne wersje
            {
                for (int j = 0; j < 21 - k; j++)
                {
                    avDif += series[i][j] /*- avSer*/ - (spectrum[j + 1 + k] /*- avSpec*/);
                }
                avDif = Mathf.Abs(avDif / 21.0f);

                if (avDif < bestDif)
                {
                    bestDif = Mathf.Round(avDif * 100.0f) / 100.0f;
                    whatSync = i;
                }
            }
        }

        Volume();

        //Debug.Log(whatSync);

        //Debug.Log(Mathf.Round(Mathf.Log(sum / 256) + 10));
        //Debug.Log("Human = " + string.Join("f, ",
        //    new List<float>(spectrum)
        //    .ConvertAll(i => i.ToString())
        //    .ToArray()));
    }
}
