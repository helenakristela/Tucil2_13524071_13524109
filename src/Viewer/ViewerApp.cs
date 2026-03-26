using System;
using System.Windows.Forms;
using System.Drawing;

public class ViewerApp : Form
{
    private Model model;
    private Camera cam = new Camera();

    public ViewerApp(string path)
    {
        this.Width = 800;
        this.Height = 600;
        this.DoubleBuffered = true;

        model = ObjLoaderViewer.Load(path);

        this.KeyDown += OnKeyDown;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Renderer.DrawModel(e.Graphics, model, cam, this.Width, this.Height);
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Left) cam.Rotate(-0.1f, 0);
        if (e.KeyCode == Keys.Right) cam.Rotate(0.1f, 0);
        if (e.KeyCode == Keys.Up) cam.Rotate(0, -0.1f);
        if (e.KeyCode == Keys.Down) cam.Rotate(0, 0.1f);

        if (e.KeyCode == Keys.W) cam.Zoom(-0.2f);
        if (e.KeyCode == Keys.S) cam.Zoom(0.2f);

        this.Invalidate();
    }

    [STAThread]
    public static void Run(string path)
    {
        Application.EnableVisualStyles();
        Application.Run(new ViewerApp(path));
    }
}
