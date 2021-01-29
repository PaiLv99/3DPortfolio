using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesTable : LowText
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

            ZombiesInfo zombiesInfo = new ZombiesInfo();

            for (int i = 0; i < subject.Length; ++i)
            {
                switch (subject[i])
                {
                    case "IDX": int.TryParse(val[i], out zombiesInfo.IDX); break;
                    case "NAME": zombiesInfo.NAME = val[i]; break;
                    case "ENEMYTYPE": System.Enum.TryParse(val[i], out zombiesInfo.ENEMYTYPE); break;
                    case "CHASINGDISTANCE": float.TryParse(val[i], out zombiesInfo.CHASINGDISTANCE); break;
                    case "HP": float.TryParse(val[i], out zombiesInfo.HP); break;
                    case "MOVESPEED": float.TryParse(val[i], out zombiesInfo.MOVESPEED); break;
                    case "ATTACKDAMAGE": int.TryParse(val[i], out zombiesInfo.ATTACKDAMAGE); break;
                    case "EXP": int.TryParse(val[i], out zombiesInfo.EXP); break;
                    case "SHOOTDISTANCE": float.TryParse(val[i], out zombiesInfo.SHOOTDISTANCE); break;
                    case "BULLETDISTANCE": float.TryParse(val[i], out zombiesInfo.BULLETDISTANCE); break;
                    case "SHOOTSPEED": float.TryParse(val[i], out zombiesInfo.SHOOTSPEED); break;
                    case "SHOOTDAMAGE": float.TryParse(val[i], out zombiesInfo.SHOOTDAMAGE); break;
                    case "SHOOTPOWER": float.TryParse(val[i], out zombiesInfo.SHOOTPOWER); break;
                }
            }
            AddInfo(zombiesInfo.IDX, zombiesInfo);
        }
    }
}
