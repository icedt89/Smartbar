namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;

    public sealed class RepositionApplicationCommand : ICommand
    {
        public RepositionApplicationCommand(Guid applicationId, Int32 column, Int32 row)
        {
            this.ApplicationId = applicationId;
            this.Column = column;
            this.Row = row;
        }

        public Guid ApplicationId { get; private set; }

        public Int32 Column { get; private set; }

        public Int32 Row { get; private set; }
    }
}
