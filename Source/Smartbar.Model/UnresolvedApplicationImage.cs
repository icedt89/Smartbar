namespace JanHafner.Smartbar.Model
{
    using System;

    internal sealed class UnresolvedApplicationImage : ApplicationImage
    {
        public override void Update(ApplicationImage applicationImage)
        {
            throw new NotSupportedException();
        }
    }
}