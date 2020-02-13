using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RenderDumpBase {
    public byte this[ulong i]{
        get { return 0; }
    }
    protected const byte lowbit = 0x0f;
    protected const byte highbit = 0xf0;
    public int scroll;
    public int headSize;
    public int skipSize;
    public int width;
    public TextMesh tm;
    public byte[] data;
    public void SetValues(ReadDump rd) {
        scroll = rd.scroll;
        headSize = rd.headSize;
        skipSize = rd.skipSize;
        width = rd.width;
    }
    public abstract void Render(Texture2D text);
    /// <summary>
    /// Format the data in the appropriate 'RenderDump' format.
    /// </summary>
    /// <param name="data"></param>
    public void Build(byte[] data) {
        this.data = InternalBuild(data);
    }
    protected abstract byte[] InternalBuild(byte[] data);
    protected Color Encode2Color(byte b) {
        Color clr = Color.black;
        clr.r = ((float)((highbit & b) >> 4) / 16);
        clr.g = ((float)(lowbit & b) / 16);
        return clr;
    }
}
