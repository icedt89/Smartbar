namespace JanHafner.Smartbar.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using JetBrains.Annotations;

    public sealed class Group
    {
        private Group()
        {
            this.Applications = new List<Application>();
        }

        internal Group([NotNull] String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Id = Guid.NewGuid();
            this.Applications = new List<Application>();
            this.Name = name;
        }

        [Key]
        public Guid Id { get; internal set; }

        [Required]
        [CanBeNull]
        public String Name { get; private set; }

        public Int32 Position { get; private set; }

        public Boolean IsSelected { get; private set; }

        public ICollection<Application> Applications { get; private set; }

        internal void Rename([NotNull] String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
        }

        internal void Select()
        {
            this.IsSelected = true;
        }

        internal void Reposition(Int32 position)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));    
            }

            this.Position = position;
        }

        internal void Unselect()
        {
            this.IsSelected = false;
        }
    }
}
