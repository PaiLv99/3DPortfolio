using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTable : LowText
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

            GunInfo gunInfo = new GunInfo();

            for (int i = 0; i < subject.Length; ++i)
            {
                switch (subject[i])
                {
                    case "IDX": int.TryParse(val[i], out gunInfo.IDX); break;
                    case "NAME": gunInfo.NAME = val[i]; break;
                    case "SHOOTSPEED": float.TryParse(val[i], out gunInfo.SHOOTSPEED); break;
                    case "BULLETSPEED": float.TryParse(val[i], out gunInfo.BULLETSPEED); break;
                    case "DAMAGE": float.TryParse(val[i], out gunInfo.DAMAGE); break;
                    case "POWER": float.TryParse(val[i], out gunInfo.POWER); break;
                    case "MAXDISTANCE": float.TryParse(val[i], out gunInfo.MAXDISTANCE); break;
                    case "RELOADCOUNT": int.TryParse(val[i], out gunInfo.RELOADCOUNT); break;
                    case "RELOADTIME": float.TryParse(val[i], out gunInfo.RELOADTIME); break;
                    case "BULLETTYPE": System.Enum.TryParse(val[i], out gunInfo.BULLETTYPE); break;
                    case "DAMAGESOCKET": int.TryParse(val[i], out gunInfo.DAMAGESOCKET); break;
                    case "POWERSOCKET": int.TryParse(val[i], out gunInfo.POWERSOCKET); break;
                    case "SPEEDSOCKET": int.TryParse(val[i], out gunInfo.SPEEDSOCKET); break;
                    case "REMAINAMMOIMAGE": gunInfo.REMAINAMMOIMAGE = val[i]; break;
                    case "REMAINAMMOBG": gunInfo.REMAINAMMOBG = val[i]; break;
                    case "MAGAZINEPREFAB": gunInfo.MAGAZINEPREFAB = val[i]; break;
                    case "MUZZLECOUNT": int.TryParse(val[i], out gunInfo.MUZZLECOUNT); break;
                }
            }
            AddInfo(gunInfo.IDX, gunInfo);
        }
    }
}
