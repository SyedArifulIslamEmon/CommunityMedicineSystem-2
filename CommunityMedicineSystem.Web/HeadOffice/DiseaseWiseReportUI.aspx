<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiseaseWiseReportUI.aspx.cs" Inherits="CommunityMedicineSystem.Web.HeadOffice.DiseaseWiseReportUI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>Head Office - Community Medicine System</title>
    <link href='http://fonts.googleapis.com/css?family=Open+Sans' rel='stylesheet' type='text/css' />

    <%--<link href="assets/css/bootstrap.css" rel="stylesheet" />--%>
    <link href="../Content/bootstrap.css" rel="stylesheet" />
    <link href="../Content/bootstrap-theme.css" rel="stylesheet" />
    <link href="../Content/datepicker.css" rel="stylesheet" />
    <link href="../assets/css/font-awesome.min.css" rel="stylesheet" />

    <link href="../assets/css/style-slide.css" rel="stylesheet" />
    <link href="../assets/css/custom.css" rel="stylesheet" />

    <link href="../assets/css/style.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server" class="form-horizontal">

        <div class="navbar navbar-inverse navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <img class="img-circle" src="../assets/img/team/logo.png" alt="">
                </div>
                <div class="navbar-collapse collapse">
                    <ul class="nav navbar-nav navbar-right">
                        <li><a href="../index.aspx">Home</a></li>
                    </ul>
                </div>

            </div>
        </div>

        <div id="about-section">
            <div class="container">
                <div class="row main-top-margin text-center">
                    <div class="col-md-8 col-md-offset-2 col-sm-12">
                        <h2>Welcome to Head Office</h2>
                        <hr />
                        <h4>
                            <strong>Disease-wise Report</strong>
                        </h4>
                    </div>

                </div>
                <hr />

                <div class="form-group">
                    <label for="diseaseDropDownList" class="col-sm-1 control-label">Disease</label>
                    <div class="col-sm-2">
                        <asp:DropDownList ID="diseaseDropDownList" runat="server" CssClass="form-control" AutoPostBack="False" OnSelectedIndexChanged="districtDropDownList_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <label for="diseaseDropDownList" class="col-sm-2 control-label">Date between</label>
                    <div class="col-sm-2">
                        <input type="text" runat="server" id="fromDateTextBox" class="form-control" readonly="readonly" />
                    </div>
                    <label for="diseaseDropDownList" class="col-sm-1 control-label">and</label>
                    <div class="col-sm-2">
                        <input type="text" runat="server" id="toDateTextBox" class="form-control" readonly="readonly" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="showButton" runat="server" Text="Show" CssClass="btn btn-primary" OnClick="showButton_Click" />
                    </div>
                </div>
            <br />

            <center>
                    <asp:GridView ID="showGridView" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="DistrictName" HeaderText="District Name" ItemStyle-Width="150">
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalPatient" HeaderText="Total Patients" ItemStyle-Width="150">
                            <ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PercentagePatient" HeaderText="% Over Population" ItemStyle-Width="120">
                            <ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
                </center>

        </div>
        </div>

        <div id="space">
        </div>


        <div class="text-center" id="social">

            <a href="#"><i class="fa fa-facebook-square fa-3x color-facebook"></i></a>
            <a href="#"><i class="fa fa-twitter-square fa-3x color-twitter"></i></a>
            <a href="#"><i class="fa fa-google-plus-square fa-3x color-google-plus"></i></a>
            <a href="#"><i class="fa fa-linkedin-square fa-3x color-linkedin"></i></a>
            <a href="#"><i class="fa fa-pinterest-square fa-3x color-pinterest"></i></a>

        </div>

        <div id="footer">
            <div class="container">
                <div class="row ">
                    &copy; 2015 Team Engine Box | All Right Reserved
                </div>
            </div>
        </div>

        <script src="../Scripts/jquery-2.1.3.js"></script>
        <script src="../Scripts/bootstrap.js"></script>
        <script src="../Scripts/bootstrap.min.js"></script>
        <script src="../Scripts/bootstrap-datepicker.js"></script>
        <script src="../assets/js/modernizr.custom.79639.js"></script>
        <script src="../assets/js/jquery.ba-cond.min.js"></script>
        <script src="../assets/js/jquery.slitslider.js"></script>
        <script src="../assets/js/custom.js"></script>

        <script>
            $(document).ready(function () {
                $(function () {
                    $("#fromDateTextBox").datepicker();
                    $("#toDateTextBox").datepicker();
                });
            });

        </script>

    </form>
</body>

</html>
