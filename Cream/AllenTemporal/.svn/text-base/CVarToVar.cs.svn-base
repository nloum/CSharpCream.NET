using System;
using System.Collections;

namespace Cream.AllenTemporal
{
    public class CVarToVar
    {
        private int cnstrt;
        private AllenVariable var1 = null, var2 = null;

        public int ConstraintIndex
        {
            get
            {
                return cnstrt;
            }
            set
            {
                cnstrt = value;
            }
        }

        public AllenVariable Var1
        {
            get
            {
                return var1;
            }
            set
            {
                var1 = value;
            }
        }

        public AllenVariable Var2
        {
            get
            {
                return var2;
            }
            set
            {
                var2 = value;
            }
        }

        public override String ToString()
        {
            if ((Var1 != null) && (Var2 != null))
            {
                return Var1 + " " + AllenEvents.GetString(ConstraintIndex) + " " + Var2;
            }
            else
            {
                return null;
            }
        }
    }

    public static class NetWorkConstraints
    {
        public static int[][][] getConstraintNetwork(Network net)
        {
            int[][][] vNetowrk = new int[net.Variables.Count][][];
            IEnumerator vFrom = net.Variables.GetEnumerator();
            while (vFrom.MoveNext())
            {
                IEnumerator vTo = net.Variables.GetEnumerator();
                vNetowrk[((Variable)vFrom.Current).Index] = new int[net.Variables.Count][];
                while (vTo.MoveNext())
                {
                    if (vTo.Current != vFrom.Current)
                    {
                        vNetowrk[((Variable)vFrom.Current).Index][((Variable)vTo.Current).Index] = new int[14];
                        vNetowrk[((Variable)vFrom.Current).Index][((Variable)vTo.Current).Index]= 
                                  getConstraints(vFrom.Current as Variable, vTo.Current as Variable);
                    }
                }
            }
            return vNetowrk;
        }

        private static int[] getConstraints(Variable v1, Variable v2)
        {
            Network network = v1.Network;
            IEnumerator cs = network.Constraints.GetEnumerator();
            int[] cons = new int[14];
            bool hasbeenConstrainted = false;
            while (cs.MoveNext())
            {
                Constraint c = (Constraint)cs.Current;
                if (c is AllenConstraint)
                {
                    AllenConstraint c1 = (AllenConstraint)c;
                    if ((c1.Vars[0] == v1) && (c1.Vars[1] == v2))
                    {
                        cons[c1.AllenEvent] = 1;
                        hasbeenConstrainted = true;
                    }
                }
            }
            if (!hasbeenConstrainted)
            {
                for (int i = 0; i < 14; i++)
                {
                    cons[i] = 1;
                }
            }
            return cons;
        }
    }
   
}
