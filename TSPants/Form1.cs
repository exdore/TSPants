using System;
using System.Drawing;
using System.Windows.Forms;

namespace TSPants
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Solver slv = new Solver();
            FileDialog fileDialog = new OpenFileDialog { Filter = @"tsp files (*.tsp)|*.tsp|All files (*.*)|*.*" };
            fileDialog.ShowDialog();
            var filePath = fileDialog.FileName;
            var data = slv.Initialize(filePath, groupBox1);
            PictureBox pictureBox = new PictureBox { Size = new Size(850, 450) };
            Controls.Add(pictureBox);
            Size = new Size(1300, 750);
            groupBox1.Location = new Point
            {
                X = 900
            };
            button2.Location = new Point
            {
                X = 900,
                Y = button2.Location.Y
            };
            pictureBox.Image = data.DrawCityList();
            var bmp = new Bitmap(pictureBox.Image);
            var result = slv.Run(data);
            pictureBox.Image = data.DrawPath(bmp, result.Edges, data);
        }
    }
}
