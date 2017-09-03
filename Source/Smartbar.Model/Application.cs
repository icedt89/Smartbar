namespace JanHafner.Smartbar.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class Application
    {
        protected Application()
        {
        }

        protected Application(Guid id)
        {
            this.Id = id;
        }

        [Key]
        public Guid Id { get; private set; }

        public Int32 Row { get; private set; }

        public Int32 Column { get; private set; }

        public void Reposition(Int32 column, Int32 row)
        {
            if (column < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }

            if (row < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            this.Column = column;
            this.Row = row;
        }
    }
}