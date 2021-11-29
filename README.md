# Prospector
Gibson's Prospector Solitaire

This contains a blend of code from the 1st and 2nd edition of the book. 

The processes of making the cards was broken out in to separate functions - AddDecorators, AddPips, AddFace, etc. In the 1st edition the code contained all of these processes within the MakeCard function.

If you go through the second edtion of the text, the code will not match up, but the cards will all be made and the deck is the same at the end of the process.

Pyramid Solitaire

This game is playable, but not exact according to the variant that this project was based on: https://cardgames.io/pyramidsolitaire/
In that version, additional features were also implemented such as an undo function, the ability to pick a card under another card if they were a match, and flipping the draw pile when it had ran out. Currently, this game has the cards in the correct positions and layers, and is technically completable, except that the conditions for moving a card are still the same as Prospector Solitaire. What still needs to be done: the functions to select a card and hold it if it is not a King, then match it with the second card to see if it adds up to 13. 

Currently this Pyramid Solitaire resembles Golf Solitaire more in terms of mechanics due to their shared conditions of moving a card if it is adjacent to the current card rank, which can be used in the same code with some adjustments. Poker Solitaire and Clock Solitaire are entirely different in terms of gameplay mechanics, with Baker's Dozen being slightly similar with the connection between adjacent cards and covered piles, but with differences in structure and other mechanics such as automatic placement of Kings on the bottoms of piles and restricting discard piles to only accept cards in order of ascending rank.

A proposed example of how to implement this function is displayed below:
                // Check if the card selected is a King. If not, check if this is the second card selected.
                // If not, establish this as the first card. Establish the second card selected as c1.rank and compare with c0.rank to see if it adds to 13.
                // If not, return false. Second card becomes new first card. If second card selected is a King, the current code will not move the King to target.
                // https://github.com/mgslack/PyramidSolitaire/blob/master/MainWin.cs
                // https://docs.unity3d.com/ScriptReference/Selection-objects.html
                // https://docs.unity3d.com/540/Documentation/ScriptReference/UI.Selectable.OnSelect.html
                // Use new variable to keep track of if a previous card was selected. If not, add 1 onto it.
                // If variable is 1, add the two card values together and compare if it's 13 according to EqualsThirteen.
