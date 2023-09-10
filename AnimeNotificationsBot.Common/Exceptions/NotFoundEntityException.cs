namespace AnimeNotificationsBot.Common.Exceptions
{
    public class NotFoundEntityException : ApplicationException
    {
        public required string EntityName { get; set; } 
        public string? PropertyName { get; set; }
        public object? PropertyValue { get; set; }

        public override string Message => $"{EntityName} with {PropertyName}: {PropertyValue} not found in database";
    }
}
