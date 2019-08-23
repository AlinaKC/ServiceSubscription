using SeviceSubscription.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSubscription.Core
{
    public interface IUserProvider
    {
        List<UserInfo> GetAllUsers();
        UserInfo GetUserDetailsById(int UserId);
        SubscribeTypeInfo HasUserSubscribed(int UserId);
        int AddUserSubscription(int UserId, int subscribeType);
        void UnSubscribeService(int UserId);
        bool HasUserUnSubscribed(int UserId);
        List<MyServicesInfo> GetReport(ReportApiInfo apiInfo);
        string GetUserEmailById(int UserId);
        List<MyServicesInfo> GetServiceDetailsByUserId(int UserId);
        bool IsEmailAddressExists(string UserEmail);
        void AddUser(UserInfo objInfo);
        void ReSubscribeService(int UserId);
    }
}
