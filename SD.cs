

namespace PamojaClassroomAdminModule
{
    public static class SD
    {
        public static string ApiBaseUrl = "https://pamojaclassapi.azurewebsites.net/";
        //public static string ApiBaseUrl = "https://localhost:44378/";
        public static string GradesTaughtUrl = ApiBaseUrl + "api/v1/GradesTaught/";
        public static string InterestUrl = ApiBaseUrl + "api/v1/Interest/";
        public static string SchoolTypeUrl = ApiBaseUrl + "api/v1/SchoolType/";
        public static string SpecializationUrl = ApiBaseUrl + "api/v1/Specialization/";
        public static string SubjectUrl = ApiBaseUrl + "api/v1/Subject/";
        public static string AccountApiPath = ApiBaseUrl + "api/v1/User/";
        public static string UserManipulationPath = ApiBaseUrl + "api/v1/UserManipulation/";
        public static int TotalUsers = 0;

    }
}
