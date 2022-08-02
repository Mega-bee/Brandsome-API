using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.BLL.Utilities
{
    public static class Constants
    {
        public static string AdminRole = "Admin";
        public static string AdminRoleNormalized = "ADMIN";
        public static string UserRole = "User";
        public static string UserRoleNormalized = "USER";
        public static string DeleteInterestsFromUserProcedure = "DeleteAllInterestsFromUser";
        public static string InsertUpdatedInterestsIntoUserProcedure = "InsertUpdatedInterestsIntoUser";
        public static string InsertAllServicesIntoNewUserProcedure = "InsertAllServicesIntoNewUser";
        public static string BusinessFollowNotificationBody = " started following you.";
        public static string PostLikeNotificationTitle = "Someone liked your post";
        public static string PostLikeNotificationBody = " liked your post.";
        public static string BusinessFollowNotificationTitle = "You have a new follower";
    }
}
