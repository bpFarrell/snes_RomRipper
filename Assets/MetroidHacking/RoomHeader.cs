using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source for header offset information
//http://wiki.metroidconstruction.com/doku.php?id=super:technical_information:room_header_format
public enum RoomArea {
    CRATERIA        = 0x0,
    BRINSTAR        = 0x1,
    NORFAIR         = 0x2,
    WRECKED_SHIP    = 0x3,
    MARIDIA         = 0x4,
    TOURIAN         = 0x5,
    CERES           = 0x6,
    DEBUG           = 0x7
}
public class RoomHeader{
    public uint headerOffset;
    /// <summary>
    /// Zone index this room appears in
    /// </summary>
    public byte roomIndex;
    public RoomArea roomArea;
    /// <summary>
    /// XY of the top left of the room inside region space
    /// </summary>
    public Vector2 pos;
    /// <summary>
    /// Width and height of the bounding box of the room
    /// </summary>
    public Vector2 size;
    public byte upScroll;
    public byte downScroll;
    public byte gfxBitFlag;
    /// <summary>
    /// list of pointers to connecting rooms
    /// </summary>
    public ushort ptrDoorOut;
    public ushort roomstate;
    /// <summary>
    /// Pointer to the layout buffer for the room (not sure if this is ptr to the compressed block, or ptr inside a decompressed bank)
    /// </summary>
    public uint ptrLevelData;
    /// <summary>
    /// tileset offset to be used by level  layout
    /// </summary>
    public byte tileset;
    public byte musicCollection;
    public byte musicPlay;
    /// <summary>
    /// ptr is custom render function used for water, and heat effects.
    /// </summary>
    public ushort ptrFX1;
    public ushort ptrEnemyPop;
    public ushort ptrEnemySet;
    public ushort layer2Scrolling;
    public ushort scrollPointer;
    public ushort unknown;
    public ushort ptrFX2;
    public ushort ptrPLM;
    public ushort bgData;
    public ushort later1_2;
    byte[] raw;
    /// <summary>
    /// Parses raw data into C# object.
    /// </summary>
    /// <param name="headerOffset"></param>
    /// <param name="data"></param>
    public RoomHeader(uint headerOffset,byte[] data) {
        this.headerOffset = headerOffset;
        raw = data;
        roomIndex           = data[0];
        roomArea            = (RoomArea)data[1];
        pos                 = new Vector2(data[2], data[3]);
        size                = new Vector2(data[4], data[5]);
        upScroll            = data[6];
        downScroll          = data[7];
        gfxBitFlag          = data[8];
        ptrDoorOut          = Get2Byte(9, 10);
        roomstate           = Get2Byte(11,12);
        ptrLevelData        = Get3Byte(13, 14, 15);
        tileset             = data[16];
        musicCollection     = data[17];
        musicPlay           = data[18];
        ptrFX1              = Get2Byte(19, 20);
        ptrEnemyPop         = Get2Byte(21, 22);
        ptrEnemySet         = Get2Byte(23, 24);
        layer2Scrolling     = Get2Byte(25, 26);
        scrollPointer               = Get2Byte(27, 28);
        unknown             = Get2Byte(29, 30);
        ptrFX2              = Get2Byte(31, 32);
        ptrPLM              = Get2Byte(33, 34);
        bgData              = Get2Byte(35, 36);
        later1_2            = Get2Byte(37, 38);
    }
    ushort Get2Byte(int index1,int index2) {
        return (ushort)((raw[index1] << 8) | raw[index2]);
    }
    uint Get3Byte(int index1, int index2, int index3) {
        return (uint)((((raw[index1] << 8) | raw[index2]) << 8) | raw[index3]);
    }
    uint Get4Byte(int index1, int index2, int index3, int index4) {
        return (uint)(((((raw[index1] << 8) | raw[index2]) << 8) | raw[index3])<<8 | raw[index3]);
    }
}
