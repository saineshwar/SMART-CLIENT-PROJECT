using DemoSmartClient.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoSmartClient.BL
{
    public class ManageDBBL
    {
        IManageDB _IManageDB;
        public ManageDBBL(IManageDB IManageDB )
        {
            _IManageDB = IManageDB;
        }

        public void CreateSqlLiteDatabaseBL()
        {
            _IManageDB.CreateSqlLiteDatabase();
        }

        public void ChangepasswordBL()
        {
            _IManageDB.Changepassword();
        }

        public void SetpasswordBL()
        {
            _IManageDB.Setpassword();
        }

        public void RemovepasswordBL()
        {
            _IManageDB.Removepassword();
        }

        public void CreatetbproductBL()
        {
            _IManageDB.Createtbproduct();
        }

        public void Createt_Login_TableBL()
        {
            _IManageDB.Createt_Login_Table();
        }
    }
}
