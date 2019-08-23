using SQLHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SeviceSubscription.Core.Model;

namespace ServiceSubscription.Core
{
    public class UserProvider : IUserProvider
    {
        public List<UserInfo> GetAllUsers()
        {
            string strSpName = "[dbo].[usp_ServiceSubscription_GetAllUsers]";
            SQLGetList sqlHAsy = new SQLGetList();
            return sqlHAsy.ExecuteAsList<UserInfo>(strSpName) as List<UserInfo>;
        }

        public UserInfo GetUserDetailsById(int UserId)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@UserId", UserId));
            string strSpName = "[dbo].[usp_ServiceSubscription_GetUserDetailsById]";
            SQLGet sqlHAsy = new SQLGet();
            return sqlHAsy.ExecuteAsObject<UserInfo>(strSpName, Param);
        }

        public SubscribeTypeInfo HasUserSubscribed(int UserId)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@UserId", UserId));
            string strSpName = "[dbo].[usp_ServiceSubscription_HasUserSubscribed]";
            SQLGet sqlHAsy = new SQLGet();
            return sqlHAsy.ExecuteAsObject<SubscribeTypeInfo>(strSpName, Param);
        }

        public int AddUserSubscription(int UserId, int subscribeType)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@UserId", UserId));
            Param.Add(new SQLParam("@SubscribeType", subscribeType));
            string strSpName = "[dbo].[usp_ServiceSubscription_AddUserSubscription]";
            SQLExecuteNonQuery sqlHAsy = new SQLExecuteNonQuery();
            return sqlHAsy.ExecuteNonQueryAsGivenType<int>(strSpName, Param, "@SubscriptionId");
        }

        public void UnSubscribeService(int UserId)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@UserId", UserId));
            string strSpName = "[dbo].[usp_ServiceSubscription_UnSubscribeService]";
            SQLExecuteNonQuery sqlHAsy = new SQLExecuteNonQuery();
            sqlHAsy.ExecuteNonQuery(strSpName, Param);
        }

        public bool HasUserUnSubscribed(int UserId)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@UserId", UserId));
            string strSpName = "[dbo].[usp_ServiceSubscription_HasUserUnSubscribed]";
            SQLGet sqlHAsy = new SQLGet();
            return sqlHAsy.ExecuteAsScalar<bool>(strSpName, Param);
        }

        public List<MyServicesInfo> GetReport(ReportApiInfo apiInfo)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@UserId", apiInfo.id));
            Param.Add(new SQLParam("@FromDate", apiInfo.fromDate));
            Param.Add(new SQLParam("@ToDate", apiInfo.toDate));
            string strSpName = "[dbo].[usp_ServiceSubscription_GetReport]";
            SQLGetList sqlHAsy = new SQLGetList();
            return sqlHAsy.ExecuteAsList<MyServicesInfo>(strSpName, Param) as List<MyServicesInfo>;
        }

        public string GetUserEmailById(int UserId)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@UserId", UserId));
            string strSpName = "[dbo].[usp_ServiceSubscription_GetUserEmailById]";
            SQLGet sqlHAsy = new SQLGet();
            return sqlHAsy.ExecuteAsScalar<string>(strSpName, Param);
        }

        public List<MyServicesInfo> GetServiceDetailsByUserId(int UserId)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@UserId", UserId));
            string strSpName = "[dbo].[usp_ServiceSubscription_GetServiceDetailsByUserId]";
            SQLGetList sqlHAsy = new SQLGetList();
            return sqlHAsy.ExecuteAsList<MyServicesInfo>(strSpName, Param) as List<MyServicesInfo>;
        }

        public bool IsEmailAddressExists(string UserEmail)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@UserEmail", UserEmail));
            string strSpName = "[dbo].[usp_ServiceSubscription_IsEmailAddressExists]";
            SQLGet sqlHAsy = new SQLGet();
            return sqlHAsy.ExecuteAsScalar<bool>(strSpName, Param);
        }

        public void AddUser(UserInfo objInfo)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@FirstName", objInfo.FirstName));
            Param.Add(new SQLParam("@LastName", objInfo.LastName));
            Param.Add(new SQLParam("@UserName", objInfo.UserName));
            Param.Add(new SQLParam("@Email", objInfo.Email));
            Param.Add(new SQLParam("@PasswordHash", objInfo.PasswordHash));
            Param.Add(new SQLParam("@PasswordSalt", objInfo.PasswordSalt));
            Param.Add(new SQLParam("@Role", objInfo.Role));
            string strSpName = "[dbo].[usp_ServiceSubscription_AddUser]";
            SQLExecuteNonQuery sqlHAsy = new SQLExecuteNonQuery();
            sqlHAsy.ExecuteNonQuery(strSpName, Param);
        }
        public void ReSubscribeService(int UserId)
        {
            List<SQLParam> Param = new List<SQLParam>();
            Param.Add(new SQLParam("@UserId", UserId));
            string strSpName = "[dbo].[usp_ServiceSubscription_ReSubscribeService]";
            SQLExecuteNonQuery sqlHAsy = new SQLExecuteNonQuery();
            sqlHAsy.ExecuteNonQuery(strSpName, Param);
        }
    }
}
