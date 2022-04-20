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
    }
}
