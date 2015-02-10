using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityMedicineSystem.DAO;

namespace CommunityMedicineSystem.DAL
{
    public class DiseaseDBGateway : CommonDBGateway
    {
        public int Save(Disease aDisease)
        {
            string query = "INSERT INTO tbl_disease VALUES ('" + aDisease.Name + "','" + aDisease.Description + "','" +
                           aDisease.TreatmentProcedure + "','" + aDisease.PreferredDrug + "')";
            ASqlConnection.Open();
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            int rowAffected = ASqlCommand.ExecuteNonQuery();
            ASqlConnection.Close();
            return rowAffected;
        }

        public Disease Find(string name)
        {
            string query = "SELECT * FROM tbl_disease WHERE name= '" + name + "' ";
            ASqlConnection.Open();
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlDataReader = ASqlCommand.ExecuteReader();
            if (ASqlDataReader.HasRows)
            {
                ASqlDataReader.Read();
                Disease aDisease = new Disease();
                aDisease.Id = (int)ASqlDataReader["id"];
                aDisease.Name = ASqlDataReader["name"].ToString();
                ASqlDataReader.Close();
                ASqlConnection.Close();
                return aDisease;
            }
            else
            {
                ASqlDataReader.Close();
                ASqlConnection.Close();
                return null;
            }
        }

        public List<Disease> GetAllDiseases()
        {
            List<Disease> diseaseList = new List<Disease>();
            string query = "SELECT * FROM tbl_disease";
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlConnection.Open();
            ASqlDataReader = ASqlCommand.ExecuteReader();
            Disease aDisease;

            while (ASqlDataReader.Read())
            {
                aDisease = new Disease();
                aDisease.Id = (int)ASqlDataReader["id"];
                aDisease.Name = ASqlDataReader["name"].ToString();
                aDisease.Description = ASqlDataReader["description"].ToString();
                aDisease.TreatmentProcedure = ASqlDataReader["treatment_procedure"].ToString();
                aDisease.PreferredDrug = ASqlDataReader["preferred_drug"].ToString();

                diseaseList.Add(aDisease);
            }

            ASqlDataReader.Close();
            ASqlCommand.Dispose();
            ASqlConnection.Close();
            return diseaseList;
        }

        public List<DiseaseReport> GetDiseaseReport(string fromDate, string toDate, string diseaseName)
        {
            string query = "SELECT t1.district_name,COUNT(t1.district_name) AS total_patient,t1.population FROM (SELECT v1.district_name,v1.voter_id,v1.population FROM v_district_wise_patient v1 WHERE v1.disease_name='" +diseaseName+ "' and v1.date BETWEEN '" +fromDate+ "' AND '" +toDate+ "') t1 GROUP BY t1.district_name,t1.population";
             
            ASqlConnection.Open();
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlDataReader = ASqlCommand.ExecuteReader();

            List<DiseaseReport> diseaseReportList = new List<DiseaseReport>();

            while (ASqlDataReader.Read())
            {
                DiseaseReport aDiseaseReport = new DiseaseReport();
                aDiseaseReport.DistrictName = ASqlDataReader["district_name"].ToString();
                aDiseaseReport.TotalPatient = (int) ASqlDataReader["total_patient"];
                aDiseaseReport.PercentagePatient = (double) ((int)ASqlDataReader["total_patient"] * 100) / (int)ASqlDataReader["population"];

                diseaseReportList.Add(aDiseaseReport);
            }

            ASqlDataReader.Close();
            ASqlCommand.Dispose();
            ASqlConnection.Close();
            return diseaseReportList;
        }
    }
}
