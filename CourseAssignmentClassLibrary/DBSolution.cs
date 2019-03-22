using System;
using System.Data;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;

namespace CourseAssignmentClassLibrary
{
    public class DBSolution
    {
        private static int rowid = 1;
        #region Properties

        public int RowID { get; set; }

        [DisplayName("Solution ID")]
        public int SolutionID { get; set; }

     

        [DisplayName("Course name")]
        public string CourseName { get; set;}


        [DisplayName("Professor name")]
        public string ProfessorName { get; set;}

        #endregion

        #region constructors

        public DBSolution()
        {
        }

        public DBSolution(IDataRecord reader)
        {
            try
            {
                SolutionID = reader.GetInt32(reader.GetOrdinal("SolutionNo"));
                CourseName = reader["SolCourse"].ToString();
                ProfessorName = reader["SolProf"].ToString();
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

        public static void DeleteAll()
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "DeleteAllSolutions";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(Ex.ToString(), "DeleteCourse");
                throw new Exception("\nException occured in Course.DeleteCourse()", Ex);
            }
        }

        public void AddSolution()
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "AddSolution";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@SolutionNo", DbType.Int32, SolutionID);
                db.AddInParameter(dbCommand, "@SolCourse", DbType.String, CourseName);
                db.AddInParameter(dbCommand, "@SolProf", DbType.String, ProfessorName);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(Ex.ToString(), "courses.Addcourse");
                throw new Exception("\nException occured in courses.Add()", Ex);
            }

        }

        public static IList GetAll()
        {
            //DataSet ds;
            IList solutionArray;
            try
            {
                rowid = 1;
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "GetAllSolutions";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                solutionArray = new ArrayList();
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        solutionArray.Add(new DBSolution(dataReader));
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

            return solutionArray;
        }

        public static IList GetByID(int ID)
        {
            IList solList = new ArrayList();
            try
            {
                rowid = 1;
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "GetSolutionByID";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@SID", DbType.Int32, ID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        solList.Add(new DBSolution(dataReader));
                    }
                }

                return solList;
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

        public static IList GetByProfID(string PName, int SID)
        {
            IList solList = new ArrayList();
            try
            {
                rowid = 1;
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "GetSolutionByProfID";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PName", DbType.String, PName);
                db.AddInParameter(dbCommand, "@SID", DbType.Int32, SID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        solList.Add(new DBSolution(dataReader));
                    }
                }

                return solList;
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
