namespace AuctionSystem.Domain.Common
{
    public static class GeneralConstants
    {
        public const string REINITIATED_COMMENT = "This transaction was cancelled and reinitiated in another branch";
        public static readonly char[] NUMBERS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static readonly char[] ALPHABET = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
        public static readonly char[] EXCLUDED_CHARACTERS = { '\'', '?', '>', '<', '*', '[', ']', '{', '}', '^' };
    }

    public static class UserAPIStatus
    {
        public const string ACTIVE = "2";
        public const string INACTIVE = "9";
    }

    public static class UserClaimFields
    {
        public const string CREATED_AT = "createdAt";
        public const string EMAIL = "emailAddress";
        public const string SESSION_ID = "sessionId";
        public const string USER_NAME = "username";
        public const string FIRST_NAME = "firstName";
        public const string LAST_NAME = "lastName";
        public const string IP_ADDRESS = "ipAddress";
    }
}
