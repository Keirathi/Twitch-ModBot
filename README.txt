Installation instructions:

A) Upgrading from an older version of ModBot:
	1) Extract the rar file to the same folder as your older version.  When WinRAR asks you if you want to overwrite files, hit "Yes to all".
	2) Run ModBot.  Your database should still be intact.  Don't forget to re-enter all of your information on the startup GUI!

B)  New Users:
	1) Extract all of the contents of the rar file into any folder.
	2) [optional] If you use LoyaltyBot and want to transfer your tokens from LoyaltyBot to ModBot, run "OneTimeConverter.exe" first.  Check your LoyaltyBot/users/example.js file if you need to remember the information to put into the boxes.
	   If you're having trouble figuring out what goes in the boxes in the OneTimeConverter, I made this handy chart:  http://i.imgur.com/2CcKkJd.png  On the left is my LoyaltyBot's example.js file.  Copy those values into the appropriate boxes!
		***Note, if you skip this step and want to do it later, you will have to delete your ModBot.sqlite file for OneTimeConverter to do anything.  Therefore I highly recommend if you want to transfer that you do it before you actually start running ModBot in your channel.
	3) Create a new account on Twitch.tv for your bot if you don't have one already.
	4) Run ModBot.   Fill in the username and password for the bot account you created, the channel you want it to run in (generally your twitch name), a name for your currency, and select the amount you want the bot to payout and on what interval.  The Subscribers Spreadsheet box is optional.