using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;

namespace CourseAssignmentClassLibrary
{
    public class Profs
    {
        private static int rowid = 1;
        #region Properties

        public int RowID { get; set; }

        [DisplayName("Professor ID")]
        public int ProfID { get; private set; }

        [DisplayName("Professor Name")]
        //[IgnoreNulls]
        [StringLengthValidator(1, 50, Ruleset = "RuleSetA",
            MessageTemplate = "Professor name must be between 1 and 50 chars")]
        public string ProfName { get; set; }

        [DisplayName("Number of Courses")]
        [DefaultValue(4)]
        [ValidatorComposition(CompositionType.Or, Ruleset = "RuleSetA", MessageTemplate = "Must be between 0 and 5")]
        [IgnoreNulls]
        [ValidatorComposition(CompositionType.Or, Ruleset = "RuleSetA", MessageTemplate = "MMust be between 0 and 5")]
        [TypeConversionValidator(typeof(Int32), Ruleset = "RuleSetA", MessageTemplate = "Must be an integer")]
        [RangeValidator(0, RangeBoundaryType.Inclusive, 5, RangeBoundaryType.Inclusive, Ruleset = "RuleSetA",
            MessageTemplate = "must be between 0 and 5")]
        public int NoOfCourses { get; set; }

        [DisplayName("Is unassigned Prof?")]
        public bool UnassignedProf { get; set; }

        #endregion

        #region constructors

        public Profs()
        {
        }

        public Profs(string profName)
        {
            ProfName = profName;
        }

        public Profs(IDataRecord reader)
        {
            try
            {
                ProfID = reader.GetInt32(reader.GetOrdinal("ProfID"));
                ProfName = reader["ProfName"].ToString();
                NoOfCourses = reader.GetInt32(reader.GetOrdinal("NoOfCourses"));
                UnassignedProf = reader.GetBoolean(reader.GetOrdinal("UnassignedProf"));
                RowID = rowid++;
                //RowID = rowid++;
            }
            catch (SqlException sqlEx)
            {
                //Throw this exception
                //errorlogging.WriteToLog(sqlEx.ToString(), "Profs(Reader)");
                throw new Exception("\nDatabase exception occured in Profs(Reader)", sqlEx);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(Ex.ToString(), "Profs(Reader)");
                throw new Exception("\nException occured in Profs(Reader)", Ex);
            }
        }

        #endregion

        #region Methods

        public static bool DeleteProf(int PID)
        {
            bool deleted;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "DeleteProf";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PID", DbType.Int32, PID);
                db.ExecuteNonQuery(dbCommand);
                deleted = true;
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(Ex.ToString(), "DeleteProf");
                throw new Exception("\nException occured in Prof.DeleteProf()", Ex);
            }
            return deleted;
        }

        public void AddProf()
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "AddProf";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PName", DbType.String, ProfName);
                db.AddInParameter(dbCommand, "@NOC", DbType.Int32, NoOfCourses);
                db.AddInParameter(dbCommand, "@UnassignedProf", DbType.Boolean, UnassignedProf);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(Ex.ToString(), "Profs.AddProf");
                throw new Exception("\nException occured in Suppliers.Add()", Ex);
            }

        }

        public void UpdateProf(int PID)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "UpdateProf";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PID", DbType.Int32, PID);
                db.AddInParameter(dbCommand, "@PName", DbType.String, ProfName);
                db.AddInParameter(dbCommand, "@NOC", DbType.Int32, NoOfCourses);
                db.AddInParameter(dbCommand, "@UnassignedProf", DbType.Boolean, UnassignedProf);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(Ex.ToString(), "Profs.Update");
                throw new Exception("\nException occured in Profs.Update()", Ex);
            }

        }

        public static IList GetAll()
        {
            //DataSet ds;
            IList profArray;
            try
            {
                rowid = 1;
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "GetAllProfs";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                //BindingList<Profs> bs = new BindingList<Profs>();
                //ds = db.ExecuteDataSet(dbCommand);
                //DataTable dt = ds.Tables[0];
                //dt.Columns.Add("RowID", typeof(Int32));
                //dt.AcceptChanges();
                profArray = new ArrayList();
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        profArray.Add(new Profs(dataReader));
                        
                    }
                }
                
            }
            catch (SqlException sqlEx)
            {
                //Throw this exception
                //errorlogging.WriteToLog(sqlEx.ToString(), "Profs.GetAll");
                throw new Exception("\nDatabase exception occured in Profs.GetAll()", sqlEx);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(sqlEx.ToString(), "Profs.GetAll");
                throw new Exception("\nDatabase exception occured in Profs.GetAll()", Ex);
            }

            return profArray;
        }

        public static Profs GetByID(int ID)
        {
            Profs prf = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                const string sqlCommand = "GetProfByID";
                DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);
                db.AddInParameter(dbCommand, "@PID", DbType.Int32, ID);
                using (IDataReader dataReader = db.ExecuteReader(dbCommand))
                {
                    while (dataReader.Read())
                    {
                        prf = new Profs(dataReader);
                    }
                }

                return prf;
            }
            catch (SqlException sqlEx)
            {
                //Throw this exception
                //errorlogging.WriteToLog(sqlEx.ToString(), "Profs.GetByID");
                throw new Exception("\nDatabase exception occured in Profs.GetByID()", sqlEx);
            }
            catch (Exception Ex)
            {
                //Throw this exception
                //errorlogging.WriteToLog(sqlEx.ToString(), "Profs.GetByID");
                throw new Exception("\nDatabase exception occured in Profs.GetByID()", Ex);
            }
        }

        #endregion
    }
}
