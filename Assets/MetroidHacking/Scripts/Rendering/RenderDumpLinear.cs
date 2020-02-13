using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RenderDumpLinear : RenderDumpBase {
    public override void Render(Texture2D text) {
        int index = headSize + (scroll * Mathf.Min(text.width, width) * skipSize);
        for (int y = 0; y < text.height; y++) {
            for (int x = 0; x < text.width; x++) {
                if (index >= data.Length || index < 0) continue;
                if (x >= width) {
                    text.SetPixel(x, y, Color.black);
                    continue;
                }
                //Color clr = clrs[dump[index] % clrs.Length];
                Color clr = Encode2Color(data[index]);
                clr.b = index;
                text.SetPixel(x, y, clr);
                index += skipSize;
            }
        }
        text.Apply();
    }
    /// <summary>
    /// Returns a range of data.
    /// </summary>
    /// <param name="start (inclusive)"></param>
    /// <param name="end (exclusive)"></param>
    /// <returns></returns>
    public byte[] GetDataBlockSegment(uint start,uint end) {
        int length = (int)(end - start);
        return GetDataBlock(start, (uint)length);
    }
    /// <summary>
    /// Returns a range of data.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public byte[] GetDataBlock(uint start,uint length)
    {
        byte[] block = new byte[length];
        Array.Copy(data, start, block, 0, length);
        return block;
    }
}
