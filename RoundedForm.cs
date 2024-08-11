using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;

public class RoundedForm : Form {
    private const int WM_NCPAINT = 0x0085;
    private const int WM_NCCALCSIZE = 0x0083;
    private const int ButtonSize = 20; // 缩小关闭按钮尺寸
    private const int ButtonPadding = 10;
    private Rectangle closeButtonRect;

    [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

    [DllImport("user32.dll")]
    private static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

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

    private bool isHoveringCloseButton = false;

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
    }

    private void RoundedForm_Paint(object sender, PaintEventArgs e) {
        // 绘制背景颜色
        using (SolidBrush brush = new SolidBrush(BackgroundColor)) {
            e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }

        // 绘制关闭按钮
        closeButtonRect = new Rectangle(this.ClientSize.Width - ButtonSize - ButtonPadding, ButtonPadding, ButtonSize, ButtonSize);

        // 关闭按钮颜色
        Color buttonColor = isHoveringCloseButton ? CloseButtonHoverColor : CloseButtonColor;

        // 绘制叉叉本体
        using (Pen pen = new Pen(buttonColor, 2)) {
            e.Graphics.DrawLine(pen, closeButtonRect.Left + 4, closeButtonRect.Top + 4, closeButtonRect.Right - 4, closeButtonRect.Bottom - 4);
            e.Graphics.DrawLine(pen, closeButtonRect.Left + 4, closeButtonRect.Bottom - 4, closeButtonRect.Right - 4, closeButtonRect.Top + 4);
        }
    }

    protected override void OnSizeChanged(EventArgs e) {
        base.OnSizeChanged(e);
        if (this.Handle != IntPtr.Zero) {
            SetWindowRegion();
        }
    }

    private void SetWindowRegion() {
        IntPtr hRgn = CreateRoundRectRgn(0, 0, this.Width, this.Height, CornerRadius, CornerRadius);
        SetWindowRgn(this.Handle, hRgn, true);
    }

    protected override void WndProc(ref Message m) {
        if (m.Msg == WM_NCCALCSIZE || m.Msg == WM_NCPAINT) {
            // Avoid default non-client area calculation and painting
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
            // 检测鼠标是否在关闭按钮上
            bool newHoverState = closeButtonRect.Contains(e.Location);
            if (newHoverState != isHoveringCloseButton) {
                isHoveringCloseButton = newHoverState;
                this.Invalidate(); // 重新绘制
            }
        }
    }

    private void RoundedForm_MouseUp(object sender, MouseEventArgs e) {
        if (e.Button == MouseButtons.Left) {
            isDragging = false;
        }
    }

    private void RoundedForm_MouseLeave(object sender, EventArgs e) {
        // 当鼠标离开窗体时，确保关闭按钮颜色恢复
        if (isHoveringCloseButton) {
            isHoveringCloseButton = false;
            this.Invalidate(); // 重新绘制
        }
    }
}
