using DataLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL = DataLayer.dlLogin;

namespace BussinessLayer
{
    public class blLogin
    {
        private enLogin _enLogin = null;
        private DAL _objDAL = null;


        public enLogin GetEntityReference
        {
            get { return this._enLogin; }
        }

        public blLogin(enLogin enLogin_)
        {
            this._enLogin = enLogin_;
        }


        private DAL GetDALReference()
        {
            if (_objDAL == null)
            {
                _objDAL = new DAL(this._enLogin);
            }
            return _objDAL;
        }
        public List<enLogin> LoginUser()
        {

            return GetDALReference().LoginUser();

        }

        public List<enLogin> RegisterUser()
        {
            return GetDALReference().RegisterUser();
        }
        public List<enAddress> SaveUserAddress(enAddress model)
        {
            return GetDALReference().SaveUserAddress(model);
        }

        public List<enAddress> GetUserAddress(int userId)
        {
            return GetDALReference().GetUserAddress(userId);
        }

        public List<enAddress> DeleteAddress(int id)
        {
            return GetDALReference().DeleteAddress(id);
        }


    }
}
