using DemoSmartClient.BL;
using DemoSmartClient.CommonLib;
using DemoSmartClient.Concrete;
using DemoSmartClient.CryptoLib;
using DemoSmartClient.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DemoSmartClient
{
    public partial class AddProduct : Form
    {
        ProductBL ProductBL;

        public AddProduct()
        {
            try
            {
                InitializeComponent();
                if (CheckInternetConnection.CheckNet() == true)
                {
                    NetConnectionChecker.Connection = true;
                }
                else
                {
                    NetConnectionChecker.Connection = false;
                }
                ProductBL = new ProductBL(new ProductConcrete());
                DGData.DataSource = ProductBL.GetData(ShareObject.CLientIDToken);
                DGData.AllowUserToAddRows = false;
                DGData.ReadOnly = true;

                lblStatus.ForeColor = Color.Black;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public void Clear()
        {
            TxtDescription.Text = string.Empty;
            TxtProductName.Text = string.Empty;
            TxtPrice.Text = string.Empty;
            TxtProductNumber.Text = string.Empty;
            ComboColor.SelectedIndex = -1;
            ComboClass.SelectedIndex = -1;
        }

        private void TxtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }


        /// <summary>
        /// Saving Product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (TxtProductNumber.Text == "")
                {
                    MessageBox.Show("Enter Product Number");
                    TxtProductNumber.Focus();
                }
                else if (TxtProductName.Text == "")
                {
                    MessageBox.Show("Enter Product Name");
                    TxtProductName.Focus();
                }
                else if (TxtPrice.Text == "")
                {
                    MessageBox.Show("Enter Price");
                    TxtPrice.Focus();
                }
                else if (ComboColor.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Color");
                    ComboColor.Focus();
                }
                else if (ComboClass.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Class");
                    ComboClass.Focus();
                }
                else
                {
                    string message = "Are you sure you want save this record";
                    if (MessageBox.Show(message, "confirmation", MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string selectedComboColor = ComboColor.Items[ComboColor.SelectedIndex].ToString();
                        string selectedComboClass = ComboClass.Items[ComboClass.SelectedIndex].ToString();

                        Product objproduct = new Product();
                        objproduct.Color = selectedComboColor;
                        objproduct.Description = TxtDescription.Text;
                        objproduct.Name = TxtProductName.Text;
                        objproduct.Price = Convert.ToDecimal(TxtPrice.Text);
                        objproduct.ProductNumber = TxtProductNumber.Text;
                        objproduct.ProductClass = selectedComboClass;
                        objproduct.CreatedDate = DateTime.Now;
                        objproduct.CLientIDToken = ShareObject.CLientIDToken;
                        // Calling Business Layer
                        ProductBL.AddProduct(objproduct);
                        // Binding data to DataGridView
                        DGData.DataSource = ProductBL.GetData(ShareObject.CLientIDToken); 
                        DGData.ReadOnly = true;
                        Clear();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Deleting Products
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "Are you sure you want to delete this record(s)";
                if (MessageBox.Show(message, "confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (DataGridViewRow item in this.DGData.SelectedRows)
                    {
                        int productID = Convert.ToInt32(item.Cells[0].Value);
                        ProductBL.DeleteProduct(productID);
                    }
                    DGData.DataSource = ProductBL.GetData(ShareObject.CLientIDToken);
                    DGData.AllowUserToAddRows = false;
                    DGData.ReadOnly = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// This is First Event which is called by BackgroundWorker 
        /// Here we Check Internet Connection and Get Product data from DataBase and Call DataPusher Method to Push data to Web server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BGWorkerDataPusher_DoWork(object sender, DoWorkEventArgs e)
        {
            if (NetConnectionChecker.Connection == true)
            {
                BackgroundProcessLogicMethod();
            }
        }

        public void BackgroundProcessLogicMethod()
        {
            var data = ProductBL.GetData(ShareObject.CLientIDToken);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                Product Product = new Product();
                Product.ProductID = Convert.ToInt32(data.Rows[i]["ProductID"]);
                Product.Name = Convert.ToString(data.Rows[i]["Name"]);
                Product.Price = Convert.ToDecimal(data.Rows[i]["Price"]);
                Product.Color = Convert.ToString(data.Rows[i]["Color"]);
                Product.Description = Convert.ToString(data.Rows[i]["Description"]);
                Product.ProductClass = Convert.ToString(data.Rows[i]["ProductClass"]);
                Product.CreatedDate = Convert.ToDateTime(data.Rows[i]["CreatedDate"]);
                Product.CLientIDToken = Convert.ToString(data.Rows[i]["CLientIDToken"]);
                Product.ProductNumber = Convert.ToString(data.Rows[i]["ProductNumber"]);
                DataPusher(Product);
                if (data.Rows.Count > 0)
                {
                    BGWorkerDataPusher.ReportProgress((data.Rows.Count * 10));
                }
                else
                {
                    BGWorkerDataPusher.ReportProgress((10 * 10));
                }
            }
        }


        /// <summary>
        /// Method takes Product object as input for Pushing Data on Web Server
        /// </summary>
        /// <param name="Product"></param>
        public void DataPusher(Product Product)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string ClientToken = ConfigurationManager.AppSettings["CLientIDToken"].ToString();
                    Uri URI = new Uri(ConfigurationManager.AppSettings["LiveURI"].ToString());
                    client.Headers.Add("Content-Type:application/json");
                    client.Headers.Add("Accept:application/json");

                    //Token APIKEY
                    client.Headers.Add("APIKEY", 
                    GenerateToken.CreateToken(ShareObject.Username, ClientToken, DateTime.Now.Ticks));
                    //Setting Call back method
                    client.UploadStringCompleted += new UploadStringCompletedEventHandler(Callback);

                    //key and IV 
                    string keyValue = ConfigurationManager.AppSettings["keyValue"].ToString();
                    string IVValue = ConfigurationManager.AppSettings["IVValue"].ToString();

                    //Serializing Object
                    string SerializeData = JsonConvert.SerializeObject(Product);
                    //Encrypting Serialized Object
                    byte[] buffer = EncryptionDecryptorTripleDES.Encryption(SerializeData, keyValue, IVValue);

                    //Converting bytes To Base64String and then Upload data
                    client.UploadStringAsync(URI, Convert.ToBase64String(buffer));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void Callback(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                }
                else if (e.Result != null || !string.IsNullOrEmpty(e.Result))
                {
                    //getting ProductID in Response
                    string finalData = e.Result;
                    if (finalData != null)
                    {
                        //Deleting Product by ProductID
                        ProductBL.DeleteProduct(Convert.ToInt32(finalData));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Used for dispalying Progress Bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                //Setting Values to display 
                progressBar1.Value = e.ProgressPercentage;
                //displaying Percentage in label   
                lblStatus.Text = "Processing......" + progressBar1.Value.ToString() + "%";
                //Refreshing DataGridView
                DGData.DataSource = null;
                DGData.Update();
                DGData.Refresh();
                //Rebinding DataGridView
                DGData.DataSource = ProductBL.GetData(ShareObject.CLientIDToken);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Timer Tick Event is used for Calling BackgroundWorker on particlar Interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TMSyncData_Tick(object sender, EventArgs e)
        {
            if (!BGWorkerDataPusher.IsBusy)
            {
                BGWorkerDataPusher.RunWorkerAsync();
            }
        }


        /// <summary>
        /// This event is Called for Refreshing DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BGWorkerDataPusher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DGData.DataSource = null;
            DGData.Update();
            DGData.Refresh();
            DGData.DataSource = ProductBL.GetData(ShareObject.CLientIDToken);
        }

        private void AddProduct_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this application?", "Confirmation",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

    }
}
