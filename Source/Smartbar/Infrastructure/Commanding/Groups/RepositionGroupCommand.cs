namespace JanHafner.Smartbar.Infrastructure.Commanding.Groups
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;

    internal sealed class RepositionGroupCommand : ICommand
    {
        public RepositionGroupCommand(Guid groupId, Int32 position)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            this.GroupId = groupId;
            this.Position = position;
        }

        public Guid GroupId { get; private set; }

        public Int32 Position { get; private set; }
    }
}
