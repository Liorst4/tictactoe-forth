char X constant X
char O constant O
char D constant DRAW
char _ constant EMPTY

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

\ ASCII character to number
: atou ( c -- u ) [char] 0 - ;

\ min <= number_to_test <= max
: in_range ( number_to_test min max -- f )
  1 +            ( number_to_test min max+1 )
  swap 1 - swap  ( number_to_test min-1 max+1 )
  rot rot over   ( max+1 number_to_test min-1 number_to_test )
  swap >         ( max+1 number_to_test f )
  rot rot swap < ( f f )
  and            ( f )
;

\ Read a board position from a user
: board_position ( -- u )
  begin
    key atou
    dup 0 8 in_range 0 = while
    drop
  repeat
;

\ Test if a given index is empty in board
: empty_cell? ( index -- f )
	      board swap cells + @
	      EMPTY =
;

\ Read a board position of an empty cell from a user
: empty_board_position ( -- u )
  begin
    board_position
    dup empty_cell? 0 = while
    drop
  repeat
;

\ Switch the value of player between X and O
: switch_player ( -- ) player @ X = if O else X then player ! ;

\ Advance the state of the game by a single turn
: turn ( -- )
  switch_player
  player @
  dup
  ." Current player: " emit cr ." Enter a number between 0 to 8:" cr
  board empty_board_position cells + !
;

\ a b and c are equal?
: three_way_equals ( a b c -- f )
  over swap
  =
  rot rot
  =
  and
;

: board@ ( n -- c )
  board swap cells + @
;

\ Check if the value of the cells in the given indices is the same.
\ Return EMPTY if they aren't the same.
\ Return X or O if they are the same.
: line_winner ( i0 i1 i2 -- c )
  board@             ( i0 i1 c2 )
  rot                ( i1 c2 i0 )
  board@             ( i1 c2 c0 )
  rot                ( c2 c0 i1 )
  board@             ( c2 c0 c1 )
  dup >r             ( c2 c0 c1 )
  three_way_equals   ( f )
  r> swap invert     ( c1 !f )
  if drop EMPTY then ( c1 )
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


create win_lines 0 c, 1 c, 2 c,
	         3 c, 4 c, 5 c,
	         6 c, 7 c, 8 c,
	         0 c, 3 c, 6 c,
	         1 c, 4 c, 7 c,
	         2 c, 5 c, 8 c,
	         0 c, 4 c, 8 c,
	         2 c, 4 c, 6 c,
here win_lines - 3 / constant amount_of_win_lines

: win_line ( index -- u u u )
  3 0 do
    dup 3 * i + win_lines + c@ swap
  loop
  drop
;

\ EMPTY if the game is not over yet, DRAW if no one won,
\ X or O if someone won
: winner ( -- c )
  amount_of_win_lines 0 do
    i win_line line_winner
    dup
    EMPTY = if
      drop
    else
      unloop
      exit
    then
  loop

  full if DRAW else EMPTY then
;

\ Clears the game state
: reset_game ( -- )
  9 0 do EMPTY board i cells + !
      loop
  X player !
;

\ Render game over screen
: game_over_screen ( game_result -- )
		   render
		   ." Game is over" cr
		   dup DRAW = if ." Draw " drop
			      else ." The winner is " emit
			      then cr
;

\ Play a game of tictactoe
: game ( -- )
  reset_game
  begin
    render
    turn
    winner
    dup
    EMPTY = while
    drop
  repeat
  game_over_screen
;

game
bye
