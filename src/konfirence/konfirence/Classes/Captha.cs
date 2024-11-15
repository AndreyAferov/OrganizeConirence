using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;

public class CaptchaGenerator
{
    public string CaptchaText { get; private set; }

    public CaptchaGenerator()
    {
        CaptchaText = GenerateCaptchaText(4);
    }

    private string GenerateCaptchaText(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        char[] captcha = new char[length];
        for (int i = 0; i < length; i++)
        {
            captcha[i] = chars[random.Next(chars.Length)];
        }
        return new string(captcha);
    }

    public void GenerateCaptcha(Canvas captchaCanvas)
    {
        captchaCanvas.Children.Clear();
        Random random = new Random();
        captchaCanvas.Background = new SolidColorBrush(Colors.White);

        for (int i = 0; i < CaptchaText.Length; i++)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = CaptchaText[i].ToString(),
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.Black)
            };

            Canvas.SetLeft(textBlock, 10 + i * 30);
            Canvas.SetTop(textBlock, random.Next(5, 15));

            captchaCanvas.Children.Add(textBlock);

            if (random.Next(0, 2) == 0)
            {
                Line line = new Line
                {
                    X1 = 10 + i * 30,
                    Y1 = random.Next(15, 30),
                    X2 = 30 + i * 30,
                    Y2 = random.Next(15, 30),
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 2
                };
                captchaCanvas.Children.Add(line);
            }
        }

        for (int i = 0; i < 50; i++)
        {
            Line noiseLine = new Line
            {
                X1 = random.Next(120),
                Y1 = random.Next(40),

                Stroke = new SolidColorBrush(Colors.Gray),
                StrokeThickness = 1
            };
            captchaCanvas.Children.Add(noiseLine);
        }
    }
}
