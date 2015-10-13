@echo off
setlocal EnableDelayedExpansion

set base=%~n0

del /F /Q "%base%.gif"

for /L %%G in (2,2,90) do ( 
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

    convert %base%.png -virtual-pixel transparent -distort SRT "50,50,%%G" "animated\%base%_!id!.png"
)

convert -delay 4 -loop 0 -dispose Background animated\%base%*.png %base%.gif

del /F /Q "animated\%base%*.png"

rem set param=
rem for /L %%G in (2,2,10) do (
rem     set param=!param!50,50,%%G 
rem )

rem set param
rem @echo on
rem convert %base%.png -virtual-pixel transparent -distort SRT "%param%" %base%.gif
