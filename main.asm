;============================================================================
; PLYK
; Game by Denis Grachev
; Started: 05-08-2024
; last edit: 26-08-2024
;============================================================================

	;DEFINE MUTE_MUSIC
	;DEFINE MUTE_MUSIC3

START_LEVEL = 1

	device	zxspectrum48
	org	#0000
rkaBegin
	db	start/#100,start&#ff
	db	(end-1)/#100,(end-1)&#ff
binStart
	disp	#0000
;============================================================================
start:				
		;ld sp,57400
		; Инициализация видео
		call initVideo			
		ld a,64 : call delay
		call initIntro						

mainLoop:
		;Ждём луч
		call waitRaster					
		; устанавливаем параметры ВГ75 и ВТ57
		call applyGPU		

		ld a,(gameState)
		cp GS_GAMEPLAY : jp z,doGameplay
		cp GS_MENU : jp z,doMenu		
		cp GS_INIT_MAP : jp z,doInitMap
		cp GS_INTRO : jp z,doIntro
		cp GS_OUTRO : jp z,doOutro
		;cp GS_START_GAME : jp z,startGame
		cp GS_STORY : jp z,doStory
		cp GS_CREDITS : jp z,doCredits

;		cp GS_INIT_INTRO : jp z,initIntro	
		jp mainLoop

startStory:
	;call waitRaster
	;call initCredits
	call waitRaster
	call initStory	
	jp mainLoop

startGame:
	call waitRaster
	ld de,tileSet : call setTileSet
	ld a,GS_INIT_MAP : ld (gameState),a	
;	call initStory	
	jp mainLoop

;============================================================================		
	include "include\system.a80"
	include "include\sound.a80"	
	include "include\controls.a80"
	include "include\tilesSystem.a80"
	include "include\spritesSystem.a80"
	include "include\animationSystem.a80"
	include "include\map.a80"	
	include "include\hero.a80"	
	include "include\clones.a80"	
	include "include\keys.a80"	
	include "include\teleportDoors.a80"	
	include "include\camera.a80"	
	include "include\gui.a80"	
	include "music\tinyPsgPlayer.a80"
	include "include\gamestates.a80"	
	include "include\dzx0_CLASSIC_z80.a80"	
	include "include\intro.a80"
	include "include\story.a80"
	include "include\menu.a80"	
	include "include\outro.a80"
	include "include\credits.a80"	
	include "font\fontSystem.a80"	

	;block 256,0
	align 256
	include "music\playerBuffered.a80"
;	block 256,0

prepareVideoMem	
	ld de,VIDEOMEM_ADDR
	;16+96+96
	ld b,16+96+96
1:		
	;dup 96*2
	ld hl,baseLine	
	call copyLine	
	;edup
	dec b : jp nz,1b
	ret

copyLine:
	ld a,(hl)
	inc hl
	cp 255 : ret z
	ld (de),a
	inc de
	jp copyLine
	

baseLine:
	db 0,0,0,0,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,#f1	
	db 255
;z=#f1
;cюда рисуются спрайты по дефолту и тайлы
VIDEOMEM_TMP_BUFFER	
	;block 16*WIDTH,0	
VIDEOMEM_ADDR = VIDEOMEM_TMP_BUFFER	+ 16*WIDTH
;	dup 96
;	db 0,0,0,0,0,0,0,0,w,3,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,#f1
;    edup		
VIDEOMEM_ADDR_2=VIDEOMEM_ADDR+96*WIDTH
;	dup 96	
;	db 0,0,0,0,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,w,0,0,0,0,#f1	
;    edup	
end
	ent
binEnd
;заменяем русские буквы
	lua	pass1
	
	endlua

; считаем контрольную сумму	
cs	= 0
	lua	pass3
	mems=_c("binStart")
	meme=_c("binEnd")
	cs=0
	for i=mems,meme-2 do
	cs=(cs+sj.get_byte(i)*257)
	end
	cs=(cs-cs%256+(cs+sj.get_byte(meme-1))%256)%65536
	_pl("cs	= "..cs)
	endlua
	display cs
	db	0,0,#e6,cs/#100,cs&#ff
rkaEnd
	savebin "plyuk.rka",rkaBegin,rkaEnd-rkaBegin

	IF (_ERRORS = 0)
		SHELLEXEC "plyuk.rka"
	ENDIF
