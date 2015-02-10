using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityMedicineSystem.BLL;
using CommunityMedicineSystem.DAO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace CommunityMedicineSystem.Web.Center
{
    public partial class TreatmentGivenUI : System.Web.UI.Page
    {
        MedicineManager aMedicineManager = new MedicineManager();
        PatientManager aPatientManager = new PatientManager();
        DiseaseManager aDiseaseManager = new DiseaseManager();
        DoctorManager aDoctorManager = new DoctorManager();
        DistrictAndThanaManager aDistrictAndThanaManager = new DistrictAndThanaManager();
        TreatmentManager aTreatmentManager = new TreatmentManager();
        CenterManager aCenterManager = new CenterManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["user"] != "center")
            //{
            //    Response.Redirect("../index.aspx");
            //}
            //else
            //{
                int centerId = 3;
                //int centerId = Convert.ToInt32(Session["CenterId"]);
                centerLabel.Text = aCenterManager.FindById(centerId).Name;
                if (!IsPostBack)
                {
                    medicineDropDownList.DataSource = aMedicineManager.GetSelectedMedicines(centerId);
                    medicineDropDownList.DataBind();

                    doctorDropDownList.DataSource = aDoctorManager.GetSelectedDoctors(centerId);
                    doctorDropDownList.DataTextField = "Name";
                    doctorDropDownList.DataValueField = "Id";
                    doctorDropDownList.DataBind();

                    diseaseDropDownList.DataSource = aDiseaseManager.GetAllDiseases();
                    diseaseDropDownList.DataTextField = "Name";
                    diseaseDropDownList.DataBind();
                }
            //}
        }

        protected void showDetailsButton_Click(object sender, EventArgs e)
        {
            Patient aPatient = aPatientManager.GetInfoFromWebService(voterIdTextBox.Text);
            if (aPatient == null)
            {
                //msgLabel.Text = "Voter ID is not Valid";
            }
            else
            {
                serviceGivenTextBox.Text = aPatientManager.GetTotalNumberOfService(Convert.ToInt64(voterIdTextBox.Text)).ToString();
                nameTextBox.Text = aPatient.Name;
                addressTextBox.Text = aPatient.Address;
                ageTextBox.Text = aPatient.Age.ToString();
            }
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add(centerLabel.Text);
            dt.Columns.Add("Disease");
            dt.Columns.Add("Medicine");
            dt.Columns.Add("Dose");
            dt.Columns.Add("Before/After Meal");
            dt.Columns.Add("Quantity Given");
            dt.Columns.Add("Note");
            

            Patient aPatient = new Patient();

            var diseaseList = diseaseName.Value;
            diseaseName.Value = "";
            string[] disease = diseaseList.Split(',');

            var medicineList = medicineName.Value;
            medicineName.Value = "";
            string[] medicine = medicineList.Split(',');

            var doseList = medicineDose.Value;
            medicineDose.Value = "";
            string[] dose = doseList.Split(',');

            var doseRuleList = medicineDoseType.Value;
            medicineDoseType.Value = "";
            string[] doseRule = doseRuleList.Split(',');

            var quantityList = medicineQuantityGiven.Value;
            medicineQuantityGiven.Value = "";
            string[] quantity = quantityList.Split(',');

            var noteList = notes.Value;
            notes.Value = "";
            string[] note = noteList.Split(',');

            string[] address = addressTextBox.Text.Split(' ');
            string districtName = address[address.Length - 1];

            aPatient.VoterId = Convert.ToInt64(voterIdTextBox.Text);
            aPatient.DistrictId = aDistrictAndThanaManager.FindDistrict(districtName).Id;

            aPatientManager.SaveService(aPatient);

            Treatment aTreatment = new Treatment();

            aTreatment.ServiceTakenId = aPatientManager.GetServiceTakenId();
            aTreatment.Observation = observationTextBox.Text;
            aTreatment.ServiceDate = DateTime.Parse(dateTextBox.Value);
            aTreatment.DoctorId = Convert.ToInt32(doctorDropDownList.SelectedValue);
            aTreatment.CenterId = Convert.ToInt32(Session["CenterId"]);

            for (int i = 0; i < disease.Length - 1; i++)
            {
                Disease aDisease = aDiseaseManager.Find(disease[i]);
                aTreatment.DiseaseId = aDisease.Id;
                Medicine aMedicine = aMedicineManager.Find(medicine[i]);
                aTreatment.MedicineId = aMedicine.Id;
                aTreatment.Dose = dose[i];
                aTreatment.DoseType = doseRule[i];
                aTreatment.Quantity = Convert.ToInt32(quantity[i]);
                aTreatment.Note = note[i];
                aTreatmentManager.SaveTreatment(aTreatment);

                MedicineStockInCenter aMedicineStockInCenter = new MedicineStockInCenter();
                aMedicineStockInCenter.MedicineId = aTreatment.MedicineId;
                aMedicineStockInCenter.Quantity = aTreatment.Quantity;
                aMedicineStockInCenter.CenterId = aTreatment.CenterId;

                aMedicineManager.DeductMedicineFromCenter(aMedicineStockInCenter);

                DataRow dr = dt.NewRow();
                dr["Disease"] = medicine[i];
                dr["Medicine"] = disease[i];
                dr["Dose"] = dose[i];
                dr["Before/After Meal"] = doseRule[i];
                dr["Quantity Given"] = quantity[i];
                dr["Note"] = note[i];
                dt.Rows.Add(dr);
            }

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Treatment_" +voterIdTextBox.Text+ ".pdf");
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView grd = new GridView();
            grd.DataSource = dt.DefaultView;
            grd.DataBind();
            grd.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 50f, 50f, 50f, 50f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }

        protected void logoutButton_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("../index.aspx");
        }

        protected void showAllHistoryButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("PatientHistoryUI.aspx?VoterId=" + voterIdTextBox.Text);
        }
    }
}