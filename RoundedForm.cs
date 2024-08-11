using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;

public class RoundedForm : Form {
    private const int WM_NCPAINT = 0x0085;
    private const int WM_NCCALCSIZE = 0x0083;
    private const int ButtonSize = 20; // 按钮尺寸
    private const int ButtonPadding = 10; // 按钮间距
    private Rectangle closeButtonRect;
    private Rectangle dotButtonRect;

    [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

    [DllImport("user32.dll")]
    private static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

    // Public properties
    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the corner radius of the rounded form.")]
    public int CornerRadius { get; set; } = 20;

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the background color of the rounded form.")]
    public Color BackgroundColor { get; set; } = Color.LightBlue;

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the color of the close button.")]
    public Color CloseButtonColor { get; set; } = Color.Black;

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the color of the close button when hovered.")]
    public Color CloseButtonHoverColor { get; set; } = Color.DarkGray;

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the color of the dot button.")]
    public Color DotButtonColor { get; set; } = Color.Gray;

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the color of the dot button when hovered.")]
    public Color DotButtonHoverColor { get; set; } = Color.DarkGray;

    [Browsable(true)]
    [Category("Behavior")]
    [Description("Gets or sets the action to be triggered when the dot button is clicked.")]
    public Action DotButtonClickAction { get; set; }

    private bool isHoveringCloseButton = false;
    private bool isHoveringDotButton = false;

    public RoundedForm() {
        this.FormBorderStyle = FormBorderStyle.None;
        this.Load += new EventHandler(RoundedForm_Load);
        this.Paint += new PaintEventHandler(RoundedForm_Paint);
        this.MouseDown += new MouseEventHandler(RoundedForm_MouseDown);
        this.MouseMove += new MouseEventHandler(RoundedForm_MouseMove);
        this.MouseUp += new MouseEventHandler(RoundedForm_MouseUp);
        this.MouseLeave += new EventHandler(RoundedForm_MouseLeave);
    }

    private void RoundedForm_Load(object sender, EventArgs e) {
        SetWindowRegion();
        ArrangeButtons();
    }

    private void RoundedForm_Paint(object sender, PaintEventArgs e) {
        // Draw background color
        using (SolidBrush brush = new SolidBrush(BackgroundColor)) {
            e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }

        // Draw close button
        closeButtonRect = new Rectangle(this.ClientSize.Width - ButtonSize - ButtonPadding, ButtonPadding, ButtonSize, ButtonSize);
        Color closeButtonColor = isHoveringCloseButton ? CloseButtonHoverColor : CloseButtonColor;

        // Draw close button body
        using (Pen pen = new Pen(closeButtonColor, 2)) {
            e.Graphics.DrawLine(pen, closeButtonRect.Left + 4, closeButtonRect.Top + 4, closeButtonRect.Right - 4, closeButtonRect.Bottom - 4);
            e.Graphics.DrawLine(pen, closeButtonRect.Left + 4, closeButtonRect.Bottom - 4, closeButtonRect.Right - 4, closeButtonRect.Top + 4);
        }

        // Draw dot button (three horizontal dots)
        dotButtonRect = new Rectangle(this.ClientSize.Width - 2 * (ButtonSize + ButtonPadding), ButtonPadding, ButtonSize, ButtonSize);
        Color dotButtonColor = isHoveringDotButton ? DotButtonHoverColor : DotButtonColor;

        // Draw three horizontal dots, centered vertically with close button
        using (SolidBrush dotBrush = new SolidBrush(dotButtonColor)) {
            int dotSize = 4;
            int startX = dotButtonRect.Left + (ButtonSize - 3 * dotSize - 2 * 3) / 2; // Adjusted spacing
            int centerY = dotButtonRect.Top + (ButtonSize - dotSize) / 2;

            e.Graphics.FillEllipse(dotBrush, startX, centerY, dotSize, dotSize);
            e.Graphics.FillEllipse(dotBrush, startX + dotSize + 3, centerY, dotSize, dotSize);
            e.Graphics.FillEllipse(dotBrush, startX + 2 * (dotSize + 3), centerY, dotSize, dotSize);
        }
    }

    protected override void OnSizeChanged(EventArgs e) {
        base.OnSizeChanged(e);
        if (this.Handle != IntPtr.Zero) {
            SetWindowRegion();
            ArrangeButtons();
        }
    }

    private void SetWindowRegion() {
        IntPtr hRgn = CreateRoundRectRgn(0, 0, this.Width, this.Height, CornerRadius, CornerRadius);
        SetWindowRgn(this.Handle, hRgn, true);
    }

    protected override void WndProc(ref Message m) {
        if (m.Msg == WM_NCCALCSIZE || m.Msg == WM_NCPAINT) {
            return;
        }

        base.WndProc(ref m);
    }

    private bool isDragging = false;
    private Point dragStartPoint;

    private void RoundedForm_MouseDown(object sender, MouseEventArgs e) {
        if (e.Button == MouseButtons.Left) {
            if (closeButtonRect.Contains(e.Location)) {
                this.Close();
            }
            else if (dotButtonRect.Contains(e.Location) && DotButtonClickAction != null) {
                DotButtonClickAction.Invoke();
            }
            else {
                isDragging = true;
                dragStartPoint = e.Location;
            }
        }
    }

    private void RoundedForm_MouseMove(object sender, MouseEventArgs e) {
        if (isDragging) {
            this.Left += e.X - dragStartPoint.X;
            this.Top += e.Y - dragStartPoint.Y;
        }
        else {
            bool newHoverClose = closeButtonRect.Contains(e.Location);
            bool newHoverDot = dotButtonRect.Contains(e.Location);

            if (newHoverClose != isHoveringCloseButton) {
                isHoveringCloseButton = newHoverClose;
                this.Invalidate();
            }

            if (newHoverDot != isHoveringDotButton) {
                isHoveringDotButton = newHoverDot;
                this.Invalidate();
            }
        }
    }

    private void RoundedForm_MouseUp(object sender, MouseEventArgs e) {
        if (e.Button == MouseButtons.Left) {
            isDragging = false;
        }
    }

    private void RoundedForm_MouseLeave(object sender, EventArgs e) {
        if (isHoveringCloseButton || isHoveringDotButton) {
            isHoveringCloseButton = false;
            isHoveringDotButton = false;
            this.Invalidate();
        }
    }

    private void ArrangeButtons() {
        closeButtonRect = new Rectangle(this.ClientSize.Width - ButtonSize - ButtonPadding, ButtonPadding, ButtonSize, ButtonSize);
        dotButtonRect = new Rectangle(this.ClientSize.Width - 2 * (ButtonSize + ButtonPadding), ButtonPadding, ButtonSize, ButtonSize);
    }
}
