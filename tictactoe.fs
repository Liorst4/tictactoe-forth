: EMPTY [char] _ ;
: X [char] X ;
: O [char] O ;
: DRAW [char] D ;

variable board 9 cells allot
variable player

\ Draw the current state of the board on screen
: render ( -- )
	page
	9 0 do i
	       board i cells + @

	       dup EMPTY = if
		  i [char] 0 + emit
		  drop
	       else
		 emit
	       then

	       i 3 mod 2 = if cr then
	drop loop
;

\ Read a board position from a user
: board_position ( -- u )
	." Enter a number between 0 to 8:" cr
	begin
		key [char] 0 -
		dup 0 >=
		swap
		dup 9 <
		rot
		and

		dup false = if
			swap
			drop
		then

		false = while
	repeat
;

\ Advance the state of the game by a single turn
: turn ( -- )
	begin
		board_position
		dup
		board
		swap
		cells + @
		EMPTY =

		dup false = if
			swap
			drop
			." Already taken " cr
		then

		false = while
	repeat
	player @
	swap
	board
	swap
	cells
	+
	!
;

\ a b and c are equal?
: three_way_equals { a b c -- f }
		   a b =
		   b c =
		   and
;

\ Check if the value of the cells in the given indices is the same.
\ Return EMPTY if they aren't the same.
\ Return X or O if they are the same.
: line_winner { i0 i1 i2 -- c }
	      board i0 cells + @
	      dup
	      board i1 cells + @
	      board i2 cells + @
	      three_way_equals invert if
		drop
		EMPTY
		exit
	      then
;

\ Are there any empty cells in board?
: full ( -- f )
       9 0 do board i cells + @
	      EMPTY = if
		false
		unloop
		exit
	      then
	   loop
       true
;

\ EMPTY if the game is not over yet, DRAW if no one won,
\ X or O if someone won
: winner ( -- c )
	 \ TODO: loop
	0 1 2 line_winner dup EMPTY = if drop else exit then
	3 4 5 line_winner dup EMPTY = if drop else exit then
	6 7 8 line_winner dup EMPTY = if drop else exit then
	0 3 6 line_winner dup EMPTY = if drop else exit then
	1 4 7 line_winner dup EMPTY = if drop else exit then
	2 5 8 line_winner dup EMPTY = if drop else exit then
	0 4 8 line_winner dup EMPTY = if drop else exit then
	2 4 6 line_winner dup EMPTY = if drop else exit then
	full if DRAW exit then
	EMPTY
;

\ Switch the value of player between X and O
: switch_player ( -- ) player @ X = if O else X then player ! ;

\ Play a game of tictactoe
: game ( -- )
       X player !
       9 0 do EMPTY board i cells + !
	   loop

       begin
	 switch_player

	 render
	 ." Turn: " player @ emit cr

	 turn

	 winner
	 dup

	 EMPTY = if
	   drop
	   true
	 else
	   render
	   ." Game is over" cr
	   dup DRAW = if
	     drop
	     ." Draw" cr
	   else
	     ." The winner is " emit cr
	   then
	   false
	 then
	 while repeat
;


game
bye
