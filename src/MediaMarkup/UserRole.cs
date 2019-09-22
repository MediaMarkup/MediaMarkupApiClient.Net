namespace MediaMarkup
{
    /// <summary>
    /// User role associated with User
    /// </summary>
    public static class UserRole
    {
        public const string Owner = "Owner";
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Reviewer = "Reviewer";
        public const string Observer = "Observer";

        public static string[] Roles => new string[]
        {
            Owner,
            Admin,
            User,
            Reviewer,
            Observer
        };
    }
}