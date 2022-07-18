using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL
{
    public static class AppSetting
    {
        public static string NoPasswordTokenProviderName = "NPTokenProvider";
        public static string AdminRole = "Admin";
        public static string AdminRoleNormalized = "ADMIN";
        public static string UserRole = "User";
        public static string UserRoleNormalized = "USER";
        public static string DeleteInterestsFromUserProcedure = "DeleteAllInterestsFromUser";
        public static string InsertUpdatedInterestsIntoUserProcedure = "InsertUpdatedInterestsIntoUser";
        public static string InsertAllServicesIntoNewUserProcedure = "InsertAllServicesIntoNewUser";
    }
}
