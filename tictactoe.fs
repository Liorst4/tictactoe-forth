: EMPTY [char] _ ;
: X [char] X ;
: O [char] O ;
: DRAW [char] D ;

: render { board_address }
	9 0 do i
		board_address i cells + @ emit
		i 3 mod 2 = if cr then
	drop loop
;

: board_position ( -- number between 0 to 8 )
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

: turn { board_address player }
	begin
		board_position
		dup
		board_address
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
	player
	swap
	board_address
	swap
	cells
	+
	!
;

: three_way_equals { a b c }
		   a b =
		   b c =
		   and
;

: line_winner { board_address i0 i1 i2 }
	      board_address i0 cells + @
	      board_address i1 cells + @
	      board_address i2 cells + @
	      three_way_equals
	      if
		      board_address i0 cells + @
	      else
		      EMPTY
	      then
;

: full { board_address }
       9 0 do i
	      board_address i cells + @
	      EMPTY = if
		drop
		false
		unloop
		exit
	      then
	      drop
	   loop
       true
;

: winner { board_address }
	 \ TODO: loop
	board_address 0 1 2 line_winner dup EMPTY = if drop else exit then
	board_address 3 4 5 line_winner dup EMPTY = if drop else exit then
	board_address 6 7 8 line_winner dup EMPTY = if drop else exit then
	board_address 0 3 6 line_winner dup EMPTY = if drop else exit then
	board_address 1 4 7 line_winner dup EMPTY = if drop else exit then
	board_address 2 5 8 line_winner dup EMPTY = if drop else exit then
	board_address 0 4 8 line_winner dup EMPTY = if drop else exit then
	board_address 2 4 6 line_winner dup EMPTY = if drop else exit then
	board_address full if DRAW exit	then
	EMPTY
;

: game { board_address player_address -- board_address }
       begin
	 \ Switch player
	 player_address @ X = if O else X then
	 player_address !

	 board_address render
	 ." Turn: " player_address @ emit cr

	 board_address
	 player_address @
	 turn

	 board_address winner
	 dup

	 EMPTY = if
	   drop
	   true
	 else
	   board_address render
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


variable player
X player !

variable board 9 cells allot
\ TODO: Loop
EMPTY board 0 cells + !
EMPTY board 1 cells + !
EMPTY board 2 cells + !
EMPTY board 3 cells + !
EMPTY board 4 cells + !
EMPTY board 5 cells + !
EMPTY board 6 cells + !
EMPTY board 7 cells + !
EMPTY board 8 cells + !

board player game
bye
