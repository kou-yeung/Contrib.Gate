@for /f "delims=" %%i in (%1) do @( call :_check %%i )
@set __x=
@exit /b 0

:_check
  @set __x=%*
  @if not "%__x:~0,1%" == "#" ( call :_set %* )
@exit /b 0

:_set
  @set %*
@exit /b 0