using System.Runtime.InteropServices;

namespace OddSlider {
    public partial class MainForm : RoundedForm {

        public MainForm() {
            InitializeComponent();
            DotButtonClickAction += () => {
                Task.Run(() => {
                    SettingsForm settings = new SettingsForm();
                    settings.ShowDialog();
                });
            };
            flatSlider1.ValueChanged = ValueChanged;
        }

        public void ValueChanged(int value) {
            WH wH = new WH();
            switch (GlobalData.Mode) {
                case Mode.音量控制:
                    wH.ChangeVolume((double)value / 100, true);
                    break;
                case Mode.屏幕亮度控制:
                    WindowsSettingsBrightnessController.Set(value);
                    using (PhysicalMonitorBrightnessController controller = new PhysicalMonitorBrightnessController()) {
                        controller.Set((uint)value);
                    }
                    break;
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            TopMost = GlobalData.TopMost;
        }
    }
}
