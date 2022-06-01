namespace SWP_Tesla_Website.Models {
    public enum Access {
        UNAUTHORIZED = -2, BANNED , USER, MOD, ADMIN, DEV
    };
    
    public static class Extensions {
        public static bool hasAccess(this Access givenAccess, Access neededAccess) {
            if (givenAccess == Access.UNAUTHORIZED || givenAccess == Access.BANNED)
                return false;

            return givenAccess >= neededAccess;
        }

        public static string getAccessName(this Access access ) {
            switch (access) {
                case Access.BANNED:
                    return "BANNED";
                case Access.USER:
                    return "User";
                case Access.MOD:
                    return "Moderator";
                case Access.ADMIN:
                    return "Administrator";
                case Access.DEV:
                    return "Developer";
                default:
                    return "UNAUTHORIZED";
            }
        }
    }
}
