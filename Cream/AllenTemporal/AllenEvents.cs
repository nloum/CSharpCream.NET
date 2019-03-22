using System;

namespace Cream.AllenTemporal
{
    public static class AllenEvents
    {
        public  const int EQUALS       = 0;
        public  const int PRECEDES     = 1;
        public  const int PRECEDEDBY   = 2;
        public  const int DURING       = 3;
        public  const int CONTAINS     = 4;
        public  const int OVERLAPS     = 5;
        public  const int OVERLAPPEDBY = 6;
        public  const int MEETS        = 7;
        public  const int METBY        = 8;
        public  const int STARTS       = 9;
        public  const int STARTEDBY    = 10;
        public  const int FINISHES     = 11;
        public  const int FINISHEDBY   = 12;
        public  static readonly int[] I  = new[] {EQUALS, PRECEDES, PRECEDEDBY, DURING, CONTAINS, OVERLAPS, OVERLAPPEDBY, 
                                                     MEETS, METBY, STARTS, STARTEDBY, FINISHES, FINISHEDBY};

        private static readonly int[] X  = new[] { PRECEDES, OVERLAPS, MEETS };
        private static readonly int[] XInverse = Inverse(X);
        private static readonly int[] Y  = new[] { DURING, OVERLAPS, STARTS };
        private static readonly int[] YInverse = Inverse(Y);
        private static readonly int[] Z  = new[] { DURING, OVERLAPPEDBY, FINISHES };
        private static readonly int[] ZInverse = Inverse(Z);
        private static readonly int[] A  = new[] { EQUALS, FINISHES, FINISHEDBY };
        private static readonly int[] B  = new[] { EQUALS, STARTS, STARTEDBY };
        private static readonly int[] U  = new[] { PRECEDES, OVERLAPS, MEETS, DURING, STARTS };
        private static readonly int[] UInverse = Inverse(U);
        private static readonly int[] V  = new[] { PRECEDES, OVERLAPS, MEETS, CONTAINS, FINISHEDBY };
        private static readonly int[] VInverse = Inverse(V);
        private static readonly int[] N  = new[] { EQUALS, FINISHES, DURING, OVERLAPS, STARTS, FINISHEDBY, 
                                                       CONTAINS, OVERLAPPEDBY, STARTEDBY};

        private static readonly int[] E  = new[] { EQUALS };
        private static readonly int[] P  = new[] { PRECEDES };
        private static readonly int[] PInverse = Inverse(P);
        private static readonly int[] D  = new[] { DURING };
        private static readonly int[] DInverse = Inverse(D);
        private static readonly int[] O  = new[] { OVERLAPS };
        private static readonly int[] OInverse = Inverse(O);
        private static readonly int[] M  = new[] { MEETS };
        private static readonly int[] MInverse = Inverse(M);
        private static readonly int[] S  = new[] { STARTS };
        private static readonly int[] SInverse = Inverse(S);
        private static readonly int[] F  = new[] { FINISHES };
        private static readonly int[] FInverse = Inverse(F);

        public static readonly int[][][] AllenComposition = new[]
                                                                { new[] { E , P , PInverse, D , DInverse, O , OInverse, M , MInverse, S , SInverse, F , FInverse },             // E
              new[] { P , P , I , U , P , P , U , P , U , P , P , U , P  },             // P
              new[] { PInverse, I , PInverse, VInverse, PInverse, VInverse, PInverse, VInverse, PInverse, VInverse, PInverse, PInverse, PInverse },             // PInverse
              new[] { D , P , PInverse, D , I , U , VInverse, P , PInverse, D , VInverse, D , U  },             // D
              new[] { DInverse, V , UInverse, N , DInverse, ZInverse, YInverse, ZInverse, YInverse, ZInverse, DInverse, YInverse, DInverse },             // DInverse
              new[] { O , P , UInverse, Y , V , X , N , P , YInverse, O , ZInverse, Y , X  },             // O
              new[] { OInverse, V , PInverse, Z , UInverse, N , XInverse, ZInverse, PInverse, Z , XInverse, OInverse, YInverse },             // OInverse
              new[] { M , P , UInverse, Y , P , P , Y , P , A , M , M , Y , P  },             // M
              new[] { MInverse, V , PInverse, Z , PInverse, Z , PInverse, B , PInverse, Z , PInverse, MInverse, MInverse },             // MInverse
              new[] { S , P , PInverse, D , V , X , Z , P , MInverse, S , B , D , X  },             // S
              new[] { SInverse, V , PInverse, Z , DInverse, ZInverse, OInverse, ZInverse, MInverse, B , S , OInverse, DInverse },             // SInverse
              new[] { F , P , PInverse, D , UInverse, Y , XInverse, M , PInverse, D , XInverse, F , A  },             // F
              new[] { FInverse, P , UInverse, Y , DInverse, O , YInverse, M , YInverse, O , DInverse, A , FInverse }              // FInverse 
            };

        /// <summary>
        /// Return the inverse interval of a given interval.
        /// <example>Example: Inverse({PrecededBy, Equal, Contains}) = {Precedes, Equal, During}</example>
        /// </summary>
        /// <param name="interval">an integer array represents the interval to be inversed</param>
        /// <returns>an integer array represents the inversed interval</returns>
        public static int[] Inverse(int[] interval)
        {
            var inv = new int[interval.Length];
            for (int i = interval.Length - 1; i >= 0; i--)
            {
                if (interval[i] == EQUALS)
                {
                    inv[i] = 0;
                }
                else if (interval[i] % 2 == 0)
                {
                    inv[i] = interval[i] - 1;
                }
                else
                {
                    inv[i] = interval[i] + 1;
                }
            }
            return inv;
        }

        /// <summary>
        /// Return the complement interval of a given interval.
        /// <example>Example: Complement({Precededby, Equal, Contains}) = 
        ///         {Precedes, During, Starts, Startedby, Finishes, FinishedBy, 
        ///          Overlaps, OverlappedBy, Meets, MetBy}</example>
        /// </summary>
        /// <param name="interval">an integer array represents the interval to be complemented</param>
        /// <returns>an integer array represents the inversed interval</returns>
        public static int[] Complement(int[] interval)
        {
            var inv = new int[13-interval.Length];
            int c = 0;
            for (int j = 0; j < I.Length; j++)
            {
                bool found = false;
                for (int i = interval.Length - 1; i >= 0; i--)
                {
                    if (I[j] == interval[i])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    inv[c++] = I[j];
                }
            }
            return inv;
        }

        public static int[] Union(int[] c1, int[] c2)
        {
            var un = new int[c1.Length + c2.Length];
            int c = 0;
            for (int i = 0; i < c1.Length; i++)
            {
                un[c++] = c1[i];
            }
            for (int i = 0; i < c2.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < c; j++)
                {
                    if (un[j] == c2[i])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    un[c++] = c2[i];
                }
            }

            var x = new int[c];
            for (int i = 0; i < c; i++)
            {
                x[i] = un[i];
            }
            return x;
        }

        public static int[] Intersection(int[] c1, int[] c2)
        {
            var un = new int[c1.Length>c2.Length?c1.Length:c2.Length];
            int c = 0;
            for (int i = 0; i < c1.Length; i++)
            {
                for (int j = 0; j < c2.Length; j++)
                {
                    if (c1[i] == c2[j])
                    {
                        un[c++] = c1[i];
                    }
                }
            }
            var x = new int[c];
            for (int i = 0; i < c; i++)
            {
                x[i] = un[i];
            }
            return x;
        }

        public static bool IsEqual(int[] c1, int[] c2)
        {
            if (c1.Length != c2.Length)
            {
                return false;
            }
            for (int i = 0; i < c1.Length; i++)
            {
                bool found = false;
                for (int j = 0; j < c2.Length; j++)
                {
                    if (c1[i] == c2[j])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }
            return true;
        }

        public static int[] Composition(int[] c1, int[] c2)
        {
            var comp = new int[0];
            for (int i = 0; i < c1.Length; i++)
            {
                for (int j = 0; j < c2.Length; j++)
                {
                    //if (c1[i])
                    //if (((int[])c1[i]).Length > 1)
                    comp = Union(comp, AllenComposition[c1[i]][c2[j]]);
                }
            }
            return comp;
        }

        public static bool Satisfy(int relation, Variable v1, Variable v2)
        {
            switch (relation)
            {
                case PRECEDES:
                    return SatisfyPRECEDES(v1, v2);

                case PRECEDEDBY:
                    return SatisfyPRECEDEDBY(v1, v2);

                case EQUALS:
                    return SatisfyEQUALS(v1, v2);

                case MEETS:
                    return SatisfyMEETS(v1, v2);

                case METBY:
                    return SatisfyMETBY(v1, v2);

                case DURING:
                    return SatisfyDURING(v1, v2);

                case CONTAINS:
                    return SatisfyCONTAINS(v1, v2);

                case STARTS:
                    return SatisfySTARTS(v1, v2);

                case STARTEDBY:
                    return SatisfySTARTEDBY(v1, v2);

                case FINISHES:
                    return SatisfyFINISHES(v1, v2);

                case FINISHEDBY:
                    return SatisfyFINISHEDBY(v1, v2);

                case OVERLAPS:
                    return SatisfyOVERLAPS(v1, v2);

                case OVERLAPPEDBY:
                    return SatisfyOVERLAPPEDBY(v1, v2);
            }
            return false;
        }

        public static bool Satisfy(int relation, int v1, int v2, int duration1, int duration2)
        {
            switch (relation)
            {
                case PRECEDES:
                    return SatisfyPRECEDES(v1, v2, duration1, duration2);

                case PRECEDEDBY:
                    return SatisfyPRECEDEDBY(v1, v2, duration1, duration2);

                case EQUALS:
                    return SatisfyEQUALS(v1, v2, duration1, duration2);

                case MEETS:
                    return SatisfyMEETS(v1, v2, duration1, duration2);

                case METBY:
                    return SatisfyMETBY(v1, v2, duration1, duration2);

                case DURING:
                    return SatisfyDURING(v1, v2, duration1, duration2);

                case CONTAINS:
                    return SatisfyCONTAINS(v1, v2, duration1, duration2);

                case STARTS:
                    return SatisfySTARTS(v1, v2, duration1, duration2);

                case STARTEDBY:
                    return SatisfySTARTEDBY(v1, v2, duration1, duration2);

                case FINISHES:
                    return SatisfyFINISHES(v1, v2, duration1, duration2);

                case FINISHEDBY:
                    return SatisfyFINISHEDBY(v1, v2, duration1, duration2);

                case OVERLAPS:
                    return SatisfyOVERLAPS(v1, v2, duration1, duration2);

                case OVERLAPPEDBY:
                    return SatisfyOVERLAPPEDBY(v1, v2, duration1, duration2);
            }
            return false;
        }

        private static bool SatisfyOVERLAPPEDBY(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if ((st1 > st2) && (lt1 > lt2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyOVERLAPPEDBY(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            int lt2 = st2 + duration2;
            if ((st1 > st2) && (lt1 > lt2))
            {
                return true;
            }
            return false;
        }

        private static bool SatisfyOVERLAPS(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if ((st1 < st2) && (lt1 < lt2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyOVERLAPS(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            int lt2 = st2 + duration2;
            if ((st1 < st2) && (lt1 < lt2))
            {
                return true;
            }
            return false;
        }

        private static bool SatisfyFINISHEDBY(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if ((st1 < st2) && (lt1 == lt2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyFINISHEDBY(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            int lt2 = st2 + duration2;
            if ((st1 < st2) && (lt1 == lt2))
            {
                return true;
            }
            return false;
        }

        private static bool SatisfyFINISHES(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if ((st1 > st2) && (lt1 == lt2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyFINISHES(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            int lt2 = st2 + duration2;
            if ((st1 > st2) && (lt1 == lt2))
            {
                return true;
            }
            return false;
        }

        private static bool SatisfySTARTEDBY(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if ((st1 == st2) && (lt1 > lt2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfySTARTEDBY(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            int lt2 = st2 + duration2;
            if ((st1 == st2) && (lt1 > lt2))
            {
                return true;
            }
            return false;
        }

        private static bool SatisfySTARTS(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if ((st1 == st2) && (lt1 < lt2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfySTARTS(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            int lt2 = st2 + duration2;
            if ((st1 == st2) && (lt1 < lt2))
            {
                return true;
            }
            return false;
        }

        private static bool SatisfyCONTAINS(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if ((st1 < st2) && (lt1 > lt2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyCONTAINS(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            int lt2 = st2 + duration2;
            if ((st1 < st2) && (lt1 > lt2))
            {
                return true;
            }
            return false;
        }

        private static bool SatisfyDURING(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if ((st1 > st2) && (lt1 < lt2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyDURING(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            int lt2 = st2 + duration2;
            if ((st1 > st2) && (lt1 < lt2))
            {
                return true;
            }
            return false;
        }

        private static bool SatisfyMETBY(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if (lt2 == st1)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyMETBY(int st1, int st2, int duration1, int duration2)
        {
            int lt2 = st2 + duration2;
            if (lt2 == st1)
            {
                return true;
            }
            return false;
        }

        private static bool SatisfyMEETS(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    if (lt1 == st2)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyMEETS(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            if (lt1 == st2)
            {
                return true;
            }
            return false;
        }

        private static bool SatisfyEQUALS(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if ((st1 == st2) && (lt1 == lt2))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyEQUALS(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            int lt2 = st2 + duration2;
            if ((st1 == st2) && (lt1 == lt2))
            {
                return true;
            }
            return false;
        }

        private static bool SatisfyPRECEDES(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                int lt1 = st1 + d1.Duration;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    if (lt1 < st2)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyPRECEDES(int st1, int st2, int duration1, int duration2)
        {
            int lt1 = st1 + duration1;
            if (lt1 < st2)
            {
                return true;
            }
            return false;
        }

        private static bool SatisfyPRECEDEDBY(Variable v1, Variable v2)
        {
            var d1 = (AllenDomain)v1.Domain;
            var d2 = (AllenDomain)v2.Domain;
            if (d1.Empty)
            {
                return false;
            }
            if (d2.Empty)
            {
                return false;
            }
            for (int st1 = d1.Min(); st1 <= d1.Max(); st1 += d1.Step)
            {
                bool found = false;
                for (int st2 = d2.Min(); st2 <= d2.Max(); st2 += d2.Step)
                {
                    int lt2 = st2 + d2.Duration;
                    if (lt2 < st1)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ((AllenDomain)(v1.Domain)).Remove(st1);
                    if (v1.Domain.Size() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool SatisfyPRECEDEDBY(int st1, int st2, int duration1, int duration2)
        {
            int lt2 = st2 + duration2;
            if (lt2 < st1)
            {
                return true;
            }
            return false;
        }

        public static String GetString(int index)
        {
            String a = "";
            switch (index)
            {

                case PRECEDES:
                    a = "PRECEDES"; break;

                case PRECEDEDBY:
                    a = "PRECEDEDBY"; break;

                case EQUALS:
                    a = "EQUAL"; break;

                case MEETS:
                    a = "MEETS"; break;

                case METBY:
                    a = "METBY"; break;

                case DURING:
                    a = "DURING"; break;

                case CONTAINS:
                    a = "CONTAINS"; break;

                case STARTS:
                    a = "STARTS"; break;

                case STARTEDBY:
                    a = "STARTEDBY"; break;

                case FINISHES:
                    a = "FINISHES"; break;

                case FINISHEDBY:
                    a = "FINISHEDBY"; break;

                case OVERLAPS:
                    a = "OVERLAP"; break;

                case OVERLAPPEDBY:
                    a = "OVERLAPPEDBY"; break;
            }
            return a ;
        }
    }
}
