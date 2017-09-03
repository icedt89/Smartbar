namespace JanHafner.Smartbar.Common.UserInterface.SetProcessAffinityMask
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JanHafner.Toolkit.Windows;
    using JetBrains.Annotations;
    using Prism.Commands;
    using Prism.Mvvm;

    public sealed class SetProcessAffinityMaskViewModel : BindableBase
    {
        [NotNull]
        private ProcessAffinityMask processAffinityMask;

        [NotNull]
        private readonly IWindowService windowService;

        public SetProcessAffinityMaskViewModel([NotNull] ProcessAffinityMask processAffinityMask, [NotNull] IWindowService windowService)
        {
            if (processAffinityMask == null)
            {
                throw new ArgumentNullException(nameof(processAffinityMask));
            }
            
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            this.windowService = windowService;

            this.AffinityMaskBits = new ObservableCollection<EditableBit>();
            this.ApplyProcessAffinityMask(processAffinityMask);
        }

        [NotNull]
        public ObservableCollection<EditableBit> AffinityMaskBits { get; private set; }

        public Int64 AffinityMask
        {
            get { return this.processAffinityMask.AffinityMask; }
        }

        [NotNull]
        public ICommand OKCommand
        {
            get
            {
                return new CommonOKCommand<SetProcessAffinityMaskViewModel>(this, viewModel =>
                {
                    viewModel.AffinityMaskBits.ForEach(editableBit => editableBit.PropertyChanged -= viewModel.BitChanged);
                }, this.windowService);
            }
        }

        [NotNull]
        public ICommand ResetCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.ApplyProcessAffinityMask(ProcessAffinityMask.ForCurrentProcess());
                });
            }
        }

        [NotNull]
        public ICommand ClearCommand
        {
            get
            {
                return new CommonNoCommand<SetProcessAffinityMaskViewModel>(this, viewModel =>
                {
                    viewModel.AffinityMaskBits.ForEach(editableBit => editableBit.PropertyChanged -= viewModel.BitChanged);
                }, this.windowService);
            }
        }

        private void ApplyProcessAffinityMask([NotNull] ProcessAffinityMask processAffinityMask)
        {
            if (processAffinityMask == null)
            {
                throw new ArgumentNullException(nameof(processAffinityMask));
            }

            this.processAffinityMask = processAffinityMask;

            this.AffinityMaskBits.ForEach(editableBit => editableBit.PropertyChanged -= this.BitChanged);
            this.AffinityMaskBits.Clear();

            var editableBits = processAffinityMask.AffinityMaskBits.Select((bit, i) => new EditableBit(bit, i, processAffinityMask.CanSet(i)));

            foreach (var editableBit in editableBits)
            {
                editableBit.PropertyChanged += this.BitChanged;
                this.AffinityMaskBits.Add(editableBit);
            }
        }

        private void BitChanged([NotNull] Object sender, [NotNull] PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs == null)
            {
                throw new ArgumentNullException(nameof(propertyChangedEventArgs));
            }

            if (propertyChangedEventArgs.PropertyName != "Set")
            {
                return;
            }

            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            var editableBit = (EditableBit)sender;
            this.processAffinityMask.SetBit(editableBit.BitIndex, editableBit.Set);

            var setBits = this.AffinityMaskBits.Where(bit => bit.Set).ToList();
            if (setBits.Count == 1)
            {
                setBits.ForEach(bit => bit.CanSet = false);
            }
            else
            {
                setBits.ForEach(bit => bit.CanSet = true);
            }
        }

        public sealed class EditableBit : BindableBase
        {
            private Boolean set;

            private Boolean canSet;

            internal EditableBit(Boolean set, Int32 bitIndex, Boolean canSet)
            {
                this.Set = set;
                this.BitIndex = bitIndex;
                this.CanSet = canSet;
            }

            public Boolean Set
            {
                get { return this.set; }
                set { this.SetProperty(ref this.set, value); }
            }

            public Int32 BitIndex { get; private set; }

            public Boolean CanSet
            {
                get { return this.canSet; }
                set { this.SetProperty(ref this.canSet, value); }
            }
        }
    }
}