namespace AnimeNotificationsBot.Common.Exceptions
{
    public class NotFoundEntityException : ApplicationException
    {
        public string EntityName { get; set; } = null!;
        public long EntityId { get; set; }

        public override string Message => $"{EntityName} with id: {EntityId} not found in database";
    }
}
