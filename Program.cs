List<Card> dealersHand = new List<Card>();
List<Card> playersHand = new List<Card>();
Console.Clear();
loadMenu();
char? playAgain = 'Y';
// winnings is how much money you have, no save system yet so you always start with $500
double winnings = Convert.ToDouble(File.ReadAllText("playerInfo.txt"));
double bet = 0;
while (playAgain != 'Q')
{
    // get a fresh deck of cards
    Stack<Card> deckOfCards = shuffleCards();
    bet = betScreen(ref winnings);
    Console.Clear();
    // get cards will draw the number of cards in the 'int' field
    dealersHand.Clear();
    playersHand.Clear();
    dealersHand = getCards(ref deckOfCards,dealersHand,2,true);
    playersHand = getCards(ref deckOfCards,playersHand,2,false);
    // gameStatus will determine if the player won, lost pushed or blackjacked
    int gameStatus = playTheGame(dealersHand,playersHand, ref deckOfCards);
    Console.SetCursorPosition(0,14);
    switch (gameStatus)
    {
        case 0: 
            winnings -= bet;
            Console.WriteLine($"You lost {bet}");
            break;
        case 1: 
            winnings += bet;
            Console.WriteLine($"You Won {bet}");
            break;
        case 2:
            Console.WriteLine($"You Won {1.5*bet}");
            winnings += 1.5*bet;
            break;
        case 3: 
            Console.WriteLine("You pushed");
            break;
        case 4:
            Console.WriteLine($"You Doubled Down and won {2*bet}");
            winnings += 2*bet;
            break;
        case 5: 
            Console.WriteLine($"You Doubled Down and Lost {2*bet}");
            winnings -= 2*bet;
            break;
    }
    Console.WriteLine("Press any key to play again or (Q) to quit");
    playAgain = Console.ReadKey(true).KeyChar;
    if(playAgain == 'q')
        playAgain = 'Q';
}
File.WriteAllText("playerInfo.txt",winnings.ToString());

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
    for (int i = size; i< num+size; i++)
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
double betScreen(ref double winnings)
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

    if (winnings <= 0)
    {
        winnings = 100;
        Console.WriteLine("As you lost everything, the bank mercifully gave you $100");
    }
    Console.Write($"How much of your ${winnings} do you want to bet? ");
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
        else if (wager < 0 || wager > winnings)
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
        altplayValue += play[i].altValue;
    }
    bool doubleDown = false;
    bool busted = false;
    bool dealBusted = false;
    int playReal = playValue>21? altplayValue:playValue;
    int dealReal = dealValue>21? altdealValue:dealValue;
    if (playReal > 21) 
    {
        busted = true;
    }
        
    if (play.Count() == 2 && playValue == 21)
    {   Console.WriteLine("BLACKJACK!");
        return 2;
    }
    if (deal.Count() == 2 && dealValue == 21)
    {
        deal[0].hidden = false;
        deal[0].printCard();
        deal[1].printCard();
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
            {
                busted = true;                
            }
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
                doubleDown = true;
                break;
            }
        }
    }
    // erase betting screen
    Console.SetCursorPosition(0,14);
    Console.WriteLine("                                       ");
    Console.WriteLine("                                       ");

    // dealer's turn
    deal[0].hidden = false;
    deal[0].printCard();
    deal[1].printCard();
    while(dealReal < 17)
    {
        deal = getCards(ref deck,deal,1,true);
        dealValue += deal[^1].value;
        altdealValue += deal[^1].altValue;
        dealReal = dealValue>21? altdealValue:dealValue;
        if (dealReal > 21)
        {
            dealBusted = true;
            break;
        }
    }

    Console.WriteLine($"{dealReal} for dealer and {playReal} for player");
    if((dealBusted == true && busted == true) || playReal == dealReal)
    {   
        return 3;
    }
    if (doubleDown)
    {
        if(busted || dealReal > playReal)
            return 5;
        else
            return 4;
    }
    else 
    {
        if(busted || dealReal > playReal)
            return 0;
        else
            return 1;
    }
    
}


