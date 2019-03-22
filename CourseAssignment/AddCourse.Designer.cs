namespace CourseAssignment
{
    partial class AddCourse
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
            System.Windows.Forms.Label courseNameLabel;
            System.Windows.Forms.Label daysOfWeekLabel;
            System.Windows.Forms.Label timeSlotLabel;
            System.Windows.Forms.Label label1;
            this.validationProvider1 = new Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.courseNameTextBox = new System.Windows.Forms.TextBox();
            this.timeSlotTextBox = new System.Windows.Forms.TextBox();
            this.DOWTextBox = new System.Windows.Forms.TextBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.courseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            courseNameLabel = new System.Windows.Forms.Label();
            daysOfWeekLabel = new System.Windows.Forms.Label();
            timeSlotLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.courseBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // courseNameLabel
            // 
            courseNameLabel.AutoSize = true;
            courseNameLabel.Location = new System.Drawing.Point(66, 29);
            courseNameLabel.Name = "courseNameLabel";
            courseNameLabel.Size = new System.Drawing.Size(74, 13);
            courseNameLabel.TabIndex = 3;
            courseNameLabel.Text = "Course Name:";
            // 
            // daysOfWeekLabel
            // 
            daysOfWeekLabel.AutoSize = true;
            daysOfWeekLabel.Location = new System.Drawing.Point(66, 95);
            daysOfWeekLabel.Name = "daysOfWeekLabel";
            daysOfWeekLabel.Size = new System.Drawing.Size(80, 13);
            daysOfWeekLabel.TabIndex = 5;
            daysOfWeekLabel.Text = "Days Of Week:";
            // 
            // timeSlotLabel
            // 
            timeSlotLabel.AutoSize = true;
            timeSlotLabel.Location = new System.Drawing.Point(66, 60);
            timeSlotLabel.Name = "timeSlotLabel";
            timeSlotLabel.Size = new System.Drawing.Size(54, 13);
            timeSlotLabel.TabIndex = 9;
            timeSlotLabel.Text = "Time Slot:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(66, 128);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(100, 13);
            label1.TabIndex = 17;
            label1.Text = "Preffered Professor:";
            // 
            // validationProvider1
            // 
            this.validationProvider1.ErrorProvider = this.errorProvider1;
            this.validationProvider1.RulesetName = "RuleSetA";
            this.validationProvider1.SourceTypeName = "CourseAssignmentClassLibrary.Course, CourseAssignmentClassLibrary";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // courseNameTextBox
            // 
            this.courseNameTextBox.Location = new System.Drawing.Point(172, 26);
            this.courseNameTextBox.Name = "courseNameTextBox";
            this.validationProvider1.SetPerformValidation(this.courseNameTextBox, true);
            this.courseNameTextBox.Size = new System.Drawing.Size(255, 20);
            this.validationProvider1.SetSourcePropertyName(this.courseNameTextBox, "CourseName");
            this.courseNameTextBox.TabIndex = 14;
            // 
            // timeSlotTextBox
            // 
            this.timeSlotTextBox.Location = new System.Drawing.Point(172, 57);
            this.timeSlotTextBox.Name = "timeSlotTextBox";
            this.validationProvider1.SetPerformValidation(this.timeSlotTextBox, true);
            this.timeSlotTextBox.Size = new System.Drawing.Size(121, 20);
            this.validationProvider1.SetSourcePropertyName(this.timeSlotTextBox, "TimeSlot");
            this.timeSlotTextBox.TabIndex = 15;
            // 
            // DOWTextBox
            // 
            this.DOWTextBox.Location = new System.Drawing.Point(172, 92);
            this.DOWTextBox.Name = "DOWTextBox";
            this.validationProvider1.SetPerformValidation(this.DOWTextBox, true);
            this.DOWTextBox.Size = new System.Drawing.Size(84, 20);
            this.validationProvider1.SetSourcePropertyName(this.DOWTextBox, "DaysOfWeek");
            this.DOWTextBox.TabIndex = 16;
            // 
            // errorLabel
            // 
            this.errorLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.errorLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.errorLabel.Location = new System.Drawing.Point(0, 212);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(543, 20);
            this.errorLabel.TabIndex = 13;
            this.errorLabel.Text = "label1";
            this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.errorLabel.Visible = false;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(372, 178);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 31);
            this.button2.TabIndex = 12;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(152, 178);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 31);
            this.button1.TabIndex = 11;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(172, 125);
            this.comboBox1.MaxDropDownItems = 12;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 19;
            // 
            // courseBindingSource
            // 
            this.courseBindingSource.DataSource = typeof(CourseAssignmentClassLibrary.Course);
            // 
            // AddCourse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(543, 232);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(label1);
            this.Controls.Add(this.DOWTextBox);
            this.Controls.Add(this.timeSlotTextBox);
            this.Controls.Add(this.courseNameTextBox);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(courseNameLabel);
            this.Controls.Add(daysOfWeekLabel);
            this.Controls.Add(timeSlotLabel);
            this.Name = "AddCourse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Course";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.courseBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider validationProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.BindingSource courseBindingSource;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox DOWTextBox;
        private System.Windows.Forms.TextBox timeSlotTextBox;
        private System.Windows.Forms.TextBox courseNameTextBox;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}