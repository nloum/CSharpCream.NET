using System;

using System.Windows.Forms;
using CourseAssignmentClassLibrary;
using System.Collections;

namespace CourseAssignment
{
    public partial class AddCourse : Form
    {
        private int EditingType=0;
        private int CID;
        private IList allprofs;
        public AddCourse(string title, int CID, string CourseName, 
                         string TimeSlot, string DOW, int? ProfID, int type):this()
        {
            Text = title;
            courseNameTextBox.Text = CourseName;
            timeSlotTextBox.Text = TimeSlot;
            DOWTextBox.Text = DOW;
            this.CID = CID;
            for (int i = 1; i < allprofs.Count; i++ )   // start with 1;
            {
                Profs pf = (Profs) allprofs[i];
                if (pf.ProfID == ProfID)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }
            EditingType = type==0 ? 0 : 1;
            
            ValidateChildren();
        }
        public AddCourse()
        {
            InitializeComponent();
            allprofs = new ArrayList();
            allprofs.Add(new Profs("No Prof Assigned"));
            foreach (Profs pf in Profs.GetAll())
            {
                allprofs.Add(pf);
            }
            comboBox1.DataSource = allprofs;
            comboBox1.DisplayMember = "ProfName";
            this.ValidateChildren();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (validationProvider1.IsValid)
            {
                Course cr = new Course();
                cr.CourseName = courseNameTextBox.Text;
                cr.TimeSlot = timeSlotTextBox.Text;
                cr.DaysOfWeek = DOWTextBox.Text;
                if (comboBox1.SelectedIndex == 0)
                {
                    cr.ProfID = null;
                }
                else
                {
                    cr.ProfID = ((Profs) allprofs[comboBox1.SelectedIndex]).ProfID;
                }
                errorLabel.Visible = false;
                if (EditingType == 0)
                {
                    cr.AddCourse();
                }
                else
                {
                    cr.UpdateCourse(CID);
                }
                Close();
            }
            else
            {
                errorLabel.Text = "There are some errors";
                errorLabel.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

       
      
    }
}
