ModBot is designed to be a user-friendly bot for twitch.tv, based heavily on LoyaltyBot.  Any questions/comments/suggestions can be sent to me on twitch (user: Keirathi), or emailed to me @ twitch.tv.modbot@gmail.com

Be sure to always use the most up-to-date version, which can be downloaded at https://sourceforge.net/projects/twitchmodbot/.

Requires .NET Framework 4.  Most Windows users should have it already, but if not:  http://www.microsoft.com/en-us/download/details.aspx?id=17718



**************************INSTALLATION***********************************
A) Upgrading from an older version of ModBot:
	1) Extract the rar file to the same folder as your older version.  When WinRAR asks you if you want to overwrite files, hit "Yes to all".
	2) Run ModBot.  Your database should still be intact.  Don't forget to re-enter all of your information on the startup GUI!

B)  New Users:
    1) If you don't already have it, download WinRAR from http://www.rarlab.com/download.htm
	2) Double click on the modbot rar file, then choose to Extract all of the contents of the rar file into any folder.
	3 [optional] If you use LoyaltyBot and want to transfer your tokens from LoyaltyBot to ModBot, run "OneTimeConverter.exe" first.  Check your LoyaltyBot/users/example.js file if you need to remember the information to put into the boxes.
	   If you're having trouble figuring out what goes in the boxes in the OneTimeConverter, I made this handy chart:  http://i.imgur.com/2CcKkJd.png  On the left is my LoyaltyBot's example.js file.  Copy those values into the appropriate boxes!
		***Note, if you skip this step and want to do it later, you will have to delete your ModBot.sqlite file for OneTimeConverter to do anything.  Therefore I highly recommend if you want to transfer that you do it before you actually start running ModBot in your channel.
	4) Create a new account on Twitch.tv for your bot if you don't have one already.
	5) While still logged in twitch as your new bot account, go here: http://twitchapps.com/tmi/
	6) Hit the "Login to API & Generate Password" button. 
	7) You should see an "Authorize" button.  Make sure you are authorizing for your bot account, then hit Authorize.
	8) It will generate a string that looks something like: "oauth:1v2k3h019x0c1cn1nd01923b10dnalodnas9d1".  Copy it.  (The whole thing, including the oauth: part).  Ignore the stuff under it about /nick etc.
	9) Run ModBot.   Fill in your bots username, and then paste the oauth stuff as your password. For the channel, you generally want to put in your normal twitch name (don't add the http://twitch.tv/ part, just your name), then decide on a name for your currency, and select the amount you want the bot to payout and on what interval.  The Subscribers Spreadsheet box is optional.


**************************SUBSCRIBERS*************************************
ModBot features 2 ways to handle subscribers: a google docs spreadsheet, or adding someone as a sub via the database.  They do not conflict with each other, so if you happen to have someones name in both, they're not getting 4x points or anything.  Just pick whichever one is more convenient for you to use, or a combination.  Really doesn't matter!

	Google Docs:
		If you already use LoyaltyBot, you can use the exact same subscribers link.
		If you don't use LoyaltyBot and want to make a google docs spreadsheet to more easily manage your subscribers list:
			1) Create a new spreadsheet at http://docs.google.com/
			2) Set cell A1 as the header "Username"
			3) Subscriber Names will then be placed in Column A cells, starting with A2. (Fig. 1)
			4) Set the Subscriber list to Public, and change the type to json. (Fig. 2)
				*Note:  Even if you don't have a Sub button, you can still add people to your sub list so that they receive double currency income.
		Figure 1: [Column Setup] - http://i.imgur.com/eyQOwGz.jpg
		Figure 2: [Changing Doc Settings] - http://i.imgur.com/jDU9xOR.jpg

	Database:
		If you don't want to use a google spreadsheet for whatever reason, you can type !admin addsub <username> or !admin removesub <username> in the channel while the bot is running.



************************USER ACCESS LEVELS********************************

Something that you need to understand for this bot are the user levels.  
You, the broadcaster, are always Level 3.  You have access to every command.
Normal users in the channel are level 0.  They have access to !<currency>, !ticket, !bid, !bet, !btag, and any custom commands that only require Level 0 Access.
Level 1 users are moderators.  They have access to opening and closing raffles, betting pools, and auctions, as well as all the Normal User commands, and any custom commands that require Level 1 Access.
Level 2 users are Super mods.  They have access to adding/removing currency from users, as well as all lower level commands, and any custom commands that require Level 2 Access.

Please only promote people you trust.  It's much less work to do the work yourself than it is to fix a mess that someone else causes.Details on how to change someone's access level are detailed in the commands section below.


**************************COMMANDS*****************************************

Admin Only Commands:

	!admin payout <number> --- Changes the amount of currency paid out every interval.
	!admin interval <number> --- Changes the interval that currency is paid out at.  Accepted values: 1,2,3,4,5,6,10,12,15,20,30,60.
	   ****NOTE -- The above 2 settings are currently *NOT* saved between sessions.  When you restart the bot, payout and interval will reset back to what you select in the intial drop down boxes.
	!admin addmod <username> --- Changes the person to a Moderator (Access Level 1).
	!admin addsuper <username> --- Changes the person to a Super mod (Access Level 2).
	!admin demote <username> --- Moves the person down 1 Access Level.  Only works if the person is a Mod or Super Mod.
	!admin setlevel <username> <number> --- Sets the person to the specified Access Level.  Can be used instead of addmod, addsuper, or demote.  Cannot change someones access level to 3 (your level) or make it less than 0.
	!admin addsub <username> --- Add someone to the internal Sub List.  Doesn't cause conflicts with a spreadsheet Sub List.
	!admin removesub <username> --- Remove someone from the internal Sub List.
	*NEW* !admin offline [enabled|disabled] --- Allows you to enable the bot to handout currency even when the stream isn't online.  Disabled by default.

	*NEW* !top <number> --- Prints out a list of the top currency holders in your channel.  For instance, !top 5 would print the top 5 people.

	Greetings:
		
		!admin greeting set <Greeting Text> --- Sets your (optional) greeting message that will be sent everytime someone joins your channel.  Using '@user' in your greeting will put the person who joined's username in the greeting. Your custom greeting is always remembered across bot sessions.  If the first character of your greeting is a /, it will be stripped.
		!admin greeting on --- Turns on your greeting messages.  Must be manually turned on each time you start the bot.
		!admin greeting off --- Turns off your greeting messages.


Super Mod + Admin Commands:

	!<currency> add <amount> <username/all> --- Adds the specified amount of currency to the username.  If "all" is supplied as the last argument instead of a username, will give the coins to everyone currently in the channel.
	!<currency> remove <amount> <username/all>  --- Removes the specified amount of currency from the username.  If "all" is supplied as the last argument instead of a username, will remove the coins from everyone currently in the channel.
	!<currency> <username> --- Manually check the currency of a specific person.


Mod + Super Mod + Admin Commands:

	Custom Commands*:
		
		!mod addcommmand <AccessLevelRequired> <command> <output>  --- Access level must be between 0 and 3.  Command and output can be any text.  Quick example:  "!mod addcom 0 !ts Come hang out with on on teamspeak" would add a !ts command that anyone in the channel could use. If the first character of the <output> is a /, it will be stripped.
		!mod removecommand <command>  --- Deletes the command.  Currently if you want to edit a command, you must delete it and re-add it.
		!mod commandlist ---  Lists all of the custom commands currently available to the channel.

		*Note about custom commands:  the <command> parameter can be ANYTHING.  If you make a command "bacon", then it will trigger anytime bacon is the first word in a sentence.  It's probably a good practice to make all commands start with an "identifier" key, such as !.  The ! isn't forced.

	Raffle:
		
		!raffle open <Price> <MaxTickets> --- Opens a new raffle.  Price must be >= 0, and Max tickets must be > 0.
		!raffle close [optional <Amount>] --- Closes the raffle and draws the first winner.  If <Amount> is specified, then it will draw that many winners.  Otherwise it will draw 1 winner. A * behind a person's name in the winner's list denotes that they are following your channel.
		!raffle draw  [optional <Amount>] --- Draws another winner from a closed raffle.  If <Amount> is specified, then it will draw that many winners.  Otherwise it will draw 1 winner. A * behind a person's name in the winner's list denotes that they are following your channel.
		!raffle cancel --- Cancels the current raffle and refunds everyone's tickets.

	Auction:

		!auction open --- Opens a new auction.  Users can bid freely until you close the auction.  Current winner is shown in the channel each time there's a new High Bid, and every 30 seconds afterwards.
		!auction close --- Closes the auction and announces the final winner.
		!auction cancel --- Cancels the auction and refunds the highest bid.

	Gambling:

		!gamble open <MaxBet> <option1>, <option2>, <option3>, ... , <optionN> --- Opens a new betting pool.  MaxBet specifies the maximum amount of coins that a user can bet.  The options must have a space and a comma between them, and there isn't a limit to the amount of options you can have.
		!gamble close --- Locks the bets so that no more bets can be made.
		!gamble winner <optionX>  ---  Closes the bet pool, and pays out people who bet on the correct option.  <optionX> must be one of numbers associated with the original options when you started your pool.
		!gamble cancel --- Cancels the bet pool, and refunds all bets.


Normal User + Mod + Super Mod + Admin Commands:

	!<currency> --- Checks your current amount of currency on the channel.
	!btag/!battletag <YourBtag> --- Sets your battletag in the database.  If you win an auction or raffle, your battletag is shown in the winner output.

	When a raffle is open:

		!ticket <numberoftickets>  --- purchases the specified number of tickets.  Must <numberoftickets> must be a number >=0.  If you buy tickets and wish to get out of the raffle, use "!ticket 0" to have your coins refunded.
		!raffle help --- Refresher output for the currently open raffle.  Details the current ticket cost and max tickets, and explains how to purchase tickets.

	When an auction is open:

		!bid <amount>  --- Bids this amount on the current auction.  If your bid is not higher than the current highest bid, nothing happens.  Highest bid in a new auction is always 0.

	When a betting pool is open:

		!bet <amount> <optionnumber>  --- Places a bet on the option you select.  <optionnumber> is specified when the pool is opened, or you can type "!bet help" for a refresher.
		!bet help --- Resends the list of options for the current bet pool, and a quick refresher on how to bet.



*A quick note about Custom Commands:  
	If you try to add a custom command that matches any of the other built-in commands (!raffle, !bet, etc), the bot will tell you that it's added but you will have no way to ever use it.  So just pick a new command name!