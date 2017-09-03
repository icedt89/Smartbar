namespace JanHafner.Smartbar.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JetBrains.Annotations;

    internal sealed class SelectedApplicationButtonsList : ICollection<ApplicationButton>
    {
        [NotNull]
        private readonly ICollection<ApplicationButton> items;

        public Int32 Count
        {
            get { return this.items.Count; }
        }

        public Boolean IsReadOnly
        {
            get { return this.items.IsReadOnly; }
        }

        public SelectedApplicationButtonsList()
        {
            this.items = new List<ApplicationButton>();
        }

        public IEnumerator<ApplicationButton> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(ApplicationButton item)
        {
            this.items.Add(item);

            item.IsSelected = true;
        }

        public void Clear()
        {
            var applicationButtons = this.items.ToList();

            this.items.Clear();

            applicationButtons.ForEach(applicationButton => applicationButton.IsSelected = false);
        }

        public Boolean Contains(ApplicationButton item)
        {
            return this.items.Contains(item);
        }

        public void CopyTo(ApplicationButton[] array, Int32 arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public Boolean Remove(ApplicationButton item)
        {
            var result = this.items.Remove(item);
            if (result)
            {
                item.IsSelected = false;
            }

            return result;
        }
    }
}
