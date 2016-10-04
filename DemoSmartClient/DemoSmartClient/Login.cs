using DemoSmartClient.CryptoLib;
using DemoSmartClient.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Windows.Forms;
using System.Configuration;
using System.Web.Script.Serialization;
using DemoSmartClient.BL;
using DemoSmartClient.Concrete;
using Newtonsoft.Json.Linq;
using DemoSmartClient.CommonLib;
using System.ComponentModel;
using System.Timers;
namespace DemoSmartClient
{
    public partial class Login : Form
    {
        LoginBL LoginBL;
        ManageDBBL _ManageDBBL;

        public Login()
        {
            InitializeComponent();
            LoginBL = new LoginBL(new LoginConcrete());
            if (CheckInternetConnection.CheckNet() == true)
            {
                NetConnectionChecker.Connection = true;
            }
            else
            {
                NetConnectionChecker.Connection = false;
            }
            _ManageDBBL = new ManageDBBL(new ManageDBConcrete());

            #region UnComment this part only to create New Database and Tables
            //  _ManageDBBL.CreateSqlLiteDatabaseBL();
            //  _ManageDBBL.SetpasswordBL();
            //  _ManageDBBL.ChangepasswordBL();
            //  _ManageDBBL.RemovepasswordBL();
            //  _ManageDBBL.CreatetbproductBL();
            //  _ManageDBBL.Createt_Login_TableBL(); 
            #endregion
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtusername.Text == "")
                {
                    MessageBox.Show("Enter Username");
                }
                else if (txtpassword.Text == "")
                {
                    MessageBox.Show("Enter Password");
                }
                else
                {
                    string Username = txtusername.Text;
                    string Password = txtpassword.Text;

                    //Local Database check 
                    var result = LoginBL.CheckUserExists(Username, EncryptandDecryptAES.Encrypt(Password));

                    if (string.IsNullOrEmpty(result) && NetConnectionChecker.Connection == false)
                    {
                        MessageBox.Show("Login Cannot be done need internet Connection");
                    }
                    else if (string.IsNullOrEmpty(result) && NetConnectionChecker.Connection == true)
                    {
                        //Method is use to Validate User Credentials from Web Server
                        ValidateUserandGetResponse(Username, Password);
                    }
                    else
                    {
                        ShareObject.CLientIDToken = result;
                        ShareObject.Username = Username;
                        MessageBox.Show("Login Successfully");
                        this.Hide();
                        Login frm1 = new Login();
                        frm1.Close();
                        AddProduct addpro = new AddProduct();
                        addpro.Show();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// //Method is use to Validate User Credentials from Web Server Using WEB API
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        public void ValidateUserandGetResponse(string Username, string Password)
        {
            try
            {
                UserLogin objvm = new UserLogin()
                   {
                       Username = Username,
                       Password = EncryptandDecryptAES.Encrypt(Password)
                   };

                ShareObject.Username = Username;
                using (var client = new WebClient())
                {
                    string ClientToken = ConfigurationManager.AppSettings["CLientIDToken"].ToString();
                    string keyValue = ConfigurationManager.AppSettings["keyValue"].ToString();
                    string IVValue = ConfigurationManager.AppSettings["IVValue"].ToString();


                    Uri URI = new Uri(ConfigurationManager.AppSettings["LoginURI"].ToString());
                    client.Headers.Add("Content-Type:application/json");
                    client.Headers.Add("APIKEY", GenerateToken.CreateToken(Username, ClientToken, DateTime.Now.Ticks));
                    client.Headers.Add("Accept:application/json");
                    client.UploadStringCompleted += new UploadStringCompletedEventHandler(Callback);

                    string SerializeData = JsonConvert.SerializeObject(objvm);

                    byte[] buffer = EncryptionDecryptorTripleDES.Encryption(SerializeData, keyValue, IVValue);
                    client.UploadStringAsync(URI, Convert.ToBase64String(buffer));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Callback Method Gets Called when WEB API is Giving Response 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Callback(object sender, UploadStringCompletedEventArgs e)
        {
            string ClientToken = ConfigurationManager.AppSettings["CLientIDToken"].ToString();
            string keyValue = ConfigurationManager.AppSettings["keyValue"].ToString();
            string IVValue = ConfigurationManager.AppSettings["IVValue"].ToString();

            if (e.Error != null)
            {


            }
            if (e.Result != null || !string.IsNullOrEmpty(e.Result))
            {
                string finalData = JToken.Parse(e.Result).ToString();

                string data = EncryptionDecryptorTripleDES.Decryption(finalData, keyValue, IVValue);

                UserLogin userlogin = JsonConvert.DeserializeObject<UserLogin>(data);

                if (userlogin != null)
                {
                    if (!string.IsNullOrEmpty(userlogin.RegistrationID))
                    {
                        var resultdata = LoginBL.CheckUserExists(userlogin.Username, userlogin.Password); //Local Database check 

                        if (!string.IsNullOrEmpty(resultdata))
                        {
                            ShareObject.CLientIDToken = userlogin.RegistrationID;
                            MessageBox.Show("Login Successfully");
                            this.Hide();
                            Login frm1 = new Login();
                            frm1.Close();
                            AddProduct addpro = new AddProduct();
                            addpro.Show();
                        }
                        else
                        {
                            var result = LoginBL.InsertLoginData(userlogin);

                            if (result != 0)
                            {
                                ShareObject.CLientIDToken = userlogin.RegistrationID;
                                MessageBox.Show("Login Successfully");
                                this.Hide();
                                Login frm1 = new Login();
                                frm1.Close();
                                AddProduct addpro = new AddProduct();
                                addpro.Show();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("InValid Credentials");
                    }
                }
                else
                {
                    MessageBox.Show("InValid Credentials");
                }
            }
            else
            {
                MessageBox.Show("InValid Credentials");
            }
        }

        private void txtusername_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtusername.Text, "^[a-zA-Z]"))
            {
                MessageBox.Show("This textbox accepts only alphabetical characters");
                txtusername.Text = string.Empty;
            }
        }

        /// <summary>
        /// In this event we check internet connection is available or not and according to 
        /// this we display image indicator, if internet connection is not there then 
        /// it will display red image and if internet connection is there then it will display green image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TM1_Tick(object sender, EventArgs e)
        {
            if (CheckInternetConnection.CheckNet() == true)
            {
                PBimgRed.Visible = false;
                NetConnectionChecker.Connection = true;
                if (PBimggreen.Visible == true)
                {
                    PBimggreen.Visible = false;
                }
                else
                {
                    PBimggreen.Visible = true;
                }
            }
            else
            {
                NetConnectionChecker.Connection = false;
                PBimggreen.Visible = false;

                if (PBimgRed.Visible == true)
                {
                    PBimgRed.Visible = false;
                }
                else
                {
                    PBimgRed.Visible = true;
                }
            }
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this application?",
                         "Confirmation",
                          MessageBoxButtons.YesNo,
                          MessageBoxIcon.Information) == DialogResult.Yes)
            {

                Application.Exit();
            }

        }

    }
}
