namespace JanHafner.Smartbar.Services
{
    using System;

    public sealed class PositionInformation
    {
        internal PositionInformation(Guid groupId, Int32 column, Int32 row, Guid? assignedApplicationId = null)
        {
            this.GroupId = groupId;
            this.Column = column;
            this.Row = row;
            this.AssignedApplicationId = assignedApplicationId;
        }

        public Guid GroupId { get; private set; }

        public Int32 Column { get; private set; }

        public Int32 Row { get; private set; }

        public Boolean IsFree
        {
            get { return !this.AssignedApplicationId.HasValue; }
        }

        public Guid? AssignedApplicationId { get; private set; }
    }
}
