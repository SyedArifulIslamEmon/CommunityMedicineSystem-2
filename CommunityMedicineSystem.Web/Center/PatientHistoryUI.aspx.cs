using System;
using System.Collections.Generic;
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
    public partial class PatientHistoryUI : System.Web.UI.Page
    {
        PatientManager aPatientManager = new PatientManager();
        CenterManager aCenterManager = new CenterManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != "center")
            {
                Response.Redirect("../index.aspx");
            }
            else
            {
                int centerId = Convert.ToInt32(Session["CenterId"]);
                centerLabel.Text = aCenterManager.FindById(centerId).Name;
                voterIdTextBox.Text = Request.QueryString[0];

                Patient aPatient = aPatientManager.GetInfoFromWebService(voterIdTextBox.Text);
                nameTextBox.Text = aPatient.Name;
                addressTextBox.Text = aPatient.Address;

                patientHistoryGridView.DataSource = aPatientManager.GetPatient(Convert.ToInt64(voterIdTextBox.Text));
                patientHistoryGridView.DataBind();
            }
        }

        protected void generatePDFButton_Click(object sender, EventArgs e)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=History_Details_" + voterIdTextBox.Text + ".pdf");
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView grd = new GridView();
            grd.DataSource = aPatientManager.GetData(Convert.ToInt64(voterIdTextBox.Text)).DefaultView;
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
    }
}