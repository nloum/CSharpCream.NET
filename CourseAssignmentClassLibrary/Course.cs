using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;

namespace CourseAssignmentClassLibrary
{
    public class Course
    {
        private static int rowid = 1;
        #region Properties

        public int RowID { get; set; }

        [DisplayName("Course ID")]
        public int CourseID { get; private set; }

        [DisplayName("Course Name")]
        //[IgnoreNulls]
        [StringLengthValidator(1, 50, Ruleset = "RuleSetA",
            MessageTemplate = "Course name must be between 1 and 50 chars")]
        public string CourseName { get; set; }

        [DisplayName("Time Slot")]
        [StringLengthValidator(13,15, Ruleset = "RuleSetA",
            MessageTemplate = "Time Slot must be between 13 and 15 chars")]
        public string TimeSlot { get; set; }

        [DisplayName("Days Of Week")]
        [StringLengthValidator(1,5, Ruleset = "RuleSetA",
            MessageTemplate = "Days of Week must be between 1 and 5 chars")]
        public string DaysOfWeek { get; set; }

        public int? ProfID{ get; set;}

        public string ProfName
        {
            get
            {
                if (ProfID != null)
                {
                    return Profs.GetByID(ProfID.Value).ProfName;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region constructors

        public Course()
        {
        }

        public Course(IDataRecord reader)
        {
            try
            {
                CourseID = reader.GetInt32(reader.GetOrdinal("CourseID"));
                CourseName = reader["CourseName"].ToString();
                TimeSlot = reader["TimeSlot"].ToString();
                DaysOfWeek = reader["DOW"].ToString();
                if (reader.IsDBNull(reader.GetOrdinal("ProfID")))
                {
                    ProfID = null;
                }
                else
                {
                    ProfID = reader.GetInt32(reader.GetOrdinal("ProfID"));
                }
                RowID = rowid++;
            }
            catch (SqlException sqlEx)
            {
                //Throw this exception
                //errorlogging.WriteToLog(sqlEx.ToString(), "course(Reader)");
                throw new Exception("\nDatabase exception occured in course(Reader)", sqlEx);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(Ex.ToString(), "course(Reader)");
                throw new Exception("\nException occured in course(Reader)", Ex);
            }
        }

        #endregion

        #region Methods

        public static bool DeleteCourse(int PID)
        {
            bool deleted = false;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "DeleteCourse";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@CID", DbType.Int32, PID);
                db.ExecuteNonQuery(dbCommand);
                deleted = true;
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(Ex.ToString(), "DeleteCourse");
                throw new Exception("\nException occured in Course.DeleteCourse()", Ex);
            }
            return deleted;
        }

        public void AddCourse()
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "AddCourse";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@CName", DbType.String, CourseName);
                db.AddInParameter(dbCommand, "@TimeSlot", DbType.String, TimeSlot.ToUpper());
                db.AddInParameter(dbCommand, "@DOW", DbType.String, DaysOfWeek.ToUpper());
                db.AddInParameter(dbCommand, "@ProfID", DbType.Int32, ProfID);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(Ex.ToString(), "courses.Addcourse");
                throw new Exception("\nException occured in courses.Add()", Ex);
            }

        }

        public void UpdateCourse(int CID)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "UpdateCourse";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@CID", DbType.Int32, CID);
                db.AddInParameter(dbCommand, "@CName", DbType.String, CourseName);
                db.AddInParameter(dbCommand, "@TimeSlot", DbType.String, TimeSlot.ToUpper());
                db.AddInParameter(dbCommand, "@DOW", DbType.String, DaysOfWeek.ToUpper());
                db.AddInParameter(dbCommand, "@ProfID", DbType.Int32, ProfID);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(Ex.ToString(), "Course.Update");
                throw new Exception("\nException occured in Course.Update()", Ex);
            }

        }

        public static IList GetAll()
        {
            //DataSet ds;
            IList courseArray;
            try
            {
                rowid = 1;
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "GetAllCourses";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                //BindingList<Profs> bs = new BindingList<Profs>();
                //ds = db.ExecuteDataSet(dbCommand);
                //DataTable dt = ds.Tables[0];
                //dt.Columns.Add("RowID", typeof(Int32));
                //dt.AcceptChanges();
                courseArray = new ArrayList();
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        courseArray.Add(new Course(dataReader));
                    }
                }

            }
            catch (SqlException sqlEx)
            {
                //Throw this exception
                //errorlogging.WriteToLog(sqlEx.ToString(), "Course.GetAll");
                throw new Exception("\nDatabase exception occured in Course.GetAll()", sqlEx);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(sqlEx.ToString(), "Course.GetAll");
                throw new Exception("\nDatabase exception occured in Course.GetAll()", Ex);
            }

            return courseArray;
        }

        public static Course GetByID(int ID)
        {
            Course crc = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "GetCourseByID";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@CID", DbType.Int32, ID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        crc = new Course(dataReader);
                    }
                }

                return crc;
            }
            catch (SqlException sqlEx)
            {
                //Throw this exception
                //errorlogging.WriteToLog(sqlEx.ToString(), "Course.GetByID");
                throw new Exception("\nDatabase exception occured in Course.GetByID()", sqlEx);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(sqlEx.ToString(), "Course.GetByID");
                throw new Exception("\nDatabase exception occured in Course.GetByID()", Ex);
            }
        }

        #endregion
    }
}
