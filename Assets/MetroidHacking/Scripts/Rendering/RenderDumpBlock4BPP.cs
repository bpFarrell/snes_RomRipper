using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDumpBlock4BPP : RenderDumpBase {
    byte[] mask = new byte[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
    const int tileByteCount = 8 * 4;
    byte Pack(byte b, byte m) {
        if ((b & m) != 0) {
            return 1;
        }
        return 0;
    }
    /// <summary>
    /// Generates a Unity Texture2D at an offset in memory interpreted as 4 bits per pixel.
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    public Texture2D RenderFullBlock(int offset) {
        Texture2D text = new Texture2D(128, 128, TextureFormat.RGB24, false, false);
        text.filterMode = FilterMode.Point;
        int tileCount = data.Length / tileByteCount;
        int index = offset*64;
        for (int ty = 0; ty < 16; ty++) {
            for (int tx = 0; tx < 16; tx++) {
                for (int y = 0; y < 8; y++) {
                    for (int x = 0; x < 8; x++) {
                        if (data.Length <= index) {
                            Debug.Log("OoR!" + index + "/" + data.Length);
                        }
                        Color clr = Encode2Color(data[index]);
                        //clr.b = index / 64;
                        text.SetPixel(x + tx * 8, y + ty * 8, clr);
                        index++;
                    }
                    if (ty * 8 + y > text.height) continue;
                }
            }
        }
        text.Apply();

        return text;
    } 
    protected override byte[] InternalBuild(byte[] data) {
        
        byte[] dump = new byte[data.Length * 2];
        int index = 0;
        int tileCount = data.Length / tileByteCount;
        for (int t = 0; t < tileCount; t++) {
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {   
                    int yoffset = y * 2;
                    byte temp = 0x00;
                    temp |= Pack(data[yoffset + t * tileByteCount + 17], mask[x]);
                    temp = (byte)(temp << 1);
                    temp |= Pack(data[yoffset + t * tileByteCount + 16], mask[x]);
                    temp = (byte)(temp << 1);
                    temp |= Pack(data[yoffset + t * tileByteCount + 1], mask[x]);
                    temp = (byte)(temp << 1);
                    temp |= Pack(data[yoffset + t * tileByteCount + 0], mask[x]);
                    dump[index] = temp;
                    index++;
                }
            }
        }
        return dump;
    }

    public override void Render(Texture2D text) {
        int tileCount = data.Length / tileByteCount;
        int index = scroll * 16 * tileByteCount * 2;
        for (int ty = 0; ty < 32; ty++) {
            for (int tx = 0; tx < 16; tx++) {
                for (int y = 0; y < 8; y++) {
                    for (int x = 0; x < 8; x++) {
                        if (data.Length <= index) {
                            Debug.Log("OoR!" + index + "/" + data.Length);
                        }
                        Color clr = Encode2Color(data[index]);
                        clr.b = index/64;
                        text.SetPixel(x + tx * 8, y + ty * 8, clr);
                        index++;
                    }
                    if (ty * 8 + y > text.height) continue;
                }
            }
        }
        text.Apply();
    }
}
