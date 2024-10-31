using System;
using System.Threading;

namespace Papchef;
public class ConsoleProgressBar
{
    private int left;
    private int top;
    private int length;

    public ConsoleProgressBar(int left, int top, int length)
    {
        this.left = left;
        this.top = top;
        this.length = length;
    }

    public void ShowProgress(int progress, string message)
    {
        if (progress < 0 || progress > length)
            throw new ArgumentException($"Invalid progress value, must be between 0 and {length} but actual {progress}.");
        
        Console.SetCursorPosition(left, top);
        double percentage = (double)progress / length * 100;
        Console.Write($"{new string('█', progress)}{new string('░', length - progress)} {percentage:0.00}% - {message}");
    }
}