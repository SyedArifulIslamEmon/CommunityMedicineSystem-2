using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityMedicineSystem.DAL;
using CommunityMedicineSystem.DAO;

namespace CommunityMedicineSystem.BLL
{
    public class DiseaseManager
    {
        DiseaseDBGateway aDiseaseDbGateway = new DiseaseDBGateway();
        DistrictAndThanaDBGateway aDistrictAndThanaDbGateway = new DistrictAndThanaDBGateway();
        public int Save(Disease aDisease)
        {
            if (aDiseaseDbGateway.Find(aDisease.Name) == null)
            {
                return aDiseaseDbGateway.Save(aDisease);
            }
            else
            {
                return 0;
            }
        }

        public List<Disease> GetAllDiseases()
        {
            return aDiseaseDbGateway.GetAllDiseases();
        }

        public Disease Find(string name)
        {
            return aDiseaseDbGateway.Find(name);
        }

        public List<DiseaseReport> GetDiseaseReport(string fromDate, string toDate, string diseaseName)
        {
            return aDiseaseDbGateway.GetDiseaseReport(fromDate, toDate, diseaseName);
        }
    }
}
