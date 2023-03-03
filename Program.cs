Stack<Card> deckOfCards = shuffleCards();
List<Card> dealersHand = new List<Card>();
List<Card> playersHand = new List<Card>();
Console.Clear();
loadMenu();
string? playAgain = "Y";
double winnings = 500;
int bet = 0;
while (playAgain != "Q")
{
    bet = betScreen(winnings);
    Console.Clear();
    dealersHand = getCards(ref deckOfCards,dealersHand,2,true);
    playersHand = getCards(ref deckOfCards,playersHand,2,false);
    int gameStatus = playTheGame(dealersHand,playersHand, ref deckOfCards);
    switch (gameStatus)
    {
        case 0: 
            winnings -= bet;
            Console.WriteLine("You lost!");
            break;
        case 1: 
            winnings += bet;
            Console.WriteLine("You Won");
            break;
        case 2:
            winnings += 1.5*bet;
            break;
        case 3: 
            Console.WriteLine("You pushed");
            break;
    }
    Console.WriteLine("Press any key to play again or (Q) to quit");
    playAgain = Console.ReadKey(true).ToString();
    playAgain = playAgain.ToUpper();
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
        cards[i].printCard();
      
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
int betScreen(double x)
{
    string placeYourBet = @"
    The game is Black Jack, your goal is to get 21.  If you do it in two cards thats a blackjack
    pays 3/2 regular win doubles your bet. The house has to hit on 16, stay on 17
    
    ";
    Console.Clear();
    Stack<Card> deck = shuffleCards();
    List<Card> topRow = new List<Card>();
    List<Card> bottomRow = new List<Card>();
    topRow = getCards(ref deck,topRow,5,true);
    bottomRow = getCards(ref deck,bottomRow,5,false);
    for (int i = 0; i<topRow.Count(); i++)
    {
        topRow[i].hidden = true;
        bottomRow[i].hidden = true;
        
        topRow[i].printCard();
        bottomRow[i].printCard();
    }

   
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

int playTheGame(List<Card> deal, List<Card> play, ref Stack<Card> deck)
{
    Console.SetCursorPosition(0,14 );
    int showing = deal[1].value;
    int dealValue = 0;
    int altdealValue = 0;
    int playValue = 0;
    int altplayValue = 0;
    for (int i = 0; i< play.Count(); i++)
    {
        dealValue += deal[i].value;
        playValue += play[i].value;
        altdealValue += deal[i].altValue;
        altplayValue += play [i].altValue;
    }
    bool busted = false;
    bool dealBusted = false;
    int playReal = playValue>21? altplayValue:playValue;
    int dealReal = dealValue>21? altdealValue:dealValue;
    if (playReal > 21) 
        busted = true;
        
    if (play.Count() == 2 && playValue == 21)
    {   Console.WriteLine("BLACKJACK!");
        return 2;
    }
    if (deal.Count() == 2 && dealValue == 21)
    {
        deal[0].hidden = false;
        deal[0].printCard();
        Console.WriteLine("Dealer has Blackjack, you lose.");
        return 0;
    }
    char choice = '\0';
    // players turn
    while(!busted)
    {
        Console.WriteLine($"You have {playReal}, the dealer is showing {showing}. ");
        Console.WriteLine("Do you want to (h)it, (s)tand, or (d)ouble ?");
        bool acceptable = false;
        while(!acceptable)
        {
            choice = (char)Console.ReadKey(true).KeyChar;
            if (choice == 'h' || choice == 's' || choice == 'd')
                acceptable = true;            
        }
        if(choice == 'h')
        {
            play= getCards(ref deck,play,1,false);
            playValue += play[^1].value;
            altplayValue += play[^1].altValue;
            playReal = playValue>21? altplayValue:playValue;
            if (playReal > 21)
                busted = true;
        }
        else if (choice == 's')
            break;
        // same as a hit, but can only do it once.
        else if (choice == 'd')
        {
            if(play.Count() > 2)
                Console.WriteLine("You can only double down on the first card");
            else
            {
                play= getCards(ref deck,play,1,false);
                playValue += play[^1].value;
                altplayValue += play[^1].altValue;
                playReal = playValue>21? altplayValue:playValue;
                if (playReal > 21)
                    busted = true;
                break;
            }
        }
    }
    // dealer's turn
    deal[0].hidden = false;
    deal[0].printCard();
    while(dealReal < 17)
    {
        deal = getCards(ref deck,deal,1,true);
        dealValue += deal[^1].value;
        altdealValue += deal[^1].altValue;
        dealReal = dealValue>21? altdealValue:dealValue;
        if (dealReal > 21)
            dealBusted = true;
    }
    if((dealBusted == true && busted == true) || playReal == dealReal)
        return 3;
    else if (playReal > dealReal)
        return 1;
    else    
        return 0;  
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


