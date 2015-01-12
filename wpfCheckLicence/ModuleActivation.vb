Module ModuleActivation
    '<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<The Key Gen>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    '<<                        By Jhnet Computers                            >>
    '<<--------------------------------<>------------------------------------>>
    '<< Handy Key generator for all your projects! Enjoy and use well!       >>
    '<< Will generate 25 digit serials, note that the dashes and the spaces  >>
    '<< are part of the key and must be included for the code to validate!   >>
    '<< There are about 3million, trillion possible keys for every program   >>
    '<< name. The program name can anything you want, it different program   >>
    '<< names create codes that only work with specific programs             >>
    '<< using a blank program name will make a code that is easily faked.    >>
    '<<                                                                      >>
    '<< Though I have only commented the 1st part of the code, the second    >>
    '<< is the mathematical reverse so it is "simple enough" to work out.    >>
    '<<                                                                      >>
    '<<   - genNumber(nameOfProgram)                                         >>
    '<<   - authKey(Key, nameOfProgram)                                      >>
    '<<                                                                      >>
    '<< Simple but effective key generation mechanism, not fool proof but    >>
    '<< by far enough for smaller projects!                                  >>
    '<<                                                                      >>
    '<<         Made by Jonathan Heathcote, 2007, Jhnet computers            >>
    '<<                   Jhnet.co.uk, mail@Jhnet.co.uk                      >>
    '<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


    Private Function getPlusMinus(chrr) As Boolean ' <<< This function retunrs either true or false
        chrr = UCase(chrr)                             '     depending on if a charachter is more than
        '     halfway through the alphabet or not...
        If Asc(chrr) - 65 < 12 Then
            getPlusMinus = True
        Else
            getPlusMinus = False
        End If
    End Function

    Public Function genNumber(appName)
        Dim appVal As Long
        Dim genVal As Long
        Dim tmpVar As String
        Dim i As Integer
        Dim seedMod As Integer

        For i = 1 To Len(appName) - 0
            appVal = appVal + Val(Asc(Mid$(appName, i, 1))) ' <<< Counts the value of each ascii chr
        Next                                                '     in the app name
        seedMod = Int((Now.Day() & Now.Month() & Now.Year() & Now.Hour & Now.Minute & Now.Second) ^ 0.2)

        For i = 0 To Int(seedMod + Now.Minute & Now.Second) ' <<< Vb's random num generator is not
            Rnd()                                                 '     very random so i will make it more
        Next                                                    '     random

        tmpVar = ""
        For i = 1 To 20                                   ' <<< Randomly create the 1st 4 parts of the code
            If Rnd < 0.5 Then                             ' <<< 1 in two chance of a letter or a number
                tmpVar = tmpVar & Chr(Int(Rnd * 25) + 65)
            Else
                tmpVar = tmpVar & Int(Rnd * 9)
            End If

            If Int(i / 5) = i / 5 And i <> 25 Then    ' <<< Add a ' - ' every 5 charachters
                tmpVar = tmpVar & " - "
            End If
        Next

        For i = 1 To Len(tmpVar) - 0                              ' <<< Creates a number based on the
            If i < Len(appName) Then                              '     first sections. Adds or takes
                If getPlusMinus(Mid(appName, i, 1)) = False Then  '     depending on various things
                    genVal = genVal + Val(Asc(Mid$(tmpVar, i, 1))) '    Makes it mathematicaly harder
                Else                                              '     to re-order the code.
                    genVal = genVal - Val(Asc(Mid$(tmpVar, i, 1)))
                End If
            Else
                If Int(i / 2) = i / 2 Then
                    genVal = genVal - Val(Asc(Mid$(tmpVar, i, 1)))
                Else
                    genVal = genVal + Val(Asc(Mid$(tmpVar, i, 1)))
                End If
            End If
        Next
        If genVal < 0 Then genVal = 0 - genVal ' <<< If the number is less than 0 then make it
        '     positive

        tmpVar = tmpVar & Mid((genVal * appVal) & "VARAA", 1, 5) ' <<< Last part of the code is the
        '     'value' of the first part of
        '     the code times the 'value'
        '     of the program name, limited
        '     to 5 charachters. "VARAA" is
        '     to make sure the result is
        '     atleast 5 chars.

        genNumber = UCase(tmpVar)    ' <<< Returns the new key
    End Function


    Public Function authKey(key, appName) As Boolean
        authKey = False
        On Error GoTo err

        Dim splt() As String
        Dim appVal As Long
        Dim genVal As Long
        Dim tempVar As String
        Dim i As Integer
        key = UCase(key)

        For i = 1 To Len(appName) - 0
            appVal = appVal + Val(Asc(Mid$(appName, i, 1)))
        Next

        splt = Split(key, " - ")
        splt(4) = ""

        tempVar = Join(splt, " - ")

        For i = 1 To Len(tempVar) - 0
            If i < Len(appName) Then
                If getPlusMinus(Mid(appName, i, 1)) = False Then
                    genVal = genVal + Val(Asc(Mid$(tempVar, i, 1)))
                Else
                    genVal = genVal - Val(Asc(Mid$(tempVar, i, 1)))
                End If
            Else
                If Int(i / 2) = i / 2 Then
                    genVal = genVal - Val(Asc(Mid$(tempVar, i, 1)))
                Else
                    genVal = genVal + Val(Asc(Mid$(tempVar, i, 1)))
                End If
            End If
        Next
        If genVal < 0 Then genVal = 0 - genVal

        splt = Split(key, " - ")

        If genVal = Val(splt(4)) / appVal Then
            authKey = True
        Else
            authKey = False
        End If


        Debug.Print(Mid((appVal * genVal) & "VARAA", 1, 5))
        Debug.Print(splt(4))

        If Mid((appVal * genVal) & "VARAA", 1, 5) = splt(4) Then
            authKey = True
        Else
            authKey = False
        End If

err:

    End Function

   
End Module
