using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

public class FlatSlider : Control {
    private int value;
    private int maximum = 100;
    private int minimum = 0;
    private int thumbSize = 20;
    private int trackHeight = 4;
    private int thumbOffsetY = 0;
    private Color trackColor = Color.LightGray;
    private Color thumbColor = Color.Blue;
    private Color thumbHoverColor = Color.DarkBlue;
    private Image trackImage;
    private Image thumbImage;
    private PictureBoxSizeMode trackMode = PictureBoxSizeMode.StretchImage;
    private PictureBoxSizeMode thumbMode = PictureBoxSizeMode.StretchImage;

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the maximum value of the slider.")]
    public int Maximum {
        get { return maximum; }
        set { maximum = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the minimum value of the slider.")]
    public int Minimum {
        get { return minimum; }
        set { minimum = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the current value of the slider.")]
    public int Value {
        get { return value; }
        set {
            this.value = Math.Max(Minimum, Math.Min(Maximum, value));
            Invalidate();
        }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the track color.")]
    public Color TrackColor {
        get { return trackColor; }
        set { trackColor = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the thumb color.")]
    public Color ThumbColor {
        get { return thumbColor; }
        set { thumbColor = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the color of the thumb when hovered.")]
    public Color ThumbHoverColor {
        get { return thumbHoverColor; }
        set { thumbHoverColor = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the track image.")]
    public Image TrackImage {
        get { return trackImage; }
        set { trackImage = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the thumb image.")]
    public Image ThumbImage {
        get { return thumbImage; }
        set { thumbImage = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the size mode for the track image.")]
    public PictureBoxSizeMode TrackImageMode {
        get { return trackMode; }
        set { trackMode = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the size mode for the thumb image.")]
    public PictureBoxSizeMode ThumbImageMode {
        get { return thumbMode; }
        set { thumbMode = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the height of the track.")]
    public int TrackHeight {
        get { return trackHeight; }
        set { trackHeight = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the vertical offset of the thumb.")]
    public int ThumbOffsetY {
        get { return thumbOffsetY; }
        set { thumbOffsetY = value; Invalidate(); }
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Gets or sets the size of the thumb.")]
    public int ThumbSize {
        get { return thumbSize; }
        set { thumbSize = value; Invalidate(); }
    }

    public FlatSlider() {
        this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
        this.Height = thumbSize + 10; // Adjust height to fit the thumb
        this.MouseDown += FlatSlider_MouseDown;
        this.MouseMove += FlatSlider_MouseMove;
        this.MouseUp += FlatSlider_MouseUp;
        this.MouseLeave += FlatSlider_MouseLeave;
        this.Cursor = Cursors.Hand; // Change cursor to indicate slider is interactive
    }

    private bool isDragging = false;
    private Rectangle thumbRect;

    protected override void OnPaint(PaintEventArgs e) {
        base.OnPaint(e);

        // Draw track image
        if (trackImage != null) {
            DrawImage(e.Graphics, trackImage, new Rectangle(0, this.Height / 2 - trackHeight / 2, this.Width, trackHeight), trackMode);
        }
        else {
            // Draw track color if no image is set
            using (SolidBrush trackBrush = new SolidBrush(trackColor)) {
                e.Graphics.FillRectangle(trackBrush, 0, this.Height / 2 - trackHeight / 2, this.Width, trackHeight);
            }
        }

        // Calculate thumb position
        int thumbX = (int)((float)(Value - Minimum) / (Maximum - Minimum) * (this.Width - thumbSize));
        thumbRect = new Rectangle(thumbX, this.Height / 2 - thumbSize / 2 + thumbOffsetY, thumbSize, thumbSize);

        // Draw thumb image
        if (thumbImage != null) {
            DrawImage(e.Graphics, thumbImage, thumbRect, thumbMode);
        }
        else {
            // Draw thumb color if no image is set
            using (SolidBrush thumbBrush = new SolidBrush(isDragging ? ThumbHoverColor : ThumbColor)) {
                e.Graphics.FillRectangle(thumbBrush, thumbRect);
            }
        }
    }

    private void DrawImage(Graphics g, Image img, Rectangle destRect, PictureBoxSizeMode mode) {
        switch (mode) {
            case PictureBoxSizeMode.StretchImage:
                g.DrawImage(img, destRect);
                break;
            case PictureBoxSizeMode.CenterImage:
                g.DrawImage(img, new Rectangle(
                    destRect.X + (destRect.Width - img.Width) / 2,
                    destRect.Y + (destRect.Height - img.Height) / 2,
                    img.Width,
                    img.Height));
                break;
            case PictureBoxSizeMode.Zoom:
                // Maintain aspect ratio
                float ratio = Math.Min((float)destRect.Width / img.Width, (float)destRect.Height / img.Height);
                int width = (int)(img.Width * ratio);
                int height = (int)(img.Height * ratio);
                g.DrawImage(img, new Rectangle(
                    destRect.X + (destRect.Width - width) / 2,
                    destRect.Y + (destRect.Height - height) / 2,
                    width,
                    height));
                break;
                // Handle other modes if needed
        }
    }

    private void FlatSlider_MouseDown(object sender, MouseEventArgs e) {
        if (thumbRect.Contains(e.Location)) {
            isDragging = true;
            Capture = true;
            UpdateValue(e.X);
        }
        else {
            // Set value based on mouse click
            UpdateValue(e.X);
        }
    }

    private void FlatSlider_MouseMove(object sender, MouseEventArgs e) {
        if (isDragging) {
            UpdateValue(e.X);
        }
    }

    private void FlatSlider_MouseUp(object sender, MouseEventArgs e) {
        if (isDragging) {
            isDragging = false;
            Capture = false;
        }
    }

    private void FlatSlider_MouseLeave(object sender, EventArgs e) {
        if (isDragging) {
            isDragging = false;
            Capture = false;
        }
    }

    private void UpdateValue(int mouseX) {
        int newValue = (int)(((float)(mouseX - thumbSize / 2) / (this.Width - thumbSize)) * (Maximum - Minimum) + Minimum);
        Value = Math.Max(Minimum, Math.Min(Maximum, newValue));
    }
}
