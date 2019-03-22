using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cream.AllenTemporal
{
    class TCSPGenerator
    {
	    public static double INV;
	    public static int ACTIVITY_PROB;
	    public static double DISTRIBUTE_DOMAIN;
	    public static int Tightness_Allen;
	    public static int MAX_DURATION;

        public static int MakeCCTSP(int N ,int D ,int C ,int T ,int CN ,int CD ,ref int Seed ,int PP ,int H ,bool Sat)
        {
            // CT means "constraint" 
            int PossibleCTs, PossibleNGs;
            // NG means "nogood pair" 
            IntPtr CTarray;
            int selectedCT, selectedNG;
            int i, j, c, r, t, k, l, civ, nciv, selected;
            int[] Relations;
            int IV, NN, step, PPP;
            IntPtr iv, ivv, Sol;
            int var1, var2, val1, val2;
            //---------------------------------------------
            // Check for valid values of N, D, C, and T. 
            if (N < 2)
            {
                Console.Write("MakeURBCSP: ***Illegal value for N: {0}\n", N);
                return 0;
            }
            if (D < 2)
            {
                Console.Write("MakeURBCSP: ***Illegal value for D: {0}\n", D);
                return 0;
            }
            if (C < 0 || C > (N + CN*CD)*(N + CN*CD - 1)/2)
            {
                Console.Write("MakeURBCSP: ***Illegal value for C: {0}\n", C);
                return 0;
            }
            if (T > D*D)
            {
                Console.Write("MakeURBCSP: ***Illegal value for T(Average Allen Relations/edge): {0}\n", T);
                return 0;
            }
            if (H < 0)
            {
                Console.Write("MakeURBCSP: ***Illegal value for H(Positive Value): {0}\n", T);
                return 0;
            }
            // starting a new sequence of random numbers 
            if (Seed.Deref < 0)
            {
                MakeCCTSP_instance = 0;
            }
            else
            {
                // increment static variable 
                ++(MakeCCTSP_instance);
            }
            //------------------------------------------------------------
            StartCSP(N, D, MakeCCTSP_instance);
            NN = N + CN*CD;
            SOPO VN;
            VN = new SOPO();
            bitset<ALLEN_RELATIONS> RMatrix;
            IntPtr CMatrix;
            CMatrix = ((IntPtr) (stdlib_h.malloc(((uint) (4*NN)))));
            for (i = 0;
                 i < NN;
                 (i)++)
            {
                CMatrix[i] = ((IntPtr) (stdlib_h.malloc(((uint) (4*NN)))));
                for (int j = 0;
                     j < NN;
                     (j)++)
                {
                    CMatrix[i][j] = false;
                }
            }
            for (i = 0;
                 i < NN;
                 (i)++)
            {
                for (int j = 0;
                     j < NN;
                     (j)++)
                {
                    // 13 relations are on
                    RMatrix[i][j].set();
                }
            }
            //----------------------------------------------------
            // set composite variables and initial variables      
            PossibleCTs = N*(N - 1)/2;
            //cout << "PossibleCTs= "<<PossibleCTs<<endl;
            CTarray = ((IntPtr) (stdlib_h.malloc(((uint) (PossibleCTs*4)))));
            PossibleNGs = ALLEN_RELATIONS;
            //NGarray = (unsigned long*) malloc(PossibleNGs * sizeof(unsigned long));
            //----------------------------------------------------
            // set activity constraint                            
            ACT aa;
            ACT a = new ACT();
            int dd, m, p, ii, ss;
            //j=IV;
            ss = (N + CN*CD + CN)*D*(N + CN*CD + CN - 1);
            aa = ((ACT) (stdlib_h.malloc(((uint) (12*ss)))));
            for ( //----------------------------------------------------
                i = 0;
                i < CN;
                (i)++)
            {
                Console.Write("{0}d : ", COMPOSITE_BASE + i);
                for (j = 0;
                     j < CD;
                     (j)++)
                {
                    cout << N + j + i*CD << " ";
                }
                cout << endl;
            }
            //-----------------------------------------------------
            // random initial variables 
            IV = ((int) (INV*(N + CN)));
            iv = new int();
            for (i = 0;
                 i < N;
                 (i)++)
            {
                iv[i] = i;
            }
            for (i = N;
                 i < N + CN;
                 (i)++)
            {
                iv[i] = COMPOSITE_BASE + i - N;
            }
            for (i = 0;
                 i < IV;
                 ++(i))
            {
                r = i + ((int) ((N + CN - i)*ran2(ref Seed)));
                c = iv[r];
                iv[r] = iv[i];
                iv[i] = c;
            }
            Console.Write("\nInitial Variables : {0}\n", IV);
            nciv = 0;
            for (i = 0;
                 i < IV;
                 (i)++)
            {
                Console.Write("   {0}  ", iv[i]);
                if (iv[i] >= COMPOSITE_BASE)
                {
                    (nciv)++;
                }
            }
            cout << endl;
            Console.Write("\nNonactive Variables : {0}\n", N + CN - IV);
            for (i = IV;
                 i < N + CN;
                 (i)++)
            {
                Console.Write("  {0}   ", iv[i]);
            }
            cout << endl;
            civ = IV;
            //int *ivv;
            ivv = new int();
            j = 0;
            for (i = 0;
                 i < N + CN;
                 (i)++)
            {
                if (iv[i] < COMPOSITE_BASE)
                {
                    ivv[(j)++] = iv[i];
                }
                else
                {
                    for (int k = 0;
                         k < CD;
                         (k)++)
                    {
                        ivv[(j)++] = N + (iv[i] - COMPOSITE_BASE)*CD + k;
                    }
                    if (i < IV)
                    {
                        civ += CD - 1;
                    }
                }
            }
            //Random a solution
            //int *Sol;
            //Sol= new int[N+CN*CD];
            Sol = ((IntPtr) (stdlib_h.malloc(((uint) (4*(N + CN*CD))))));
            for (i = 0;
                 i < N + CN*CD;
                 (i)++)
            {
                Sol[i] = ((int) (D*ran2(ref Seed)));
                //cout << i << "=" << Sol[i] << endl;
            }
            i = 0;
            for (var1 = 0;
                 var1 < N - 1;
                 ++(var1))
            {
                for (var2 = var1 + 1;
                     var2 < N;
                     ++(var2))
                {
                    CTarray[(i)++] = var1 << 16 | var2;
                }
            }
            //cout << "Possible " << i << endl;
            //from here
            // Select C constraints. 
            int ICount = 0, NCount = 0, BCount = 0, CICount = 0;
            for (c = 0;
                 c < C;
                 ++(c))
            {
                // Choose a random number between c and PossibleCTs - 1, inclusive. 
                r = c + ((int) (ran2(ref Seed)*(PossibleCTs - c)));
                // Swap elements [c] and [r]. 
                selectedCT = CTarray[r];
                CTarray[r] = CTarray[c];
                CTarray[c] = selectedCT;
                // Broadcast the constraint. 
                CMatrix[((int) (CTarray[c] >> 16))][((int) (CTarray[c] & 0x0000FFFF))] = true;
                CMatrix[((int) (CTarray[c] & 0x0000FFFF))][((int) (CTarray[c] >> 16))] = true;
                for ( //------------------------------------------------------------ 
                    // count remaining numbers of constraints of initial variables 
                    i = 0;
                    i < N + CN*CD;
                    (i)++)
                {
                    if (((int) (CTarray[c] >> 16)) == ivv[i])
                    {
                        k = i;
                    }
                    else if (((int) (CTarray[c] & 0x0000FFFF)) == ivv[i])
                    {
                        l = i;
                    }
                }
                if (k < civ && l < civ)
                {
                    (ICount)++;
                    if (k > N || l > N)
                    {
                        (CICount)++;
                    }
                }
                else if (k > civ && l > civ)
                {
                    (NCount)++;
                }
                else
                {
                    (BCount)++;
                }
            }
            // add composite variables here !!!
            stdlib_h.free(CTarray);
            PPP = PP/CN;
            NN = N + CN*CD - CD;
            // to add to the csp
            PossibleCTs = NN;
            CTarray = new uint();
            PossibleNGs = ALLEN_RELATIONS;
            for (int ii = 0;
                 ii < CN;
                 (ii)++)
            {
                for (j = 0;
                     j < CD;
                     (j)++)
                {
                    k = 0;
                    for (var1 = 0;
                         var1 < NN + CD;
                         (var1)++)
                    {
                        if (var1 < N + ii*CD || var1 >= N + ii*CD + CD)
                        {
                            CTarray[(k)++] = var1 << 16 | N + ii*CD + j;
                        }
                    }
                    for ( //cout <<"k=" <<k<<endl;
                        //if (PPP>PossibleCTs) PPP=PossibleCTs;
                        c = 0;
                        c < PPP;
                        ++(c))
                    {
                        do
                        {
                            r = c + ((int) (ran2(ref Seed)*(PossibleCTs - c)));
                        } while (CMatrix[((int) (CTarray[r] >> 16))][((int) (CTarray[r] & 0x0000FFFF))] != IntPtr.Zero);
                        // Swap elements [c] and [r]. 
                        selectedCT = CTarray[r];
                        CTarray[r] = CTarray[c];
                        CTarray[c] = selectedCT;
                        CMatrix[((int) (CTarray[c] >> 16))][((int) (CTarray[c] & 0x0000FFFF))] = true;
                        CMatrix[((int) (CTarray[c] & 0x0000FFFF))][((int) (CTarray[c] >> 16))] = true;
                        //cout << CMatrix[(int)(CTarray[c] >> 16)][(int)(CTarray[c] & 0x0000FFFF)] << endl;
                        //------------------------------------------------------------ 
                        // count remaining numbers of constraints of initial variables 
                        int l;
                        for (i = 0;
                             i < N + CN*CD;
                             (i)++)
                        {
                            if (((int) (CTarray[c] >> 16)) == ivv[i])
                            {
                                k = i;
                            }
                            else if (((int) (CTarray[c] & 0x0000FFFF)) == ivv[i])
                            {
                                l = i;
                            }
                        }
                        if (k < civ && l < civ)
                        {
                            //cout << " ** "<<ICount << " ** ";
                            (ICount)++;
                            if (k > N || l > N)
                            {
                                (CICount)++;
                            }
                        }
                        else if (k > civ && l > civ)
                        {
                            (NCount)++;
                        }
                        else
                        {
                            (BCount)++;
                        }
                    }
                }
            }
            // Random Tightness T !! here
            NN = N + CN*CD;
            for ( //SOPO *VN=new SOPO[NN] ;
                i = 0;
                i < NN;
                (i)++)
            {
                r = MIN_DURATION + ((int) (ran2(ref Seed)*(MAX_DURATION - MIN_DURATION)));
                VN[i].Duration = r;
                step = 1 + ((int) (ran2(ref Seed)*STEP));
                do
                {
                    r = ((int) (ran2(ref Seed)*(H - VN[i].Duration)));
                } while (r + D*step + VN[i].Duration > H);
                VN[i].Begin = r;
                VN[i].End = VN[i].Begin + (D - 1)*step + VN[i].Duration;
                VN[i].Step = step;
                for (j = 0;
                     j < i;
                     (j)++)
                {
                    if (CMatrix[i][j] != IntPtr.Zero)
                    {
                        if (SetRelations(T, D, RMatrix[i][j], VN[i], VN[j], i, j, Sat, Sol))
                        {
                            RMatrix[j][i] = Inverse(RMatrix[i][j]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (j < i)
                {
                    (i)--;
                }
            }
            // print SOPO and Relations
            Console.Write("\nNumeric Constriants \n");
            for (i = 0;
                 i < NN;
                 (i)++)
            {
                Console.Write(
                    "{0}d = [{1}d,{2}d,{3}d,{4}d]\n"
                    , i
                    , VN[i].Begin
                    , VN[i].End
                    , VN[i].Duration
                    , VN[i].Step);
            }
            cout << endl;
            // Random Activity constraints
            int d = (N + CN - IV)*ACTIVITY_PROB;
            Console.Write("Activity Constraints : {0}", d);
            cout << endl;
            for (i = IV;
                 i < N + CN;
                 (i)++)
            {
                ii = 0;
                for (l = 0;
                     l < i;
                     (l)++)
                {
                    if (iv[l] < COMPOSITE_BASE)
                    {
                        for (p = 0;
                             p < D;
                             (p)++)
                        {
                            aa[ii].X = iv[l];
                            aa[ii].A = p;
                            aa[(ii)++].Y = iv[i];
                        }
                    }
                    else
                    {
                        dd = N + (iv[l] - COMPOSITE_BASE)*CD;
                        for (p = dd;
                             p < dd + CD;
                             (p)++)
                        {
                            aa[ii].X = iv[l];
                            aa[ii].A = p;
                            aa[(ii)++].Y = iv[i];
                            for (m = 0;
                                 m < D;
                                 (m)++)
                            {
                                aa[ii].X = p;
                                aa[ii].A = m;
                                aa[(ii)++].Y = iv[i];
                            }
                        }
                    }
                }
                for (c = 0;
                     c < ACTIVITY_PROB;
                     (c)++)
                {
                    r = c + ((int) ((ii - c)*ran2(ref Seed)));
                    a.X = aa[r].X;
                    a.A = aa[r].A;
                    a.Y = aa[r].Y;
                    aa[r].X = aa[c].X;
                    aa[r].A = aa[c].A;
                    aa[r].Y = aa[c].Y;
                    aa[c].X = a.X;
                    aa[c].A = a.A;
                    aa[c].Y = a.Y;
                    Console.Write("{0}d ={1}d -> {2}\n", aa[c].X, aa[c].A, aa[c].Y);
                }
                (j)++;
            }
            aa = null;
            //****************************************************
            //random solution
            Console.Write("\nBinary Constraints :: Disjunctive of ALLEN's Relations\n\n");
            for (i = 0;
                 i < NN;
                 (i)++)
            {
                for (j = 0;
                     j < i;
                     (j)++)
                {
                    if (CMatrix[i][j] != IntPtr.Zero)
                    {
                        Console.Write("{0}d   {1}d : {2} ", i, j, RMatrix[i][j].count());
                        for ( //cout << RMatrix[i][j]<<endl;
                            k = 0;
                            k < ALLEN_RELATIONS;
                            (k)++)
                        {
                            if (RMatrix[i][j].test(k))
                            {
                                PrintRelation(k);
                            }
                        }
                        cout << endl;
                    }
                }
            }
            cout << "\n---------------------------------------------------------------\n";
            cout << "# Constraints of Initial CCCSP Instance       = " << ((int) (ICount - CICount + CICount/CD)) <<
            " (" << (ICount - CICount + CICount/CD)*100/IV*(IV - 1)/2 << "%)" << endl;
            cout << "# Constraints of CSP                          = " << C << " (" << C*100/N*(N - 1)/2 << "%)" << endl;
            cout << "# Constraints of Nonactive Variables          = " << NCount << endl;
            cout << "# Constriants of Init and Non Variables       = " << BCount << endl;
            cout << "# Constriants of CCCSP                        = " << C + PP/CD << " (" <<
            (C + PP/CD)*100/(N + CN)*(N + CN - 1)/2 << "%)" << endl;
            EndCSP();
            VN = null;
            iv = null;
            ivv = null;
            Sol = null;
            CTarray = null;
            for (int i = 0;
                 i < NN;
                 (i)++)
            {
                CMatrix[i] = null;
                RMatrix[i] = null;
            }
            CMatrix = null;
            RMatrix = null;
            return 1;
        }

        public static int Gnerate(string[] argv)
        {
            int argc = argv.Length;
            IntPtr CMatrix;
            bitset<ALLEN_RELATIONS> RMatrix;
            int i, j, r, t;
            // CT means "constraint" 
            int PossibleCTs, PossibleNGs;
            int selectedCT, selectedNG;
            int var1, var2, val1, val2;
            int N, D, C, T, I, CN, CD, H, NN;
            int S;
            bool Sat = false;
            //check number of input parameters
            if (argc < 10)
            {
                //printf("usage: gcctcsp #vars #alpha #r #p #composites #domains #iv #act #horizon seed #instances [-sat]\n");
                Console.Out.WriteLine("=======================================================================================");
                Console.Out.WriteLine("usage:: gcctcsp #Var Alpha(>0.5) #r #p #composite #c-domain #iv #act #horizon seed #instances [-sat]\n\n");
                Console.Out.WriteLine("To save the generated instance into a file, uses the direct command \">\" \n");
                Console.Out.WriteLine("usage:: gcccsp #Vars Alpha(>0.5) #r #p #composites #domains #iv #act #horizon seed #instances [-sat] > FILENAME \n\n");
                Console.Out.WriteLine("----------------------------------------------------------------------------------------\n");
                Console.Out.WriteLine("Var        = the number of simple variables");
                Console.Out.WriteLine("Alpha      = a real number and greater than 0.5 (see Model RB for the full details)");
                Console.Out.WriteLine("r          = a real number and greater than 0 (see Model RB for the full details)");
                Console.Out.WriteLine("p          = the precent of incompatible pairs/each constraint (tightness)");
                Console.Out.WriteLine("Composite  = the number of composite variables");
                Console.Out.WriteLine("c-domain   = the domain size of composite variables");
                Console.Out.WriteLine("iv         = the precent of initial variables (default active variables)");
                Console.Out.WriteLine("act        = the precent of activity constraints");
                Console.Out.WriteLine("horizon    = the maximum time time for the problem");
                Console.Out.WriteLine("seed       = randomly generated seed");
                Console.Out.WriteLine("instance   = the number of instances that want to randomly generate by these given parameters");
                Console.Out.WriteLine("-sat       = an optional parameter to force the generator to generat consistent instances");
                Console.Out.WriteLine("---------------------------------------------------------------------------------------");
                Console.Out.WriteLine("ex  % gcctcsp 20 0.6 0.5 0.5 5 3 0.7 0.01 300 999 1 ");
                Console.Out.WriteLine("ex  % gcctcsp 20 0.6 0.5 0.5 5 3 0.7 0.01 300 999 1 > instance1");
                Console.Out.WriteLine("=======================================================================================");
                return 0;
            }
            //set input parameters
            N = Convert.ToInt32(argv[1]);
            D = (int) (Math.Pow(N, Convert.ToDouble(argv[2])));
            C = (int) (Convert.ToDouble(argv[3])*N*Math.Log(N));
            T = (int) (Convert.ToDouble(argv[4])*Math.Pow(D, 2));
            // use to escape local optimal when generate relations
            Tightness_Allen = Convert.ToDouble(argv[4])*ALLEN_RELATIONS;
            CN = Convert.ToInt32(argv[5]);
            CD = Convert.ToInt32(argv[6]);
            INV = Convert.ToDouble(argv[7]);
            ACTIVITY_PROB = (D*N + CN*CD)*Convert.ToInt32(argv[8]);
            H = Convert.ToInt32(argv[9]);
            MAX_DURATION = H/4;
            S = Convert.ToInt32(argv[10]);
            I = Convert.ToInt32(argv[11]);
            NN = N + CN*CD;
            // SET DISTRIBUTED_DOMAIN
            if (Convert.ToDouble(argv[4]) <= 0.2 && Convert.ToDouble(argv[4]) >= 0.1)
            {
                DISTRIBUTE_DOMAIN = 0.2;
            }
            else if (Convert.ToDouble(argv[4]) <= 0.3 && Convert.ToDouble(argv[4]) >= 0.21)
            {
                DISTRIBUTE_DOMAIN = 0.3;
            }
            else if (Convert.ToDouble(argv[4]) <= 0.4 && Convert.ToDouble(argv[4]) >= 0.31)
            {
                DISTRIBUTE_DOMAIN = 0.4;
            }
            else if (Convert.ToDouble(argv[4]) <= 0.5 && Convert.ToDouble(argv[4]) >= 0.41)
            {
                DISTRIBUTE_DOMAIN = 0.7;
            }
            else if (Convert.ToDouble(argv[4]) <= 0.6 && Convert.ToDouble(argv[4]) >= 0.51)
            {
                DISTRIBUTE_DOMAIN = 0.8;
            }
            else if (Convert.ToDouble(argv[4]) <= 0.7 && Convert.ToDouble(argv[4]) >= 0.61)
            {
                DISTRIBUTE_DOMAIN = 0.8;
            }
            else if (Convert.ToDouble(argv[4]) <= 0.8 && Convert.ToDouble(argv[4]) >= 0.71)
            {
                DISTRIBUTE_DOMAIN = 0.9;
            }
            else if (Convert.ToDouble(argv[4]) <= 0.9 && Convert.ToDouble(argv[4]) >= 0.81)
            {
                DISTRIBUTE_DOMAIN = 0.9;
            }
            else if (Convert.ToDouble(argv[4]) > 0.9)
            {
                DISTRIBUTE_DOMAIN = 0.9;
            }
            //----------------------------------------------------------
            if (argc == 13 && !(argv[12].Equals("-sat")))
            {
                Sat = true;
            }
            Console.Write("Ramdom generated CCTCSP instance parameters : \n");
            Console.Write(" {0}  {1}2f  {2}2f  {3}2f {4}   {5}   {6}2f {7}2f {8}   {9}   {10}\n"
                           , N, Convert.ToDouble(argv[2]), Convert.ToDouble(argv[3]), Convert.ToDouble(argv[4])
                           , Convert.ToDouble(argv[5]), Convert.ToDouble(argv[6]), Convert.ToDouble(argv[7])
                           , Convert.ToDouble(argv[8]), H, S, I);
            Console.Write("#Vars  #Domains #Constraints #Tightness #C-Var #C-Domains #Horizon #Seed #Instances\n");
            Console.Write(" {0}      {1}         {2}           {3}       {4}       {5}        {6}      {7}      {8}\n"
                          , N, D, C, T, CN, CD, H, S, I);
            double aaa = ((float) C)/N*(N - 1)/2;
            int PP = ((int) (Math.Ceiling((N + CN)*(N + CN - 1)/2*aaa))) - C;
            Console.Write("Constraints of composite variables : {0} \n", PP/CN*CD*CN);
            // Seed passed to ran2() must initially be negative. 
            if (S > 0)
            {
                S = -(S);
            }
            for (i = 0;
                 i < I;
                 ++(i))
            {
                if (MakeCCTSP(N, D, C, T, CN, CD, ref S, PP, H, Sat) == 0)
                {
                    return 0;
                }
            }
            return 1;
        }
    }
}

//===========================================
//Translation of code submitted from 24.72.92.62
//Converted from C++ by xlat on 2008Feb11.1952
//C++ parser version:2006Jan.A0

//suppressed time_h
public class stdlib_h
{
	[DllImport("stdlib.h.xml", SetLastError=true)]
	public static extern byte[] malloc(uint arg0);

	[DllImport("stdlib.h.xml", SetLastError=true)]
	public static extern int atoi(ref char arg0);

	[DllImport("stdlib.h.xml", SetLastError=true)]
	public static extern double atof(ref char arg0);

	[DllImport("stdlib.h.xml", SetLastError=true)]
	public static extern void free(byte[] arg0);

}
public class Globals
{
	[DllImport("DllXXX", SetLastError=true)]
	public static extern void ConstraintTightness(SOPO SOPOs ,int NN ,int DZ);

	///<summary>
	///=================== MAIN PROGRAM ==================================
	///------Globle variables-------
	///</summary>
	public static float INV;
	public static int ACTIVITY_PROB;
	public static float DISTRIBUTE_DOMAIN;
	public static int Tightness_Allen;
	public static int MAX_DURATION;
	///<summary>
	///-----------------------------
	///Rule R189: 'main' changed into 'Main'
	///</summary>
	public static int Main(string[] argv)
	{
		int argc = argv.Length;
		IntPtr CMatrix;
		bitset < ALLEN_RELATIONS > RMatrix;
		int i ,j ,r ,t;
		// CT means "constraint" 
		int PossibleCTs ,PossibleNGs;
		int selectedCT ,selectedNG;
		int var1 ,var2 ,val1 ,val2;
		int N ,D ,C ,T ,I ,CN ,CD ,H ,NN;
		int S;
		bool Sat = false;
		//check number of input parameters
		if(argc < 10)
		{
			//printf("usage: gcctcsp #vars #alpha #r #p #composites #domains #iv #act #horizon seed #instances [-sat]\n");
			cout << "=======================================================================================" << endl;
			cout << "usage:: gcctcsp #Var Alpha(>0.5) #r #p #composite #c-domain #iv #act #horizon seed #instances [-sat]\n\n";
			cout << "To save the generated instance into a file, uses the direct command \">\" \n";
			cout << "usage:: gcccsp #Vars Alpha(>0.5) #r #p #composites #domains #iv #act #horizon seed #instances [-sat] > FILENAME \n\n";
			cout << "----------------------------------------------------------------------------------------\n";
			cout << "Var        = the number of simple variables" << endl;
			cout << "Alpha      = a real number and greater than 0.5 (see Model RB for the full details)" << endl;
			cout << "r          = a real number and greater than 0 (see Model RB for the full details)" << endl;
			cout << "p          = the precent of incompatible pairs/each constraint (tightness)" << endl;
			cout << "Composite  = the number of composite variables" << endl;
			cout << "c-domain   = the domain size of composite variables" << endl;
			cout << "iv         = the precent of initial variables (default active variables)" << endl;
			cout << "act        = the precent of activity constraints" << endl;
			cout << "horizon    = the maximum time time for the problem" << endl;
			cout << "seed       = randomly generated seed" << endl;
			cout << "instance   = the number of instances that want to randomly generate by these given parameters" << endl;
			cout << "-sat       = an optional parameter to force the generator to generat consistent instances" << endl;
			cout << "---------------------------------------------------------------------------------------" << endl;
			cout << "ex  % gcctcsp 20 0.6 0.5 0.5 5 3 0.7 0.01 300 999 1 " << endl;
			cout << "ex  % gcctcsp 20 0.6 0.5 0.5 5 3 0.7 0.01 300 999 1 > instance1" << endl;
			cout << "=======================================================================================" << endl;
			return 0;
		}
		//set input parameters
		N = stdlib_h.atoi(ref argv[1]);
		D = ( ( int ) ( pow(( ( double ) N ) ,stdlib_h.atof(ref argv[2]))) );
		C = ( ( int ) ( stdlib_h.atof(ref argv[3]) * N * log(( ( double ) N ))) );
		T = ( ( int ) ( stdlib_h.atof(ref argv[4]) * pow(( ( double ) D ) ,2)) );
		// use to escape local optimal when generate relations
		Tightness_Allen = ( ( int ) ( stdlib_h.atof(ref argv[4]) * ALLEN_RELATIONS) );
		CN = stdlib_h.atoi(ref argv[5]);
		CD = stdlib_h.atoi(ref argv[6]);
		INV = stdlib_h.atof(ref argv[7]);
		ACTIVITY_PROB = ( ( int ) ((D * N + CN * CD) * stdlib_h.atof(ref argv[8])) );
		H = stdlib_h.atoi(ref argv[9]);
		MAX_DURATION = H / 4;
		S = stdlib_h.atoi(ref argv[10]);
		I = stdlib_h.atoi(ref argv[11]);
		NN = N + CN * CD;
		// SET DISTRIBUTED_DOMAIN
		if(stdlib_h.atof(ref argv[4]) <= 0.2 && stdlib_h.atof(ref argv[4]) >= 0.1)
		{
			DISTRIBUTE_DOMAIN = 0.2;
		}
		else if(stdlib_h.atof(ref argv[4]) <= 0.3 && stdlib_h.atof(ref argv[4]) >= 0.21)
		{
			DISTRIBUTE_DOMAIN = 0.3;
		}
		else if(stdlib_h.atof(ref argv[4]) <= 0.4 && stdlib_h.atof(ref argv[4]) >= 0.31)
		{
			DISTRIBUTE_DOMAIN = 0.4;
		}
		else if(stdlib_h.atof(ref argv[4]) <= 0.5 && stdlib_h.atof(ref argv[4]) >= 0.41)
		{
			DISTRIBUTE_DOMAIN = 0.7;
		}
		else if(stdlib_h.atof(ref argv[4]) <= 0.6 && stdlib_h.atof(ref argv[4]) >= 0.51)
		{
			DISTRIBUTE_DOMAIN = 0.8;
		}
		else if(stdlib_h.atof(ref argv[4]) <= 0.7 && stdlib_h.atof(ref argv[4]) >= 0.61)
		{
			DISTRIBUTE_DOMAIN = 0.8;
		}
		else if(stdlib_h.atof(ref argv[4]) <= 0.8 && stdlib_h.atof(ref argv[4]) >= 0.71)
		{
			DISTRIBUTE_DOMAIN = 0.9;
		}
		else if(stdlib_h.atof(ref argv[4]) <= 0.9 && stdlib_h.atof(ref argv[4]) >= 0.81)
		{
			DISTRIBUTE_DOMAIN = 0.9;
		}
		else if(stdlib_h.atof(ref argv[4]) > 0.9)
		{
			DISTRIBUTE_DOMAIN = 0.9;
		}
		//----------------------------------------------------------
		if(argc == 13 && !(strcmp(argv[12],"-sat")))
		{
			Sat = true;
		}
		Console.Write("Ramdom generated CCTCSP instance parameters : \n");
		Console.Write(
		   " {0}  {1}2f  {2}2f  {3}2f {4}   {5}   {6}2f {7}2f {8}   {9}   {10}\n"
		   ,N
		   ,stdlib_h.atof(ref argv[2])
		   ,stdlib_h.atof(ref argv[3])
		   ,stdlib_h.atof(ref argv[4])
		   ,stdlib_h.atoi(ref argv[5])
		   ,stdlib_h.atoi(ref argv[6])
		   ,stdlib_h.atof(ref argv[7])
		   ,stdlib_h.atof(ref argv[8])
		   ,H
		   ,S
		   ,I);
		Console.Write(
		   "#Vars  #Domains #Constraints #Tightness #C-Var #C-Domains #Horizon #Seed #Instances\n");
		Console.Write(
		   " {0}      {1}         {2}           {3}       {4}       {5}        {6}      {7}      {8}\n"
		   ,N
		   ,D
		   ,C
		   ,T
		   ,CN
		   ,CD
		   ,H
		   ,S
		   ,I);
		double aaa = ( ( float ) C ) / N * (N - 1) / 2;
		int PP = ( ( int ) ( ceil((N + CN) * (N + CN - 1) / 2 * aaa)) ) - C;
		Console.Write("Constraints of composite variables : {0} \n",PP / CN * CD * CN);
		// Seed passed to ran2() must initially be negative. 
		if(S > 0)
		{
			S = -(S);
		}
		for(i = 0;
		i < I;++(i))
		{
			if(MakeCCTSP(N ,D ,C ,T ,CN ,CD ,ref S ,PP ,H ,Sat) == 0)
			{
				return 0;
			}
		}
		return 1;
	}
	public static int MakeCCTSP(int N ,int D ,int C ,int T ,int CN ,int CD ,ref int Seed ,int PP ,int H ,bool Sat)
	{
		// CT means "constraint" 
		int PossibleCTs ,PossibleNGs;
		// NG means "nogood pair" 
		IntPtr CTarray;
		int selectedCT ,selectedNG;
		int i ,j ,c ,r ,t ,k ,l ,civ ,nciv ,selected;
		int[] Relations;
		int IV ,NN ,step ,PPP;
		IntPtr iv ,ivv ,Sol;
		int var1 ,var2 ,val1 ,val2;
		//---------------------------------------------
		// Check for valid values of N, D, C, and T. 
		if(N < 2)
		{
			Console.Write("MakeURBCSP: ***Illegal value for N: {0}\n",N);
			return 0;
		}
		if(D < 2)
		{
			Console.Write("MakeURBCSP: ***Illegal value for D: {0}\n",D);
			return 0;
		}
		if(C < 0 || C > (N + CN * CD) * (N + CN * CD - 1) / 2)
		{
			Console.Write("MakeURBCSP: ***Illegal value for C: {0}\n",C);
			return 0;
		}
		if(T > D * D)
		{
			Console.Write("MakeURBCSP: ***Illegal value for T(Average Allen Relations/edge): {0}\n",T);
			return 0;
		}
		if(H < 0)
		{
			Console.Write("MakeURBCSP: ***Illegal value for H(Positive Value): {0}\n",T);
			return 0;
		}
		// starting a new sequence of random numbers 
		if(Seed.Deref < 0)
		{
			MakeCCTSP_instance = 0;
		}
		else
		{
			// increment static variable 
			++(MakeCCTSP_instance);
		}
		//------------------------------------------------------------
		StartCSP(N ,D ,MakeCCTSP_instance);
		NN = N + CN * CD;
		SOPO VN;
		VN = new SOPO();
		bitset < ALLEN_RELATIONS > RMatrix;
		IntPtr CMatrix;
		CMatrix = ( ( IntPtr ) ( stdlib_h.malloc(( ( uint ) ( 4 * NN ) ))) );
		for(i = 0;
		i < NN;(i)++)
		{
			CMatrix[i ] = ( ( IntPtr ) ( stdlib_h.malloc(( ( uint ) ( 4 * NN ) ))) );
			for(int j = 0;
			j < NN;(j)++)
			{
				CMatrix[i ][j ] = false;
			}
		}
		for(i = 0;
		i < NN;(i)++)
		{
			for(int j = 0;
			j < NN;(j)++)
			{
				// 13 relations are on
				RMatrix[i ][j ].set();
			}
		}
		//----------------------------------------------------
		// set composite variables and initial variables      
		PossibleCTs = N * (N - 1) / 2;
		//cout << "PossibleCTs= "<<PossibleCTs<<endl;
		CTarray = ( ( IntPtr ) ( stdlib_h.malloc(( ( uint ) ( PossibleCTs * 4) ))) );
		PossibleNGs = ALLEN_RELATIONS;
		//NGarray = (unsigned long*) malloc(PossibleNGs * sizeof(unsigned long));
		//----------------------------------------------------
		// set activity constraint                            
		ACT aa;
		ACT a = new ACT();
		int dd ,m ,p ,ii ,ss;
		//j=IV;
		ss = (N + CN * CD + CN) * D * (N + CN * CD + CN - 1);
		aa = ( ( ACT ) ( stdlib_h.malloc(( ( uint ) ( 12 * ss ) ))) );
		for(//----------------------------------------------------
		i = 0;
		i < CN;(i)++)
		{
			Console.Write("{0}d : ",COMPOSITE_BASE + i);
			for(j = 0;
			j < CD;(j)++)
			{
				cout << N + j + i * CD << " ";
			}
			cout << endl;
		}
		//-----------------------------------------------------
		// random initial variables 
		IV = ( ( int ) ( INV * (N + CN)) );
		iv = new int();
		for(i = 0;
		i < N;(i)++)
		{
			iv[i ] = i;
		}
		for(i = N;
		i < N + CN;(i)++)
		{
			iv[i ] = COMPOSITE_BASE + i - N;
		}
		for(i = 0;
		i < IV;++(i))
		{
			r = i + ( ( int ) ((N + CN - i) * ran2(ref Seed)) );
			c = iv[r ];
			iv[r ] = iv[i ];
			iv[i ] = c;
		}
		Console.Write("\nInitial Variables : {0}\n",IV);
		nciv = 0;
		for(i = 0;
		i < IV;(i)++)
		{
			Console.Write("   {0}  ",iv[i ]);
			if(iv[i ] >= COMPOSITE_BASE)
			{
				(nciv)++;
			}
		}
		cout << endl;
		Console.Write("\nNonactive Variables : {0}\n",N + CN - IV);
		for(i = IV;
		i < N + CN;(i)++)
		{
			Console.Write("  {0}   ",iv[i ]);
		}
		cout << endl;
		civ = IV;
		//int *ivv;
		ivv = new int();
		j = 0;
		for(i = 0;
		i < N + CN;(i)++)
		{
			if(iv[i ] < COMPOSITE_BASE)
			{
				ivv[(j)++] = iv[i ];
			}
			else
			{
				for(int k = 0;
				k < CD;(k)++)
				{
					ivv[(j)++] = N + (iv[i ] - COMPOSITE_BASE) * CD + k;
				}
				if(i < IV)
				{
					civ += CD - 1;
				}
			}
		}
		//Random a solution
		//int *Sol;
		//Sol= new int[N+CN*CD];
		Sol = ( ( IntPtr ) ( stdlib_h.malloc(( ( uint ) ( 4 * (N + CN * CD)) ))) );
		for(i = 0;
		i < N + CN * CD;(i)++)
		{
			Sol[i ] = ( ( int ) ( D * ran2(ref Seed)) );
			//cout << i << "=" << Sol[i] << endl;
		}
		i = 0;
		for(var1 = 0;
		var1 < N - 1;++(var1))
		{
			for(var2 = var1 + 1;
			var2 < N;++(var2))
			{
				CTarray[(i)++] = var1 << 16 | var2;
			}
		}
		//cout << "Possible " << i << endl;
		//from here
		// Select C constraints. 
		int ICount = 0,NCount = 0,BCount = 0,CICount = 0;
		for(c = 0;
		c < C;++(c))
		{
			// Choose a random number between c and PossibleCTs - 1, inclusive. 
			r = c + ( ( int ) ( ran2(ref Seed) * (PossibleCTs - c)) );
			// Swap elements [c] and [r]. 
			selectedCT = CTarray[r ];
			CTarray[r ] = CTarray[c ];
			CTarray[c ] = selectedCT;
			// Broadcast the constraint. 
			CMatrix[( ( int ) ( CTarray[c ] >> 16) ) ][( ( int ) ( CTarray[c ] & 0x0000FFFF) ) ] = true;
			CMatrix[( ( int ) ( CTarray[c ] & 0x0000FFFF) ) ][( ( int ) ( CTarray[c ] >> 16) ) ] = true;
			for(//------------------------------------------------------------ 
			// count remaining numbers of constraints of initial variables 
			i = 0;
			i < N + CN * CD;(i)++)
			{
				if(( ( int ) ( CTarray[c ] >> 16) ) == ivv[i ])
				{
					k = i;
				}
				else if(( ( int ) ( CTarray[c ] & 0x0000FFFF) ) == ivv[i ])
				{
					l = i;
				}
			}
			if(k < civ && l < civ)
			{
				(ICount)++;
				if(k > N || l > N)
				{
					(CICount)++;
				}
			}
			else if(k > civ && l > civ)
			{
				(NCount)++;
			}
			else
			{
				(BCount)++;
			}
		}
		// add composite variables here !!!
		stdlib_h.free(CTarray);
		PPP = PP / CN;
		NN = N + CN * CD - CD;
		// to add to the csp
		PossibleCTs = NN;
		CTarray = new uint();
		PossibleNGs = ALLEN_RELATIONS;
		for(int ii = 0;
		ii < CN;(ii)++)
		{
			for(j = 0;
			j < CD;(j)++)
			{
				k = 0;
				for(var1 = 0;
				var1 < NN + CD;(var1)++)
				{
					if(var1 < N + ii * CD || var1 >= N + ii * CD + CD)
					{
						CTarray[(k)++] = var1 << 16 | N + ii * CD + j;
					}
				}
				for(//cout <<"k=" <<k<<endl;
				//if (PPP>PossibleCTs) PPP=PossibleCTs;
				c = 0;
				c < PPP;++(c))
				{
					do{
						r = c + ( ( int ) ( ran2(ref Seed) * (PossibleCTs - c)) );
					}
					while(CMatrix[( ( int ) ( CTarray[r ] >> 16) ) ][( ( int ) ( CTarray[r ] & 0x0000FFFF) ) ] != IntPtr.Zero);
					// Swap elements [c] and [r]. 
					selectedCT = CTarray[r ];
					CTarray[r ] = CTarray[c ];
					CTarray[c ] = selectedCT;
					CMatrix[( ( int ) ( CTarray[c ] >> 16) ) ][( ( int ) ( CTarray[c ] & 0x0000FFFF) ) ] = true;
					CMatrix[( ( int ) ( CTarray[c ] & 0x0000FFFF) ) ][( ( int ) ( CTarray[c ] >> 16) ) ] = true;
					//cout << CMatrix[(int)(CTarray[c] >> 16)][(int)(CTarray[c] & 0x0000FFFF)] << endl;
					//------------------------------------------------------------ 
					// count remaining numbers of constraints of initial variables 
					int l;
					for(i = 0;
					i < N + CN * CD;(i)++)
					{
						if(( ( int ) ( CTarray[c ] >> 16) ) == ivv[i ])
						{
							k = i;
						}
						else if(( ( int ) ( CTarray[c ] & 0x0000FFFF) ) == ivv[i ])
						{
							l = i;
						}
					}
					if(k < civ && l < civ)
					{
						//cout << " ** "<<ICount << " ** ";
						(ICount)++;
						if(k > N || l > N)
						{
							(CICount)++;
						}
					}
					else if(k > civ && l > civ)
					{
						(NCount)++;
					}
					else
					{
						(BCount)++;
					}
				}
			}
		}
		// Random Tightness T !! here
		NN = N + CN * CD;
		for(//SOPO *VN=new SOPO[NN] ;
		i = 0;
		i < NN;(i)++)
		{
			r = MIN_DURATION + ( ( int ) ( ran2(ref Seed) * (MAX_DURATION - MIN_DURATION)) );
			VN[i ].Duration = r;
			step = 1 + ( ( int ) ( ran2(ref Seed) * STEP) );
			do{
				r = ( ( int ) ( ran2(ref Seed) * (H - VN[i ].Duration)) );
			}
			while(r + D * step + VN[i ].Duration > H);
			VN[i ].Begin = r;
			VN[i ].End = VN[i ].Begin + (D - 1) * step + VN[i ].Duration;
			VN[i ].Step = step;
			for(j = 0;
			j < i;(j)++)
			{
				if(CMatrix[i ][j ] != IntPtr.Zero)
				{
					if(SetRelations(T ,D ,RMatrix[i ][j ],VN[i ],VN[j ],i ,j ,Sat ,Sol))
					{
						RMatrix[j ][i ] = Inverse(RMatrix[i ][j ]);
					}
					else
					{
						break;
					}
				}
			}
			if(j < i)
			{
				(i)--;
			}
		}
		// print SOPO and Relations
		Console.Write("\nNumeric Constriants \n");
		for(i = 0;
		i < NN;(i)++)
		{
			Console.Write(
			   "{0}d = [{1}d,{2}d,{3}d,{4}d]\n"
			   ,i
			   ,VN[i ].Begin
			   ,VN[i ].End
			   ,VN[i ].Duration
			   ,VN[i ].Step);
		}
		cout << endl;
		// Random Activity constraints
		int d =(N + CN - IV) * ACTIVITY_PROB;
		Console.Write("Activity Constraints : {0}",d);
		cout << endl;
		for(i = IV;
		i < N + CN;(i)++)
		{
			ii = 0;
			for(l = 0;
			l < i;(l)++)
			{
				if(iv[l ] < COMPOSITE_BASE)
				{
					for(p = 0;
					p < D;(p)++)
					{
						aa[ii ].X = iv[l ];
						aa[ii ].A = p;
						aa[(ii)++].Y = iv[i ];
					}
				}
				else
				{
					dd = N + (iv[l ] - COMPOSITE_BASE) * CD;
					for(p = dd;
					p < dd + CD;(p)++)
					{
						aa[ii ].X = iv[l ];
						aa[ii ].A = p;
						aa[(ii)++].Y = iv[i ];
						for(m = 0;
						m < D;(m)++)
						{
							aa[ii ].X = p;
							aa[ii ].A = m;
							aa[(ii)++].Y = iv[i ];
						}
					}
				}
			}
			for(c = 0;
			c < ACTIVITY_PROB;(c)++)
			{
				r = c + ( ( int ) ((ii - c) * ran2(ref Seed)) );
				a.X = aa[r ].X;
				a.A = aa[r ].A;
				a.Y = aa[r ].Y;
				aa[r ].X = aa[c ].X;
				aa[r ].A = aa[c ].A;
				aa[r ].Y = aa[c ].Y;
				aa[c ].X = a.X;
				aa[c ].A = a.A;
				aa[c ].Y = a.Y;
				Console.Write("{0}d ={1}d -> {2}\n",aa[c ].X ,aa[c ].A ,aa[c ].Y);
			}
			(j)++;
		}
		aa = null;
		//****************************************************
		//random solution
		Console.Write("\nBinary Constraints :: Disjunctive of ALLEN's Relations\n\n");
		for(i = 0;
		i < NN;(i)++)
		{
			for(j = 0;
			j < i;(j)++)
			{
				if(CMatrix[i ][j ] != IntPtr.Zero)
				{
					Console.Write("{0}d   {1}d : {2} ",i ,j ,RMatrix[i ][j ].count());
					for(//cout << RMatrix[i][j]<<endl;
					k = 0;
					k < ALLEN_RELATIONS;(k)++)
					{
						if(RMatrix[i ][j ].test(k))
						{
							PrintRelation(k);
						}
					}
					cout << endl;
				}
			}
		}
		cout << "\n---------------------------------------------------------------\n";
		cout << "# Constraints of Initial CCCSP Instance       = " << ( ( int ) ( ICount - CICount + CICount / CD ) ) << " (" << (ICount - CICount + CICount / CD) * 100 / IV * (IV - 1) / 2 << "%)" << endl;
		cout << "# Constraints of CSP                          = " << C << " (" << C * 100 / N * (N - 1) / 2 << "%)" << endl;
		cout << "# Constraints of Nonactive Variables          = " << NCount << endl;
		cout << "# Constriants of Init and Non Variables       = " << BCount << endl;
		cout << "# Constriants of CCCSP                        = " << C + PP / CD << " (" << (C + PP / CD) * 100 / (N + CN) * (N + CN - 1) / 2 << "%)" << endl;
		EndCSP();
		VN = null;
		iv = null;
		ivv = null;
		Sol = null;
		CTarray = null;
		for(int i = 0;
		i < NN;(i)++)
		{
			CMatrix[i ] = null;
			RMatrix[i ] = null;
		}
		CMatrix = null;
		RMatrix = null;
		return 1;
	}
	///<summary>
	///------------------------FUNCTIONS-----------------------------
	///</summary>
	public static void StartCSP(int N ,int D ,int instance)
	{
		Console.Write("\nInstance {0}\n",instance);
	}
	public static void AddConstraint(int var1 ,int var2)
	{
		Console.Write("\n{0}d {1}d : ",var1 ,var2);
	}
	public static void AddNogood(int val1 ,int val2)
	{
		Console.Write(" {0} {1} ",val1 ,val2);
	}
	public static void EndCSP()
	{
		Console.Write("\n");
	}
	public static void PrintRelation(int R)
	{
		switch(R)
		{
		case P_R:	cout << "P";
			break;
		case p_R:	cout << "p";
			break;
		case S_R:	cout << "S";
			break;
		case s_R:	cout << "s";
			break;
		case F_R:	cout << "F";
			break;
		case f_R:	cout << "f";
			break;
		case M_R:	cout << "M";
			break;
		case m_R:	cout << "m";
			break;
		case O_R:	cout << "O";
			break;
		case o_R:	cout << "o";
			break;
		case D_R:	cout << "D";
			break;
		case d_R:	cout << "d";
			break;
		case E_R:	cout << "E";
			break;
		}
	}
	public static INTERVAL AssociateInterval(int Value ,SOPO Sopo)
	{
		INTERVAL pair = new INTERVAL();
		pair.start = Sopo.Begin + Value * Sopo.Step;
		pair.end = pair.start + Sopo.Duration;
		return pair;
	}
	public static int Relation(INTERVAL X ,INTERVAL Y)
	{
		if(Satisfied(X ,P_R,Y))
		{
			return P_R;
		}
		else if(Satisfied(X ,P_R,Y))
		{
			return P_R;
		}
		else if(Satisfied(X ,p_R,Y))
		{
			return p_R;
		}
		else if(Satisfied(X ,S_R,Y))
		{
			return S_R;
		}
		else if(Satisfied(X ,s_R,Y))
		{
			return s_R;
		}
		else if(Satisfied(X ,F_R,Y))
		{
			return F_R;
		}
		else if(Satisfied(X ,f_R,Y))
		{
			return f_R;
		}
		else if(Satisfied(X ,M_R,Y))
		{
			return M_R;
		}
		else if(Satisfied(X ,m_R,Y))
		{
			return m_R;
		}
		else if(Satisfied(X ,O_R,Y))
		{
			return O_R;
		}
		else if(Satisfied(X ,o_R,Y))
		{
			return o_R;
		}
		else if(Satisfied(X ,D_R,Y))
		{
			return D_R;
		}
		else if(Satisfied(X ,d_R,Y))
		{
			return d_R;
		}
		else if(Satisfied(X ,E_R,Y))
		{
			return E_R;
		}
		//return value: Added during conversion from C++
		return 0;
	}
	public static bool Satisfied(INTERVAL X ,int R ,INTERVAL Y)
	{
		// check if X and Y interval satisfy relation R
		switch(R)
		{
		case P_R:	return X.start < Y.start && X.end < Y.start;
		case p_R:	// p
			return Satisfied(Y ,P_R,X);
		case S_R:	// S
			return X.start == Y.start && X.end < Y.end;
		case s_R:	// s
			return Satisfied(Y ,S_R,X);
		case M_R:	// M
			return X.end == Y.start;
		case m_R:	// m
			return Satisfied(Y ,M_R,X);
		case O_R:	// O
			return X.start < Y.start && X.end > Y.start && X.end < Y.end;
		case o_R:	// o
			return Satisfied(Y ,O_R,X);
		case D_R:	// D
			return X.start > Y.start && X.end < Y.end;
		case d_R:	// d
			return Satisfied(Y ,D_R,X);
		case F_R:	// F
			return X.start > Y.start && X.end == Y.end;
		case f_R:	// f
			return Satisfied(Y ,F_R,X);
		case E_R:	// E
			return X.start == Y.start && Y.end == X.end;
		}
		//return value: Added during conversion from C++
		return 0;
	}
	///<summary>
	/// Case 1: only p can be hold
	/// Case 2: only P can be hold
	/// Case 3: remove E
	/// Case 4: remove S,F,D
	/// Case 5: remove s,f,d
	/// remove DdSsFf
	/// Case 6: remove M,P
	/// remove M
	/// remove P
	/// Case 7: remove m,p
	/// remove m
	/// remove p
	/// Case 8: remove S,s,O,d
	/// remove S
	/// remove s
	/// remove O
	/// remove d
	/// Case 9: remove F,f,D
	/// remove F
	/// remove f
	/// remove D
	///  remove S
	/// remove S
	/// Case 12: remove F,f
	/// remove F
	/// remove f
	///</summary>
	public static void[] C = Inverse(C[i ][j ]);
	///<summary>
	///cout << i << "-" << j << " " << C[i][j] << endl;
	///cout << j << "-" << i << " " << C[j][i] << endl;
	/// end if
	///end function
	///</summary>
	public static int d1;
	public static int d2;
	public static int[] Tightness;
	public static INTERVAL X = new INTERVAL();
	public static INTERVAL Y = new INTERVAL();
	public static void printf = { "%3d %3d : "};
	public static void printf = { "=  %3d ", Tightness[k ]};
	public static void CountConstraint(ref SOPO SOPOs ,int NN ,int DZ ,int i ,int j ,ref int Tightness)
	{
		int k;
		int d1 ,d2;
		INTERVAL X = new INTERVAL(),Y = new INTERVAL();
		for(k = 0;
		k < ALLEN_RELATIONS;(k)++)
		{
			Tightness[k ] = 0;
		}
		for(d1 = 0;
		d1 < DZ;(d1)++)
		{
			X = AssociateInterval(d1 ,SOPOs[i ]);
			for(d2 = 0;
			d2 < DZ;(d2)++)
			{
				Y = AssociateInterval(d2 ,SOPOs[j ]);
				(Tightness[Relation(X ,Y)])++;
			}
		}
	}
	public static int d1;
	public static int d2;
	public static int i;
	public static int ccount;
	public static int j;
	public static int[] Tightness;
	public static INTERVAL X = new INTERVAL();
	public static INTERVAL Y = new INTERVAL();
	public static void DD = ( ( IntPtr ) ( stdlib_h.malloc(( ( uint ) ( 4 * (ALLEN_RELATIONS + 1)) ))) );
	public static int avg = ( ( int ) ( ceil(T * TIGHTNESS_RANGE)) );
	public static int ccout;
	public static int yyy = f(0,T + avg ,Tightness);
	public static int e;
	public static void Yes = f(nn + 1,yy - Tightness[nn ],Tightness) + Tightness[nn ];
	public static void Max = No;
	public static void YN = false;
	///<summary>
	///RMatrix.reset(nn);
	///return No;
	///</summary>
	public static void Max = Yes;
	public static void YN = true;
	///<summary>
	/// reset Tightness;
	///</summary>
	public static int max(int X ,int Y)
	{
		if(X >= Y)
		{
			return X;
		}
		else
		{
			return Y;
		}
		//return value: Added during conversion from C++
		return 0;
	}
	///<summary>
	///********************************************************************   3. This random number generator is from William H. Press, et al.,      _Numerical Recipes in C_, Second Ed. with corrections (1994),       p. 282.  This excellent book is available through the      WWW at http://nr.harvard.edu/nr/bookc.html.      The specific section concerning ran2, Section 7.1, is in      http://cfatab.harvard.edu/nr/bookc/c7-1.ps ********************************************************************
	/// ran2() - Return a random floating point value between 0.0 and    1.0 exclusive.  If idum is negative, a new series starts (and    idum is made positive so that subsequent calls using an unchanged    idum will continue in the same sequence). 
	///</summary>
	public static float ran2(ref int idum)
	{
		int j;
		int k;
		float temp;
		// initialize 
		if(idum.Deref <= 0)
		{
			// prevent idum == 0 
			if(-(idum.Deref) < 1)
			{
				idum.Deref = 1;
			}
			else
			{
				// make idum positive 
				idum.Deref = -(idum.Deref);
			}
			ran2_idum2 = idum.Deref;
			for(j = NTAB + 7;
			j >= 0;(j)--)
			{
				// load the shuffle table  
				k = idum.Deref / IQ1;
				idum.Deref = IA1 * (idum.Deref - k * IQ1) - k * IR1;
				if(idum.Deref < 0)
				{
					idum.Deref += IM1;
				}
				if(j < NTAB)
				{
					ran2_iv[j ] = idum.Deref;
				}
			}
			ran2_iy = ran2_iv[0];
		}
		k = idum.Deref / IQ1;
		idum.Deref = IA1 * (idum.Deref - k * IQ1) - k * IR1;
		if(idum.Deref < 0)
		{
			idum.Deref += IM1;
		}
		k = ran2_idum2 / IQ2;
		ran2_idum2 = IA2 * (ran2_idum2 - k * IQ2) - k * IR2;
		if(ran2_idum2 < 0)
		{
			ran2_idum2 += IM2;
		}
		j = ran2_iy / (1 + (IM1 - 1) / NTAB);
		ran2_iy = ran2_iv[j ] - ran2_idum2;
		ran2_iv[j ] = idum.Deref;
		if(ran2_iy < 1)
		{
			ran2_iy += IM1 - 1;
		}
		if((temp = 1.0 / IM1 * ran2_iy) > 1.0 - EPS)
		{
			// avoid endpoint 
			return 1.0 - EPS;
		}
		else
		{
			return temp;
		}
		//return value: Added during conversion from C++
		return 0;
	}
	///<summary>
	///originally, static variable 'instance' in function
	///</summary>
	public static int Main_instance;
	///<summary>
	///originally, static variable 'instance' in function
	///</summary>
	public static int MakeCCTSP_instance;
	///<summary>
	///originally, static variable 'idum2' in function
	///</summary>
	public static int ran2_idum2 = 123456789;
	///<summary>
	///originally, static variable 'iy' in function
	///</summary>
	public static int ran2_iy = 0;
	///<summary>
	///originally, static variable 'iv' in function
	///</summary>
	public static int[] ran2_iv;
}
///<summary>
///--------------------------------------------------------------------
///</summary>
public struct ACT
{
	public int X;
	public int A;
	public int Y;
}
public struct SOPO
{
	public int Begin;
	public int End;
	public int Duration;
	public int Step;
}
public struct INTERVAL
{
	public int start;
	public int end;
}

