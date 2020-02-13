using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDumpLinear8BPP : RenderDumpLinear {
    protected override byte[] InternalBuild(byte[] data) {
        byte[] dump = new byte[data.Length];
        data.CopyTo(dump, 0);
        return dump;
    }
}
