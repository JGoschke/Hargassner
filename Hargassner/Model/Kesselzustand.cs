using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hargassner.Model
{
    public class KesselzustandMap : Dictionary<int, string>
    {
        public KesselzustandMap()
        {
            this.Add(0, " 0 Heizung aus");
            this.Add(1, " 1 in Kuerze Pellets nachfuellen");
            this.Add(2, " 2 Pellets nachfuellen");
            this.Add(3, " 3 Rost Zu-Check");
            this.Add(4, " 4 Kessel Start");
            this.Add(5, " 5");
            this.Add(6, " 6 Zuendueberwachung");
            this.Add(7, " 7 Zuendung");
            this.Add(8, " 8");
            this.Add(9, " 9 Leistungsbrand");
            this.Add(10, "10 Gluterhaltung");
            this.Add(11, "11 Entaschung einleiten");
            this.Add(12, "12 Entaschung warten");
            this.Add(13, "13 Entaschung");
            this.Add(14, "14");
            this.Add(15, "15 Reinigung Start");
            this.Add(16, "16 Reinigung");
            this.Add(17, "17 Renigung Ende");
        }

    }
}
