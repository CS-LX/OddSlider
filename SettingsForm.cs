using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Design.AxImporter;

namespace OddSlider {
    public partial class SettingsForm : Form {
        public SettingsForm() {
            InitializeComponent();

            // 初始化 ListBox
            InitializeListBox();

            listBox1.SelectedItem = GlobalData.Mode.ToString();
            topMostCheckbox.Checked = GlobalData.TopMost;

            // 处理选项改变事件
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;

        }

        private void InitializeListBox() {
            // 清空现有项
            listBox1.Items.Clear();

            // 获取枚举类型
            var enumType = typeof(Mode);

            // 获取所有枚举值并添加到 ListBox
            foreach (var name in Enum.GetNames(enumType)) {
                listBox1.Items.Add(name);
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e) {
            // 获取选中的项并更新 currentOption
            var selectedName = listBox1.SelectedItem as string;
            if (selectedName != null) {
                GlobalData.Mode = (Mode)Enum.Parse(typeof(Mode), selectedName);
                // 根据 currentOption 执行相应操作
                // 例如: 
                // UpdateSomeUI(currentOption);
            }
        }

        private void topMostCheckbox_CheckedChanged(object sender, EventArgs e) {
            GlobalData.TopMost = topMostCheckbox.Checked;
        }
    }
}
