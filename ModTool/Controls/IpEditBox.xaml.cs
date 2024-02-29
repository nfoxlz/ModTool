using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ModTool.Controls
{
    /// <summary>
    /// IPEditBox.xaml 的交互逻辑
    /// </summary>
    public partial class IpEditBox : UserControl
    {
        public IpEditBox()
        {
            InitializeComponent();
        }

        public byte[] Value
        {
            get { return (byte[])GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private bool isValueChanging;

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(byte[]), typeof(IpEditBox), new PropertyMetadata((byte[])[0, 0, 0, 0], (d, e) =>
            {
                var box = (IpEditBox)d;

                box.isValueChanging = true;
                box.AddressTextBox1.Text = box.Value[0].ToString();
                box.AddressTextBox2.Text = box.Value[1].ToString();
                box.AddressTextBox3.Text = box.Value[2].ToString();
                box.AddressTextBox4.Text = box.Value[3].ToString();
                box.isValueChanging = false;
            }));

        private void AddressTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var key = e.Key;
            var textBox = (TextBox)sender;
            if ((key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9) || key == Key.Delete || key == Key.Enter || key == Key.Tab)
                return;

            if (key == Key.Back)
            {
                // 删除光标前面的数字，如果光标前没有数字，会跳转到前面一个输入框
                // 先Focus到了前面一个输入框，再执行的删除操作，所以会删除前面输入框中的一个数字
                if (textBox.CaretIndex == 0)
                    SetTextBoxFocus(textBox, false, false);
            }
            else if (key == Key.Left)
            {
                // 光标已经在当前输入框的最左边，则跳转到前面一个输入框
                if (textBox.CaretIndex == 0)
                {
                    SetTextBoxFocus(textBox, false, false);
                    // 得设置true，不然光标在前面一个输入框里也会移动一次
                    // 例如前一个输入框中的数字是123，Focus后，光标在数字3右边
                    // 不设置true，会移动到数字2和数字3之间
                    e.Handled = true;
                }
            }
            else if (key == Key.Right)
            {
                // 光标已经在当前输入框的最右边，则跳转到后面一个输入框
                if (textBox.SelectionStart == textBox.Text.Length)
                {
                    SetTextBoxFocus(textBox, true, false);
                    // true同理
                    e.Handled = true;
                }
            }
            else
            {
                // 不是上述按键，就不处理
                e.Handled = true;
            }
        }

        private void AddressTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            var key = e.Key;
            var textBox = (TextBox)sender;
            if (key == Key.Delete || key == Key.Enter || key == Key.Back || key == Key.Tab || key == Key.Left || key == Key.Right)
                return;

            if ((key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9))
            {
                // 当前输入框满三个数字后
                // 跳转到后面一个输入框
                if (textBox.Text.Length == 3)
                {
                    if (int.Parse(textBox.Text) < 0 || int.Parse(textBox.Text) > 255)
                    {
                        textBox.Text = "255";
                        //MessageBox.Show(textBox.Text + "不是有效项。请指定一个介于0和255间的值。", "错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    SetTextBoxFocus(textBox, true, true);
                }
            }
            else
            {
                // 不是上述按键，就不处理
                e.Handled = true;
            }
        }

        /// <summary>
        /// 设置当前输入框的前面或后面的输入框获取焦点，以及是否全选内容
        /// </summary>
        /// <param name="curretTextBox">当前输入框</param>
        /// <param name="isBack">是否是后面的输入框（false为前面的输入框）</param>
        /// <param name="isSelectAll">是否全选内容</param>
        private void SetTextBoxFocus(TextBox curretTextBox, bool isBack, bool isSelectAll)
        {
            // 所有的ip输入框
            var textBoxIPList = new List<TextBox>();
            foreach (UIElement item in GridIPAddress.Children)
                if (item.GetType() == typeof(TextBox))
                    textBoxIPList.Add((TextBox)item);

            // 要聚焦的输入框
            TextBox? nextTextBox = null;
            // 往后
            if (isBack)
            {
                // 当前输入框是前三个，那么就取后一个输入框
                int index = textBoxIPList.IndexOf(curretTextBox);
                if (index <= 2)
                    nextTextBox = textBoxIPList[index + 1];
            }
            // 往前
            else
            {
                // 当前输入框是后三个，那么就取前一个输入框
                int index = textBoxIPList.IndexOf(curretTextBox);
                if (index >= 1)
                    nextTextBox = textBoxIPList[index - 1];
            }

            // 设置焦点 全选内容
            if (nextTextBox != null)
            {
                nextTextBox.Focus();
                if (isSelectAll)
                    nextTextBox.SelectAll();
            }
        }

        private void AddressTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValueChanging)
                return;

            byte.TryParse(AddressTextBox1.Text, out Value[0]);
        }

        private void AddressTextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValueChanging)
                return;

            byte.TryParse(AddressTextBox2.Text, out Value[1]);
        }

        private void AddressTextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValueChanging)
                return;

            byte.TryParse(AddressTextBox3.Text, out Value[2]);
        }

        private void AddressTextBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValueChanging)
                return;

            byte.TryParse(AddressTextBox4.Text, out Value[3]);
        }
    }
}
