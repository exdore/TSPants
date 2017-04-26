using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TSPants
{
    public class Data
    {
        public List<City> Cities { get; set; }
        public double[,] Distances { get; set; }

        public void ReadCitiesCoordinates(string path)
        {
            Cities = new List<City>();
            StreamReader reader = new StreamReader(path);
            var line = reader.ReadLine();
            do
            {
                if (line != null)
                {
                    var splitLine = line.Split(' ');
                    Cities.Add(new City
                    {
                        X = Convert.ToInt32(splitLine[1]),
                        Y = Convert.ToInt32(splitLine[2]),
                        Name = Convert.ToInt32(splitLine[0])
                    });
                }
            } while ((line = reader.ReadLine()) != null && line != "");
            reader.Close();
        }

        public void CalculateDistances(GroupBox grpBox)
        {
            var size = Cities.Count;
            Distances = new double[size, size];
            var rbt = grpBox.Controls.Cast<RadioButton>().FirstOrDefault(item => item.Checked);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (rbt != null && rbt.Name == "euclid")
                        Distances[i, j] = Math.Sqrt((Cities[i].X - Cities[j].X) * (Cities[i].X - Cities[j].X)
                                                    + (Cities[i].Y - Cities[j].Y) * (Cities[i].Y - Cities[j].Y));
                    else
                        Distances[i, j] = Math.Abs(Cities[i].X - Cities[j].X) + Math.Abs(Cities[i].Y - Cities[j].Y);
                }
            }
        }

        public Bitmap DrawCityList()
        {
            var maxX = Cities.Max(item => item.X);
            var maxY = Cities.Max(item => item.Y);
            foreach (var city in Cities)
            {
                city.X = Convert.ToInt32((double)city.X / maxX * 800);
                city.Y = Convert.ToInt32((double)city.Y / maxY * 400);
            }
            Bitmap bmp = new Bitmap(850, 450);
            Graphics graph = Graphics.FromImage(bmp);
            foreach (var city in Cities)
            {
                graph.FillEllipse(new SolidBrush(Color.Black), city.X, city.Y, 5, 5);
            }
            return bmp;
        }

        public Bitmap DrawPath(Bitmap bmp, List<Edge> path, Data data)
        {
            Graphics graph = Graphics.FromImage(bmp);
            for (int i = 0; i < path.Count; i++)
            {
                graph.DrawLine(Pens.Black, new Point(data.Cities[path[i].Start].X, data.Cities[path[i].Start].Y), new Point(data.Cities[path[i].End].X, data.Cities[path[i].End].Y));
            }
            graph.DrawLine(Pens.Black, new Point(data.Cities[path[0].Start].X, data.Cities[path[0].Start].Y), new Point(data.Cities[path.Last().End].X, data.Cities[path.Last().End].Y));
            return bmp;
        }
    }
}
