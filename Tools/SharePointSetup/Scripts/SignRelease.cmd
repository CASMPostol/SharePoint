#rem
Echo on
Echo %1
call "%VS110COMNTOOLS%VsDevCmd.bat"
Echo ON
signtool sign /a /n CAS %1
Echo "I am coping %1 => %2"
xcopy /y %1 %2

