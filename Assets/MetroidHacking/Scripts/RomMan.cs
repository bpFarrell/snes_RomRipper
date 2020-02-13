using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class RomMan
{
    private byte[] _romData;
    private bool _hasHeader;
    private const ushort HEADER_SIZE = 0x200;
    private int _headerOffset {
        get { return (_hasHeader ? HEADER_SIZE : 0); }
    }
    public int Length => _romData.Length- _headerOffset;
    public byte this [int i]{
        get {
            return _romData[i+ _headerOffset];
        }
    }
    public RomMan(byte[] data)
    {
        Init(data);
    }
    public RomMan(string path)
    {
        Init(_romData = File.ReadAllBytes(path));
    }
    private void Init(byte[] data)
    {
        CheckForHeader(data);
        _romData = data;
    }
    private void CheckForHeader(byte[] data)
    {
        //TODO Figure out a better way to do a header check
        int nullCount = 0;
        for(int i = 0; i < Math.Min(HEADER_SIZE, data.Length); i++)
        {
            if (data[i] == 0x00) nullCount++;
        }
        if (nullCount > 400)
        {
            _hasHeader = true;
            Debug.Log("header compinsation enabled");
        }
        else
        {
            Debug.Log("No Header on rom");
        }
    }
    public byte[] GetDataBlock(uint start, uint length)
    {
        byte[] block = new byte[length];
        Array.Copy(_romData, start+_headerOffset, block, 0, length);
        return block;
    }

    public byte[] GetDataBlock(int start, int length)
    {
        return GetDataBlock((uint)start, (uint) length);
    }
    public static implicit operator byte[](RomMan rm)
    {
         return rm.GetDataBlock(0, rm.Length);
    }
}