# astroblox-rpg
a rpg battle system as a take-home assignment for astroblox.

## Development Process and Timeline
### Set Up Project: ~30 minutes
This involved setting up the project and contained basic tasks like setting up github, creating a new Unity project and importing a few assets like TMP, Naughty Attributes etc. It also included me thinking through how exactly i wanted to implement the game.

### Battle Screen: ~1:30 minutes
I wanted to ensure that the game was pretty configurable. So I created ScriptableObjects to contain different type of agents. A developer can easily create infinite different types of Agents and let the user choose between these when setting up a battle. Also tried to keep the design very simple and intuitive. 

### Actual Battle: ~1:45 hours
Majority of my remaining time was spent creating the logic for the rpg and putting enough feedback that an observer can fully know what's going on over the battle. This included setting the buff and debuff system, damage systems and healing systems that were controlled by individual agents. The BattleManager class watches the entire battle as it goes on and dictated the game time and other factors

### Polish
I spent a fair amount of time adding some polish to the game. This includes responsive bars, different feedback animations when receiving and dealing damage amongst others. I could have done without this and finished earlier but i like to add a bit of juice because all games deserve juice. I can't account for polish time individually cos it was included in every step of the way

### Documentation ~15 minutes
This includes time spent writing this Readme. I started writing a more extensive document but I realised I had started going a bit overtime so I simplified it significantly.