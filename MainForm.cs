namespace OddSlider {
    public partial class MainForm : RoundedForm {
        public MainForm() {
            InitializeComponent();
            DotButtonClickAction += () => { 
                SettingsForm settings = new SettingsForm();
                settings.ShowDialog();
            };
        }
    }
}
