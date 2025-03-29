namespace RecipeApp.Shared.SharedResponse
{
    public class UpdateApplicationUserSettingsCommandShared
    {
        public int Id { get; set; }
        public byte PreferredTheme { get; set; }
        public bool AcceptNewDishNotification { get; set; }
    }
}