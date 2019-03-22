using System;
using System.Windows.Forms;
using CourseAssignmentClassLibrary;

namespace CourseAssignment
{
    public partial class AddProf : Form
    {
        private int EditingType=0;
        private int PID;
        public AddProf(string title, int PID, string ProfName, int NOC, bool UnassignedProf, int type):this()
        {
            Text = title;
            profNameTextBox.Text = ProfName;
            numericUpDown1.Text = Convert.ToString(NOC);
            checkBox1.Checked = UnassignedProf;
            this.PID = PID;
            if (type==0)
            {
                EditingType = 0;
            }
            else
            {
                EditingType = 1;
            }
            this.ValidateChildren();
        }
        public AddProf()
        {
            InitializeComponent();
            this.ValidateChildren();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (validationProvider1.IsValid)
            {
                Profs pf = new Profs();
                pf.ProfName = profNameTextBox.Text;
                pf.NoOfCourses = Convert.ToInt16(numericUpDown1.Text);
                pf.UnassignedProf = checkBox1.Checked;
                errorLabel.Visible = false;
                if (EditingType == 0)
                {
                    pf.AddProf();
                }
                else
                {
                    pf.UpdateProf(PID);
                }
                button1.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                errorLabel.Text = "There are some errors";
                errorLabel.Visible = true;
            }
        }

       private void validationProvider1_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        {
            if (e.SourcePropertyName == "NoOfCourses")
            {
                string originalValue = e.ValueToConvert as string;
                int convertedValue;
                bool success = Int32.TryParse(originalValue, out convertedValue);
                if (success)
                {
                    e.ConvertedValue = convertedValue;
                }
                else
                {
                    e.ConversionErrorMessage = "No. Of Courses Points must be a valid integer";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
