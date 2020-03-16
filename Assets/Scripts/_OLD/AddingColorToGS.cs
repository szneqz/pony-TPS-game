using UnityEngine;

public static class AddingColor
{
    public static Texture2D AlphaBlend(this Texture2D aBottom, Color add)
    {
        Color[] bData = aBottom.GetPixels();    //wstawiam info o pixelach do zmiennej
        int count = bData.Length;   //ilosc pixeli
        Color[] rData = new Color[count];   //zmienna koloru do każdego pixela
        for (int i = 0; i < count; i++)     //ustawianie koloru do każdego pixela
        {
            Color B = bData[i];
            Color R = B * (Color.black + add);
            rData[i] = R;
        }
        Texture2D res = new Texture2D(aBottom.width, aBottom.height); //wielkosc tekstury
        res.SetPixels(rData);
        res.Apply();    //akceptacja
        return res;     //zwrot tekstury
    }
}