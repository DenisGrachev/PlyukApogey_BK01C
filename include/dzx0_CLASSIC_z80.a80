; -----------------------------------------------------------------------------
; ZX0 8080 decoder by Ivan Gorodetsky - OLD FILE FORMAT v1 
; Based on ZX0 z80 decoder by Einar Saukas
; v1 (2021-02-15) - 103 bytes forward / 100 bytes backward
; v2 (2021-02-17) - 101 bytes forward / 100 bytes backward
; v3 (2021-02-22) - 99 bytes forward / 98 bytes backward
; v4 (2021-02-23) - 98 bytes forward / 97 bytes backward
; v5 (2021-08-16) - 94 bytes forward and backward (slightly faster)
; v6 (2021-08-17) - 92 bytes forward / 94 bytes backward (forward version slightly faster)
; v7 (2022-04-30) - 92 bytes forward / 94 bytes backward (source address now in DE, slightly faster)
; -----------------------------------------------------------------------------
; Parameters (forward):
;   DE: source address (compressed data)
;   BC: destination address (decompressing)
;
; Parameters (backward):
;   DE: last source address (compressed data)
;   BC: last destination address (decompressing)
; -----------------------------------------------------------------------------
; compress forward with <-c> option (<-classic> for salvador)
;
; compress backward with <-b -c> options (<-b -classic> for salvador)
;
; Compile with The Telemark Assembler (TASM) 3.2
; -----------------------------------------------------------------------------

;#define BACKWARD

	ifdef BACKWARD
	define NEXT_HL dec hl
	define NEXT_DE dec de
	define NEXT_BC dec bc
	else
	define NEXT_HL inc hl
	define NEXT_DE inc de
	define NEXT_BC inc bc
	endif

dzx0:
	ifdef BACKWARD
		ld hl,1
		push hl
		dec l
	else
		ld hl,0FFFFh
		push hl
		inc hl
	endif
		ld a,080h
dzx0_literals:
		call dzx0_elias
		call dzx0_ldir
		jp c, dzx0_new_offset
		call dzx0_elias
dzx0_copy:
		ex de,hl
		ex (sp),hl
		push hl
		add hl,bc
		ex de,hl
		call dzx0_ldir
		ex de,hl
		pop hl
		ex (sp),hl
		ex de,hl
		jp nc, dzx0_literals
dzx0_new_offset:
		call dzx0_elias
	ifdef BACKWARD
		inc sp
		inc sp
		dec h
		ret z
		dec l
		push af
		ld a,l
	else
		ld h,a
		pop af
		xor a
		sub l
		ret z
		push hl
	endif
		rra : ld h,a
		ld a, (de)
		rra : ld l,a
		NEXT_DE
	ifdef BACKWARD
		inc hl
	endif
		ex (sp),hl
		ld a,h
		ld hl,1
	ifdef BACKWARD
		call c, dzx0_elias_backtrack
	else
		call nc, dzx0_elias_backtrack
	endif
		inc hl
		jp dzx0_copy
dzx0_elias:
		inc l
dzx0_elias_loop:	
		add a, a
		jp nz, dzx0_elias_skip
		ld a, (de)
		NEXT_DE
		rla
dzx0_elias_skip:
	ifdef BACKWARD
		ret nc
	else
		ret c
	endif
dzx0_elias_backtrack:
		add hl,hl
		add a, a
		jp nc, dzx0_elias_loop
		jp dzx0_elias

dzx0_ldir:
		push af
dzx0_ldir1:
		ld a, (de)
		ld (bc),a
		NEXT_DE
		NEXT_BC
		dec hl
		ld a,h
		or l
		jp nz, dzx0_ldir1
		pop af
		add a, a
		ret
