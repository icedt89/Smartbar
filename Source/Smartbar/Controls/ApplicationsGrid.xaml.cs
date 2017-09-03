namespace JanHafner.Smartbar.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common.UserInterface;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Infrastructure;
    using JanHafner.Smartbar.Views.Group;
    using JetBrains.Annotations;
    using MahApps.Metro.Controls;
    using Application = JanHafner.Smartbar.Model.Application;

    internal partial class ApplicationsGrid
    {
        [NotNull]
        private readonly SelectedApplicationButtonsList selectedApplicationButtons;

        [NotNull]
        private readonly ICollection<ApplicationButton> applicationButtons;

        [NotNull]
        private readonly PotentialApplicationButtonsForDropList potentialApplicationButtonsForDrop;

        [CanBeNull]
        private ApplicationButton applicationButtonWhichInitiatedDragEnter;

        private Point initialMouseDragPosition;

        private Boolean dragInitiated;

        private Boolean suppressNextMouseLeftButtonUp;


        public ApplicationsGrid()
        {
            this.selectedApplicationButtons = new SelectedApplicationButtonsList();
            this.applicationButtons = new List<ApplicationButton>();
            this.potentialApplicationButtonsForDrop = new PotentialApplicationButtonsForDropList();

            this.DataContextChanged += (sender, args) =>
            {
                if (args.NewValue as GroupViewModel == null)
                {
                    throw new ArgumentException($"DataContext of ApplicationsGrid needs to be of type '{typeof(GroupViewModel).Name}' in order to work!");
                }

                if (args.OldValue != null && args.OldValue != args.NewValue)
                {
                    var oldGroupViewModel = (GroupViewModel) args.OldValue;
                    oldGroupViewModel.Applications.CollectionChanged -= this.ApplicationsOnCollectionChanged;

                    this.OnInitialized(EventArgs.Empty);
                }
            };
        }

        private GroupViewModel GroupViewModel
        {
            get { return (GroupViewModel) this.DataContext; }
        }

        #region GridCellContentSize

        public static readonly DependencyProperty GridCellContentSizeProperty = DependencyProperty.Register("GridCellContentSize", typeof(Int32), typeof(ApplicationsGrid), new PropertyMetadata(32, GridCellContentSizeChangedCallback,
            (dp, baseValue) =>
            {
                var coercedValue = (Int32)baseValue;
                if (coercedValue < 32)
                {
                    coercedValue = 32;
                }

                return coercedValue;
            }));

        private static void GridCellContentSizeChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var applicationsGrid = (ApplicationsGrid)dependencyObject;
            if (!applicationsGrid.IsInitialized)
            {
                return;
            }

            foreach (var applicationButton in applicationsGrid.applicationButtons)
            {
                applicationsGrid.ApplyGridCellContentSize(applicationButton, applicationsGrid.GridCellContentSize);
            }

            applicationsGrid.ApplyOwnWidthIfContainedInTabControl();
        }

        private void ApplyGridCellContentSize(ApplicationButton applicationButton, Int32 gridCellContentSize)
        {
            applicationButton.Width = gridCellContentSize;
            applicationButton.Height = gridCellContentSize;
        }

        public Int32 GridCellContentSize
        {
            get { return (Int32) this.GetValue(GridCellContentSizeProperty); }
            set { this.SetValue(GridCellContentSizeProperty, value); }
        }

        #endregion

        #region GridCellSpacing

        public static readonly DependencyProperty GridCellSpacingProperty = DependencyProperty.Register("GridCellSpacing", typeof (Int32), typeof (ApplicationsGrid), new PropertyMetadata(1, GridCellSpacingChangedCallback,
            (dp, baseValue) =>
            {
                var coercedValue = (Int32)baseValue;
                if (coercedValue < 1)
                {
                    coercedValue = 1;
                }

                return coercedValue;
            }));

        private static void GridCellSpacingChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var applicationsGrid = (ApplicationsGrid)dependencyObject;
            if (!applicationsGrid.IsInitialized)
            {
                return;
            }

            foreach (var applicationButton in applicationsGrid.applicationButtons)
            {
                applicationsGrid.ApplyGridCellSpacing(applicationButton, applicationsGrid.GridCellSpacing);
            }

            applicationsGrid.ApplyOwnWidthIfContainedInTabControl();
        }

        private void ApplyGridCellSpacing(ApplicationButton applicationButton, Int32 gridCellSpacing)
        {
            var buttonThickness = new Thickness
            {
                Left = gridCellSpacing,
                Top = gridCellSpacing
            };

            if (Grid.GetColumn(applicationButton) == 0)
            {
                buttonThickness.Left = gridCellSpacing - 1;
            }

            if (Grid.GetRow(applicationButton) == 0)
            {
                buttonThickness.Top = gridCellSpacing - 1;
            }

            if (this.Columns - 1 == Grid.GetColumn(applicationButton))
            {
                buttonThickness.Right = gridCellSpacing - 1;
            }

            if (this.Rows - 1 == Grid.GetRow(applicationButton))
            {
                buttonThickness.Bottom = gridCellSpacing - 1;
            }

            applicationButton.Margin = buttonThickness;
        }

        public Int32 GridCellSpacing
        {
            get { return (Int32) this.GetValue(GridCellSpacingProperty); }
            set { this.SetValue(GridCellSpacingProperty, value); }
        }

        #endregion

        #region Rows

        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows", typeof (Int32), typeof (ApplicationsGrid), new PropertyMetadata(1, RowsChangedCallback,
            (dp, baseValue) =>
            {
                var coercedValue = (Int32) baseValue;
                if (coercedValue < 1)
                {
                    coercedValue = 1;
                }

                return coercedValue;
            }));

        private static void RowsChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var applicationsGrid = (ApplicationsGrid)dependencyObject;
            if (!applicationsGrid.IsInitialized)
            {
                return;
            }

            var oldRowCount = (Int32)dependencyPropertyChangedEventArgs.OldValue;
            if (applicationsGrid.Rows > oldRowCount)
            {
                applicationsGrid.ExpandGrid(oldRowCount, applicationsGrid.Rows, 0, applicationsGrid.Columns);

                // Reapply margin to old last row in order to compute margins of newly added rows correctly.
                foreach (var applicationButton in applicationsGrid.applicationButtons.Where(ab => Grid.GetRow(ab) == oldRowCount - 1))
                {
                    applicationsGrid.ApplyGridCellSpacing(applicationButton, applicationsGrid.GridCellSpacing);
                }
            }
            else if (applicationsGrid.Rows < oldRowCount)
            {
                applicationsGrid.ShrinkGrid(oldRowCount, applicationsGrid.Rows, Grid.GetRow, (ag, current) => applicationsGrid.RowDefinitions.RemoveAt(current - 1));
            }
        }

        public Int32 Rows
        {
            get { return (Int32) this.GetValue(RowsProperty); }
            set { this.SetValue(RowsProperty, value); }
        }

        #endregion

        private void ExpandGrid(Int32 startRowCount, Int32 destinationRowCount, Int32 startColumnCount, Int32 destinationColumnCount)
        {
            var applications = this.GroupViewModel.Applications;
            for (var i = startRowCount; i < destinationRowCount; i++)
            {
                this.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Auto
                });

                for (var j = startColumnCount; j < destinationColumnCount; j++)
                {
                    this.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });

                    var application = applications.SingleOrDefault(app => app.Row == i && app.Column == j);
                    this.CreateChildControlAt(application, i, j);
                }
            }

            this.ApplyOwnWidthIfContainedInTabControl();
        }

        private void ApplyOwnWidthIfContainedInTabControl()
        {
            // Force the width of the grid to be at least as large as the width of all children! Was trial and error...
            this.Width = (this.Columns * this.GridCellContentSize) + (this.Columns * this.GridCellSpacing) + this.GridCellSpacing - 1;

            var tabControl = this.TryFindParent<TabControl>();
            if (tabControl != null)
            {
                tabControl.Width = this.Width + 3; // Was trial and error....
            }
        }

        private void ShrinkGrid(Int32 startCount, Int32 destinationCount,
            [NotNull] Func<ApplicationButton, Int32> applicationButtonPropertySelector,
            [NotNull] Action<ApplicationsGrid, Int32> rowOrColumnDefintionCollectionCleanupAction)
        {
            if (applicationButtonPropertySelector == null)
            {
                throw new ArgumentNullException(nameof(applicationButtonPropertySelector));
            }

            if (rowOrColumnDefintionCollectionCleanupAction == null)
            {
                throw new ArgumentNullException(nameof(rowOrColumnDefintionCollectionCleanupAction));
            }

            for (var i = startCount; i > destinationCount; i--)
            {
                foreach (var applicationButton in this.applicationButtons.Where(ab => applicationButtonPropertySelector(ab) == i - 1).ToList())
                {
                    this.applicationButtons.Remove(applicationButton);
                    this.Children.Remove(applicationButton);
                }

                rowOrColumnDefintionCollectionCleanupAction(this, i);
            }

            // Reapply margin to old last row in order to compute margins of newly added rows correctly.
            foreach (var applicationButton in this.applicationButtons.Where(ab => applicationButtonPropertySelector(ab) == destinationCount - 1))
            {
                this.ApplyGridCellSpacing(applicationButton, this.GridCellSpacing);
            }

            this.ApplyOwnWidthIfContainedInTabControl();
        }

        #region Columns

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof (Int32), typeof (ApplicationsGrid), new PropertyMetadata(1, ColumnsChangedCallback,
            (dp, baseValue) =>
            {
                var coercedValue = (Int32)baseValue;
                if (coercedValue < 1)
                {
                    coercedValue = 1;
                }

                return coercedValue;
            }));

        private static void ColumnsChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var applicationsGrid = (ApplicationsGrid)dependencyObject;
            if (!applicationsGrid.IsInitialized)
            {
                return;
            }

            var oldColumnCount = (Int32)dependencyPropertyChangedEventArgs.OldValue;
            if (applicationsGrid.Columns > oldColumnCount)
            {
                applicationsGrid.ExpandGrid(0, applicationsGrid.Rows, oldColumnCount, applicationsGrid.Columns);

                // Reapply margin to old last row in order to compute margins of newly added rows correctly.
                foreach (var applicationButton in applicationsGrid.applicationButtons.Where(ab => Grid.GetColumn(ab) == oldColumnCount - 1))
                {
                    applicationsGrid.ApplyGridCellSpacing(applicationButton, applicationsGrid.GridCellSpacing);
                }
            }
            else if (applicationsGrid.Columns < oldColumnCount)
            {
                applicationsGrid.ShrinkGrid(oldColumnCount, applicationsGrid.Columns, Grid.GetColumn, (ag, current) => applicationsGrid.ColumnDefinitions.RemoveAt(current - 1));
            }
        }

        public Int32 Columns
        {
            get { return (Int32) this.GetValue(ColumnsProperty); }
            set { this.SetValue(ColumnsProperty, value); }
        }

        #endregion

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.RowDefinitions.Clear();
            this.ColumnDefinitions.Clear();

            this.Children.Clear();
            this.applicationButtons.Clear();

            this.ExpandGrid(0, this.Rows, 0, this.Columns);

            this.GroupViewModel.Applications.CollectionChanged += this.ApplicationsOnCollectionChanged;
        }

        #region Drag & Drop

        private void CancelDragDrop()
        {
            this.dragInitiated = false;
            this.potentialApplicationButtonsForDrop.Clear();
            this.applicationButtonWhichInitiatedDragEnter = null;
            this.initialMouseDragPosition = new Point();
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            ApplicationViewModel applicationViewModel = null;
            var draggedInsideApplicationButton = this.FindApplicationButton((DependencyObject)e.Source, out applicationViewModel);
            if (draggedInsideApplicationButton == this.applicationButtonWhichInitiatedDragEnter)
            {
                // DragEnter occured but is called implicitly by moving the mouse on the initial button.
                this.potentialApplicationButtonsForDrop.Clear();

                return;
            }

            this.potentialApplicationButtonsForDrop.Clear();

            var foundDataObjectTranslators = this.GroupViewModel.FindDataObjectTranslators(e.Data);
            var translatedData = foundDataObjectTranslators.SelectMany(dot => dot.TranslateData(e.Data));
            var foundApplicationCreationHandlers = this.GroupViewModel.FindApplicationCreationHandlers(translatedData).Where(t => t.Item2 != null).ToList();

            var positionInformations = this.GroupViewModel.GetPositionInformation(applicationViewModel.Row, applicationViewModel.Column);
            if (e.KeyStates.HasFlag(DragDropKeyStates.AltKey))
            {
                positionInformations = positionInformations.Where(pi => pi.IsFree);
            }

            var applicationButtonsForDrop =
                this.JoinApplicationButtons(
                    positionInformations.Take(foundApplicationCreationHandlers.Count)
                        .Zip(foundApplicationCreationHandlers, (pi, ach) => new
                        {
                            PositionInformation = pi,
                            ApplicationCreationHandlerData = ach
                        }), (ab, _) =>
                        {
                            var avm = (ApplicationViewModel) ab.DataContext;
                            return avm.Row == _.PositionInformation.Row &&
                                   avm.Column == _.PositionInformation.Column;
                        }, (ab, _) => new
                        {
                            ApplicationButton = ab,
                            _.PositionInformation,
                            _.ApplicationCreationHandlerData
                        }).ToList();
            foreach (var positionInformationWithHandler in applicationButtonsForDrop)
            {
                var potentialApplicationButtonForDropInformation = new PotentialApplicationButtonForDropInformation(positionInformationWithHandler.ApplicationButton, positionInformationWithHandler.ApplicationCreationHandlerData.Item2, positionInformationWithHandler.PositionInformation, positionInformationWithHandler.ApplicationCreationHandlerData.Item1);

                positionInformationWithHandler.ApplicationButton.Preview(new Object());

                this.potentialApplicationButtonsForDrop.Add(potentialApplicationButtonForDropInformation);
            }

            this.dragInitiated = true;
            e.Handled = true;
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            var draggedInsideApplicationButton = this.FindApplicationButton((DependencyObject)e.Source);
            var childControlUnderCursor = draggedInsideApplicationButton?.InputHitTest(e.GetPosition((IInputElement) e.Source));
            if (childControlUnderCursor != null && (e.AllowedEffects != DragDropEffects.None && e.Effects != DragDropEffects.None))
            {
                // DragLeave occured but Effects and AllowedEffects is none, than Escape was pressed
                // DragLeave was called for the button or one of its contents, but the mouse was already moved outside the control
                return;
            }

            this.CancelDragDrop();

            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        protected override async void OnDrop(DragEventArgs e)
        {
            var importApplicationInformations = this.potentialApplicationButtonsForDrop.Select(_ => new ImportApplicationInformation(_.Data, _.PositionInformation, _.ApplicationCreationHandler)).ToList();

            if (importApplicationInformations.Any())
            {
                await this.GroupViewModel.ImportApplicationsAsync(importApplicationInformations);
            }

            this.CancelDragDrop();
            this.selectedApplicationButtons.Clear();
            e.Effects = DragDropEffects.None;

            e.Handled = true;
        }

        private IEnumerable<T> JoinApplicationButtons<TSource, T>(
            IEnumerable<TSource> sourceList, Func<ApplicationButton, TSource, Boolean> joinPredicate,
            Func<ApplicationButton, TSource, T> result)
        {
            foreach (var source in sourceList)
            {
                foreach (
                    var applicationButton in
                        this.applicationButtons.Where(applicationButton => joinPredicate(applicationButton, source)))
                {
                    yield return result(applicationButton, source);
                }
            }
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            // Is not called if drag is initiated by windows, eg no call to DragDrop.DoDragDrop(...) :(!
            if (!e.EscapePressed)
            {
                return;
            }

            e.Action = DragAction.Cancel;

            this.CancelDragDrop();
            this.selectedApplicationButtons.Clear();

            e.Handled = true;
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (e.Key != Key.Escape)
            {
                return;
            }

            var applicationButton = this.FindApplicationButton((DependencyObject)e.Source);
            this.suppressNextMouseLeftButtonUp = applicationButton != null && applicationButton.IsPressed;
            if (this.suppressNextMouseLeftButtonUp)
            {
                applicationButton.Unpress();
                applicationButton.ReleaseMouseCapture();
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (this.dragInitiated)
            {
                return;
            }

            if (this.suppressNextMouseLeftButtonUp)
            {
                e.Handled = true;
                this.suppressNextMouseLeftButtonUp = false;
                return;
            }

            var applicationButton = this.FindApplicationButton((DependencyObject)e.Source);
            if (applicationButton == null || !applicationButton.IsSelectable)
            {
                this.CancelDragDrop();
                this.selectedApplicationButtons.Clear();
                return;
            }

            // Sometimes the Multi Select Action will not work, this workaround sets the focus programmatically to the control under the cursor.
            var hitTestControl = this.InputHitTest(e.GetPosition(this)) as ApplicationButton;
            if (hitTestControl != null && applicationButton != hitTestControl)
            {
                applicationButton = hitTestControl;
                hitTestControl.Focus();
            }

            if (DragDropHelper.IsMultiSelectAction)
            {
                if (applicationButton.IsSelected)
                {
                    this.selectedApplicationButtons.Remove(applicationButton);
                }
                else
                {
                    this.selectedApplicationButtons.Add(applicationButton);
                }

                e.Handled = true;
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (this.dragInitiated)
            {
                return;
            }

            this.initialMouseDragPosition = e.GetPosition(this);
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            this.selectedApplicationButtons.Clear();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (this.dragInitiated)
            {
                return;
            }

            ApplicationButton applicationButtonToCapture = null;

            // Capture mouse so that DragEnter can ignore a drag & drop on the button that started the operation.
            if (this.selectedApplicationButtons.Any())
            {
                applicationButtonToCapture = this.InputHitTest(e.GetPosition(this)) as ApplicationButton;
                applicationButtonToCapture?.CaptureMouse();
            }

            e.Handled = true;

            var currentMousePosition = e.GetPosition(this);
            var cursorPositionDifference = this.initialMouseDragPosition - currentMousePosition;
            if (e.LeftButton == MouseButtonState.Pressed 
                && this.initialMouseDragPosition != new Point() 
                && (Math.Abs(cursorPositionDifference.X) > SystemParameters.MinimumHorizontalDragDistance
                || Math.Abs(cursorPositionDifference.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                this.applicationButtonWhichInitiatedDragEnter = applicationButtonToCapture ?? this.FindApplicationButton((DependencyObject)e.Source); ;
                
                if (this.applicationButtonWhichInitiatedDragEnter == null || !this.applicationButtonWhichInitiatedDragEnter.IsDragSource)
                {
                    return;
                }

                // Cost me one hour to find the source of a bug, where OnDrop(...) was called twice in a row: Set dragInitiated immediately to true, 
                // to prevent this method from being called twice if the mouse is moved just a little bit and OnDrop(...) has not reset the drag & drop!
                this.dragInitiated = true;

                var sourceGroupId = this.GroupViewModel.Id;
                var dataObject = this.selectedApplicationButtons.Any() 
                    ? new DataObject(ApplicationDragDropData.Format, new ApplicationDragDropData(sourceGroupId, this.selectedApplicationButtons.Select(ab => ab.ApplicationId).ToArray())) 
                    : new DataObject(ApplicationDragDropData.Format, new ApplicationDragDropData(sourceGroupId, this.applicationButtonWhichInitiatedDragEnter.ApplicationId));

                // DoDragDrop returns DragDropEffects.None if QueryContinueDrag cancelled the application initiated drag & drop operation
                var doDragDropResult = DragDrop.DoDragDrop(this.applicationButtonWhichInitiatedDragEnter, dataObject, DragDropEffects.Link | DragDropEffects.Copy | DragDropEffects.Move);
                if (doDragDropResult == DragDropEffects.None)
                {
                    this.dragInitiated = false;
                }
            }
        }

        #endregion

        [CanBeNull]
        private ApplicationButton FindApplicationButton(DependencyObject maybeChildControl)
        {
            var result = maybeChildControl as ApplicationButton;
            return result ?? maybeChildControl.TryFindParent<ApplicationButton>();
        }

        [CanBeNull]
        private ApplicationButton FindApplicationButton(DependencyObject maybeChildControl,
            [CanBeNull] out ApplicationViewModel applicationViewModel)
        {
            applicationViewModel = null;
            var result = this.FindApplicationButton(maybeChildControl);
            if (result != null)
            {
                applicationViewModel = (ApplicationViewModel)result.DataContext;
            }

            return result;
        }

        private void ApplicationsOnCollectionChanged(Object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs == null)
            {
                throw new ArgumentNullException(nameof(notifyCollectionChangedEventArgs));
            }

            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newApplication in notifyCollectionChangedEventArgs.NewItems.Cast<Application>())
                    {
                        this.RecreateChildControlAt(newApplication, newApplication.Row, newApplication.Column);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var deletedApplication in notifyCollectionChangedEventArgs.OldItems.Cast<Application>())
                    {
                        this.RecreateChildControlAt(null, deletedApplication.Row, deletedApplication.Column);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.OnInitialized(EventArgs.Empty);
                    break;
            }
        }

        private void RecreateChildControlAt(Application application, Int32 row, Int32 column)
        {
            var controlAtCurrentPosition =
                this.applicationButtons
                    .SingleOrDefault(
                        applicationButton =>
                            Grid.GetRow(applicationButton) == row && Grid.GetColumn(applicationButton) == column);
            if (controlAtCurrentPosition != null)
            {
                this.applicationButtons.Remove(controlAtCurrentPosition);
                this.Children.Remove(controlAtCurrentPosition);
            }

            this.CreateChildControlAt(application, row, column);
        }

        private void CreateChildControlAt(Application application, Int32 row, Int32 column)
        {
            var applicationButton = this.GroupViewModel.CreateApplicationButton(application, column, row);

            Grid.SetColumn(applicationButton, column);
            Grid.SetRow(applicationButton, row);

            this.ApplyGridCellSpacing(applicationButton, this.GridCellSpacing);
            this.ApplyGridCellContentSize(applicationButton, this.GridCellContentSize);

            this.applicationButtons.Add(applicationButton);
            this.Children.Add(applicationButton);
        }
    }
}
