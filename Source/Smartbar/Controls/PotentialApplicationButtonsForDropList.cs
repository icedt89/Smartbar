namespace JanHafner.Smartbar.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Infrastructure;
    using JetBrains.Annotations;

    internal sealed class PotentialApplicationButtonsForDropList : IEnumerable<PotentialApplicationButtonForDropInformation>
    {
        [NotNull]
        private readonly ICollection<PotentialApplicationButtonForDropInformation> items;

        public Int32 Count
        {
            get { return this.items.Count; }
        }

        public PotentialApplicationButtonsForDropList()
        {
            this.items = new List<PotentialApplicationButtonForDropInformation>();
        }

        public IEnumerator<PotentialApplicationButtonForDropInformation> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(PotentialApplicationButtonForDropInformation item)
        {
            this.items.Add(item);

            item.ApplicationButton.IsPotentialDropTarget = true;
        }

        public void Clear()
        {
            var applicationButtons = this.items.ToList();

            this.items.Clear();

            applicationButtons.ForEach(this.AfterItemRemoved);
        }

        public Boolean Contains(ApplicationButton item)
        {
            return this.items.Any(_ => _.ApplicationButton == item);
        }

        public Boolean Remove(ApplicationButton item)
        {
            var potentialApplicationButtonForDropInformation = this.items.Single(_ => _.ApplicationButton == item);
            var result = this.items.Remove(potentialApplicationButtonForDropInformation);
            if (result)
            {
               this.AfterItemRemoved(potentialApplicationButtonForDropInformation);
            }

            return result;
        }

        private void AfterItemRemoved([NotNull] PotentialApplicationButtonForDropInformation item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.ApplicationButton.IsPotentialDropTarget = false;
            item.ApplicationButton.Unpreview();
        }
    }
}
