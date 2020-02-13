using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomExtracter : MonoBehaviour {
    public string dataPath;
    public List<RoomHeader> rooms = new List<RoomHeader>();
	void Start () {
        RenderDumpLinear8BPP rdl = new RenderDumpLinear8BPP();
        rdl.Build(Rom.smRom);
        
        string[] strRoomOffsets = System.IO.File.ReadAllText(Application.dataPath + dataPath).Split('\n');
        uint[] roomOffsets = new uint[strRoomOffsets.Length];
        byte[][] headers = new byte[roomOffsets.Length][];
        
        for(int x = 0; x < roomOffsets.Length; x++){
            string temp = strRoomOffsets[x].Substring(0, 8);
            roomOffsets[x] = System.Convert.ToUInt32(temp, 16);
            headers[x] = rdl.GetDataBlock(roomOffsets[x], 39);
            rooms.Add(new RoomHeader(roomOffsets[x], headers[x]));
        }
        Debug.Log("Complete!");
        BuildRooms();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void BuildRooms() {
        for(int x = 0;x< rooms.Count; x++) {
            //Create object and add RoomHeaderProxy
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.AddComponent<RoomHeaderProxy>().Init(rooms[x]);
        }
    }
}
