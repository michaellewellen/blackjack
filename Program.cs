Stack<Card> deckOfCards = shuffleCards();
List<Card> dealersHand = new List<Card>();
List<Card> playersHand = new List<Card>();
Console.Clear();
loadMenu();
string playAgain = "Y";
int winnings = 500;
int bet = 0;
while (playAgain.ToUpper() != "Q")
{
    bet = betScreen(winnings);

}

Stack<Card> shuffleCards()
{
    Stack<Card> deck = new Stack<Card>();
    Random rand = new Random();
    int[] nums = new int[52];
    for (int i = 0; i< nums.Length; i++)
    {
        int num = 0;
        bool isUnique = false;
        while(!isUnique)
        {
            num = rand.Next(1,53);
            if (!nums.Contains(num))
                isUnique = true;
        }
        nums[i] = num;
        Card temp = new Card(num);
        deck.Push(temp);
    }
    return deck;
}
List<Card> getCards(ref Stack<Card> deck, List<Card> cards,int num, bool isDealer)
{
    Card temp;
    int size = cards.Count();
    for (int i = cards.Count(); i< num+size; i++)
    {
        temp = deck.Pop();
        if (i==0 && isDealer == true)
            temp.hidden = true;
        if (isDealer)
            temp.yValue = 0;
        else 
            temp.yValue = 8;
        temp.xValue = i * 5;
        cards.Add(temp);
      
    }
    return cards;
}
void loadMenu()
{
    Console.Clear();
    string welcome = @"
  _______ __            __    _______            __    __ __ 
 |   _   |  .---.-.----|  |--|   _   .---.-.----|  |--|  |  |
 |.  1   |  |  _  |  __|    <|___|   |  _  |  __|    <|__|__|
 |.  _   |__|___._|____|__|__|.  |   |___._|____|__|__|__|__|
 |:  1    \                  |:  1   |                       
 |::.. .  /                  |::.. . |                       
 `-------'                   `-------'                       ";
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(welcome);
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.White;

    Console.WriteLine("\n              Press any key to continue");
    Console.ReadKey(true);

}
int betScreen(int x)
{
    string placeYourBet = @"
    The game is Black Jack, your goal is to get 21.  If you do it in two cards thats a blackjack
    pays 3/2 regular win doubles your bet. The house has to hit on 16, stay on 17
    
    ";
    Console.Clear();
    Stack<Card> deck = shuffleCards();
    List<Card> topRow = new List<Card>();
    List<Card> bottomRow = new List<Card>();
    topRow = getCards(ref deck,topRow,20,true);
    bottomRow = getCards(ref deck,bottomRow,20,false);
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Red;
    for (int i = 0; i<20; i++)
    {
        topRow[i].hidden = true;
        bottomRow[i].hidden = true;
        
        topRow[i].printCard();
        bottomRow[i].printCard();
    }

    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.White;
    Console.SetCursorPosition(0,15);
    Console.WriteLine(placeYourBet);
    Console.Write($"How much of your ${x} do you want to bet? ");
    bool validBet = false;
    int wager = 0;
    while(!validBet)
    {
        validBet = Int32.TryParse(Console.ReadLine(), out wager);
        if (!validBet)
        {
            Console.WriteLine("Partner you'll find a number works best");
        }
        else if (wager == 0)
        {
            Console.WriteLine("Ain't much point in playing if you don't bet anything.");
            validBet = false;
        }
        else if (wager < 0 || wager > x)
        {
            Console.WriteLine("You just don't have that kinda money");
            validBet = false;
        }
    }
    return wager;
}

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