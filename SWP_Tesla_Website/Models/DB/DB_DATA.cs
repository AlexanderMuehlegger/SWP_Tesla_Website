namespace SWP_Tesla_Website.Models.DB {
    public class DB_DATA {
        private static string IP = "remotemysql.com";
        private static string port = "3306";
        private static string database = "NbCTUONEyW";
        private static string user = "NbCTUONEyW";
        private static string password = "VeUP0OXCUj";

        public static string connectionStr = (port == "")? $"server={IP};port={port};database={database};uid={user};password={password}"
                                            : $"server={IP};database={database};uid={user};password={password}";
    }
}
