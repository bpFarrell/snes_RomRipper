using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ReadDump : MonoBehaviour {
    public enum DumpRenderType {
        Block4BPP,
        Linear8BPP,
        Linear4BPP
    }
    public DumpRenderType dumpRenderType;
    byte[] dump;
    Material mat;
    Texture2D text;
    const byte lowbit = 0x0f;
    const byte highbit = 0xf0;
    Color[] clrs;
    private int _headSize = 0;
    public int headSize = 0;
    private int _skipSize = 1;
    public int skipSize = 1;
    private int _scroll = 0;
    public int scroll = 0;
    private int _width = 64;
    public int width = 64;
    private int imageWidth = 128;
    private int imageHeigh = 256;
    private RenderDumpBase renderer;
    [HideInInspector]
    // Use this for initialization
    void Start () {
        Init();
        SetRenderer(dumpRenderType, Rom.smRom);
        UpdatePreview();
    }
    void Init() {
        mat = GetComponent<MeshRenderer>().material;

        text = new Texture2D(imageWidth, imageHeigh, TextureFormat.RGBAFloat, false);
        text.filterMode = FilterMode.Point;
        text.anisoLevel = 0;
        mat.mainTexture = text;
        mat.SetVector("_Size", new Vector4(imageWidth, imageHeigh, 0, 0));
    }
    void SetRenderer(DumpRenderType renderType,byte[] data) {
        switch (renderType) {
            case DumpRenderType.Block4BPP:
                renderer = new RenderDumpBlock4BPP();
                break;
            case DumpRenderType.Linear8BPP:
                renderer = new RenderDumpLinear8BPP();
                break;
            case DumpRenderType.Linear4BPP:
                renderer = new RenderDumpLinear4BPP();
                break;
        }
        renderer.Build(data);
    }
	void Update () {
        if (headSize != _headSize) {
            _headSize = headSize;
            UpdatePreview();
        }
        if (skipSize != _skipSize) {
            _skipSize = skipSize;
            UpdatePreview();
        }
        if (scroll != _scroll) {
            _scroll = scroll;
            UpdatePreview();
        }
        if (width != _width) {
            _width = width;
            UpdatePreview();
        }
	}
    void UpdatePreview() {
        renderer.SetValues(this);
        renderer.Render(text);
    }
}
