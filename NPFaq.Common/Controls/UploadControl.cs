using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
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
    public class UploadControl : ItemsControl
    {
        private ItemsPresenter itemsPresenter;
        private Panel itemsPanel;
        protected Button AddButton { get; private set; }

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }
        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(UploadControl), new PropertyMetadata(null));
        public UploadControl()
        {
            this.DefaultStyleKey = typeof(UploadControl);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var grid = GetTemplateChild("grid") as Grid;
            if (grid != null)
            {
                AddButton = new Button() { Style = grid.Resources["AddButton"] as Style };
                AddButton.Click += AddButton_Click;
            }
            itemsPresenter = GetTemplateChild("ItemsPresenter") as ItemsPresenter;
            if (itemsPresenter != null)
            {
                itemsPresenter.LayoutUpdated += ItemsPresenter_LayoutUpdated;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                ItemsSource = ItemsSource ?? new ObservableCollection<UploadItemInfo>();
                IList<UploadItemInfo> itemssource = ItemsSource as IList<UploadItemInfo>;
                var item = new UploadItemInfo(dialog.File.Name, dialog.File.OpenRead());
                itemssource.Add(item);
                if (AddCommand != null)
                    AddCommand.Execute(null);
            }
        }

        private void ItemsPresenter_LayoutUpdated(object sender, EventArgs e)
        {
            itemsPresenter.LayoutUpdated -= ItemsPresenter_LayoutUpdated;
            if (VisualTreeHelper.GetChildrenCount(this.itemsPresenter) > 0)
            {
                itemsPanel = (Panel)VisualTreeHelper.GetChild(this.itemsPresenter, 0);
                itemsPanel.Children.Add(AddButton);
            }
        }
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UploadItem();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
        }
    }
    public class UploadItem : Control
    {
        protected Button CancelButton { get; private set; }
        public UploadItem()
        {
            DefaultStyleKey = typeof(UploadItem);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CancelButton = GetTemplateChild("CancelButton") as Button;
            if (CancelButton != null)
                CancelButton.Click += CancelButton_Click;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var uc = Utils.VisualTreeHelper1.GetParentObject<UploadControl>(this);
            if (uc != null)
            {
                IList<UploadItemInfo> itemssource = uc.ItemsSource as IList<UploadItemInfo>;
                if (itemssource != null)
                    itemssource.Remove(this.DataContext as UploadItemInfo);
            }
        }
    }
    public class UploadItemInfo
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public UploadDataState State { get; set; }
        public Stream Stream { get; set; }
        public UploadItemInfo()
        {
            State = UploadDataState.Normal;
        }
        public UploadItemInfo(string name, Stream stream) : this()
        {
            Name = name;
            Stream = stream;
        }
    }
    public enum UploadDataState
    {
        Normal,
        Uploading,
        Uploaded
    }
}
