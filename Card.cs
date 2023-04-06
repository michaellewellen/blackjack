class Card
{
    // default constructor
    public Card(){}
    // constructor
    public Card(int num)
    {
        suit = getSuit((num-1)/13);
        number = getNumber ((num-1)%13);
        value = getValue(number);
        if (value == 11)
            altValue = 1;
        else    
            altValue = value;
        hidden = false;
        xValue = 0;
        yValue = 0;

    }
    char getSuit(int x)
    {
        switch (x)
        {
            case 0:
                return '♠';
            case 1:
                return '♣';
            case 2:
                return '♥';
            case 3:
                return '♦';
            default:
                return '0';
        }
    }
    char getNumber(int x)
    {
        switch (x)
        {
            case 0:
                return 'A';
            case 1:
                return '2';
            case 2:
                return '3';
            case 3:
                return '4';
            case 4:
                return '5';
            case 5:
                return '6';
            case 6:
                return '7';
            case 7:
                return '8';
            case 8:
                return '9';
            case 9:
                return 'T';
            case 10:
                return 'J';
            case 11:
                return 'Q';
            case 12:
                return 'K';
            default:
                return '0';
        }
    }
    int getValue(int x)
    {
        switch (x)
        {
            case 'A':
                return 11;
            case '2':
                return 2;
            case '3':
                return 3;
            case '4':
                return 4;
            case '5':
                return 5;
            case '6':
                return 6;
            case '7':
                return 7;
            case '8':
                return 8;
            case '9':
                return 9;
            case 'T':
            case 'J':
            case 'Q':
            case 'K':
                return 10;
            default:
                return 0;
        }
    }
    public void printCard()
    {
        Console.SetCursorPosition(xValue,yValue);
        Console.SetCursorPosition(xValue,yValue);
        Console.SetCursorPosition(xValue,yValue);
        Console.SetCursorPosition(xValue,yValue);
        if(hidden)
        {
            Console.WriteLine("┌───────┐");   
            Console.SetCursorPosition(xValue,yValue+1);
            Console.WriteLine("│▒▒▒▒▒▒▒│");
            Console.SetCursorPosition(xValue,yValue+2);
            Console.WriteLine("│▒▒▒▒▒▒▒│");
            Console.SetCursorPosition(xValue,yValue+3);
            Console.WriteLine("│▒▒▒▒▒▒▒│");
            Console.SetCursorPosition(xValue,yValue+4);
            Console.WriteLine("│▒▒▒▒▒▒▒│");
            Console.SetCursorPosition(xValue,yValue+5);
            Console.WriteLine("└───────┘");
        }
        else
        {
            Console.WriteLine("┌───────┐");   
            Console.SetCursorPosition(xValue,yValue+1);
            Console.WriteLine($"│{number}{suit}     │");
            Console.SetCursorPosition(xValue,yValue+2);
            Console.WriteLine("│       │");
            Console.SetCursorPosition(xValue,yValue+3);
            Console.WriteLine($"│   {suit}   │");
            Console.SetCursorPosition(xValue,yValue+4);
            Console.WriteLine("│       │");
            Console.SetCursorPosition(xValue,yValue+5);
            Console.WriteLine("└───────┘");            
        }
    }
    public char suit;
    public char number;
    public int value;
    public int altValue;
    public bool hidden;
    public int xValue;
    public int yValue;
}


