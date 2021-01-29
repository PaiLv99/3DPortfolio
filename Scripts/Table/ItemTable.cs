using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTable : LowText
{
    protected override void Parse(string text)
    {
        string[] row = text.Split('\n');

        List<string> rowlist = new List<string>();

        for (int i = 0; i < row.Length; ++i)
        {
            if (!string.IsNullOrEmpty(row[i]))
            {
                string s = row[i].Replace('\r', ' ');
                rowlist.Add(s.Trim());
            }
        }

        string[] subject = rowlist[0].Split(',');

        for (int j = 1; j < rowlist.Count; ++j)
        {
            string[] val = rowlist[j].Split(',');

            ItemInfo itemInfo = new ItemInfo();

            for (int i = 0; i < subject.Length; ++i)
            {
                switch (subject[i])
                {
                    case "IDX":
                        int.TryParse(val[i], out itemInfo.IDX);
                        break;
                    case "NAME":
                        itemInfo.NAME = val[i];
                        break;
                    case "UINAME":
                        itemInfo.UINAME = val[i];
                        break;
                    case "ITEMTYPE":
                        System.Enum.TryParse(val[i], out itemInfo.ITEMTYPE);
                        break;
                    case "ITEMVALUES":
                        string[] values = val[i].Split('|');
                        itemInfo.ITEMVALUES = new float[values.Length];
                        for (int v = 0; v < values.Length; v++)
                            float.TryParse(values[v], out itemInfo.ITEMVALUES[v]);
                        break;
                    case "ITEMPARTS":
                        string[] parts = val[i].Split('|');
                        itemInfo.ITEMPARTS = parts;
                        break;
                    case "COST":
                        int.TryParse(val[i], out itemInfo.COST);
                        break;
                    case "PREFABPATH":
                        itemInfo.PREFABPATH = val[i];
                        break;
                    case "IMAGEPATH":
                        itemInfo.IMAGEPATH = val[i];
                        break;
                    case "LORE":
                        itemInfo.LORE = val[i];
                        break;
                }
            }
            AddInfo(itemInfo.IDX, itemInfo);
        }
    }
}