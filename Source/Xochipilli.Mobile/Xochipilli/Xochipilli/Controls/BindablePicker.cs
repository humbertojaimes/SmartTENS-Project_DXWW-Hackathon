using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xochipilli.Controls
{
    public class BindablePicker : Picker
    {
        public BindablePicker()
        {
            this.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(List<string>), typeof(BindablePicker), null, BindingMode.OneWay, null, propertyChanged: OnItemsSourceChanged);

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = bindable as BindablePicker;
            picker.Items.Clear();
            if ((List<string>)newValue != null)
            {
                //now it works like "subscribe once" but you can improve
                foreach (var item in (List<string>)newValue)
                {
                    picker.Items.Add(item.ToString());
                }
            }
        }

        public static readonly BindableProperty SelectedItemProperty =
           BindableProperty.Create("SelectedItem", typeof(string), typeof(BindablePicker), null, BindingMode.TwoWay, null, propertyChanged: OnSelectedItemChanged);

        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as BindablePicker;
            if (newvalue != null)
            {
                picker.SelectedIndex = picker.Items.IndexOf(newvalue.ToString());
            }
        }

        public List<string> ItemsSource
        {
            get { return (List<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = Items[SelectedIndex];
            }
        }
    }
}