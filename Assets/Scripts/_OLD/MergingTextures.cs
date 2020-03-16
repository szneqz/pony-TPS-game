﻿using UnityEngine;

public static class ImageHelpers
{
    public static Texture2D AlphaBlend(this Texture2D aBottom, Texture2D aTop)
    {
        if (aBottom.width != aTop.width || aBottom.height != aTop.height)
            throw new System.InvalidOperationException("AlphaBlend only works with two equal sized images");
        Color[] bData = aBottom.GetPixels();
        Color[] tData = aTop.GetPixels();
        int count = bData.Length;
        Color[] rData = new Color[count];
        for (int i = 0; i < count; i++)
        {
            Color B = bData[i];
            Color T = tData[i];
            float srcF = T.a;
            float destF = 1f - T.a;
            float alpha = srcF + destF * B.a;
            Color R = (T * srcF + B * B.a * destF) / alpha;
            R.a = alpha;
            rData[i] = R;
        }
        Texture2D res = new Texture2D(aTop.width, aTop.height);
        res.SetPixels(rData);
        res.Apply();
        return res;
    }
}
