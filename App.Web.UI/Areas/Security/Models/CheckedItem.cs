namespace App.Web.UI.Areas.Security.Models
{
    public class CheckedItem<T>
    {
        public T Item { get; set; }
        public bool Checked { get; set; }
    }
}