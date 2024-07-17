public class Spinner(ConsolePosition pos)
{
    private static char[] chars = ['|', '/', '-', '\\'];

    private byte index = 0;

    public void Spin()
    {
        index += 1;
        char c = chars[index % chars.Length];

        Console.SetCursorPosition(pos.left, pos.top);
        Console.Write(c);
    }
}
