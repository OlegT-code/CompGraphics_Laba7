using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CompGraphics_Laba7
{
    public partial class Form1 : Form // Size Form: 1 000, 600, Size PictureBox: 975, 490
    {
        // Битовая картинка pictureBox
        Bitmap pictureBoxBitMap;
        // Битовая картинка динамического изображения
        Bitmap spriteBitMap;
        // Битовая картинка для временного хранения области экрана
        Bitmap cloneBitMap;
        // Графический контекст picturebox
        Graphics g_pictureBox;
        // Графический контекст спрайта
        Graphics g_sprite;
        int x, y; // Координаты автобуса
        int width = 300, height = 175; // Ширина и высота автобуса
        Timer timer;

        public Form1()
        {
            InitializeComponent();
        }

        // Функция рисования спрайта (корабля)
        void DrawSprite()
        {
            Pen myOkna = new Pen(Color.BurlyWood, 2);
            // Определение кистей
            SolidBrush myCorp = new SolidBrush(Color.Black);
            SolidBrush myTrum = new SolidBrush(Color.Green);
            SolidBrush myTrub = new SolidBrush(Color.Gray);
            // Рисование и закраска труб, трюма и корпуса корабля
            g_sprite.FillRectangle(myTrub, 100, 0, 25, 25);
            g_sprite.FillRectangle(myTrub, 150, 0, 25, 25);

            g_sprite.FillPolygon(myCorp, new Point[] {
                new Point(0, 75),new Point(300, 75),
                new Point(235, 175), new Point(50, 175),
                new Point(0, 75)
            });
            g_sprite.FillRectangle(myTrum, 50, 25, 175, 50);

            g_sprite.DrawEllipse(myOkna, 90, 40, 20, 20);
            g_sprite.DrawEllipse(myOkna, 130, 40, 20, 20);
            g_sprite.DrawEllipse(myOkna, 170, 40, 20, 20);
        }
            
        // Функция сохранения части изображения шириной
        void SavePart(int xt, int yt)
        {
            Rectangle cloneRect = new Rectangle(xt, yt, width, height);
            System.Drawing.Imaging.PixelFormat format =
            pictureBoxBitMap.PixelFormat;
            // Клонируем изображение, заданное прямоугольной областью
            cloneBitMap = pictureBoxBitMap.Clone(cloneRect, format);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Создаём Bitmap для pictureBox1 и графический контекст
            pictureBox1.Image = Image.FromFile("D:\\ocean.jpg");
            pictureBoxBitMap = new Bitmap(pictureBox1.Image);
            g_pictureBox = Graphics.FromImage(pictureBox1.Image);
            // Создаём Bitmap для спрайта и графический контекст
            spriteBitMap = new Bitmap(width, height);
            g_sprite = Graphics.FromImage(spriteBitMap);
            // Рисование и закраска моря
            int r = 50, iks = 50;
            while (iks <= pictureBox1.Width + r)
            {
                g_pictureBox.FillPie(Brushes.Blue, -50 + iks, 375, 50, 50, 0, -180);
                iks += 50;
            }
            // Рисуем автобус на графическом контексте g_sprite
            DrawSprite();
            // Создаём Bitmap для временного хранения части изображения
            cloneBitMap = new Bitmap(width, height);
            // Задаем начальные координаты вывода движущегося объекта
            x = 0; y = 200;
            // Сохраняем область экрана перед первым выводом объекта
            SavePart(x, y);
            // Выводим автобус на графический контекст g_pictureBox
            g_pictureBox.DrawImage(spriteBitMap, x, y);
            // Перерисовываем pictureBox1
            pictureBox1.Invalidate();
            // Создаём таймер с интервалом 100 миллисекунд
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer1_Tick);
        }

        // Обрабатываем событие от таймера
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Восстанавливаем затёртую область статического изображения
            g_pictureBox.DrawImage(cloneBitMap, x, y);
            // Изменяем координаты для следующего вывода автобуса
            x += 6;
            // Проверяем на выход изображения автобуса за правую границу
            if (x > pictureBox1.Width - 1) x = pictureBox1.Location.X;
            // Сохраняем область экрана перед первым выводом автобуса
            SavePart(x, y);
            // Выводим автобус
            g_pictureBox.DrawImage(spriteBitMap, x, y);
            // Перерисовываем pictureBox1
            pictureBox1.Invalidate();
        }
        // Включаем таймер по нажатию на кнопку
        private void button1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }
    }
}
