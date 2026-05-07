using EntityLayer;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Xotty.Models;

namespace DataLayer
{
    public   class dlLogin
    {
        private enLogin _enLogin = null;

        public dlLogin(enLogin enLogin_)
        {
            this._enLogin = enLogin_;
        }

        public List<enLogin> LoginUser()
        {
            List<enLogin> filterInformation = new List<enLogin>();
            using (IDataReader idr = SqlHelper.ExecuteReader(ApplicationSettings.DefaultConnectionString, "USP_LoginUser", new object[]  {
                                                  _enLogin.Email,
                                                  ErrorViewModel.HashPassword(_enLogin.Password)

                                              }))
            {
                while (idr.Read())
                {
                    enLogin filterInformation1 = new enLogin();
                    ConstructObjectForFilter(idr, filterInformation1);
                    filterInformation.Add(filterInformation1);
                }
                return filterInformation;
            }
        }

        public List<enLogin> RegisterUser()
        {
            List<enLogin> filterInformation = new List<enLogin>();
            using (IDataReader idr = SqlHelper.ExecuteReader(ApplicationSettings.DefaultConnectionString, "USP_RegisterUser", new object[]  {
                                                 _enLogin.Name,
                                               _enLogin.Email,
                                               ErrorViewModel.HashPassword(_enLogin.Password)

                                              }))
            {
                while (idr.Read())
                {
                    enLogin filterInformation1 = new enLogin();
                    ConstructObjectRegisterForFilter(idr, filterInformation1);
                    filterInformation.Add(filterInformation1);
                }
                return filterInformation;
            }
        }


        public List<enAddress> SaveUserAddress(enAddress model)
        {
            List<enAddress> list = new List<enAddress>();

            using (IDataReader idr = SqlHelper.ExecuteReader( ApplicationSettings.DefaultConnectionString,"USP_SaveUserAddress",  new object[]  {
                                                     model.Id,
                                                     model.UserId,
                                                     model.FullName,
                                                     model.MobileNo,
                                                     model.AddressLine,
                                                     model.City,
                                                     model.StateName,
                                                     model.Pincode,
                                                     model.AddressType,
                                                     model.IsDefault
                }))
            {
                while (idr.Read())
                {
                    enAddress obj = new enAddress();

                    obj.Status = Convert.ToInt32(idr["Status"]);
                    obj.Message = Convert.ToString(idr["Message"]);

                    list.Add(obj);
                }
            }

            return list;
        }
        public List<enAddress> GetUserAddress(int userId)
        {
            List<enAddress> list = new List<enAddress>();

            using (IDataReader idr = SqlHelper.ExecuteReader(ApplicationSettings.DefaultConnectionString, "USP_GetUserAddress", new object[] {
                                                     userId
               }))
            {
                while (idr.Read())
                {
                    enAddress obj = new enAddress();

                    obj.Id = Convert.ToInt32(idr["Id"]);
                    obj.UserId = Convert.ToInt32(idr["UserId"]);
                    obj.FullName = Convert.ToString(idr["FullName"]);
                    obj.MobileNo = Convert.ToString(idr["MobileNo"]);
                    obj.AddressLine = Convert.ToString(idr["AddressLine"]);
                    obj.City = Convert.ToString(idr["City"]);
                    obj.StateName = Convert.ToString(idr["StateName"]);
                    obj.Pincode = Convert.ToString(idr["Pincode"]);
                    obj.AddressType = Convert.ToString(idr["AddressType"]);
                    obj.IsDefault = Convert.ToBoolean(idr["IsDefault"]);

                    list.Add(obj);
                }
            }

            return list;
        }
        public List<enAddress> DeleteAddress(int id)
        {
            List<enAddress> list = new List<enAddress>();

            using (IDataReader idr = SqlHelper.ExecuteReader( ApplicationSettings.DefaultConnectionString,"USP_DeleteUserAddress", new object[] {
                                                          id
                }))
            {
                while (idr.Read())
                {
                    enAddress obj = new enAddress();

                    obj.Status = Convert.ToInt32(idr["Status"]);
                    obj.Message = Convert.ToString(idr["Message"]);

                    list.Add(obj);
                }
            }

            return list;
        }

        public void ConstructObjectForFilter(IDataReader idr, enLogin obj)
        {
            obj.Status = idr["Status"] != DBNull.Value ? Convert.ToInt32(idr["Status"]) : 0;
            obj.Message = idr["Message"] != DBNull.Value ? Convert.ToString(idr["Message"]) : "";
            obj.Name = idr["Name"] != DBNull.Value ? Convert.ToString(idr["Name"]) : "";
            obj.Email = idr["Email"] != DBNull.Value ? Convert.ToString(idr["Email"]) : "";
            obj.Id = idr["Id"] != DBNull.Value ? Convert.ToInt32(idr["Id"]) : 0;
        }

        public void ConstructObjectRegisterForFilter(IDataReader idr, enLogin obj)
        {
            obj.Status = idr["Status"] != DBNull.Value ? Convert.ToInt32(idr["Status"]) : 0;
            obj.Message = idr["Message"] != DBNull.Value ? Convert.ToString(idr["Message"]) : "";
           
        }
    }
}
