<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignUp.aspx.cs" Inherits="SignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    

    <title>Signup Page</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />


    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

     <!-- Select2 library CSS and JS -->
    <link href="select2/select2.min.css" rel="stylesheet" />
    <script src="select2/select2.min.js"></script>

    <!--Custom CSS and JS-->
    <link href="StyleSheet.css" rel="stylesheet" />
    <script src="JavaScript.js"></script>

    <style>
        .custom-width {
            width: 97%; /* Adjust the percentage as needed */
            /* You can set a maximum width if necessary */
        }

        .custom-button {
            background-color: #3F51B5;
            color: #fff;
            transition: background-color 0.3s ease; /* Smooth transition for color change */
        }

            .custom-button:hover {
                background-color: #6573c3; /* Darker shade of blue on hover */
                color: #F5F5F5;
            }

        .custom-width-sm {
            max-width: 400px; /* Adjust this value as needed */
        }

        .card label b {
            font-weight: bold;
        }

        .lds-eclipse {
            width: 80px;
            height: 80px;
            position: relative;
        }

            .lds-eclipse div {
                position: absolute;
                top: 8px;
                width: 65px;
                height: 65px;
                border-radius: 50%;
                background: #fff;
                animation-timing-function: cubic-bezier(0, 1, 1, 0);
            }

                .lds-eclipse div:nth-child(1) {
                    left: 8px;
                    animation: lds-eclipse1 0.6s infinite;
                }

                .lds-eclipse div:nth-child(2) {
                    left: 8px;
                    animation: lds-eclipse2 0.6s infinite;
                }

                .lds-eclipse div:nth-child(3) {
                    left: 32px;
                    animation: lds-eclipse2 0.6s infinite;
                }

                .lds-eclipse div:nth-child(4) {
                    left: 56px;
                    animation: lds-eclipse3 0.6s infinite;
                }

        @keyframes lds-eclipse1 {
            0% {
                transform: scale(0);
            }

            100% {
                transform: scale(1);
            }
        }

        @keyframes lds-eclipse3 {
            0% {
                transform: scale(1);
            }

            100% {
                transform: scale(0);
            }
        }

        @keyframes lds-eclipse2 {
            0% {
                transform: translate(0, 0);
            }

            100% {
                transform: translate(24px, 0);
            }
        }

        .centered-spinner {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%) scale(1.2); /* Increase size by 20% */
        }
    </style>


    <script>
        function showSpinner() {
            document.getElementById('loadingSpinner').style.display = 'block';
        }
    </script>
</head>
<body>
   <form id="form1" runat="server">
        <div class="container mt-4 mx-auto col-md-8">
            <div class="card shadow p-3 mb-5 bg-body rounded">
                <div class="card-header  text-dark text-center">
                    <h2 class="fw-bold">Vendor Registration</h2>
                </div>
                <div class="card-body">
                     <div class="form-group">
                        <label for="instituteType" class="fw-bold">Vendor Type:</label>
                        <asp:DropDownList ID="ddlInstituteType" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="instituteName" class="fw-bold">Vendor Name:</label>
                        <asp:TextBox ID="txtInstituteName" CssClass="form-control rounded-0" runat="server" placeholder="Enter Vendor Name" Required="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="firstName" class="fw-bold">Contact Person Name:</label>
                        <asp:TextBox ID="txtFirstName" CssClass="form-control rounded-0" runat="server" placeholder="Enter First Name" Required="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="email" class="fw-bold">Email ID:</label>
                        <asp:TextBox ID="txtEmail" CssClass="form-control rounded-0" runat="server" placeholder="Enter Email ID" Required="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="mobileNumber" class="fw-bold">Mobile Number:</label>
                        <asp:TextBox ID="txtMobileNumber" CssClass="form-control rounded-0" runat="server" placeholder="Enter Mobile Number" Required="true"></asp:TextBox>
                    </div>
                    <div class="text-center">
                        <asp:Button ID="btnSignup" runat="server" CssClass="btn mt-3 custom-button" Text="Sign Up" OnClick="btnSignup_Click" OnClientClick="showSpinner()" />

                    </div>
                    <div id="loadingSpinner" style="display: none;">
                        <!-- Include your SVG file directly -->
                        <object type="image/svg+xml" data="Eclipse-1s-250px.svg" class="centered-spinner"></object>
                        
                    </div>                    

                </div>
            </div>
        </div>
    </form>



    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</body>
</html>
