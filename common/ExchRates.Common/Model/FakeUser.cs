namespace ExchRates.Common.Model
{
    /// <summary>
    ///     Тип пользователя, необходимый для тестовой аутентификации.
    /// </summary>
    public class FakeUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}