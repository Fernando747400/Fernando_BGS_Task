# Fernando_BGS_Task

[Download Game Here](https://fernando747.itch.io/ninja-ch)

## Building the Foundation
I started building the foundation of the entire game: a Scriptable Object architecture that enabled me to create items for each piece of the character's body. This also allowed me to seamlessly link prices, icons, special features/stats, and so on into a single file.

After that, I developed the method to link this Scriptable Object to my character, how to update it, and how to keep track of the purchased and equipped items.

## Art and Game Design
I then shifted focus to the art and game design in general. Since I couldn't create art quickly for the project, I designed the game around the aesthetic I could achieve with my available art packs and assets. This is how I ended up designing a combat/RPG game.

Since the main character had two knives, I envisioned it as a type of hunter. I then imported my medieval village and incorporated a fantasy enemy to fight.

This was an interesting experience since I designed the game around the art instead of the other way around.

I had to do a bit of clean-up for the animations and some assets.

## Combat Design and Testing
Once I had the art locked in place, I started designing and testing the combat style, determining which stats were important, and establishing the game's feel.

I debated between having an infinite number of rounds or a clear ending. I ended up opting for a mix of both. You can take as long as you want to achieve the goal, but there's a defined endpoint rather than just surviving forever.

## Building the Brain
After that, I started to build the brain of the machine. I wrote code to handle UI, player/enemy pseudo state machines, animator, and movement for both. I implemented stats for both of them in a way that allowed me to modify values on the fly without worrying about constant changes to the game/code.

## Testing and Finalization

Finally, I moved on to the win/lose conditions and conducted testing of the whole game. Like any other production, I tried to leave as few bugs as possible. The game isn't 100% perfect, but it didn't have any game-breaking bugs (that I encountered).

The game ended up shipping in a very solid state given the time frame and limitations of the project, most of which came from outside of my reach.

## Personal Assessment
In the personal assessment, I can say I didn't work within my comfort zone 100%. I tried two brand new systems (or systems I hadn't used in a long time). So, I made sure to work with something familiar that I could master (like architecture and Scriptable Objects) alongside something new or rusty, like the NavMesh/Cinemachine. It's been so long since we could make our own custom solutions for these kinds of problems.

I wish I had more time for audio. I made sure to find a pleasing song that could last at least a full playthrough. But I'm aware of how rich the world of audio is and how much it can bring to the table. I just had to cut it. (It made me very sad since I love integrating FMOD and getting hands-on with the music side of projects too, not only the code architecture.)