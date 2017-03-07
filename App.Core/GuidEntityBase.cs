using System;

namespace App.Core
{
    public class GuidEntityBase : EntityBase<string>
    {
        public override bool IsNewEntity
        {
            get { return String.IsNullOrEmpty(Id); }
        }
    }
}
