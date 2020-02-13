using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMap : MonoBehaviour {
    const int MAP_CHIPSET_BLOCK   = 0x00D800;
    const uint MAP_LOOKUP_START   = 0x1A76C0;
    const uint MAP_LOOKUP_END     = 0x1B01FE;
    public Texture2D chipset;
    public static Material mat;
    byte[] mapTileBytes;
    void Start () {
        //PPU texture data
        RenderDumpBlock4BPP block4BPP = new RenderDumpBlock4BPP();
        block4BPP.Build(Rom.smRom);
        chipset = block4BPP.RenderFullBlock(MAP_CHIPSET_BLOCK);
        
        mat = new Material(Shader.Find("Unlit/MapRender"));
        mat.mainTexture = chipset;

        //CPU placement/meta data
        RenderDumpLinear8BPP linear8BPP = new RenderDumpLinear8BPP();
        linear8BPP.Build(Rom.smRom);
        mapTileBytes = linear8BPP.GetDataBlockSegment(MAP_LOOKUP_START, MAP_LOOKUP_END);

        ConstructMap();
    }
    const int LINE_WIDTH = 32;
	void ConstructMap() {
        for(int x = 0; x < mapTileBytes.Length/2; x ++) {
            new MapTile(
                mapTileBytes[x * 2 + 0],
                mapTileBytes[x * 2 + 1],
                x % LINE_WIDTH, x / LINE_WIDTH);
        }
    }
    class MapTile {
        const byte INVALID_TILE = 0x1F;
        const byte PADDING_TILE = 0xFF;
        const byte MIRROR_X_BIT = 0x40;
        const byte MIRROR_Y_BIT = 0x80;
        bool mirrorX;
        bool mirrorY;
        int index;
        int tileX;
        int tileY;
        const float REC16 = 1.0f / 16.0f;
        public MapTile(byte index, byte meta,int xpos,int ypos) {
            //early out for null tiles
            if (index == INVALID_TILE || index == PADDING_TILE)
                return;
            this.index = index;
            mirrorX = (meta & MIRROR_X_BIT) != 0;
            mirrorY = (meta & MIRROR_Y_BIT) != 0;
            tileX = this.index % 16;
            tileY = this.index / 16;
            BuildQuad(xpos, ypos);
        }
        //TODO try to decouple from untiy more.
        private void BuildQuad(int xpos, int ypos) {
            //Generate the gameobject and place it in the world.
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.name = index+"  "+tileX+","+tileY;
            go.transform.position = new Vector3(xpos, 1-ypos, 0);
            Mesh mesh = go.GetComponent<MeshFilter>().mesh;
            List<Vector2> uvs = new List<Vector2>();
            
            //Setup each vertex attribute with tile data.
            for(int i = 0; i < 4; i++) {
                Vector2 uv = mesh.uv[i];
                if (mirrorX)
                    uv.x = 1 - uv.x;
                if(!mirrorY)
                    uv.y = 1 - uv.y;
                uv *= 0.96f;
                uv.x += 0.02f;
                uv.y += 0.02f;
                uv *= REC16;
                uv.x += tileX * REC16;
                uv.y += tileY * REC16;
                uvs.Add(uv);
            }
            mesh.SetUVs(0, uvs);
            mesh.UploadMeshData(true);
            go.GetComponent<MeshFilter>().mesh = mesh;
            go.GetComponent<MeshRenderer>().material = BuildMap.mat;
        }
    }
}
