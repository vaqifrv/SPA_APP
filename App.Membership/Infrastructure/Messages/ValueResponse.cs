namespace App.Membership.Infrastructure.Messages
{
    public class ValueResponse<T> : ResponseBase
    {
        public T Value { get; set; }
    }
}