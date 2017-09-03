namespace JanHafner.Smartbar.Extensibility.BuiltIn
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;

    public interface IUpdateApplicationImageCommand : ICommand
    {
        Guid ApplicationWithImageId { get; }
    }
}