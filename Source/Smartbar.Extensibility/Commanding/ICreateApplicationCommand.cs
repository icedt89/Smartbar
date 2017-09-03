namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;

    public interface ICreateApplicationCommand : ICommand
    {
        Guid TargetGroupId { get; set; }
        
        Int32 TargetRow { get; set; }
        
        Int32 TargetColumn { get; set; } 

        ApplicationCreateTargetBehavior ApplicationCreateTargetBehavior { get; set; }
    }
}