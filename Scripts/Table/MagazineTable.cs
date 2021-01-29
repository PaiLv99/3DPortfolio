using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineTable : LowText
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

            MagazineInfo magazineInfo = new MagazineInfo();

            for (int i = 0; i < subject.Length; ++i)
            {
                switch (subject[i])
                {
                    case "IDX":
                        int.TryParse(val[i], out magazineInfo.IDX);
                        break;
                    case "NAME":
                        magazineInfo.NAME = val[i];
                        break;
                    case "UINAME":
                        magazineInfo.UINAME = val[i];
                        break;
                }
            }
            AddInfo(magazineInfo.IDX, magazineInfo);
        }
    }
}
