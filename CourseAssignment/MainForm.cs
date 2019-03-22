using System;
using System.IO;
using System.Windows.Forms;
using Cream.CourseAssignment;
using Cream;
using CourseAssignment;
using CourseAssignmentClassLibrary;
using System.Collections;


namespace CourseAssignment
{
    public partial class ca : Form
    {
        private BindingSource profBS;
        private BindingSource courseBS;
        private BindingSource solutionBS;
        private int solIndex = 1;
        private int numberOfSolutions;
        private int err;
        private IList[] Notes;
        public ca()
        {
            InitializeComponent();
            refreshProfs_Click();
            if (err == 0)
            refreshCourses_Click(null, null);
            else
            {
                
                button1.Enabled = false;
            }
        }

        #region Text file

        private void button1_Click()
        {
            var net = new CourseNetwork();
            string fileName = SelectTextFile(".");
            if (fileName != null)
            {
                //----------------------
                // Prof reading 
                StreamReader re = File.OpenText(fileName);
                string input = re.ReadLine();
                int n;
                try
                {
                    n = Convert.ToInt16(input);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to read no of prof ");
                    return;
                }
                var professors = new Professor[n];
                for (int i = 0; i < n; i++)
                {
                    input = re.ReadLine();
                    int m;
                    string[] s = input.Split(';');

                    try
                    {
                        m = Convert.ToInt16(s[1]); // no of events
                    }
                    catch
                    {
                        Console.WriteLine("Failed to read no of courses per prof " + s[0]);
                        return;
                    }
                    professors[i] = new Professor(net, m, s[0]);
                }
                //-----------------------
                // course reading
                input = re.ReadLine();
                try
                {
                    n = Convert.ToInt16(input);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to read no of courses ");
                    return;
                }
                var course = new IntVariable[n];
                int noOfCourses = n;
                var TimeSlots = new int[n][];
                var DOW = new string[n];
                int max = net.Professors.Count - 1;
                for (int i = 0; i < n; i++)
                {
                    input = re.ReadLine();
                    string[] s = input.Split(' ');
                    string cName = s[0]; // no of events
                    course[i] = new IntVariable(net, 0, max, cName);
                    TimeSlots[i] = ParseTimeSlot(s[1]);
                    DOW[i] = s[2];
                }
                CreateNotEqualConstraint(n, DOW, TimeSlots, course);

                ///-----------------
                /// 
                input = re.ReadLine();
                try
                {
                    n = Convert.ToInt16(input);
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to read no of equal constraints ");
                    return;
                }
                try
                {
                    for (int i = 0; i < n; i++)
                    {
                        input = re.ReadLine();
                        string[] s = input.Split(' ');
                        int ProfIndex = Convert.ToInt16(s[0]);
                        int CourseIndex = Convert.ToInt16(s[1]);
                        course[CourseIndex].Equals(ProfIndex);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error in reading equal constraint ");
                    return;
                }

                re.Close();

                Professor dummyPorf = professors[net.Professors.Count - 1];
                dummyPorf.Courses = 0; // always initialize with 0
                new Count(net, course);
                int sum = 0;
                if (net.Professors != null)
                {
                    foreach (Professor p in net.Professors)
                    {
                        sum += p.Courses;
                    }
                }
                int overAllAVG = sum /(net.Professors.Count-1);
                if (sum < noOfCourses)
                {
                    dummyPorf.Courses = noOfCourses - sum;
                }
                else if (checkBox1.Checked)
                {
                    if (sum > noOfCourses) // must distribute evenly
                    {
                        int sumOver = 0;
                        int c = 0;
                        foreach (Professor p in net.Professors)
                        {
                            if (p.Courses > overAllAVG)
                            {
                                sumOver += p.Courses;
                                c++;
                            }
                        }
                        int avg = 0;
                        if (c > 0)
                        {
                            avg = sumOver / (c-1);
                        }
                        foreach (Professor p in net.Professors)
                        {
                            if (p.Courses >= avg)
                            {
                                p.Courses = avg;
                            }
                        }
                        int sumAfter = 0;
                        foreach (Professor f in net.Professors)
                        {
                            sumAfter += f.Courses;
                        }
                        int diff = noOfCourses - sumAfter;
                        if (diff > 0)
                        {
                            while (diff > 0)
                            {
                                foreach (Professor g in net.Professors)
                                {
                                    if (diff <= 0)
                                    {
                                        break;
                                    }
                                    if (g.RealNoOfCourses > avg)
                                    {
                                        g.Courses++;
                                        diff--;
                                    }
                                }
                            }
                        }
                        else if (diff < 0)
                        {
                            while (diff < 0)
                            {
                                foreach (Professor g in net.Professors)
                                {
                                    if (diff >= 0)
                                    {
                                        break;
                                    }
                                    if (g.RealNoOfCourses > avg)
                                    {
                                        g.Courses--;
                                        diff++;
                                    }
                                }
                            }
                        }
                    }
                }
                Solve(net, course, noOfCourses);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="net"></param>
        /// <param name="course"></param>
        /// <param name="noOfCourses"></param>
        private void Solve(CourseNetwork net, IntVariable[] course, int noOfCourses)
        {
            //net.Objective = course[0];
            Solver solver;
            if (radioButton6.Checked)
            {
                solver = new IterativeBranchAndBoundSearch(net, Solver.Minimize);
            }
            else if (radioButton4.Checked)
            {
                solver = new DefaultSolver(net, Solver.Minimize);
                solver.SolverStrategy = (Solver.StrategyMethod) numericUpDown3.Value;
            }
            else if (radioButton3.Checked)
            {
                //int opt = Solver.BETTER;
                solver = new TabooSearch(net, Solver.Minimize);
            }
            else
            {
                net.Objective = course[0];
                solver = new SimulatedAnneallingSearch(net, Solver.Minimize);
            }

            long timer = DateTime.Now.Ticks;
            int count = 1;
            //StreamWriter sw = new StreamWriter(".\\out.txt");
            DBSolution.DeleteAll();
            Notes = new IList[(int) numericUpDown1.Value];
            Solution bestSolution = null;
            for (solver.Start((long) numericUpDown2.Value); solver.WaitNext(); solver.Resume())
            {
                Solution sol = solver.Solution;
                if (count == 1)
                {
                    bestSolution = sol;
                }
                else
                {
                    if (bestSolution != null)
                        if (sol.Weight > bestSolution.Weight)
                            bestSolution = sol;
                }
                Notes[count - 1] = new ArrayList {"Weight= " + sol.Weight + "\n"};
                for (int i = 0; i < net.Professors.Count; i++)
                {
                    int pcount = 0;
                    for (int j = 0; j < net.Variables.Count; j++)
                    {
                        if (!((Variable) net.Variables[j]).IsValueType)
                        {
                            if (sol.GetIntValue(((Variable) net.Variables[j])) == i)
                            {
                                pcount++;
                            }
                        }
                    }
                    if (pcount < ((Professor) net.Professors[i]).RealNoOfCourses)
                    {
                        Notes[count - 1].Add("Prof. " + ((Professor) net.Professors[i]).ToString() +
                                             " not consistent.. needs " +
                                             (((Professor) net.Professors[i]).RealNoOfCourses - pcount) +
                                             " assignment(s) more!!" + "\n");
                        // sw.WriteLine("Prof. " + ((Professor)net.Professors[i]).toString() + " not consistent.. needs " +
                        //     (((Professor)net.Professors[i]).Courses - pcount) + " assignment(s) more!!");
                    }
                }

                Console.Out.WriteLine();
                for (int i = 0; i < noOfCourses; i++)
                {
                    //if (!((Variable)(net.Variables[i])).IsValueType)
                    //{
                    //sw.WriteLine(course[i].Name + " = " + ((Professor)net.Professors[sol.getIntValue(course[i])]).Name);
                    var dbSolution = new DBSolution
                                         {
                                             SolutionID = count,
                                             CourseName = course[i].Name,
                                             ProfessorName =
                                                 ((Professor) net.Professors[sol.GetIntValue(course[i])]).Name
                                         };
                    dbSolution.AddSolution();
                    //}
                }
                //sw.WriteLine("=================================");
                count++;
                //if (solver is DefaultSolver)
                //{
                if (count == numericUpDown1.Value + 1)
                    break;
                //}
                //else
                //{
                //   break;
                //}
            }
            Console.WriteLine(bestSolution);
            timer = DateTime.Now.Ticks - timer;
            //sw.WriteLine("timer: " + timer);
            //sw.WriteLine("Count=" + count);
            //sw.Close();
            solutionBS = new BindingSource();
            if (count > 1)
            {
                solutionBS.DataSource = DBSolution.GetByID(1);
                bindingNavigator1.BindingSource = solutionBS;
                solutionViewGrid.DataSource = solutionBS;
                solIndex = 1;
                firstSol.Enabled = false;
                prevSol.Enabled = false;
                SolUpDown.Minimum = 1;
                SolUpDown.Maximum = count - 1;
                SolUpDown.Value = 1;
                SolUpDown.Enabled = true;
                if (count == 2)
                {
                    nextSol.Enabled = false;
                    lastSol.Enabled = false;
                }
                else
                {
                    nextSol.Enabled = true;
                    lastSol.Enabled = true;
                }
            }
            else
            {
                SolUpDown.Minimum = 0;
                SolUpDown.Maximum = 0;
                SolUpDown.Enabled = false;
                firstSol.Enabled = false;
                prevSol.Enabled = false;
                nextSol.Enabled = false;
                lastSol.Enabled = false;
            }
            label2.Text = "";
            if (count > 1)
            {
                for (int y = 0; y < Notes[solIndex - 1].Count; y++)
                {
                    label2.Text += Convert.ToString(Notes[solIndex - 1][y]);
                }
            }
            numberOfSolutions = count - 1;
            solutionViewGrid.Columns[2].Visible = false;
            if (timer/10000/1000 == 0)
            {
                MessageBox.Show((count - 1) + " Solution(s) found in " + timer/1000.0 + " MS");
            }
            else
            {
                MessageBox.Show((count - 1) + " Solution(s) found in " + timer/10000.0/1000 + " second(s)");
            }
        }

        private static void CreateNotEqualConstraint(int n, string[] DOW, int[][] TimeSlots, IntVariable[] course)
        {
            for (int first = 0; first < n; first++)
            {

                for (int second = first + 1; second < n; second++)
                {
                    for (int m = 0; m < DOW[first].Length; m++)
                    {
                        char c = DOW[first][m];
                        if (DOW[second].Contains(c.ToString()))
                        {
                            if (TimeSlots[first][0] == TimeSlots[second][0])
                            {
                                course[first].NotEquals(course[second]);
                                break;
                            }
                            if ((TimeSlots[first][0] < TimeSlots[second][0]) &&
                                (TimeSlots[first][1] > TimeSlots[second][0]))
                            {
                                course[first].NotEquals(course[second]);
                                break;
                            }
                            if ((TimeSlots[first][0] < TimeSlots[second][1]) &&
                                (TimeSlots[first][1] > TimeSlots[second][1]))
                            {
                                course[first].NotEquals(course[second]);
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        private static int[] ParseTimeSlot(string ts)
        {
            while (ts.IndexOf(' ') >= 0)
                ts = ts.Remove(ts.IndexOf(' '), 1);
            var timeSlot = new int[2];
            string[] s = ts.Split('-');
            for (int j = 0; j < 2; j++)
            {
                string d = s[j];
                string[] f = d.Split(':');
                int i1 = Convert.ToInt16(f[0]); // hour of starting time slot
                string h = f[1];
                int k1 = Convert.ToInt16(h.Substring(0, 1)) * 10;
                int k2 = Convert.ToInt16(h.Substring(1, 1));
                int i2 = k1 + k2;
                string l = String.Concat(h[2], h[3]);
                if ((l.ToUpper().Equals("PM")) && (i1 != 12))
                {
                    i1 += 12;
                }
                timeSlot[j] = i1 * 100 + i2;
            }
            return timeSlot;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialDirectory"></param>
        /// <returns></returns>
        private static string SelectTextFile(string initialDirectory)
        {
            var dialog = new OpenFileDialog
                             {
                                 Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                                 InitialDirectory = initialDirectory,
                                 Title = "Select a text file"
                             };
            return (dialog.ShowDialog() == DialogResult.OK)
               ? dialog.FileName : null;
        }


        #region Databse File

        private void button2_Click()
        {
            profBS.MoveFirst();
            var net = new CourseNetwork();
            var professors = new Professor[profBS.Count];
            profBS.MoveFirst();
            int UnassignedIndex = -1;
            while (true)
            {
                int m = ((Profs) (profBS.Current)).NoOfCourses;
                string s = ((Profs) (profBS.Current)).ProfName;
                professors[profBS.Position] = new Professor(net, m, s);
                if (((Profs) profBS.Current).UnassignedProf)
                {
                    UnassignedIndex = profBS.Position;
                }
                if (profBS.Position == profBS.Count - 1)
                {
                    break;
                }
                profBS.MoveNext();
            }

            //-----------------
            courseBS.MoveFirst();
            var course = new IntVariable[courseBS.Count];
            int noOfCourses = courseBS.Count;
            var TimeSlots = new int[courseBS.Count][];
            var DOW = new string[courseBS.Count];
            int max = net.Professors.Count - 1;
            for (int i = 0; i < courseBS.Count; i++)
            {
                string cName = ((Course) (courseBS.Current)).CourseName;
                course[i] = new IntVariable(net, 0, max, cName);
                TimeSlots[i] = ParseTimeSlot(((Course) (courseBS.Current)).TimeSlot);
                DOW[i] = ((Course) (courseBS.Current)).DaysOfWeek;
                if (((Course) (courseBS.Current)).ProfID != null)
                {
                    int c = 0;
                    for (int j = 0; j < profBS.Count; j++)
                    {
                        var pf = (Profs) profBS[j];
                        if (pf.ProfID == ((Course) (courseBS.Current)).ProfID.Value)
                        {
                            c = pf.RowID - 1;
                            break;
                        }
                    }
                    course[i].Equals(c);
                }
                courseBS.MoveNext();
            }
            /////////////////////////
            /// /////////////////
            /// ///////////////
            /// 
            //course[1].equals(14);
            course[1].Equals(15, ConstraintTypes.Soft, 70);
            course[1].Equals(17, ConstraintTypes.Soft, 100);
            course[0].Equals(17, ConstraintTypes.Soft, 90);
            course[2].Equals(3, ConstraintTypes.Soft, 50);
            CreateNotEqualConstraint(courseBS.Count, DOW, TimeSlots, course);
            new Count(net, course);
            //Professor dummyPorf = professors[net.Professors.Count - 1];
            //dummyPorf.Courses = 0;    // always initialize with 0
            int sum = 0;
            if (net.Professors != null)
            {
                foreach (Professor p in net.Professors)
                {
                    sum += p.Courses;
                }
            }
            bool OK = true;
            int overAllAVG = sum/(net.Professors.Count - 1);
            if (sum < noOfCourses)
            {
                if (UnassignedIndex > -1)
                {
                    professors[UnassignedIndex].Courses = noOfCourses - sum;
                }
                else
                {
                    MessageBox.Show(
                        "Number of courses is more than the professors' courses..\nYou should assign Unassigned professor!");
                    OK = false;
                }
            }
            else if (checkBox1.Checked)
            {
                if (sum > noOfCourses) // must distribute evenly
                {
                    int sumOver = 0;
                    int c = 0;
                    foreach (Professor p in net.Professors)
                    {
                        if (p.Courses > overAllAVG)
                        {
                            sumOver += p.Courses;
                            c++;
                        }
                    }
                    int avg = 0;
                    if (c > 0)
                    {
                        avg = sumOver/(c - 1);
                    }
                    foreach (Professor p in net.Professors)
                    {
                        if (p.Courses > avg)
                        {
                            p.Courses = avg;
                        }
                    }
                    int sumAfter = 0;
                    foreach (Professor f in net.Professors)
                    {
                        sumAfter += f.Courses;
                    }
                    int diff = noOfCourses - sumAfter;
                    if (diff > 0)
                    {
                        while (diff > 0)
                        {
                            foreach (Professor g in net.Professors)
                            {
                                if (diff <= 0)
                                {
                                    break;
                                }
                                if (g.RealNoOfCourses > avg)
                                {
                                    g.Courses++;
                                    diff--;
                                }
                            }
                        }
                    }
                    else if (diff < 0)
                    {
                        while (diff < 0)
                        {
                            foreach (Professor g in net.Professors)
                            {
                                if (diff >= 0)
                                {
                                    break;
                                }
                                if (g.RealNoOfCourses > avg)
                                {
                                    g.Courses--;
                                    diff++;
                                }
                            }
                        }
                    }
                }
            }
            if (OK)
            {
                Solve(net, course, noOfCourses);
            }
        }

        #endregion

        #region Prof tab

        private void refreshProfs_Click()
        {
            try
            {
                profBS = new BindingSource(Profs.GetAll(), "");
                ProfsGridView.DataSource = profBS;
                ProfsNavigator.BindingSource = profBS;
                comboBox1.DataSource = profBS;
                if (profBS.Count > 0)
                {
                    comboBox1.Enabled = true;
                    var tbs = new BindingSource(Profs.GetAll(), "") {new Profs("All Professors")};
                    comboBox1.DataSource = tbs;
                    comboBox1.DisplayMember = "ProfName";
                    comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                }
                else
                {
                    comboBox1.Enabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Could not connect to SQL Server" + "\n" + "Please make sure SQL server express is installed");
                err = 1;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var ap = new AddProf();
            ap.ShowDialog();
            DialogResult action = ap.DialogResult;
            if (action == DialogResult.OK)
            {
                int position = profBS.Position;
                refreshProfs_Click();
                profBS.Position = position;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            refreshProfs_Click();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(this,
                           "Do you really want to delete this professor?",
                           "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                if (Profs.DeleteProf(((Profs)profBS.Current).ProfID))
                {
                    profBS.RemoveCurrent();
                    int position = profBS.Position;
                    refreshProfs_Click();
                    profBS.Position = position;
                }
                else
                {
                    MessageBox.Show("Cannot delete this professor...some courses are related to this professor!!");
                }
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //(Profs) profBS.Current;
            var ap = new AddProf("Update Professor", ((Profs)(profBS.Current)).ProfID,
                ((Profs)(profBS.Current)).ProfName, ((Profs)(profBS.Current)).NoOfCourses,
                ((Profs)(profBS.Current)).UnassignedProf, 1);
            ap.ShowDialog();
            int position = profBS.Position;
            refreshProfs_Click();
            profBS.Position = position;
        }

        private void ProfsGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            toolStripButton2_Click(null, null);

        }
        #endregion

        #region Course tab

        private void refreshCourses_Click(object sender, EventArgs e)
        {
            try
            {
                courseBS = new BindingSource(Course.GetAll(), "");
                coursesGridView.DataSource = courseBS;
                coursesNavigator.BindingSource = courseBS;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot connect to SQL Server" + "\n" + ex.Message);
                err = 1;
            }
        }



        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            var ac = new AddCourse();
            ac.ShowDialog();
            int position = courseBS.Position;
            refreshCourses_Click(null, null);
            courseBS.Position = position;
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            var ac = new AddCourse("Update Course", ((Course)(courseBS.Current)).CourseID,
                ((Course)(courseBS.Current)).CourseName, ((Course)(courseBS.Current)).TimeSlot,
                ((Course)(courseBS.Current)).DaysOfWeek, ((Course)(courseBS.Current)).ProfID, 1);
            ac.ShowDialog();
            int position = courseBS.Position;
            refreshCourses_Click(null, null);
            courseBS.Position = position;
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(this,
                          "Do you really want to delete this course?",
                          "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                if (Course.DeleteCourse(((Course)courseBS.Current).CourseID))
                {
                    courseBS.RemoveCurrent();
                    int position = courseBS.Position;
                    refreshCourses_Click(null, null);
                    courseBS.Position = position;
                }
                else
                {
                    MessageBox.Show("Cannot delete this course");
                }
            }
        }

        private void coursesGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            toolStripButton10_Click(null, null);
        }

        #endregion

        private void button1_Click_1(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var wf = new waitingForm();
            wf.Show();
            label2.Text = "";
            solutionViewGrid.DataSource = null;
            bindingNavigator1.BindingSource = null;
            if (radioButton1.Checked)
            {
                button1_Click();
            }
            else
            {
                button2_Click();
            }

            wf.Close();
            Cursor = Cursors.Default;
        }

        private void firstSol_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            solutionBS = new BindingSource();
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
            {
                solutionBS.DataSource = DBSolution.GetByID(1);
            }
            else
            {
                string pName = ((Profs)comboBox1.Items[comboBox1.SelectedIndex]).ProfName;
                solutionBS = new BindingSource {DataSource = DBSolution.GetByProfID(pName, 1)};
            }
            solutionViewGrid.DataSource = solutionBS;
            bindingNavigator1.BindingSource = solutionBS;
            solIndex = 1;
            label2.Text = "";
            for (int y = 0; y < Notes[solIndex - 1].Count; y++)
            {
                label2.Text += Convert.ToString(Notes[solIndex - 1][y]);
            }
            SolUpDown.ValueChanged += null;
            SolUpDown.Value = solIndex;
            SolUpDown.ValueChanged += SolUpDown_ValueChanged;
            firstSol.Enabled = false;
            prevSol.Enabled = false;
            nextSol.Enabled = true;
            lastSol.Enabled = true;
            Cursor = Cursors.WaitCursor;
        }

        private void prevSol_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            solutionBS = new BindingSource();
            solIndex = (int)SolUpDown.Value;
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
            {
                solutionBS.DataSource = DBSolution.GetByID(--solIndex);
            }
            else
            {
                string pName = ((Profs)comboBox1.Items[comboBox1.SelectedIndex]).ProfName;
                solutionBS = new BindingSource
                                 {
                                     DataSource = DBSolution.GetByProfID(pName, --solIndex)
                                 };
            }
            solutionViewGrid.DataSource = solutionBS;
            bindingNavigator1.BindingSource = solutionBS;
            label2.Text = "";
            for (int y = 0; y < Notes[solIndex - 1].Count; y++)
            {
                label2.Text += Convert.ToString(Notes[solIndex - 1][y]);
            }
            SolUpDown.ValueChanged += null;
            SolUpDown.Value = solIndex;
            SolUpDown.ValueChanged += SolUpDown_ValueChanged;
            if (solIndex == 1)
            {
                firstSol.Enabled = false;
                prevSol.Enabled = false;
            }
            nextSol.Enabled = true;
            lastSol.Enabled = true;
            Cursor = Cursors.Default;
        }

        private void nextSol_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            solIndex = (int)SolUpDown.Value;
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
            {
                solutionBS.DataSource = DBSolution.GetByID(++solIndex);
            }
            else
            {
                string pName = ((Profs)comboBox1.Items[comboBox1.SelectedIndex]).ProfName;
                solutionBS.DataSource = DBSolution.GetByProfID(pName, ++solIndex);
            }
            solutionViewGrid.DataSource = solutionBS;
            bindingNavigator1.BindingSource = solutionBS;
            label2.Text = "";
            for (int y = 0; y < Notes[solIndex - 1].Count; y++)
            {
                label2.Text += Convert.ToString(Notes[solIndex - 1][y]);
            }

            if (solIndex == numberOfSolutions)
            {
                nextSol.Enabled = false;
                lastSol.Enabled = false;
            }
            firstSol.Enabled = true;
            prevSol.Enabled = true;
            Cursor = Cursors.Default;
            SolUpDown.ValueChanged += null;
            SolUpDown.Value = solIndex;
            SolUpDown.ValueChanged += SolUpDown_ValueChanged;
        }

        private void lastSol_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            solutionBS = new BindingSource();
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
            {
                solutionBS.DataSource = DBSolution.GetByID(numberOfSolutions);
            }
            else
            {
                string pName = ((Profs)comboBox1.Items[comboBox1.SelectedIndex]).ProfName;
                solutionBS = new BindingSource
                                 {
                                     DataSource = DBSolution.GetByProfID(pName, numberOfSolutions)
                                 };
            }
            solIndex = numberOfSolutions;
            solutionViewGrid.DataSource = solutionBS;
            bindingNavigator1.BindingSource = solutionBS;
            label2.Text = "";
            for (int y = 0; y < Notes[solIndex - 1].Count; y++)
            {
                label2.Text += Convert.ToString(Notes[solIndex - 1][y]);
            }
            SolUpDown.ValueChanged += null;
            SolUpDown.Value = solIndex;
            SolUpDown.ValueChanged += SolUpDown_ValueChanged;
            nextSol.Enabled = false;
            lastSol.Enabled = false;
            firstSol.Enabled = true;
            prevSol.Enabled = true;
            Cursor = Cursors.Default;
        }

        private void SolUpDown_ValueChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            solutionBS = new BindingSource();
            solIndex = Convert.ToInt16(SolUpDown.Value);
            label2.Text = "";
            if (solIndex < 1) return;
            if (Notes == null) return;
            for (int y = 0; y < Notes[solIndex - 1].Count; y++)
            {
                label2.Text += Convert.ToString(Notes[solIndex - 1][y]);
            }
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
            {
                solutionBS.DataSource = DBSolution.GetByID(solIndex);
            }
            else
            {
                string pName = ((Profs)comboBox1.Items[comboBox1.SelectedIndex]).ProfName;
                solutionBS.DataSource = DBSolution.GetByProfID(pName, solIndex);
            }
            solutionViewGrid.DataSource = solutionBS;
            bindingNavigator1.BindingSource = solutionBS;
            if (solIndex == 1)
            {
                firstSol.Enabled = false;
                prevSol.Enabled = false;
            }
            else
            {
                firstSol.Enabled = true;
                prevSol.Enabled = true;
            }
            if (solIndex == numberOfSolutions)
            {
                nextSol.Enabled = false;
                lastSol.Enabled = false;
            }
            else
            {
                nextSol.Enabled = true;
                lastSol.Enabled = true;
            }
            Cursor = Cursors.Default;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void ca_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show(this,
                                              "Do you really want to exit the application?",
                                              "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if ((dr == DialogResult.No) || (dr == DialogResult.Cancel))
            {
                e.Cancel = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            /*this.Cursor = Cursors.WaitCursor;
            if (radioButton4.Checked)
            {
                numericUpDown1.Enabled = true;
            }
            else if (radioButton3.Checked || radioButton5.Checked)
            {
                numericUpDown1.Enabled = false;
            }
            this.Cursor = Cursors.Default;*/
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            refreshProfs_Click();
            refreshCourses_Click(null, null);
            Cursor = Cursors.Default;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
            {
                SolUpDown_ValueChanged(null, null);
            }
            else
            {

                if (numberOfSolutions > 0)
                {
                    string pName = ((Profs)comboBox1.Items[comboBox1.SelectedIndex]).ProfName;
                    solutionBS = new BindingSource
                                     {
                                         DataSource = DBSolution.GetByProfID(pName, (int) SolUpDown.Value)
                                     };
                    solutionViewGrid.DataSource = solutionBS;
                    bindingNavigator1.BindingSource = solutionBS;
                }
            }
            Cursor = Cursors.Default;
        }

    }
}