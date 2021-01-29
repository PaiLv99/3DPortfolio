using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharTable : LowText
{
    protected override void Parse( string text )
    {
        string[] row =text.Split('\n');

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

            CharInfo chInfo = new CharInfo();

            for (int i = 0; i < subject.Length; ++i)
            {
                switch (subject[i])
                {
                    case "IDX":
                        int.TryParse(val[i], out chInfo.IDX);
                        break;
                    case "NAME":
                        chInfo.NAME = val[i];
                        break;
                    case "HEALTH":
                        float.TryParse(val[i], out chInfo.HEALTH);
                        break;
                    case "HUNGRY":
                        float.TryParse(val[i], out chInfo.HUNGRY);
                        break;
                    case "SPEED":
                        float.TryParse(val[i], out chInfo.SPEED);
                        break;
                    case "STARTGUN":
                        chInfo.STARTGUN = val[i];
                        break;
                    case "UINAME":
                        chInfo.UINAME = val[i];
                        break;
                    case "LORE":
                        chInfo.LORE = val[i];
                        break;
                }
            }
            AddInfo(chInfo.IDX, chInfo);
        }
    }
}
