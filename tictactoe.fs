: EMPTY [char] _ ;
: X [char] X ;
: O [char] O ;

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

: player_move { board_address }
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
;


variable board 9 cells allot
\ TODO: Loop
EMPTY board 0 cells + !
EMPTY board 1 cells + !
EMPTY board 2 cells + !
EMPTY board 3 cells + !
EMPTY board 4 cells + !
EMPTY board 5 cells + !
EMPTY board 6 cells + !
X board 7 cells + !
X board 8 cells + !

X board board player_move cells + !
board render

bye
