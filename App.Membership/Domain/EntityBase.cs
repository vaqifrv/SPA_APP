namespace App.Membership.Domain
{
    public abstract class EntityBase
    {
        public virtual bool IsValid
        {
            get;
            private set;
        }

        public virtual bool Validate()
        {
            throw new System.NotImplementedException();
        }

    }

}