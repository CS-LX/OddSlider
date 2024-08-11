namespace OddSlider {
    public partial class MainForm : RoundedForm {
        public MainForm() {
            InitializeComponent();
            DotButtonClickAction += () => { 
                SettingsForm settings = new SettingsForm();
                settings.ShowDialog();
            };
            flatSlider1.ValueChanged = ValueChanged;
        }

        public void ValueChanged(int value) {
            WH wH = new WH();
            wH.ChangeVolume((double)value / 100, true);
        }
    }
}
