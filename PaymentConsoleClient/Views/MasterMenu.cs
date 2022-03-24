using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace PaymentConsoleClient.Views;

public class MasterMenu
{
    private int _selectedIndex;
    private readonly Dictionary<object, string> _options;
    private readonly string _prompt;

    public MasterMenu(string prompt, Dictionary<object,string> options)
    {
        _options = options;
        _prompt = prompt;
        _selectedIndex = 0;
    }

    private void DisplayOptions()
    {
        WriteLine(_prompt);
        
        for (int i = 0; i < _options.Count; i++)
        {
            KeyValuePair<object, string> entry = _options.ElementAt(i);
            
            string currentOption = entry.Value;
            string prefix;
            if (i == _selectedIndex)
            {
                prefix = "*";
                ForegroundColor = ConsoleColor.Black;
                BackgroundColor = ConsoleColor.White;
            }
            else
            {
                prefix = " ";
                ForegroundColor = ConsoleColor.White;
                BackgroundColor = ConsoleColor.Black;
            }
            WriteLine($"{prefix} << {currentOption} >>");
        }
        ResetColor();
    }

    public object Run()
    {
        ConsoleKey keyPressed;
        do
        {
            Clear();
            DisplayOptions();
            ConsoleKeyInfo keyInfo = ReadKey(true);
            keyPressed = keyInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow when _selectedIndex > 0:
                    _selectedIndex--;
                    break;
                case ConsoleKey.DownArrow when _selectedIndex < _options.Count -1:
                    _selectedIndex++;
                    break;
            }
            
        } while (keyPressed != ConsoleKey.Enter);

        var selectedEnum = _options.ElementAt(_selectedIndex).Key;
        return selectedEnum;
    }
}