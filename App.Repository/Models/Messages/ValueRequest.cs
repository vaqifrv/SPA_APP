namespace App.Repository.Models.Messages
{
    public class ValueRequest<T> : RequestBase
    {
        public T Value { get; set; }
    }
}