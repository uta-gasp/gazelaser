@echo off
setlocal EnableDelayedExpansion

set source=%~n0
set source=%source:~0,-4%
set target=%~n0

del /F /Q "%target%.gif"

set max=40
for /L %%G in (0,1,%max%) do ( 
    set id=
    if %%G lss 10 (
        set id=00%%G
    ) else (
        if %%G lss 100 (
            set id=0%%G
        ) else (
            set id=%%G
        )
    )

    set getScale=calc 0.85 + 0.15 * cos(6.2831853 * %%G / (%max% + 1^)^)
    for /f "tokens=*" %%S IN ('!getScale!') DO set scale=%%S
    convert %source%.png -virtual-pixel transparent -distort SRT "50,50 !scale! 0" "animated\%target%_!id!.png"
)

convert -delay 4 -loop 0 -dispose Background animated\%target%*.png %target%.gif

del /F /Q "animated\%target%*.png"

rem set param=
rem for /L %%G in (2,2,10) do (
rem     set param=!param!50,50,%%G 
rem )

rem set param
rem @echo on
rem convert %base%.png -virtual-pixel transparent -distort SRT "%param%" %base%.gif
