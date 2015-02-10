using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommunityMedicineSystem.BLL;

namespace CommunityMedicineSystem.Web.HeadOffice
{
    public partial class DiseaseWiseReportUI : System.Web.UI.Page
    {
        DiseaseManager aDiseaseManager = new DiseaseManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                diseaseDropDownList.DataSource = aDiseaseManager.GetAllDiseases();
                diseaseDropDownList.DataTextField = "Name";
                diseaseDropDownList.DataValueField = "Name";
                diseaseDropDownList.DataBind();
            }
        }

        protected void showButton_Click(object sender, EventArgs e)
        {
            showGridView.DataSource = aDiseaseManager.GetDiseaseReport(fromDateTextBox.Value, toDateTextBox.Value,
                diseaseDropDownList.SelectedValue);
            showGridView.DataBind();
        }
    }
}