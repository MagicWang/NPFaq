using System;
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
    public class GalleryControl : ItemsControl
    {
        private Rectangle leftArrow;
        private Rectangle rightArrow;
        private Image image;
        private ListBox galleryLB;
        public GalleryControl()
        {
            this.DefaultStyleKey = typeof(GalleryControl);
        }
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (galleryLB != null && Items.Count > 0)
                galleryLB.SelectedIndex = 0;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            leftArrow = GetTemplateChild("LeftArrow") as Rectangle;
            rightArrow = GetTemplateChild("RightArrow") as Rectangle;
            image = GetTemplateChild("Image") as Image;
            galleryLB = GetTemplateChild("GalleryLB") as ListBox;
            if (galleryLB != null && Items.Count > 0)
                galleryLB.SelectedIndex = 0;
            leftArrow.MouseLeftButtonUp += LeftArrow_MouseLeftButtonUp;
            rightArrow.MouseLeftButtonUp += RightArrow_MouseLeftButtonUp;
            image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            galleryLB.SelectedIndex = galleryLB.SelectedIndex < galleryLB.Items.Count - 1 ? galleryLB.SelectedIndex + 1 : galleryLB.Items.Count - 1;
        }

        private void RightArrow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            galleryLB.SelectedIndex = galleryLB.SelectedIndex < galleryLB.Items.Count - 1 ? galleryLB.SelectedIndex + 1 : galleryLB.Items.Count - 1;
        }

        private void LeftArrow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            galleryLB.SelectedIndex = galleryLB.SelectedIndex > 0 ? galleryLB.SelectedIndex - 1 : 0;
        }
    }
}
