using UnityEngine;
using System.Collections.Generic;

public class BulletTable : LowText
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

            BulletInfo bulletInfo = new BulletInfo();

            for (int i = 0; i < subject.Length; ++i)
            {
                switch (subject[i])
                {
                    case "IDX":
                        int.TryParse(val[i], out bulletInfo.IDX);
                        break;
                    case "NAME":
                        bulletInfo.NAME = val[i];
                        break;
                    case "UINAME":
                        bulletInfo.UINAME = val[i];
                        break;
                    case "BULLETTYPE":
                        System.Enum.TryParse(val[i], out bulletInfo.BULLETTYPE);
                        break;
                    case "VALUE":
                        int.TryParse(val[i], out bulletInfo.VALUE);
                        break;
                    case "PREFABPATH":
                        bulletInfo.PREFABPATH = val[i];
                        break;
                    case "IMAGEPATH":
                        bulletInfo.IMAGEPATH = val[i];
                        break;
                    case "LORE":
                        bulletInfo.LORE = val[i];
                        break;
                }
            }
            AddInfo(bulletInfo.IDX, bulletInfo);
        }
    }
}