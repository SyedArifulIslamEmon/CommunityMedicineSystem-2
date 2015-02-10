using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityMedicineSystem.DAO;

namespace CommunityMedicineSystem.DAL
{
    public class PatientDBGateway : CommonDBGateway
    {
        public int Save(Patient aPatient)
        {
            string query = "INSERT INTO tbl_patient VALUES ('" + aPatient.VoterId + "')";
            ASqlConnection.Open();
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            int rowAffected = ASqlCommand.ExecuteNonQuery();
            ASqlConnection.Close();
            return rowAffected;
        }

        public Patient Find(long voterId)
        {
            string query = "SELECT * FROM tbl_patient WHERE voter_id='" + voterId + "'";
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlConnection.Open();
            ASqlDataReader = ASqlCommand.ExecuteReader();

            if (ASqlDataReader.HasRows)
            {
                Patient aPatient = new Patient();
                ASqlDataReader.Read();
                aPatient.Id = (int)ASqlDataReader["id"];
                aPatient.VoterId = (long)ASqlDataReader["voter_id"];

                ASqlDataReader.Close();
                ASqlCommand.Dispose();
                ASqlConnection.Close();
                return aPatient;
            }
            else
            {
                ASqlDataReader.Close();
                ASqlCommand.Dispose();
                ASqlConnection.Close();
                return null;
            }
        }

        public void SaveService(Patient aPatient)
        {
            int patientId;
            Patient patient = Find(aPatient.VoterId);
            if (patient == null)
            {
                Save(aPatient);
                patientId = Find(aPatient.VoterId).Id;
            }
            else
            {
                patientId = patient.Id;
            }

            string query = "INSERT INTO tbl_service_taken VALUES ('" + patientId + "')";
            ASqlConnection.Open();
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlCommand.ExecuteNonQuery();
            ASqlConnection.Close();
        }

        public int GetServiceTakenId()
        {
            string query = "SELECT MAX(id) FROM tbl_service_taken";
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlConnection.Open();
            ASqlDataReader = ASqlCommand.ExecuteReader();
            ASqlDataReader.Read();
            int serviceTakenId = (int)ASqlDataReader[0];
            ASqlDataReader.Close();
            ASqlCommand.Dispose();
            ASqlConnection.Close();
            return serviceTakenId;
        }

        public int GetTotalNumberOfService(long voterId)
        {
            string query = "SELECT COUNT(patient.voter_id) FROM tbl_service_taken serviceTaken" +
                           " JOIN tbl_patient patient ON patient.id = serviceTaken.patient_id" +
                           " WHERE patient.voter_id='" + voterId + "'";
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlConnection.Open();
            ASqlDataReader = ASqlCommand.ExecuteReader();
            ASqlDataReader.Read();
            int totalNumberOfService = (int)ASqlDataReader[0];
            ASqlDataReader.Close();
            ASqlCommand.Dispose();
            ASqlConnection.Close();
            return totalNumberOfService;
        }

        public DataTable GetData(long voterId)
        {
            ASqlConnection.Open();
            DataTable aDataTable = new DataTable();
            string query = "SELECT * FROM v_generate_pdf_patient_history WHERE voter_id = '" + voterId + "' ORDER BY date";
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlDataReader = ASqlCommand.ExecuteReader();

            aDataTable.Clear();
            
            aDataTable.Columns.Add("Center Name");
            aDataTable.Columns.Add("Disease Name");
            aDataTable.Columns.Add("Medicine Name");
            aDataTable.Columns.Add("Dose");
            aDataTable.Columns.Add("Before/After Meal");
            aDataTable.Columns.Add("Doctor Name");


            while (ASqlDataReader.Read())
            {
                DataRow aDataRow = aDataTable.NewRow();
                aDataRow["Center Name"] = ASqlDataReader["center_name"].ToString();
                aDataRow["Disease Name"] = ASqlDataReader["disease_name"].ToString();
                aDataRow["Medicine Name"] = ASqlDataReader["medicine_name"].ToString();
                aDataRow["Dose"] = ASqlDataReader["dose"].ToString();
                aDataRow["Before/After Meal"] = ASqlDataReader["dose_rules"].ToString();
                aDataRow["Doctor Name"] = ASqlDataReader["doctor_name"].ToString();

                aDataTable.Rows.Add(aDataRow);
            }

            ASqlDataReader.Close();
            ASqlConnection.Close();
            return aDataTable;
        }

        public List<ViewPatientHistory> GetPatient(long voterId)
        {
            int i = 0;
            ASqlConnection.Open(); 
            string query = "SELECT * FROM v_patient_history WHERE voter_id = '" + voterId + "' ORDER BY date";
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlDataReader = ASqlCommand.ExecuteReader();

            List<ViewPatientHistory> aViewPatientHistoryList = new List<ViewPatientHistory>();

            while (ASqlDataReader.Read())
            {
                i++;
                ViewPatientHistory aViewPatientHistory = new ViewPatientHistory();
                aViewPatientHistory.CenterName = ASqlDataReader["name"].ToString();
                aViewPatientHistory.HistoryNo = i;

                aViewPatientHistoryList.Add(aViewPatientHistory);
            }

            ASqlDataReader.Close();
            ASqlConnection.Close();
            return aViewPatientHistoryList;
        }
    }
}
