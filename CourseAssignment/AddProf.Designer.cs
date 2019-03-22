namespace CourseAssignment
{
    partial class AddProf
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label noOfCoursesLabel;
            System.Windows.Forms.Label profNameLabel;
            this.profNameTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.validationProvider1 = new Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.errorLabel = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            noOfCoursesLabel = new System.Windows.Forms.Label();
            profNameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // noOfCoursesLabel
            // 
            noOfCoursesLabel.AutoSize = true;
            noOfCoursesLabel.Location = new System.Drawing.Point(12, 54);
            noOfCoursesLabel.Name = "noOfCoursesLabel";
            noOfCoursesLabel.Size = new System.Drawing.Size(100, 13);
            noOfCoursesLabel.TabIndex = 0;
            noOfCoursesLabel.Text = "Number of Courses:";
            // 
            // profNameLabel
            // 
            profNameLabel.AutoSize = true;
            profNameLabel.Location = new System.Drawing.Point(12, 9);
            profNameLabel.Name = "profNameLabel";
            profNameLabel.Size = new System.Drawing.Size(85, 13);
            profNameLabel.TabIndex = 4;
            profNameLabel.Text = "Professor Name:";
            // 
            // profNameTextBox
            // 
            this.profNameTextBox.Location = new System.Drawing.Point(118, 6);
            this.profNameTextBox.Name = "profNameTextBox";
            this.validationProvider1.SetPerformValidation(this.profNameTextBox, true);
            this.profNameTextBox.Size = new System.Drawing.Size(255, 20);
            this.validationProvider1.SetSourcePropertyName(this.profNameTextBox, "ProfName");
            this.profNameTextBox.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(118, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 31);
            this.button1.TabIndex = 6;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(289, 105);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 31);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // validationProvider1
            // 
            this.validationProvider1.ErrorProvider = this.errorProvider1;
            this.validationProvider1.RulesetName = "RuleSetA";
            this.validationProvider1.SourceTypeName = "CourseAssignmentClassLibrary.Profs, CourseAssignmentClassLibrary";
            this.validationProvider1.ValueConvert += new System.EventHandler<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs>(this.validationProvider1_ValueConvert);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(118, 52);
            this.numericUpDown1.Name = "numericUpDown1";
            this.validationProvider1.SetPerformValidation(this.numericUpDown1, true);
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.validationProvider1.SetSourcePropertyName(this.numericUpDown1, "NoOfCourses");
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // errorLabel
            // 
            this.errorLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.errorLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.errorLabel.Location = new System.Drawing.Point(0, 139);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(488, 20);
            this.errorLabel.TabIndex = 9;
            this.errorLabel.Text = "label1";
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.errorLabel.Visible = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(289, 55);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(121, 17);
            this.validationProvider1.SetSourcePropertyName(this.checkBox1, "UnassignedProf");
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Is Unassigned Prof?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // AddProf
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(488, 159);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(noOfCoursesLabel);
            this.Controls.Add(profNameLabel);
            this.Controls.Add(this.profNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddProf";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Professor";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox profNameTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider validationProvider1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}