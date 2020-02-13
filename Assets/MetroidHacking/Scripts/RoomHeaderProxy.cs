using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHeaderProxy : MonoBehaviour
{
    static Dictionary<RoomArea, GameObject> areas = new Dictionary<RoomArea, GameObject>();
    public RoomHeader header;
    public void Init(RoomHeader header) {
        this.header = header;
        CalculateProxyGameObject();
    }
    private void CalculateProxyGameObject() {
        //quick and dirty grouping of the rooms into their regions
        if (!areas.ContainsKey(header.roomArea)){
            areas.Add(header.roomArea, new GameObject(header.roomArea.ToString()));
        }
        Vector3 size = new Vector3(header.size.x, header.size.y, 0.1f);
        Vector3 pos = new Vector3(header.pos.x, header.pos.y, 0);
        pos.x += size.x * 0.5f + 0.1f;
        pos.y += size.y * 0.5f + 0.1f;
        pos.y *= -1;
        size.x -= 0.2f;
        size.y -= 0.2f;
        transform.position = pos;
        transform.localScale = size;
        gameObject.name = header.roomIndex + ": " + header.roomArea.ToString() + " - " + header.headerOffset.ToString("X");
        transform.transform.parent = areas[header.roomArea].transform;
    }
}
