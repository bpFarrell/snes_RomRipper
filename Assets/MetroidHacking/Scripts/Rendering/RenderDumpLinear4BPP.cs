using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDumpLinear4BPP : RenderDumpLinear {
    protected override byte[] InternalBuild(byte[] data) {
        byte[] dump = new byte[data.Length * 2];
        for (int x = 0; x < data.Length; x++) {
            dump[x * 2 + 0] = (byte)((highbit & data[x]) >> 4);
            dump[x * 2 + 1] = (byte)((lowbit  & data[x]) >> 0);
        }
        return dump;
    }
}
