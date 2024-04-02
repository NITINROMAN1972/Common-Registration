using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SignUp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindVendorTypes();
        }
    }

    protected void BindVendorTypes()
    {
        string constr = ConfigurationManager.ConnectionStrings["Ginie"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("select RefID,VendorType from VendorType757", con))
            {
                con.Open();
                ddlInstituteType.DataSource = cmd.ExecuteReader();
                ddlInstituteType.DataTextField = "VendorType";
                ddlInstituteType.DataValueField = "RefID";
                ddlInstituteType.DataBind();
                con.Close();
            }
        }
        // Add a default item
        ddlInstituteType.Items.Insert(0, new ListItem("---- Select Vendor Type ----", ""));
    }


    protected void btnSignup_Click(object sender, EventArgs e)
    {

        // Get the values from the form fields
        string orgType = ddlInstituteType.SelectedItem.Text;
        string InstName = txtInstituteName.Text;
        string firstName = txtFirstName.Text;
        string email = txtEmail.Text;
        string mobileNumber = txtMobileNumber.Text;

        // Check if email and mobile number are valid before proceeding
        if (!IsValidEmail(email) || !IsValidMobileNumber(mobileNumber))
        {
            return; // Stop further processing if validation fails

        }

        // Generate a 3-digit random number
        Random random = new Random();
        int randomNum = random.Next(100, 1000); // Generates a random number between 100 and 999

        // Split the first name to get the first word before space for userLogin
        string userLogin = firstName.Split(' ')[0] + randomNum;

        // Generate a random password for the user
        string password = GenerateRandomPassword(firstName);

        // Set the user role as "Vendor"
        string userRole = "Vendor";

        // Call a method to save the data into the database
        
        SaveUserData(orgType, InstName, firstName, email, mobileNumber, userRole, userLogin, password);
        

        // Clear the form fields after saving      
        ddlInstituteType.ClearSelection(); // Clear selection of dropdown
        txtInstituteName.Text = ""; // Clear text field
        txtFirstName.Text = ""; // Clear text field
        txtEmail.Text = ""; // Clear text field
        txtMobileNumber.Text = ""; // Clear text field

        // Send email to the user with username and password
        SendEmail(email, firstName, userLogin, password);

        // Hide loading spinner and show SweetAlert popup for successful sign up
        string successScript = @"<script type='text/javascript'>
                                Swal.close();
                                Swal.fire({
                                    title: 'Sign Up Successful !',
                                    html: 'Your username and password have been sent to your registered email address for email verification.',
                                    icon: 'success',
                                    confirmButtonText: 'OK'
                                });
                            </script>";
        ScriptManager.RegisterStartupScript(this, GetType(), "SignUpSuccess", successScript, false);
    }


    private bool IsValidEmail(string email)
    {
        var emailPattern = new System.Text.RegularExpressions.Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        if (!emailPattern.IsMatch(email))
        {
            // Show SweetAlert for invalid email format
            string script = @"<script type='text/javascript'>
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: 'Please enter a valid email address.'
                            });
                         </script>";
            ScriptManager.RegisterStartupScript(this, GetType(), "InvalidEmail", script, false);
            return false;
        }
        return true;
    }


    private bool IsValidMobileNumber(string mobileNumber)
    {
        var mobilePattern = new System.Text.RegularExpressions.Regex(@"^0?[1-9][0-9]{9}$");
        if (!mobilePattern.IsMatch(mobileNumber))
        {
            // Show SweetAlert for invalid mobile number format
            string script = @"<script type='text/javascript'>
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: 'Please enter a valid 10-digit mobile number.'
                            });
                         </script>";
            ScriptManager.RegisterStartupScript(this, GetType(), "InvalidMobileNumber", script, false);
            return false;
        }
        return true;
    }




    private string GenerateRandomPassword(string username)
    {
        const string specialChars = "@#_"; // Special characters to be added after the username
        Random random = new Random();

        // Use StringBuilder for better performance
        StringBuilder passwordBuilder = new StringBuilder();

        // Add part of the username to the password (up to 4 characters)
        int charsToAdd = Math.Min(username.Length, 4); // Add up to 4 characters from the username
        for (int i = 0; i < charsToAdd; i++)
        {
            passwordBuilder.Append(username[i]);
        }

        // Add a special character from the defined set
        passwordBuilder.Append(specialChars[random.Next(specialChars.Length)]);

        // Add three random numbers
        for (int i = 0; i < 3; i++)
        {
            passwordBuilder.Append(random.Next(10)); // Add random number between 0 and 9
        }

        // Convert StringBuilder to string
        string password = passwordBuilder.ToString();

        return password;
    }

    private void SaveUserData(string orgType, string InstName, string firstName, string email, string mobileNumber, string userRole, string username, string password)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Ginie"].ConnectionString;
        string query = "INSERT INTO UserMaster757 (GID, Row, Active, Ver, UserID,VendorType, VendorName, UserName,UserFirstName, UserEmail, UserMobile, UserRole, UserLogin, UserPassword, SaveBy, SaveOn) " +
                       "VALUES (@GID, @Row, @Active, @Ver, @UserID ,@VendorType, @VendorName, @UserName,@UserFirstName, @UserEmail, @UserMobile, @UserRole, @UserLogin, @UserPassword, @SaveBy, @SaveOn)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                Guid _Guid = Guid.NewGuid();
                Guid ActGuid = Guid.NewGuid();

                command.Parameters.AddWithValue("@GID", _Guid);
                command.Parameters.AddWithValue("@Row", ActGuid);
                command.Parameters.AddWithValue("@Active", "1");
                command.Parameters.AddWithValue("@Ver", "1");
                command.Parameters.AddWithValue("@UserID", GetUserID(connection));
                command.Parameters.AddWithValue("@VendorType", ddlInstituteType.SelectedValue);
                command.Parameters.AddWithValue("@VendorName", InstName);
                command.Parameters.AddWithValue("@UserName", firstName);
                command.Parameters.AddWithValue("@UserFirstName", firstName);
                command.Parameters.AddWithValue("@UserEmail", email);
                command.Parameters.AddWithValue("@UserMobile", mobileNumber);
                command.Parameters.AddWithValue("@UserRole", userRole);
                command.Parameters.AddWithValue("@UserLogin", username);
                command.Parameters.AddWithValue("@UserPassword", password);

                // Set SaveBy to null (assuming it's nullable)
                command.Parameters.AddWithValue("@SaveBy", GetUserID(connection));

                // Set SaveOn to the current date and time
                command.Parameters.AddWithValue("@SaveOn", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    private int GetUserID(SqlConnection connection)
    {
        int userID = 0;
        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            string query = "SELECT ISNULL(MAX(UserID), 0) + 1 FROM UserMaster757";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    userID = Convert.ToInt32(result);
                }
            }
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        return userID;
    }


    private void UpdateSaveByAndSaveOn(int userID, SqlConnection connection)
    {
        string updateQuery = "UPDATE UserMaster874 SET SaveBy = @SaveBy, SaveOn = @SaveOn WHERE UserID = @UserID";
        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
        {
            updateCommand.Parameters.AddWithValue("@SaveBy", userID);
            updateCommand.Parameters.AddWithValue("@SaveOn", DateTime.Now);
            updateCommand.Parameters.AddWithValue("@UserID", userID);
            updateCommand.ExecuteNonQuery();
        }
    }



    private void SendEmail(string email, string firstName, string username, string password)
    {
        try
        {
            string senderEmail = "giti4277@gmail.com"; // Your Gmail address
            string senderPassword = "fimr vnyp atdt qoff "; // Your Gmail password
            string smtpServer = "smtp.gmail.com";
            int port = 587;

            string subject = "Welcome to MMGPA Procurement Management System!";
            string body = $@"Dear {firstName},

Thank you for choosing MMGPA Procurement Management System. We extend a warm welcome to you as a new member of the Maharashtra Medical Goods Procurement Authority's (MMGPA) dedicated Procurement Management System. We are excited to have you join our platform and embark on this journey with us. Our Procurement Management System is specially designed to meet the unique needs of MMGPA, providing a seamless and efficient solution for the acquisition of medical equipment and medicines across Maharashtra.

Your login credentials are as follows:

UserName: {username}
Password: {password}

Best regards,

Maharashtra Medical Goods Procurement Authority (MMGPA).";

            // Create a new SmtpClient instance
            using (var smtpClient = new System.Net.Mail.SmtpClient(smtpServer, port))
            {
                // Enable SSL/TLS connection
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                // Enable SSL for the client
                smtpClient.EnableSsl = true;

                // Set the credentials
                smtpClient.Credentials = new NetworkCredential(senderEmail.Trim(), senderPassword.Trim());

                // Create a new MailMessage instance
                using (MailMessage mailMessage = new MailMessage(senderEmail, email))
                {
                    // Set the subject and body of the email
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    try
                    {
                        // Send the email
                        smtpClient.Send(mailMessage);
                        Console.WriteLine("Email sent successfully!");
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception or display an error message
            Console.WriteLine("Error sending email: " + ex.Message);
        }
    }




}