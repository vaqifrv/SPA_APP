namespace App.Web.UI.Infrastructure
{
    public class ObjectWrapperWithNameOfSet
    {
        public string NameOfSet { get; set; }
        public object WrappedObject { get; set; }
        public ObjectWrapperWithNameOfSet(string nameOfSet, object wrappedObject)
        {
            this.NameOfSet = nameOfSet;
            this.WrappedObject = wrappedObject;
        }
    }
}
