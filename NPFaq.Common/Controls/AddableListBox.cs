using NPFaq.Common.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NPFaq.Common.Controls
{
    public class AddableListBox : ListBox
    {
        public AddableListBox()
        {
            this.DefaultStyleKey = typeof(AddableListBox);
            this.LayoutUpdated += AddableListBox_LayoutUpdated;
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            AddableListBoxItem item = new AddableListBoxItem();
            if (this.ItemContainerStyle != null)
            {
                item.Style = this.ItemContainerStyle;
            }
            return item;
        }
        private void AddableListBox_LayoutUpdated(object sender, EventArgs e)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var item = ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                var addBtn = VisualTreeHelper1.GetChildObject<Button>(item, "AddButton");
                var removeBtn = VisualTreeHelper1.GetChildObject<Button>(item, "RemoveButton");
                if (i == Items.Count - 1)
                {
                    addBtn.Visibility = Visibility.Visible;
                    removeBtn.Visibility = Visibility.Collapsed;
                }
                else
                {
                    addBtn.Visibility = Visibility.Collapsed;
                    removeBtn.Visibility = Visibility.Visible;
                }
            }
        }

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }
        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(AddableListBox), new PropertyMetadata(null));

        public ICommand RemoveCommand
        {
            get { return (ICommand)GetValue(RemoveCommandProperty); }
            set { SetValue(RemoveCommandProperty, value); }
        }
        public static readonly DependencyProperty RemoveCommandProperty =
            DependencyProperty.Register("RemoveCommand", typeof(ICommand), typeof(AddableListBox), new PropertyMetadata(null));

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (Items.Count > 1)
                {
                    SelectedItem = Items[Items.Count - 1];
                    UpdateLayout();
                    ScrollIntoView(SelectedItem);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    var item = ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                    var numberTB = VisualTreeHelper1.GetChildObject<TextBlock>(item, "NumberTB");
                    numberTB.Text = (i + 1).ToString();
                }
            }
        }
    }
    class AddableListBoxItem : ListBoxItem
    {
        private AddableListBox addableListBox;
        protected Button AddButton { get; private set; }
        protected Button RemoveButton { get; private set; }
        public AddableListBoxItem()
        {
            this.DefaultStyleKey = typeof(AddableListBoxItem);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            addableListBox = Utils.VisualTreeHelper1.GetParentObject<AddableListBox>(this);
            AddButton = GetTemplateChild("AddButton") as Button;
            RemoveButton = GetTemplateChild("RemoveButton") as Button;
            if (AddButton != null)
                AddButton.Click += AddButton_Click;
            if (RemoveButton != null)
                RemoveButton.Click += RemoveButton_Click;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var type = addableListBox.ItemsSource.GetType();
            if (type.IsGenericType)
            {
                var genericArgument = type.GetGenericArguments().FirstOrDefault();
                var obj = Activator.CreateInstance(genericArgument);
                var source = addableListBox.ItemsSource as IList;
                source.Add(obj);
            }
            if (addableListBox.AddCommand != null)
                addableListBox.AddCommand.Execute(null);
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var source = addableListBox.ItemsSource as IList;
            source.Remove(this.DataContext);
            if (addableListBox.RemoveCommand != null)
                addableListBox.RemoveCommand.Execute(null);
        }
    }
}
