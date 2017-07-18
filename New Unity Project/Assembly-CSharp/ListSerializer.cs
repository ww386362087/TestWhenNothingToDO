﻿using System;
using System.Collections;
using System.Collections.Generic;

[ObjectTypeSerializer(typeof(List<>))]
public class ListSerializer : ICustomizedObjectSerializer
{
    private const string DOM_ATTR_IS_NULL = "isNull";
    private const string DOM_NAME_ITEM = "item";

    public bool IsObjectTheSame(object o, object oPrefab)
    {
        if ((o == null) || (oPrefab == null))
        {
            return false;
        }
        IList list = (IList) o;
        IList list2 = (IList) oPrefab;
        if (list.Count != list2.Count)
        {
            return false;
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != list2[i])
            {
                return false;
            }
        }
        return true;
    }

    public void ObjectDeserialize(ref object o, BinaryNode node)
    {
        IList list = (IList) o;
        int childNum = node.GetChildNum();
        for (int i = 0; i < childNum; i++)
        {
            list.Add(GameSerializer.GetObject(node.GetChild(i)));
        }
    }
}

