Imports System.Web
Imports System.Net
Imports System.IO
Imports System.Text
Imports Newtonsoft.Json.Linq
Imports WanaKanaNet

Module Module1

    Dim Words(0) As Word
    Dim DebugMode As Boolean
    Dim LinkChoice As String
    Class Word
        Public Word As String = ""
        Public Furigana As String = ""
        Public Types() As String
        Public Meanings() As String = {""}
        Public Page As Integer
    End Class
    Sub ChooseLine(Filename, Message)
        Console.Clear()
        If Filename.contains(".txt") = False Then
            Filename &= ".txt"
        End If
        Dim AllFileText As String = ""
        Dim Lines() As String
        AllFileText = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\" & Filename)
        Lines = AllFileText.Split(vbCr)
        Try
            Console.WriteLine(Message)
            Console.WriteLine(AllFileText)
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            If DebugMode = True Then
                Console.WriteLine(ex.Message)
            Else
                Console.WriteLine("There isn't anything to show!")
                Console.ReadLine()
                Main()
            End If
            Console.ForegroundColor = ConsoleColor.White
        End Try
        Array.Resize(Lines, Lines.Length - 1)
        For Remove = 0 To Lines.Length - 1
            Lines(Remove) = Lines(Remove).Replace(vbLf, "")
        Next
        Dim LinesBackup() As String = Lines
        Dim Line As Integer = 0
        Dim Correct As Boolean
        Do Until Correct = True
            Console.SetCursorPosition(0, 0)
            If Line < 0 Then
                Line = 0
            ElseIf Line > Lines.Length - 1 Then
                Line = Lines.Length - 1
            End If
            If Lines.Length = 0 Then
                Console.WriteLine("There isn't anything to show!")
                Console.ReadLine()
                Main()
            End If
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine(Message)
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            For Printer = 0 To Lines.Length - 1
                If Printer = Line Then
                    Console.BackgroundColor = ConsoleColor.Gray
                    Console.ForegroundColor = ConsoleColor.Black
                    Console.WriteLine(Lines(Printer))
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    Console.WriteLine(Lines(Printer))
                End If
            Next
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("[Up/Down Arrow keys] Move up and down")
            Console.WriteLine("[Backspace] Delete item")
            Console.WriteLine("[Enter] Save")
            Console.WriteLine("[U] Undo deletes")
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White

            Dim KeyReader As ConsoleKeyInfo = Console.ReadKey
            If KeyReader.Key = ConsoleKey.DownArrow Then
                If Line < Lines.Length - 1 Then
                    Line += 1
                End If
            ElseIf KeyReader.Key = ConsoleKey.UpArrow Then
                If Line > 0 Then
                    Line -= 1
                End If
            ElseIf KeyReader.Key = ConsoleKey.Enter Or KeyReader.Key = ConsoleKey.Escape Then
                Correct = True
            ElseIf KeyReader.Key = ConsoleKey.Delete Then
                Console.WriteLine()
                Console.WriteLine("Are you sure you want to delete this?")
                Console.Write("Press ")
                Console.BackgroundColor = ConsoleColor.White
                Console.ForegroundColor = ConsoleColor.Black
                Console.Write("Delete")
                Console.BackgroundColor = ConsoleColor.Black
                Console.ForegroundColor = ConsoleColor.White
                Console.WriteLine(" again to confirm")

                KeyReader = Console.ReadKey
                If KeyReader.Key = ConsoleKey.Delete Then
                    Lines(Line) = ""
                    For CurrentLine = 1 To Lines.Length - 1
                        If Lines(CurrentLine - 1) = "" Then
                            Lines(CurrentLine - 1) = Lines(CurrentLine)
                            Lines(CurrentLine) = ""
                        End If
                    Next
                    Array.Resize(Lines, Lines.Length - 1)
                    Console.Clear()
                    System.IO.File.WriteAllText("C:\ProgramData\Japanese Conjugation Helper\" & Filename, "")
                    Using Writer As System.IO.TextWriter = System.IO.File.AppendText("C:\ProgramData\Japanese Conjugation Helper\" & Filename)
                        For ToWrite = 0 To Lines.Length - 1
                            Writer.WriteLine(Lines(ToWrite))
                        Next
                        Writer.Close()
                    End Using
                End If
            ElseIf KeyReader.Key = ConsoleKey.U Then
                Console.Clear()
                If LinesBackup(LinesBackup.Length - 1) = "" Then
                    Array.Resize(LinesBackup, LinesBackup.Length - 1)
                End If
                Lines = LinesBackup
                Line = 0
            ElseIf KeyReader.Key = ConsoleKey.Q Then
                Line = Lines.Length * (2 / 7)
            ElseIf KeyReader.Key = ConsoleKey.W Then
                Line = Lines.Length * (3 / 6)
            ElseIf KeyReader.Key = ConsoleKey.E Then
                Line = Lines.Length * (5 / 7)
            End If
        Loop
        Main()
    End Sub
    Sub Main()

        Randomize()
        Console.InputEncoding = System.Text.Encoding.Unicode
        Console.OutputEncoding = System.Text.Encoding.Unicode
        Console.ForegroundColor = ConsoleColor.White
        Const QUOTE = """"
        Console.Clear()
        'For the input of Japanese Chaaracters
        Console.Title() = "Japanese Conjugation Helper"

        Try
            Dim info = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time")
            Dim localServerTime As DateTimeOffset = DateTimeOffset.Now
            Dim localTime As DateTimeOffset = TimeZoneInfo.ConvertTime(localServerTime, info)
            Dim TimeInJapan As String = localTime.ToString
            TimeInJapan = Left(localTime.DateTime, (localTime.DateTime).ToString.IndexOf("M") + 1)
            If Int((100) * Rnd()) <> 2 Then
                Console.WriteLine("Japan: " & TimeInJapan)
            Else
                Console.WriteLine("日本: " & TimeInJapan)
            End If
        Catch
        End Try

        If Int((100) * Rnd()) <> 2 Then
            Console.WriteLine("Enter a command, or type " & QUOTE & "/h" & QUOTE & " for help")
            If DebugMode = True Then
                Console.WriteLine("(Debug mode is active)")
            End If
        Else
            Console.WriteLine("コマンドを入力したらどう？ OwO")
        End If

        'This is getting the word that is being searched ready for more accurate search with ActualSearchWord, ActualSearch Word (should) always be in japanese while Word won't be if the user inputs english or romaji:
        Dim Word As String = Console.ReadLine.ToLower.Trim 'This is the word that will be searched, this needs to be kept the same because it is the original search value that may be needed later
        If Word = "_debug_" Then
            DebugMode = True
            Main()
        End If

        If Word.Contains("/save") = True And Word.Length < 8 Then
            ChooseLine("WordSaves.txt", "Here are your saved words:")
            Console.ReadLine()
            Main()
        End If

        If Word = "" Or Word.IndexOf(vbCrLf) <> -1 Then
            Main()
        End If

        If Word = "/load" Then
            LoadWordJson()
            Main()
        End If

        If Left(Word, 2) = "//" Then
            WordJsonSearch(Word)
        End If

        Word = Word.Replace("！", "!").Replace("・", "/").Replace("＆", "&")

        If Word.IndexOf("/audio ") <> -1 Then
            VerbAudioGen(Right(Word, Word.Length - 7))
        ElseIf Word = "/audio" Then
            Console.WriteLine("What word would you like audio for?")
            Word = Console.ReadLine.ToLower.Trim
            VerbAudioGen(Word)
        End If

        If Left(Word, 9) = "/heylingo" Then
            If Word.Length > 12 Then
                HeyLingoDownload(Mid(Word, 11))
            Else
                HeyLingoDownload("japanese")
            End If
        End If

        If Word = "/sub" Or Left(Word, 9) = "/subtitle" Then
            DownloadSubtitles()
        End If

        If Word.IndexOf("/listening ") <> -1 Then
            ListeningPractice(Right(Word, Word.Length - 11))
        ElseIf Word = "/listening" Then
            Console.WriteLine("What word would you like listening practice on?")
            Word = Console.ReadLine.ToLower.Trim
            ListeningPractice(Word)
        End If

        If Word = "/ranki" Or Word = "/revanki" Or Word = "/reverseanki" Or Word = "/reversea" Then
            ReverseAnki()
        End If

        If Word = "/kt" Or Word = "/kanji" Or Word = "/kanjitest" Or Word = "/k" Or Left(Word, 2) = "/kt" Then
            KanjiTest()
        End If

        If Word = "/i" Or Word = "/info" Or Word = "/information" Then
            Console.WriteLine("Enter kanji string")
            Word = Console.ReadLine
            KanjiInfo(Word, 1)
        ElseIf Left(Word, 3) = "/i " Then

            Word = Mid(Word, 3)
            KanjiInfo(Word, 1)
        End If

        If Word = "/donate" Then
            Process.Start("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "www.buymeacoffee.com/hoptodot")
            Main()
        End If

        If Word = "/history" Then
            Console.Clear()
            Console.WriteLine("Search history:")
            Try
                Dim HistoryFile As String = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\SearchHistory.txt")
                Console.WriteLine(HistoryFile)
            Catch
                Console.WriteLine("You have no history.")
                Console.ReadLine()
                Main()
            End Try
            Console.WriteLine("do '/b' to go to your most recent search")
            Word = Console.ReadLine.ToLower.Trim

            If Word = "/b" Or Word = "/last" Or Word = "/back" Or Word = "/previous" Then
                Console.Clear()
                Try
                    Dim LastSearchFile As String = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\LastSearch.txt")
                    If LastSearchFile.Length < 3 Then
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("Error: FileWriter.Close")
                        Console.ForegroundColor = ConsoleColor.White
                    Else
                        Console.WriteLine(LastSearchFile)
                    End If
                Catch
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Error: FileWriter.Close")
                    Console.ForegroundColor = ConsoleColor.White
                End Try
                Console.ReadLine()
                Main()
            End If

            Main()
        End If

        If Word = "/b" Or Word = "/last" Or Word = "/back" Or Word = "/previous" Then
            Console.Clear()
            Try
                Dim LastSearchFile As String = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\LastSearch.txt")
                If LastSearchFile.Length < 7 Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Error: FileWriter.Close; Short")
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    Console.WriteLine(LastSearchFile)
                End If

            Catch
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Error: FileWriter.Close")
                Console.ForegroundColor = ConsoleColor.White
            End Try
            Console.ReadLine()
            Main()
        End If

        Dim Translated As String = ""
        If Left(Word, 1) = "!" Then
            Word = Mid(Word, 2)
            Console.Clear()
            Console.WriteLine("Translate:")

            Console.WriteLine(Word)
            Word = Word.Replace(QUOTE, "`")
            Word = Word.Replace("'", "`")
            Console.WriteLine()

            Translated = GTranslate(Word, "ja", "en")
            If Translated.ToLower = Word Then
                Translated = Translated.Replace("`", QUOTE)
                Translated = GTranslate(Word, "en", "ja")
                Console.WriteLine(Translated)

                Console.WriteLine()

                Translated = Translated.Replace("`", QUOTE)
                Translated = GTranslate(Translated, "ja", "en")
                Console.WriteLine(Translated)
            Else
                Translated = Translated.Replace("`", QUOTE)
                Console.WriteLine(Translated)
            End If

            Console.ReadLine()
            Main()
        End If

        If Left(Word, 4) = "/git" Or Word = "/project" Then
            Process.Start("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "github.com/hopto-dot/Japanese-Conjugation-Helper")
            Main()
        End If

        If Left(Word, 7) = "/review" Or Left(Word, 5) = "/rate" Or Left(Word, 9) = "/feedback" Then
            Process.Start("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "forms.gle/t1EYz8vWqg9VVrwL6")
            Main()
        End If

        If Left(Word, 5) = "/file" Then
            Try
                Process.Start("C:\ProgramData\Japanese Conjugation Helper")
            Catch Ex As Exception
                Console.Clear()
                Console.ForegroundColor = ConsoleColor.Red
                If DebugMode = True Then
                    Console.WriteLine(Ex.Message)
                Else
                    Console.WriteLine("No program files have been created yet.")
                    Console.WriteLine("Type '/prefs' to get started.")
                    Console.ForegroundColor = ConsoleColor.White
                End If
                If Console.ReadLine.ToLower = "/prefs" Then
                    Preferences()
                End If
            End Try
            Main()
        End If

        If Left(Word, 5) = "/pref" Then
            Preferences()
        End If

        If Word = "/" Then
            Main()
        End If

        If Left(Word, 3) = "/r " Then
            ReadingPractice(Right(Word, Word.Length - 3))
        End If
        If Left(Word, 2) = "/r" Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Wrong format.")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End If

        If Left(Word, 3) = "/p " Then
            ConjugationPractice(Right(Word, Word.Length - 3))
        End If

        If Word = "/h" Or Word = "/help" Then
            Console.Clear()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Find more info in the wiki (on GitHub).")
            Console.WriteLine("If you want to give feedback, request a feature or report bugs do '/feedback' or '/rate'")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine()
            Console.WriteLine("List of commands (note: commands are case sensitive):")

            Console.WriteLine()
            Console.WriteLine("type " & QUOTE & "!" & QUOTE & " and then a sentence, the program will translate it. (Works for English or Japanese)")
            Console.WriteLine()
            Console.WriteLine("/h: brings up this help menu, if you want more help with a command then add the [command] parameter:")
            Console.WriteLine("Syntax: /h [command]")

            Console.WriteLine()
            Console.WriteLine("WordLookup: To lookup a word and bring up information and conjugation patterns, simply type and English or Japanese word, Japanese words can also be written using romaji (english transliteration), surround words in quotes to make sure the program knows that it is definitely the english word you are seaching for and not romaji. Example: " & QUOTE & "hate" & QUOTE)
            Console.WriteLine("Syntax: [english/japanese/romaji word]")

            Console.WriteLine("KanjiInfo: Bring up lots of useful information about all the kanji you input")
            Console.WriteLine("Type /i, then enter in as many kanji as you want and press enter")
            Console.WriteLine()

            Console.WriteLine()
            Console.WriteLine("KanjiTest: Enter Japanese text and then generate a quiz of the kanji in that text")
            Console.WriteLine("Syntax: /k")

            Console.WriteLine()
            Console.WriteLine("Translate: Translate Japanese into English or the other way round")
            Console.WriteLine("Syntax: ![text]")

            Console.WriteLine("History: See a small amount of information on your recent searches.")
            Console.WriteLine("A maximum of 20 recent results will show up.")
            Console.WriteLine("Syntax: /history")
            Console.WriteLine()

            Console.WriteLine()
            Console.WriteLine("/r: reading practice, to use this command (sentences) must be in a specific format")
            Console.WriteLine("Syntax: /r (sentences) [2]")
            Console.WriteLine("do " & QUOTE & "/help /r" & QUOTE & " to see more about this command")

            Console.WriteLine()
            Console.WriteLine("/p: start a small quiz that helps you conjugate verbs, this only works with verbs but will later work with adjectives and nouns, the (word) parameter is the word that you want help conjugating")
            Console.WriteLine("Syntax: /p [english/japanese/romaji word]")

            Console.WriteLine()
            Console.WriteLine("/prefs: Changes program preferences")

            Console.WriteLine()
            Console.WriteLine("/git: Brings the program repository page on github using Chrome")

            Console.WriteLine()
            Console.WriteLine("/files: Opens the folder the program uses to safe preferences")
            Console.WriteLine("Note: Only works once you have used the '/prefs' command")
            Console.ReadLine()
            Main()
        End If

        If Left(Word, 3) = "/h " Then
            Help(Right(Word, Word.Length - 3))
        End If
        If Left(Word, 6) = "/help " Then
            Help(Right(Word, Word.Length - 6))
        End If

        If Left(Word, 1) = "/" Then
            Main()
        End If

        'Comment Syntax:
        '_ = space
        '# = number
        '< start
        '> = end

        If Word.Length > 4 And Word.IndexOf("&&") <> -1 Then
            SearchMultiple(Word)
        End If

        If Word.Length > 2 Then
            If Mid(Word, Word.Length - 2, 1) = " " And IsNumeric(Right(Word, 2)) = True Then 'If search is '_#'>
                Console.WriteLine("Searching for " & Right(Word, 2) & " definitions of '" & Left(Word, Word.Length - 3) & "'...")
                WordConjugate(Left(Word, Word.Length - 2), Right(Word, 2))
            ElseIf Mid(Word, Word.Length - 1, 1) = " " And IsNumeric(Right(Word, 1)) = True And Right(Word, 1) <> "1" Then 'If search isn't '_1'>
                Console.WriteLine("Searching for " & Right(Word, 1) & " definitions of '" & Left(Word, Word.Length - 2) & "'...")
                WordConjugate(Left(Word, Word.Length - 2), Right(Word, 1))
            ElseIf Mid(Word, Word.Length - 1, 1) = " " And IsNumeric(Right(Word, 1)) = True And Right(Word, 1) = "1" Then 'If the search is '_1'>
                Console.WriteLine("Searching for '" & Word & "'...")
                WordConjugate(Left(Word, Word.Length - 2), Right(Word, 1))
            Else 'If the search has no number parameter (but may have an s paramter)
                Console.WriteLine("Searching for '" & Word & "'...")
                WordConjugate(Word, 1)
            End If
        Else 'If the search has no number parameter (but may have an s paramter) and search length is less than 3
            Console.WriteLine("Searching for " & Word & "...")
            WordConjugate(Word, 1)
        End If

        Main()
    End Sub
    Sub DownloadSubtitles()
        Console.Clear()
        Dim Anime As String
        Dim Language As String

        Console.WriteLine("What anime would you like subtitles for? (Sometimes the anime will be in romaji sometimes in english)")
        Anime = Console.ReadLine.ToLower.Replace("-", " ").Replace("  ", " ")

        Console.WriteLine("What language would you like subtitles for?")
        Language = Console.ReadLine.ToLower

        If Language.Contains("en") = True Or Language.Contains("glish") = True Then
            Language = "English"
        ElseIf Language.Contains("j") = True Then
            Language = "Japanese"
        Else
            Language = "Japanese"
        End If

        Dim Client As New WebClient
        Dim HTML As String
        If Language = "Japanese" Then
            Try
                HTML = Client.DownloadString(New Uri("https://kitsunekko.net/dirlist.php?dir=subtitles%2Fjapanese%2F"))
            Catch Ex As Exception
                If DebugMode = True Then
                    Console.WriteLine(Ex.Message)
                    Console.ReadLine()
                End If
            End Try
        ElseIf Language = "English" Then
            Try
                HTML = Client.DownloadString(New Uri("https://kitsunekko.net/dirlist.php?dir=subtitles%2F"))
            Catch ex As Exception
                If DebugMode = True Then
                    Console.WriteLine(ex.Message)
                    Console.ReadLine()
                End If
            End Try
        End If

        Dim Snip1, Snip2 As Integer
        Snip1 = HTML.IndexOf("<tbody>") + 5
        HTML = Mid(HTML, Snip1)
        Snip2 = HTML.IndexOf("</tbody>")
        HTML = Left(HTML, Snip2)

        Dim Animes(), Found(0), Temp As String
        Animes = Split(HTML, "</tr>")

        For Search = 0 To Animes.Length - 1
            Temp = Animes(Search).ToLower.Replace("-", " ").Replace("+", " ")
            Temp = Temp.Replace("  ", " ")
            If Temp.IndexOf(Anime.ToLower) <> -1 Then
                Found(Found.Length - 1) = Animes(Search)
                Array.Resize(Found, Found.Length + 1)
            End If
        Next
        Array.Resize(Found, Found.Length - 1)

        If Found.Length = 0 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Anime wasn't found")
            Console.WriteLine("Maybe try searching for the english title, or romaji title")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        ElseIf Found.Length > 30 Then
            Array.Resize(Found, 30)
        End If

        Dim AnimeNames(Found.Length - 1) As String
        For N = 0 To AnimeNames.Length - 1
            AnimeNames(N) = Found(N)

            Snip1 = AnimeNames(N).IndexOf("<strong>") + 9
            AnimeNames(N) = Mid(AnimeNames(N), Snip1)
            Snip2 = AnimeNames(N).IndexOf("</strong>")
            AnimeNames(N) = Left(AnimeNames(N), Snip2)
        Next
        Console.Clear()
        Dim Correct As Boolean = False
        Dim Line As Integer = 0
        Dim Selected() As Boolean
        Do Until Correct = True
            Console.SetCursorPosition(0, 0)
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Select Anime to download subtitles for:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            For Printer = 0 To AnimeNames.Length - 1
                If Printer = Line Then
                    Console.BackgroundColor = ConsoleColor.Gray
                    Console.ForegroundColor = ConsoleColor.Black
                    Console.WriteLine(AnimeNames(Printer))
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    Console.WriteLine(AnimeNames(Printer))
                End If
            Next
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("[Up/Down Arrow keys] Move up and down")
            Console.WriteLine("[Enter] Select anime")
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White


            Dim KeyReader As ConsoleKeyInfo = Console.ReadKey
            If KeyReader.Key = ConsoleKey.DownArrow Then
                If Line < AnimeNames.Length - 1 Then
                    Line += 1
                End If
            ElseIf KeyReader.Key = ConsoleKey.UpArrow Then
                If Line > 0 Then
                    Line -= 1
                End If
            ElseIf KeyReader.Key = ConsoleKey.Escape Then
                Main()
            ElseIf KeyReader.Key = ConsoleKey.Enter Then
                Correct = True
            ElseIf KeyReader.Key = ConsoleKey.Q Then
                Line = AnimeNames.Length * (2 / 7)
            ElseIf KeyReader.Key = ConsoleKey.W Then
                Line = AnimeNames.Length * (3 / 6)
            ElseIf KeyReader.Key = ConsoleKey.E Then
                Line = AnimeNames.Length * (5 / 7)
            End If
        Loop

        Snip1 = Found(Line).IndexOf("<a href=""") + 10
        Found(Line) = Mid(Found(Line), Snip1)
        Snip2 = Found(Line).IndexOf("""")
        Found(Line) = Left(Found(Line), Snip2)

        HTML = Client.DownloadString(New Uri("https://kitsunekko.net" & Found(Line)))

        Found = Split(HTML, "</tr>")
        Array.Resize(Found, Found.Length - 1)
        ReDim AnimeNames(Found.Length - 1)
        For N = 0 To Found.Length - 1
            AnimeNames(N) = Found(N)

            Snip1 = AnimeNames(N).IndexOf("<strong>") + 9
            AnimeNames(N) = Mid(AnimeNames(N), Snip1)
            Snip2 = AnimeNames(N).IndexOf("</strong>")
            AnimeNames(N) = Left(AnimeNames(N), Snip2)
        Next

        Correct = False
        Array.Resize(Selected, Found.Length)
        Console.Clear()
        Line = 0
        Do Until Correct = True
            Console.SetCursorPosition(0, 0)
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Select Episodes to download using enter:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            For Printer = 0 To AnimeNames.Length - 1
                If Printer = Line And Selected(Printer) = True Then
                    Console.BackgroundColor = ConsoleColor.White
                    Console.ForegroundColor = ConsoleColor.Black
                    Console.WriteLine(AnimeNames(Printer))
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = ConsoleColor.White
                ElseIf Printer = Line Or Selected(Printer) = True Then
                    Console.BackgroundColor = ConsoleColor.Gray
                    Console.ForegroundColor = ConsoleColor.Black
                    Console.WriteLine(AnimeNames(Printer))
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    Console.WriteLine(AnimeNames(Printer))
                End If
            Next
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("[D] Start downloads")
            Console.WriteLine("[Enter] Select")
            Console.WriteLine("[Backspace] Deselect")
            Console.WriteLine("[Spacebar] Select all")
            Console.WriteLine("[Tab] Deselect all")
            Console.WriteLine("[Esc] Go back to main")
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White

            Dim KeyReader As ConsoleKeyInfo = Console.ReadKey
            If KeyReader.Key = ConsoleKey.DownArrow Then
                If Line < AnimeNames.Length - 1 Then
                    Line += 1
                End If
            ElseIf KeyReader.Key = ConsoleKey.UpArrow Then
                If Line > 0 Then
                    Line -= 1
                End If
            ElseIf KeyReader.Key = ConsoleKey.Escape Then
                Main()
            ElseIf KeyReader.Key = ConsoleKey.Enter Then
                If Selected(Line) = True Then
                    Selected(Line) = False
                Else
                    Selected(Line) = True
                End If
            ElseIf KeyReader.Key = ConsoleKey.Delete Or KeyReader.Key = ConsoleKey.Backspace Then
                Selected(Line) = False
            ElseIf KeyReader.Key = ConsoleKey.Tab Then
                For Delete = 0 To Selected.Length - 1
                    Selected(Delete) = False
                Next
            ElseIf KeyReader.Key = ConsoleKey.Spacebar Then
                For All = 0 To Selected.Length - 1
                    Selected(All) = True
                Next
            ElseIf KeyReader.Key = ConsoleKey.D Then
                Correct = True
            ElseIf KeyReader.Key = ConsoleKey.Home Then
                Line = 0
            ElseIf KeyReader.Key = ConsoleKey.End Then
                Line = AnimeNames.Length - 1
            ElseIf KeyReader.Key = ConsoleKey.Q Then
                Line = AnimeNames.Length * (2 / 7)
            ElseIf KeyReader.Key = ConsoleKey.W Then
                Line = AnimeNames.Length * (3 / 6)
            ElseIf KeyReader.Key = ConsoleKey.E Then
                Line = AnimeNames.Length * (5 / 7)
            End If
        Loop

        If Line > AnimeNames.Length - 1 Then
            Line += AnimeNames.Length - 1
        ElseIf Line < 0 Then
            Line = 0
        End If

        Correct = False
        Console.Clear()
        Console.WriteLine("Downloading:")
        My.Computer.FileSystem.CreateDirectory(Environ$("USERPROFILE") & "\Downloads\Subtitles\" & Language)
        Correct = False
        Dim S As Integer = 0
        Do Until Correct = True
            If Selected(S) = True Then
                Snip1 = Found(S).IndexOf("<a href=""") + 10
                Found(S) = Mid(Found(S), Snip1)
                Snip2 = Found(S).IndexOf("""")
                Found(S) = Left(Found(S), Snip2)

                Try
                    Client.DownloadFile("https://kitsunekko.net/" & Found(S), Environ$("USERPROFILE") & "\Downloads\Subtitles\" & Language & "\" & Found(S).Replace("subtitles", "").Replace("/japanese/", "").Replace("/", "").Replace("/", ""))
                    Console.ForegroundColor = ConsoleColor.Green
                    Console.WriteLine(Found(S))
                    Console.ForegroundColor = ConsoleColor.White
                Catch ex As Exception
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Failed: " & Found(S))
                    Console.ForegroundColor = ConsoleColor.White
                End Try

            End If
            S += 1
            If S >= Selected.Length Then
                Correct = True
            End If
        Loop

        Console.WriteLine("Would you like to be taken to the downloads?")
        If Console.ReadLine.ToLower.IndexOf("y") <> -1 Then
            Process.Start("explorer.exe", Environ$("USERPROFILE") & "\Downloads\Subtitles\" & Language)
        End If
        Main()
    End Sub
    Sub ListeningPractice(Word)
        Const QUOTE = """"
        Dim Gender As String = "male"
        If Word.indexOf("!f") <> -1 Then
            Word = Word.replace("!f", "")
            Gender = "female"
        ElseIf Word.indexOf("!m") <> -1 Then
            Word = Word.replace("!m", "")
            Gender = "male"
        End If
        Word = Word.Replace(" ", "")

        Console.Clear()
        Console.WriteLine("Searching " & QUOTE & Word & QUOTE)
        Dim WordURL As String
        Dim Client As New WebClient
        Dim HTML As String = ""
        Client.Encoding = System.Text.Encoding.UTF8
        Try
            WordURL = ("https: //jisho.org/search/" & Word)
            HTML = Client.DownloadString(New Uri(WordURL))
        Catch
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Search failed")
            Console.WriteLine("Check you are connected to the internet")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try
        Dim HTMLTemp As String = HTML

        Dim ActualSearchWord As String = ""
        Dim ActualSearch1stAppearance As Integer = 0
        Dim ActualSearch2ndAppearance As Integer = 0
        Dim WordLink As String = ""
        Dim Max As Integer = 3
        Dim Types As String = ""

        Console.WriteLine("Trying to get information for " & QUOTE & Word & QUOTE)
        Try
            ActualSearchWord = RetrieveClassRange(HTMLTemp, "<span class=" & QUOTE & "text" & QUOTE & ">", "</div>", "Actual word search")
            ActualSearchWord = Mid(ActualSearchWord, 30)
            ActualSearchWord = ActualSearchWord.Replace("<span>", "")
            ActualSearchWord = ActualSearchWord.Replace("</span>", "")
            ActualSearchWord = ActualSearchWord.Trim

            'Getting the link of the actual word:
            ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & QUOTE & "text" & QUOTE & ">")
            HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
            ActualSearch1stAppearance = HTMLTemp.IndexOf("meanings-wrapper") 'used to be "concept_light clearfix"
            HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
            ActualSearch1stAppearance = HTMLTemp.IndexOf("jisho.org/word/")
            ActualSearch2ndAppearance = Mid(HTMLTemp, HTMLTemp.IndexOf("jisho.org/word/")).IndexOf(QUOTE & ">")
            WordLink = Mid(HTMLTemp, ActualSearch1stAppearance + 1, ActualSearch2ndAppearance - 1)
        Catch
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Word not found!")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try

        If ActualSearchWord.IndexOf("<") <> -1 Or ActualSearchWord.IndexOf(">") <> -1 Or ActualSearchWord.IndexOf("span") <> -1 Then
            ActualSearchWord = RetrieveClassRange(HTML, "<div class=" & QUOTE & "concept_light clearfix" & QUOTE & ">", "</div>", "Actual word search")
            ActualSearchWord = RetrieveClassRange(ActualSearchWord, "text", "</span", "Actual word search")
            ActualSearchWord = Mid(ActualSearchWord, 17)
            ActualSearchWord = ActualSearchWord.Replace("<span>", "")
            ActualSearchWord = ActualSearchWord.Replace("</span>", "")
        End If

        If ActualSearchWord.Length = 0 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Word not found!")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End If

        Types = TypeScraper("jisho.org/search/" & ActualSearchWord).replace("&#39;", "").tolower

        If Types.IndexOf("verb") = -1 Or Types.IndexOf("suru") <> -1 Or Types.IndexOf("noun") <> -1 Then
            If Types.IndexOf("i-adjective") = -1 Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("You can only download audio files for verbs.")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()
                Main()
            End If
        End If
        Dim Verb, IAdj As Boolean
        If Types.IndexOf("verb") <> -1 Then
            Verb = True
        ElseIf Types.IndexOf("i-adj") <> -1 Then
            IAdj = True
        End If

        Dim Furigana As String = ""
        Dim FuriganaStart As Integer
        If Verb = True Then
            Try
                Furigana = RetrieveClassRange(HTML, "</a></li><li><a", "</a></li><li><a href=" & QUOTE & "//jisho.org", "Furigana")
                If Furigana.Length <> 0 Then
                    FuriganaStart = Furigana.IndexOf("search for")
                    Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 11)
                    FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
                    If FuriganaStart <> -1 Then 'if </a></li><li> Is found Then it will be removed as well as everything after it
                        Furigana = Left(Furigana, FuriganaStart)
                    End If
                Else
                    Furigana = ""
                End If

                If Furigana = ActualSearchWord Or Furigana = "" Then 'This will repeat the last attempt to get the furigana, because the last furigana failed and got 'Sentences for [word using kanji]' instead of 'Sentences for [word using kana]'
                    Furigana = RetrieveClassRange(HTML, "</a></li><li><a", "</a></li><li><a href=" & QUOTE & "//jisho.org", "Furigana")
                    If Furigana.Length > 30 And Furigana.Length <> 0 Then
                        FuriganaStart = Furigana.IndexOf("search for")
                        Furigana = Mid(Furigana, FuriganaStart + 5)

                        FuriganaStart = Furigana.IndexOf("search for")
                        Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 11)
                        FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
                        Furigana = Left(Furigana, FuriganaStart)
                    Else
                        If FuriganaStart <> -1 Or Furigana.Length > 20 Then
                            Furigana = ""
                        End If
                    End If
                End If

                If Furigana = ActualSearchWord Or Furigana = "" Or Furigana.Length > 20 Then 'Another try
                    Furigana = RetrieveClassRange(HTML, "kanji-3-up kanji", "</span><span></span>", "Furigana")
                    If Furigana.Length < 30 And Furigana.Length <> 0 Then
                        Furigana = Mid(Furigana, Furigana.LastIndexOf(">") + 2)
                    Else
                        Furigana = ""
                    End If

                End If

                If Furigana.Length < 1 Or IsNothing(Furigana) Then
                    Furigana = RetrieveClassRange(HTML, "audio id=" & QUOTE & "audio_" & ActualSearchWord, "<source src=", "Furigana2")
                    Furigana = Furigana.Replace("<", "")
                    Furigana = Furigana.Replace("audio id=" & QUOTE & "audio_" & ActualSearchWord & ":", "")
                    Furigana = Furigana.Replace(QUOTE & ">", "")
                End If
            Catch
            End Try
        End If


        Console.WriteLine("Searching for " & QUOTE & Gender & "_" & ActualSearchWord & QUOTE & " on OJAD")
        Dim Snip1, Snip2 As Integer
        Dim WordID As String = ""
        WordURL = "http://www.gavo.t.u-tokyo.ac.jp/ojad/search/index/sortprefix:difficulty/narabi1:difficulty_asc/narabi2:mola_asc/narabi3:proc_asc/yure:visible/curve:fujisaki/details:visible/limit:100/word:" & ActualSearchWord
        Try
            HTML = Client.DownloadString(New Uri(WordURL))
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(QUOTE & ActualSearchWord & QUOTE & " wasn't found on OJAD")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try

        Try
            WordID = HTML
            Snip1 = WordID.IndexOf("<tr id=" & QUOTE & "word_")
            If Snip1 = -1 Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(QUOTE & ActualSearchWord & QUOTE & " wasn't found on OJAD")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()
                Main()
            End If
            Snip1 += 14
            WordID = Mid(WordID, Snip1)
            Snip2 = WordID.IndexOf(QUOTE & ">")
            WordID = Left(WordID, Snip2)
        Catch
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(QUOTE & ActualSearchWord & QUOTE & " wasn't found on OJAD")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try

        Console.WriteLine("Getting word ID")
        HTML = Client.DownloadString(New Uri(WordURL))
        Dim AudioBase As String = "http://www.gavo.t.u-tokyo.ac.jp/ojad/app/webroot/sound4/wav/male/"
        Dim Key As String = ""
        Console.Clear()
        For ID = 0 To 36
            Console.SetCursorPosition(0, 0)
            Console.WriteLine("Getting audio ID")
            Console.WriteLine("Testing ID " & ID & "/36")
            Key = ID
            If Key.Length = 1 Then
                Key = "00" & Key
            ElseIf Key.Length = 2 Then
                Key = "0" & Key
            End If
            Try
                WordURL = AudioBase & Key & "/" & WordID & "_1_1_male.wav"
                HTML = Client.DownloadString(New Uri(WordURL))
            Catch
                WordURL = ""
            End Try

            If HTML.IndexOf("<!DOCTYPE html>") <> -1 Then
                Continue For
            End If
            If Word <> "" Then
                ID = 999
                Continue For
            End If
        Next

        If Key > 35 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("It seems that you didn't enter a verb. Only verbs work, sorry!")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End If

        Console.Clear()
        Console.WriteLine("Downloading audio:")
        AudioBase &= Key & "/" & WordID
        Try
            System.IO.Directory.Delete("C:\ProgramData\Japanese Conjugation Helper\Audio", True)
        Catch
        End Try
        My.Computer.FileSystem.CreateDirectory("C:\ProgramData\Japanese Conjugation Helper\Audio")
        Dim Conjugation As String = ""
        Dim Tries As Integer = 0
        Dim MaxAudio As Integer = 12
        If IAdj = True Then
            MaxAudio = 9
        End If
        For AudioID = 1 To MaxAudio
            If IAdj = True Then
                If AudioID = 1 Then
                    WordURL = AudioBase & "_" & AudioID & "_1_male.wav"
                Else
                    WordURL = AudioBase & "_" & AudioID + 1 & "_1_male.wav"
                End If
            Else
                WordURL = AudioBase & "_" & AudioID & "_1_male.wav"
            End If

            If Gender = "female" Then
                WordURL = WordURL.Replace("male", "female")
            End If
            If Verb = True Then
                Select Case AudioID
                    Case 1
                        Conjugation = "dictionary"
                    Case 2
                        Conjugation = "masu"
                    Case 3
                        Conjugation = "te"
                    Case 4
                        Conjugation = "past"
                    Case 5
                        Conjugation = "negative"
                    Case 6
                        Conjugation = "negative_past"
                    Case 7
                        Conjugation = "ba_conditional"
                    Case 8
                        Conjugation = "causative"
                    Case 9
                        Conjugation = "passive"
                    Case 10
                        Conjugation = "imperative"
                    Case 11
                        Conjugation = "potential"
                    Case 12
                        Conjugation = "volitional"
                End Select
            Else
                Select Case AudioID
                    Case 1
                        Conjugation = "dictionary"
                    Case 2
                        Conjugation = "polite"
                    Case 3
                        Conjugation = "adverb"
                    Case 4
                        Conjugation = "te"
                    Case 5
                        Conjugation = "past"
                    Case 6
                        Conjugation = "negative"
                    Case 7
                        Conjugation = "negative_past"
                    Case 8
                        Conjugation = "ba-conditional"
                End Select
            End If
            Tries = 0
            Do Until Tries = -1 Or Tries = 2
                Try
                    Client.DownloadFile(WordURL, "C:\ProgramData\Japanese Conjugation Helper\Audio\" & AudioID & ".wav")
                    Tries = -1
                Catch
                    Tries += 1
                End Try
            Loop
        Next

        Dim AudioLeft As String = ""
        For ToAdd = 1 To MaxAudio
            AudioLeft &= ToAdd
        Next
        Dim AudioLeft2 As String = AudioLeft

        If MaxAudio = 9 Then
            MaxAudio = 8
        End If

        Dim Ichidan, Godan As Boolean
        If Verb = True Then
            If Types.IndexOf("ichidan") <> -1 Then
                Ichidan = True
            Else
                Godan = True
            End If
        End If

        Dim Last As String = Right(Furigana, 1)
        Dim LastAdd As String = ""
        Dim LastAdd2 As String = ""
        Dim LastAddPot As String = ""

        Dim PlainVerb As String = ""
        Dim masuStem As String = ""
        Dim NegativeStem As String = ""
        Dim Potential As String = ""
        Dim Causative As String = ""
        Dim Conditional As String = ""
        Dim teStem As String = ""
        Dim Volitional As String = ""
        Dim Passive As String = ""
        Dim Imperative As String = ""
        Dim ShortPastEnding As String = ""
        If Verb = True Then
            PlainVerb = Furigana
            If Godan = True Then
                If Last = "む" Then
                    LastAdd = "み"
                    LastAdd2 = "もう"
                End If
                If Last = "ぶ" Then
                    LastAdd = "び"
                    LastAdd2 = "ぼう"
                End If
                If Last = "ぬ" Then
                    LastAdd = "に"
                    LastAdd2 = "のう"
                End If
                If Last = "す" Then
                    LastAdd = "し"
                    LastAdd2 = "そう"
                End If
                If Last = "ぐ" Then
                    LastAdd = "ぎ"
                    LastAdd2 = "ごう"
                End If
                If Last = "く" Then
                    LastAdd = "き"
                    LastAdd2 = "こう"
                End If
                If Last = "る" Then
                    LastAdd = "り"
                    LastAdd2 = "ろう"
                End If
                If Last = "つ" Then
                    LastAdd = "ち"
                    LastAdd2 = "とう"
                End If
                If Last = "う" Then
                    LastAdd = "い"
                    LastAdd2 = "おう"
                End If
                masuStem = Left(PlainVerb, PlainVerb.Length - 1) & LastAdd
                Volitional = Left(PlainVerb, PlainVerb.Length - 1) & LastAdd2
            Else
                masuStem = Left(PlainVerb, PlainVerb.Length - 1)
                Volitional = Left(PlainVerb, PlainVerb.Length - 1) & "よう"
            End If

            'Creating negative stems (Last add) and Potential forms
            If Godan = True Then
                If Last = "む" Then
                    LastAdd = "ま"
                    LastAddPot = "める"
                End If
                If Last = "ぶ" Then
                    LastAdd = "ば"
                    LastAddPot = "べる"
                End If
                If Last = "ぬ" Then
                    LastAdd = "な"
                    LastAddPot = "ねる"
                End If
                If Last = "す" Then
                    LastAdd = "さ"
                    LastAddPot = "せる"
                End If
                If Last = "ぐ" Then
                    LastAdd = "が"
                    LastAddPot = "げる"
                End If
                If Last = "く" Then
                    LastAdd = "か"
                    LastAddPot = "ける"
                End If
                If Last = "る" Then
                    LastAdd = "ら"
                    LastAddPot = "れる"
                End If
                If Last = "つ" Then
                    LastAdd = "た"
                    LastAddPot = "てる"
                End If
                If Last = "う" Then
                    LastAdd = "わ"
                    LastAddPot = "える"
                End If
                NegativeStem = Left(PlainVerb, PlainVerb.Length - 1) & LastAdd
                Potential = Left(PlainVerb, PlainVerb.Length - 1) & LastAddPot
                Causative = Left(PlainVerb, PlainVerb.Length - 1) & LastAdd & "せる"
                Conditional = Left(Potential, Potential.Length - 1) & "ば"
                Passive = Left(PlainVerb, PlainVerb.Length - 1) & LastAdd & "れる"
                Imperative = Left(Potential, Potential.Length - 1)
            Else
                NegativeStem = Left(PlainVerb, PlainVerb.Length - 1)
                Potential = Left(PlainVerb, PlainVerb.Length - 1) & "られる"
                Causative = Left(PlainVerb, PlainVerb.Length - 1) & "させる"
                Conditional = Left(PlainVerb, PlainVerb.Length - 1) & "れば"
                Passive = masuStem & "られる"
                Imperative = masuStem + "ろ"
            End If

            'Creating te-form stem of searched word
            If Godan = True Then
                If Last = "む" Or Last = "ぶ" Or Last = "ぬ" Then
                    LastAdd = "んで"
                End If
                If Last = "す" Then
                    LastAdd = "して"
                End If
                If Last = "ぐ" Then
                    LastAdd = "いで"
                End If
                If Last = "く" Then
                    LastAdd = "いて"
                End If
                If Last = "る" Or Last = "つ" Or Last = "う" Then
                    LastAdd = "って"
                End If
                teStem = Left(PlainVerb, PlainVerb.Length - 1) & LastAdd
            Else
                teStem = Left(PlainVerb, PlainVerb.Length - 1) & "て"
            End If

            If PlainVerb = "来る" Then
                Imperative = "来い"
            ElseIf PlainVerb = "する" Then
                Imperative = "しろ"
            End If

            'For ShortPastForm:
            ShortPastEnding = Right(teStem, 1)
            If ShortPastEnding = "て" Then
                ShortPastEnding = "た"
            ElseIf ShortPastEnding = "で" Then
                ShortPastEnding = "だ"
            ElseIf Ichidan = True Then
                ShortPastEnding = "た"
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Error: Short past tense")
                Console.ForegroundColor = ConsoleColor.White
            End If
        End If


        Randomize()
        Dim AudioPlay As Integer = 0
        Dim AudioPlay2 As Integer = 0

        Dim Response As String = ""
        Dim NumberResponse As Integer = 0
        Dim IsNumber As Boolean = False
        Dim CorrectConjugation As String = ""
        Do Until AudioLeft.Length = 0
            IsNumber = False
            If AudioLeft = "0" Then
                AudioLeft = "10"
            End If
            Do Until IsNumber = True 'Generating a suitable random number
                AudioPlay = Int((MaxAudio) * Rnd() + 1)
                If AudioLeft.Contains(AudioPlay) Then
                    IsNumber = True
                End If
            Loop

            If Verb = True Then
                IsNumber = False
                Tries = 0
                Do Until IsNumber = True
                    Select Case AudioPlay
                        Case 1
                            Conjugation = "dictionary"
                            CorrectConjugation = PlainVerb
                        Case 2
                            Conjugation = "masu"
                            CorrectConjugation = masuStem + "ます"
                        Case 3
                            Conjugation = "te"
                            CorrectConjugation = teStem
                        Case 4
                            Conjugation = "past"
                            CorrectConjugation = Left(teStem, teStem.Length - 1) & ShortPastEnding
                        Case 5
                            Conjugation = "negative"
                            CorrectConjugation = NegativeStem + "ない"
                        Case 6
                            Conjugation = "negative_past"
                            CorrectConjugation = NegativeStem + "なかった"
                        Case 7
                            Conjugation = "ba_conditional"
                            CorrectConjugation = Conditional
                        Case 8
                            Conjugation = "causative"
                            CorrectConjugation = Causative
                        Case 9
                            Conjugation = "passive"
                            CorrectConjugation = Passive
                        Case 10
                            Conjugation = "imperative"
                            CorrectConjugation = Imperative
                        Case 11
                            Conjugation = "potential"
                            CorrectConjugation = Potential
                        Case 12
                            Conjugation = "volitional"
                            CorrectConjugation = Volitional
                    End Select

                    Console.Clear()
                    Select Case Tries
                        Case 1
                            Console.ForegroundColor = ConsoleColor.Yellow
                        Case 2
                            Console.ForegroundColor = ConsoleColor.DarkYellow
                        Case 3
                            Console.ForegroundColor = ConsoleColor.Red
                        Case 4, 5
                            Console.ForegroundColor = ConsoleColor.DarkRed
                    End Select
                    Console.WriteLine("Type " & ActualSearchWord & "'s " & Conjugation & " form.")
                    Console.ForegroundColor = ConsoleColor.White

                    Response = WanaKana.ToHiragana(Console.ReadLine.Trim)
                    If Response = CorrectConjugation Then
                        IsNumber = True
                    End If
                    If Tries = 5 Then
                        IsNumber = True
                        Console.WriteLine("The answer was " & CorrectConjugation)
                        My.Computer.Audio.Play("C:\ProgramData\Japanese Conjugation Helper\Audio\" & AudioPlay & ".wav", AudioPlayMode.WaitToComplete)
                        Console.Clear()
                    End If
                    Tries += 1
                Loop
            End If
            AudioLeft = AudioLeft.Replace(AudioPlay, "")

            IsNumber = False
            If AudioLeft2 = "0" Then
                AudioLeft2 = "10"
            End If
            Do Until IsNumber = True 'Generating a suitable random number
                AudioPlay2 = Int((MaxAudio) * Rnd() + 1)
                If AudioLeft2.Contains(AudioPlay2) Then
                    IsNumber = True
                End If
            Loop
            Tries = 0
            If Verb = False Then
                Select Case AudioPlay2
                    Case 1
                        Conjugation = "dictionary"
                    Case 2
                        Conjugation = "polite"
                    Case 3
                        Conjugation = "adverb"
                    Case 4
                        Conjugation = "te"
                    Case 5
                        Conjugation = "past"
                    Case 6
                        Conjugation = "negative"
                    Case 7
                        Conjugation = "negative_past"
                    Case 8
                        Conjugation = "ba-conditional"
                End Select
            End If

            IsNumber = False
            Do Until IsNumber = True
                My.Computer.Audio.Play("C:\ProgramData\Japanese Conjugation Helper\Audio\" & AudioPlay2 & ".wav", AudioPlayMode.Background)
                Console.Clear()
                Console.WriteLine("What form is the audio in?")
                Console.WriteLine()
                Select Case Tries
                    Case 1
                        Console.ForegroundColor = ConsoleColor.Yellow
                    Case 2
                        Console.ForegroundColor = ConsoleColor.DarkYellow
                    Case 3, 4, 5
                        Console.ForegroundColor = ConsoleColor.Red
                    Case Is > 5
                        Console.ForegroundColor = ConsoleColor.DarkRed
                End Select
                If Verb = True Then
                    Console.WriteLine("1 = dictionary")
                    Console.WriteLine("2 = masu ")
                    Console.WriteLine("3 = te-form")
                    Console.WriteLine("4 = past")
                    Console.WriteLine("5 = negative")
                    Console.WriteLine("6 = negative past")
                    Console.WriteLine("7 = ba-conditional")
                    Console.WriteLine("8 = causative")
                    Console.WriteLine("9 = passive")
                    Console.WriteLine("10 = imperative")
                    Console.WriteLine("11 = potential")
                    Console.WriteLine("12 = volitional")
                Else
                    Console.WriteLine("1 = dictionary form")
                    Console.WriteLine("2 = polite")
                    Console.WriteLine("3 = adverb")
                    Console.WriteLine("4 = te-form")
                    Console.WriteLine("5 = past")
                    Console.WriteLine("6 = negative")
                    Console.WriteLine("7 = negative past")
                    Console.WriteLine("8 = ba-conditional")
                End If
                Console.ForegroundColor = ConsoleColor.White
                Response = Console.ReadLine
                If IsNumeric(Response) = True Then
                    NumberResponse = CInt(Response)
                    If NumberResponse > 0 And NumberResponse <= MaxAudio Then
                        IsNumber = True
                    End If
                End If

                If IsNumber = True Then
                    IsNumber = False
                    If AudioPlay2 = NumberResponse Then
                        IsNumber = True
                        AudioLeft = AudioLeft.Replace(AudioPlay2, "")
                    ElseIf Ichidan = True Then
                        If AudioPlay2 = 9 Or AudioPlay2 = 11 Then
                            If NumberResponse = 9 Or NumberResponse = 11 Then
                                IsNumber = True
                                AudioLeft2 = AudioLeft2.Replace(AudioPlay2, "")
                            End If
                        End If
                    Else
                        Tries += 1
                    End If
                End If
                If AudioLeft2.Length = 0 Then
                    IsNumber = False
                End If
            Loop

        Loop

        Console.WriteLine()
        Console.WriteLine("Done!")
        Console.ReadLine()
        Main()
    End Sub
    Sub VerbAudioGen(ByVal Word)
        Const QUOTE = """"
        Dim Gender As String = "male"
        If Word.indexOf("!f") <> -1 Then
            Word = Word.replace("!f", "")
            Gender = "female"
        ElseIf Word.indexOf("!m") <> -1 Then
            Word = Word.replace("!m", "")
            Gender = "male"
        End If
        Word = Word.Replace(" ", "")

        Console.Clear()
        Console.WriteLine("Searching " & QUOTE & Word & QUOTE)
        Dim WordURL As String
        Dim Client As New WebClient
        Dim HTML As String = ""
        Client.Encoding = System.Text.Encoding.UTF8
        Try
            WordURL = ("https://jisho.org/search/" & Word)
            HTML = Client.DownloadString(New Uri(WordURL))
        Catch
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Search failed")
            Console.WriteLine("Check you are connected to the internet")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try
        Dim HTMLTemp As String = HTML

        Dim ActualSearchWord As String = ""
        Dim ActualSearch1stAppearance As Integer = 0
        Dim ActualSearch2ndAppearance As Integer = 0
        Dim WordLink As String = ""
        Dim Max As Integer = 3
        Dim Types As String = ""

        Console.WriteLine("Trying to get information for " & QUOTE & Word & QUOTE)
        Try
            ActualSearchWord = RetrieveClassRange(HTMLTemp, "<span class=" & QUOTE & "text" & QUOTE & ">", "</div>", "Actual word search")
            ActualSearchWord = Mid(ActualSearchWord, 30)
            ActualSearchWord = ActualSearchWord.Replace("<span>", "")
            ActualSearchWord = ActualSearchWord.Replace("</span>", "")
            ActualSearchWord = ActualSearchWord.Trim

            'Getting the link of the actual word:
            ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & QUOTE & "text" & QUOTE & ">")
            HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
            ActualSearch1stAppearance = HTMLTemp.IndexOf("meanings-wrapper") 'used to be "concept_light clearfix"
            HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
            ActualSearch1stAppearance = HTMLTemp.IndexOf("jisho.org/word/")
            ActualSearch2ndAppearance = Mid(HTMLTemp, HTMLTemp.IndexOf("jisho.org/word/")).IndexOf(QUOTE & ">")
            WordLink = Mid(HTMLTemp, ActualSearch1stAppearance + 1, ActualSearch2ndAppearance - 1)
        Catch
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Word not found!")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try

        If ActualSearchWord.IndexOf("<") <> -1 Or ActualSearchWord.IndexOf(">") <> -1 Or ActualSearchWord.IndexOf("span") <> -1 Then
            ActualSearchWord = RetrieveClassRange(HTML, "<div class=" & QUOTE & "concept_light clearfix" & QUOTE & ">", "</div>", "Actual word search")
            ActualSearchWord = RetrieveClassRange(ActualSearchWord, "text", "</span", "Actual word search")
            ActualSearchWord = Mid(ActualSearchWord, 17)
            ActualSearchWord = ActualSearchWord.Replace("<span>", "")
            ActualSearchWord = ActualSearchWord.Replace("</span>", "")
        End If

        If ActualSearchWord.Length = 0 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Word not found!")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End If

        Types = TypeScraper("jisho.org/search/" & ActualSearchWord).replace("&#39;", "").tolower

        If Types.IndexOf("verb") = -1 Or Types.IndexOf("suru") <> -1 Or Types.IndexOf("noun") <> -1 Then
            If Types.IndexOf("i-adjective") = -1 Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("You can only download audio files for verbs.")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()
                Main()
            End If
        End If
        Dim Verb, IAdj As Boolean
        If Types.IndexOf("verb") <> -1 Then
            Verb = True
        ElseIf Types.IndexOf("i-adj") <> -1 Then
            IAdj = True
        End If

        Console.WriteLine("Searching for " & QUOTE & Gender & "_" & ActualSearchWord & QUOTE & " on OJAD")
        Dim Snip1, Snip2 As Integer
        Dim WordID As String = ""
        WordURL = "http://www.gavo.t.u-tokyo.ac.jp/ojad/search/index/sortprefix:difficulty/narabi1:difficulty_asc/narabi2:mola_asc/narabi3:proc_asc/yure:visible/curve:fujisaki/details:visible/limit:100/word:" & ActualSearchWord
        Try
            HTML = Client.DownloadString(New Uri(WordURL))
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(QUOTE & ActualSearchWord & QUOTE & " wasn't found on OJAD")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try

        Try
            WordID = HTML
            Snip1 = WordID.IndexOf("<tr id=" & QUOTE & "word_")
            If Snip1 = -1 Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(QUOTE & ActualSearchWord & QUOTE & " wasn't found on OJAD")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()
                Main()
            End If
            Snip1 += 14
            WordID = Mid(WordID, Snip1)
            Snip2 = WordID.IndexOf(QUOTE & ">")
            WordID = Left(WordID, Snip2)
        Catch
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(QUOTE & ActualSearchWord & QUOTE & " wasn't found on OJAD")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try

        Console.WriteLine("Getting word ID")
        HTML = Client.DownloadString(New Uri(WordURL))
        Dim AudioBase As String = "http://www.gavo.t.u-tokyo.ac.jp/ojad/sound4/mp3/male/"
        Dim Key As String = ""
        Console.Clear()
        For ID = 0 To 36
            Console.SetCursorPosition(0, 0)
            Console.WriteLine("Getting audio ID")
            Console.WriteLine("Testing ID " & ID & "/36")
            Key = ID
            If Key.Length = 1 Then
                Key = "00" & Key
            ElseIf Key.Length = 2 Then
                Key = "0" & Key
            End If
            Try
                WordURL = AudioBase & Key & "/" & WordID & "_1_1_male.mp3"
                HTML = Client.DownloadString(New Uri(WordURL))
            Catch
                WordURL = ""
            End Try

            If HTML.IndexOf("<!DOCTYPE html>") <> -1 Then
                Continue For
            End If
            If Word <> "" Then
                ID = 999
                Continue For
            End If
        Next

        If Key > 35 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("It seems that you didn't enter a verb. Only verbs work, sorry!")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End If

        Console.Clear()
        Console.WriteLine("Downloading audio:")
        AudioBase &= Key & "/" & WordID
        My.Computer.FileSystem.CreateDirectory(Environ$("USERPROFILE") & "\Downloads\Conjugation Audio\" & ActualSearchWord & " Conjugations")
        Dim Conjugation As String = ""
        Dim Tries As Integer = 0
        Dim MaxAudio As Integer = 12
        If IAdj = True Then
            MaxAudio = 9
        End If
        For AudioID = 1 To MaxAudio
            WordURL = AudioBase & "_" & AudioID & "_1_male.mp3?"
            If Gender = "female" Then
                WordURL = WordURL.Replace("male", "female")
            End If
            If Verb = True Then
                Select Case AudioID
                    Case 1
                        Conjugation = "dictionary"
                    Case 2
                        Conjugation = "masu"
                    Case 3
                        Conjugation = "te"
                    Case 4
                        Conjugation = "past"
                    Case 5
                        Conjugation = "negative"
                    Case 6
                        Conjugation = "negative_past"
                    Case 7
                        Conjugation = "ba_conditional"
                    Case 8
                        Conjugation = "causative"
                    Case 9
                        Conjugation = "passive"
                    Case 10
                        Conjugation = "imperative"
                    Case 11
                        Conjugation = "potential"
                    Case 12
                        Conjugation = "volitional"
                End Select
            Else
                Select Case AudioID
                    Case 1
                        Conjugation = "dictionary"
                    Case 2
                        Conjugation = "dictionary2"
                    Case 3
                        Conjugation = "polite"
                    Case 4
                        Conjugation = "adverb"
                    Case 5
                        Conjugation = "te"
                    Case 6
                        Conjugation = "past"
                    Case 7
                        Conjugation = "negative"
                    Case 8
                        Conjugation = "negative_past"
                    Case 9
                        Conjugation = "ba-conditional"
                End Select
            End If
            Tries = 0
            Do Until Tries = -1 Or Tries = 2
                Try
                    Client.DownloadFile(WordURL, Environ$("USERPROFILE") & "\Downloads\Conjugation Audio\" & ActualSearchWord & " Conjugations\" & Conjugation & "_" & Gender & ".mp3")
                    Console.ForegroundColor = ConsoleColor.Green
                    Console.WriteLine("Downloaded " & Conjugation & "_" & Gender & ".mp3")
                    Console.ForegroundColor = ConsoleColor.White
                    Tries = -1
                Catch
                    Tries += 1
                End Try
            Loop
            If Tries = 2 Then
                Console.ForegroundColor = ConsoleColor.Yellow
                Console.WriteLine("It seems there isn't an audio file for " & Conjugation & " form")
                Console.ForegroundColor = ConsoleColor.White
            End If
        Next

        Console.ForegroundColor = ConsoleColor.Blue
        Console.WriteLine("Audio is in Downloads in a folder called " & QUOTE & "Conjugation Audio" & QUOTE)
        Console.WriteLine("Type 'yes' to be taken there.")
        Console.ForegroundColor = ConsoleColor.White
        If Console.ReadLine().ToLower.IndexOf("yes") <> -1 Then
            Process.Start("explorer.exe", Environ$("USERPROFILE") & "\Downloads\Conjugation Audio\" & ActualSearchWord & " Conjugations")
        End If
        Main()
    End Sub
    Sub DisplayDefinitions(ByVal SelectedDefinition, ByVal SelectedType, ByVal DefG1, ByVal DisplayType)
        'Displaying word definitions WITH corresponding the word types: -------------------------
        Dim NumberCheckD As String = ""
        Dim NumberCheckT As String = ""
        Dim Type As Integer = 0
        Dim MatchT As Boolean = False
        Dim Definition As Integer = 0
        Dim SB1, SB2 As Integer
        Dim BArea, BArea2 As String
        Do Until Definition = SelectedDefinition.Length
            NumberCheckD = Right(SelectedDefinition(Definition), 2)
            If NumberCheckD.IndexOf(".") <> -1 Then 'This is checking for a "." because this will mess up the 'is numberic function if it does exist
                NumberCheckD = Right(NumberCheckD, 1)
            End If
            If IsNumeric(NumberCheckD) = False Then
                NumberCheckD = Right(SelectedDefinition(Definition), 1)
            End If
            If IsNumeric(NumberCheckD) = False Then
                Console.WriteLine("Error: Conjugate; Definition no; D")
            End If
            NumberCheckD = NumberCheckD.Replace(" ", "")


            MatchT = False
            Type = 0
            Do Until Type = SelectedType.Length Or MatchT = True
                MatchT = False
                If SelectedType(Type) = "!" Then
                    Type += 1
                    Continue Do
                End If


                NumberCheckT = Right(SelectedType(Type), 2)
                If IsNumeric(NumberCheckT) = False Then
                    NumberCheckT = Right(SelectedType(Type), 1)
                End If
                NumberCheckT = NumberCheckT.Replace(" ", "")

                If NumberCheckT = NumberCheckD Then
                    If Definition < DefG1 + 1 Then
                        If Definition <> 0 Then
                            Console.WriteLine()
                            WriteToFile("", "LastSearch")
                        End If
                        Console.WriteLine(Left(SelectedType(Type), SelectedType(Type).Length - NumberCheckT.Length))
                        WriteToFile(Left(SelectedType(Type), SelectedType(Type).Length - NumberCheckT.Length), "LastSearch")
                    ElseIf Definition > DefG1 And SelectedType(Type).IndexOf("aux") <> -1 Or Definition > DefG1 And SelectedType(Type).IndexOf("fix") <> -1 Then
                        Console.WriteLine()
                        Console.WriteLine(Left(SelectedType(Type), SelectedType(Type).Length - NumberCheckT.Length))
                        Console.WriteLine()
                        WriteToFile("", "LastSearch")
                        WriteToFile(Left(SelectedType(Type), SelectedType(Type).Length - NumberCheckT.Length), "LastSearch")
                        WriteToFile("", "LastSearch")

                        'Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length))
                        If Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[") = -1 Then
                            Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length))
                            WriteToFile(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), "LastSearch")
                        Else
                            SB1 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[")
                            SB2 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("]")
                            BArea = Mid(Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), SB1 + 1, SB2 + 1 - SB1)

                            If BArea.IndexOf("kana") = -1 Then
                                Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                                Console.ForegroundColor = ConsoleColor.DarkGray
                                Console.WriteLine(BArea)
                                Console.ForegroundColor = ConsoleColor.White
                                Console.WriteLine()
                                WriteToFile(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""), "LastSearch")
                                WriteToFile(BArea, "LastSearch")
                                WriteToFile("", "LastSearch")
                            Else
                                SB1 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[")
                                SB2 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("]")
                                BArea = Mid(Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), SB1 + 1, SB2 + 1 - SB1)

                                If BArea.IndexOf("kana") = -1 Then
                                    Console.Write(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                                    Console.ForegroundColor = ConsoleColor.DarkGray
                                    Console.WriteLine(BArea)
                                    Console.ForegroundColor = ConsoleColor.White
                                    WriteToFile(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""), "LastSearch")
                                    WriteToFile(BArea, "LastSearch")
                                Else
                                    BArea2 = BArea 'BArea is acting like a temp

                                    If BArea.IndexOf("Usually written using kana alone") <> -1 Then 'BArea will be set below if it has more than one thing
                                        BArea2 = BArea.Replace("Usually written using kana alone", "")
                                        BArea2 = BArea2.Replace(", ", "")
                                    End If

                                    If BArea2.Length > 3 Then
                                        BArea2 = BArea2.Replace("See also", "See also ")
                                        Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "") & BArea2)
                                        WriteToFile(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "") & BArea2, "LastSearch")
                                    Else
                                        Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                                        WriteToFile(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""), "LastSearch")
                                    End If
                                End If
                            End If
                        End If

                    End If
                    MatchT = True
                    SelectedType(Type) = "!"
                    Continue Do
                End If
                Type += 1
            Loop

            If Definition < DefG1 + 1 Then
                If Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[") = -1 Then
                    Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length))
                    WriteToFile(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), "LastSearch")
                Else
                    SB1 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[")
                    SB2 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("]")
                    BArea = Mid(Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), SB1 + 1, SB2 + 1 - SB1)

                    If BArea.IndexOf("kana") = -1 Then
                        Console.Write(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                        BArea = BArea.Replace("See also", "See also ")
                        Console.ForegroundColor = ConsoleColor.DarkGray
                        Console.WriteLine(BArea)
                        Console.ForegroundColor = ConsoleColor.White
                        WriteToFile(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""), "LastSearch")
                        WriteToFile(BArea, "LastSearch")
                    Else
                        BArea2 = BArea 'BArea is acting like a temp

                        If BArea.IndexOf("Usually written using kana alone") <> -1 Then 'BArea will be set below if it has more than one thing
                            BArea2 = BArea.Replace("Usually written using kana alone", "")
                            BArea2 = BArea2.Replace(", ", "")
                        End If

                        If BArea2.Length > 3 Then
                            BArea2 = BArea2.Replace("See also", "See also ")
                            Console.Write(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                            Console.ForegroundColor = ConsoleColor.DarkGray
                            Console.WriteLine(BArea2)
                            Console.ForegroundColor = ConsoleColor.White
                            WriteToFile(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""), "LastSearch")
                            WriteToFile(BArea, "LastSearch")
                        Else
                            Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                            WriteToFile(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""), "LastSearch")
                        End If
                    End If
                End If
            End If
            Definition += 1
        Loop

        If SelectedDefinition.Length > DefG1 Then
            Console.ForegroundColor = ConsoleColor.DarkGray
            Console.WriteLine("[this word has a total of " & SelectedDefinition.Length & " definitions]")
            Console.ForegroundColor = ConsoleColor.White
        End If

        'Continue where you were the the last sub
    End Sub
    Sub SearchMultiple(ByVal Search)
        Const Quote = """"
        Search = Search.ToLower

        For Clear = -1 To 10
            Search = Search.replace(" s=" & Clear, "")
            Search = Search.replace("s=" & Clear & " ", "")
            Search = Search.replace("s=" & Clear, "")
            Search = Search.replace("s=", "")
            If Right(Search, 2) = CStr(Clear) Then
                Search = Left(Search, Search.length - 2)
            End If
        Next

        Dim Searches() As String = Search.split("&&")

        Searches(1) = Searches(2)
        Array.Resize(Searches, Searches.Length - 1)

        Dim WordURL As String = "https://jisho.org/search/" & Searches(0)
        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8

        Dim HTML As String
        HTML = Client.DownloadString(New Uri(WordURL))
        Dim AddingTemp As String = ""

        If HTML.IndexOf("No matches for") <> -1 Then
            Console.WriteLine("Looking for similar words...")
        End If

        'If HTML.IndexOf("zen_bar") <> -1 And Anki = False Then
        'TranslateSentence(Word)
        'End If

        Dim HTMLTemp As String = HTML
        Dim ActualSearchWord As String = ""
        Dim ActualSearch1stAppearance As Integer = 0
        Dim WordLink As String = ""
        Dim FoundWords(1) As String
        Dim FoundDefinitions(1) As String
        Dim FoundWordLinks(1) As String
        Dim FoundTypes(1) As String
        'Dim ScrapFull As String
        Dim ActualSearch2ndAppearance As String
        Dim Definition1 As String = ""

        Dim CommonWord(1) As Boolean
        'One word scrapping ---------------------------------------------------------------------------------------------
        Try
            'Getting word link:
            ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & Quote & "text" & Quote & ">")
            HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)

            'Checking if word is "common":
            If Left(HTMLTemp, 300).LastIndexOf("Common word") <> -1 Then
                CommonWord(0) = "True"
            End If

            'Continue getting word link:
            ActualSearch1stAppearance = HTMLTemp.IndexOf("meanings-wrapper") 'used to be "concept_light clearfix"
            HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
            ActualSearch1stAppearance = HTMLTemp.IndexOf("jisho.org/word/")
            ActualSearch2ndAppearance = Mid(HTMLTemp, HTMLTemp.IndexOf("jisho.org/word/")).IndexOf(Quote & ">")
            WordLink = Mid(HTMLTemp, ActualSearch1stAppearance + 1, ActualSearch2ndAppearance - 1)
            FoundWordLinks(0) = WordLink
        Catch
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("That word doesn't exist... Atleast, it seems that way :O")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try

        ActualSearchWord = RetrieveClassRange(HTML, "<span class=" & Quote & "text" & Quote & ">", "</div>", "Actual word search")
        ActualSearchWord = Mid(ActualSearchWord, 30)
        ActualSearchWord = ActualSearchWord.Replace("<span>", "")
        ActualSearchWord = ActualSearchWord.Replace("</span>", "")

        ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 8)

        FoundDefinitions(0) = WordLinkScraper(WordLink).replace("&#39;", "")
        FoundTypes(0) = TypeScraper(WordLink).replace("&#39;", "")

        Dim Furigana As String = ""
        Dim FuriganaStart As Integer
        Try
            Furigana = RetrieveClassRange(HTML, "</a></li><li><a", "</a></li><li><a href=" & Quote & "//jisho.org", "Furigana")
            If Furigana.Length <> 0 Then
                FuriganaStart = Furigana.IndexOf("search for")
                Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 11)
                FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
                If FuriganaStart <> -1 Then 'if </a></li><li> Is found Then it will be removed as well as everything after it
                    Furigana = Left(Furigana, FuriganaStart)
                End If
            Else
                Furigana = ""
            End If

            If Furigana = ActualSearchWord Or Furigana = "" Then 'This will repeat the last attempt to get the furigana, because the last furigana failed and got 'Sentences for [word using kanji]' instead of 'Sentences for [word using kana]'
                Furigana = RetrieveClassRange(HTML, "</a></li><li><a", "</a></li><li><a href=" & Quote & "//jisho.org", "Furigana")
                If Furigana.Length > 30 And Furigana.Length <> 0 Then
                    FuriganaStart = Furigana.IndexOf("search for")
                    Furigana = Mid(Furigana, FuriganaStart + 5)

                    FuriganaStart = Furigana.IndexOf("search for")
                    Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 11)
                    FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
                    Furigana = Left(Furigana, FuriganaStart)
                Else
                    If FuriganaStart <> -1 Or Furigana.Length > 20 Then
                        Furigana = ""
                    End If
                End If
            End If

            If Furigana = ActualSearchWord Or Furigana = "" Or Furigana.Length > 20 Then 'Another try
                Furigana = RetrieveClassRange(HTML, "kanji-3-up kanji", "</span><span></span>", "Furigana")
                If Furigana.Length < 30 And Furigana.Length <> 0 Then
                    Furigana = Mid(Furigana, Furigana.LastIndexOf(">") + 2)
                Else
                    Furigana = ""
                End If

            End If

            If Furigana.Length < 1 Or IsNothing(Furigana) Then
                Furigana = RetrieveClassRange(HTML, "audio id=" & Quote & "audio_" & ActualSearchWord, "<source src=", "Furigana2")
                Furigana = Furigana.Replace("<", "")
                Furigana = Furigana.Replace("audio id=" & Quote & "audio_" & ActualSearchWord & ":", "")
                Furigana = Furigana.Replace(Quote & ">", "")
            End If
        Catch
        End Try

        Dim Furiganas(1) As String
        If Furigana <> "" Then
            Furiganas(0) = Furigana
        End If

        Dim Defintions1() As String = FoundDefinitions(0).Split("|")
        Dim Types1() As String = FoundTypes(0).Split("|")

        For Add = 1 To Defintions1.Length
            Defintions1(Add - 1) = Defintions1(Add - 1).Replace("&quot;", Quote) & Add
        Next

        Console.Clear()
        DisplayDefinitions(Defintions1, Types1, 5, 2)
        Console.BackgroundColor = ConsoleColor.DarkGray
        Try
            If Furigana(0) <> "" Then
                Console.WriteLine(ActualSearchWord & " (" & Furiganas(0) & ")")
            Else
                Console.WriteLine(ActualSearchWord)
            End If
        Catch
            Console.WriteLine(ActualSearchWord)
        End Try

        Console.BackgroundColor = ConsoleColor.Black
        Console.WriteLine()
        KanjiInfo(ActualSearchWord, 2)
        Console.WriteLine()
        Console.WriteLine("-------------------------------------------------------------------------------")
        Console.WriteLine()

        'end of first word scrapping  _______________________________________________________________________________________________________________________

        WordURL = "https://jisho.org/search/" & Searches(1)
        HTML = Client.DownloadString(New Uri(WordURL))
        HTMLTemp = HTML

        Dim ActualSearchWord2 As String = ""
        Try
            'Getting word link:
            ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & Quote & "text" & Quote & ">")
            HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)

            'Checking if word is "common":
            If Left(HTMLTemp, 300).LastIndexOf("Common word") <> -1 Then
                CommonWord(1) = "True"
            End If

            'Continue getting word link:
            ActualSearch1stAppearance = HTMLTemp.IndexOf("meanings-wrapper") 'used to be "concept_light clearfix"
            HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
            ActualSearch1stAppearance = HTMLTemp.IndexOf("jisho.org/word/")
            ActualSearch2ndAppearance = Mid(HTMLTemp, HTMLTemp.IndexOf("jisho.org/word/")).IndexOf(Quote & ">")
            WordLink = Mid(HTMLTemp, ActualSearch1stAppearance + 1, ActualSearch2ndAppearance - 1)
            FoundWordLinks(1) = WordLink
        Catch
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("That word doesn't exist... Atleast, it seems that way :O")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try

        FoundDefinitions(1) = WordLinkScraper(WordLink).replace("&#39;", "")
        FoundTypes(1) = TypeScraper(WordLink).replace("&#39;", "")

        ActualSearchWord2 = RetrieveClassRange(HTML, "<span class=" & Quote & "text" & Quote & ">", "</div>", "Actual word search2nd")
        If ActualSearchWord2.Length < 2 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Word couldn't be found")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End If
        ActualSearchWord2 = Mid(ActualSearchWord2, 30)
        ActualSearchWord2 = ActualSearchWord2.Replace("<span>", "")
        ActualSearchWord2 = ActualSearchWord2.Replace("</span>", "")

        ActualSearchWord2 = Left(ActualSearchWord2, ActualSearchWord2.Length - 8)

        Try
            Furigana = RetrieveClassRange(HTML, "</a></li><li><a", "</a></li><li><a href=" & Quote & "//jisho.org", "Furigana")
            If Furigana.Length <> 0 Then
                FuriganaStart = Furigana.IndexOf("search for")
                Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 11)
                FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
                If FuriganaStart <> -1 Then 'if </a></li><li> Is found Then it will be removed as well as everything after it
                    Furigana = Left(Furigana, FuriganaStart)
                End If
            Else
                Furigana = ""
            End If

            If Furigana = ActualSearchWord Or Furigana = "" Then 'This will repeat the last attempt to get the furigana, because the last furigana failed and got 'Sentences for [word using kanji]' instead of 'Sentences for [word using kana]'
                Furigana = RetrieveClassRange(HTML, "</a></li><li><a", "</a></li><li><a href=" & Quote & "//jisho.org", "Furigana")
                If Furigana.Length > 30 And Furigana.Length <> 0 Then
                    FuriganaStart = Furigana.IndexOf("search for")
                    Furigana = Mid(Furigana, FuriganaStart + 5)

                    FuriganaStart = Furigana.IndexOf("search for")
                    Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 11)
                    FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
                    Furigana = Left(Furigana, FuriganaStart)
                Else
                    If FuriganaStart <> -1 Or Furigana.Length > 20 Then
                        Furigana = ""
                    End If
                End If
            End If

            If Furigana = ActualSearchWord Or Furigana = "" Or Furigana.Length > 20 Then 'Another try
                Furigana = RetrieveClassRange(HTML, "kanji-3-up kanji", "</span><span></span>", "Furigana")
                If Furigana.Length < 30 And Furigana.Length <> 0 Then
                    Furigana = Mid(Furigana, Furigana.LastIndexOf(">") + 2)
                Else
                    Furigana = ""
                End If

            End If

            If Furigana.Length < 1 Or IsNothing(Furigana) Then
                Furigana = RetrieveClassRange(HTML, "audio id=" & Quote & "audio_" & ActualSearchWord, "<source src=", "Furigana2")
                Furigana = Furigana.Replace("<", "")
                Furigana = Furigana.Replace("audio id=" & Quote & "audio_" & ActualSearchWord & ":", "")
                Furigana = Furigana.Replace(Quote & ">", "")
            End If
        Catch
        End Try

        If Furigana <> "" Then
            Furiganas(1) = Furigana
        End If

        Dim Defintions2() As String = FoundDefinitions(1).Split("|")
        Dim Types2() As String = FoundTypes(1).Split("|")

        For Add = 1 To Defintions2.Length
            Defintions2(Add - 1) = Defintions2(Add - 1).Replace("&quot;", Quote) & Add
        Next

        DisplayDefinitions(Defintions2, Types2, 5, 2)
        Console.BackgroundColor = ConsoleColor.DarkGray
        If Furigana(0) <> "" Then
            Console.WriteLine(ActualSearchWord2 & " (" & Furiganas(1) & ")")
        Else
            Console.WriteLine(ActualSearchWord2)
        End If
        Console.BackgroundColor = ConsoleColor.Black
        Console.WriteLine()
        KanjiInfo(ActualSearchWord, 2)

        Console.ReadLine()
        Main()
    End Sub

    Sub SaveWord(Word, Furigana, Definition)
        If Dir$("C:\ProgramData\Japanese Conjugation Helper\WordSaves.txt") = "" Then
            File.Create("C:\ProgramData\Japanese Conjugation Helper\WordSaves.txt").Dispose()
        End If

        Dim HistoryFile As String = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\WordSaves.txt")
        Dim Saves() As String = HistoryFile.Split(vbLf)

        Dim HistoryWriter As System.IO.StreamWriter
        HistoryWriter = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\WordSaves.txt", True)

        If Furigana.Length > 0 Then
                HistoryWriter.WriteLine(Definition & " - " & Word & " (" & Furigana & ")")
            Else
                HistoryWriter.WriteLine(Definition & " - " & Word)
            End If
        HistoryWriter.Close()
    End Sub

    Sub WordConjugate(ByRef Word As String, ByVal WordIndex As Integer)
        Const QUOTE = """"
        Dim SEquals As String = ""
        Dim DefG1Test As String = ""
        Dim DefG1 As Integer = 10

        'Code for the "s=" parameter; only shows the more advanced forms:
        Dim AdvancedParam As Integer = 0

        'Making a file to use as "/back" to see last search instantly:
        Try
            File.Create("C:\ProgramData\Japanese Conjugation Helper\LastSearch.txt").Dispose()
        Catch
        End Try

        Dim PreferenceReader As String = ""
        Dim GeneralSettings(0) As String
        Try
            PreferenceReader = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
            GeneralSettings = PreferenceReader.Split(vbCrLf)
            For Trimmer = 0 To GeneralSettings.Length - 1
                GeneralSettings(Trimmer) = GeneralSettings(Trimmer).Trim
            Next
            Try
                AdvancedParam = Right(GeneralSettings(0), 1)
            Catch
                AdvancedParam = 1
            End Try

            Try
                If IsNumeric(Right(GeneralSettings(1), 2)) = True Then
                    DefG1Test = Right(GeneralSettings(1), 2)
                Else
                    DefG1Test = Right(GeneralSettings(1), 1)
                End If
                If IsNumeric(DefG1Test) = True Then
                    DefG1 = CInt(DefG1Test)
                Else
                    DefG1 = 10
                End If
            Catch
                DefG1 = 10
            End Try

        Catch
            DefG1 = 10
            AdvancedParam = 1
        End Try

        SEquals = Word.IndexOf(" s=")
        Dim Read As String = ""
        If SEquals <> -1 Then
            If IsNumeric(Mid(Word, SEquals + 4, 1)) = False Then 'If the user tried to enter a letter for a S parameter
                Console.WriteLine("The 's' parameter can only be a number")
                Console.ReadLine()
                Main()
            End If
            AdvancedParam = Mid(Word, SEquals + 4, 1)

            If AdvancedParam < 0 Or AdvancedParam > 4 Then 'Making sure the S parameter is 1-3
                Console.WriteLine()

                Do Until IsNumeric(Read) = True
                    Console.WriteLine("The 's' parameter must be in the range 1-4")
                    Console.WriteLine("Please type a number in the range 1-4")
                    Read = Console.ReadLine
                Loop
                Do Until Read > -1 And Read < 5
                    If Read < 0 Or Read > 4 Then
                        Console.WriteLine("The 's' parameter must be in the range 1-4")
                        Console.WriteLine("Please type a number in the range 1-4")
                    End If
                    Read = Console.ReadLine
                Loop
                Word = Word.Replace(Mid(Word, SEquals + 4, 1), Read)
                AdvancedParam = Read
            End If

            If IsNumeric(Mid(Word, SEquals + 5, 1)) = True Then 'If the user didn't enter a number S parameter
                Console.WriteLine()
                Console.WriteLine("The 's' parameter can only be one digit")
                Console.WriteLine("Did you mean to type 's=" & Mid(Word, SEquals + 4, 1) & "'?")
                Console.WriteLine()
                Console.WriteLine("Type " & QUOTE & "Y" & QUOTE & " if that is was you meant.")
                If Console.ReadLine().ToLower = "y" Then
                    Word = Word.Replace(Mid(Word, SEquals + 5, 1), "")
                Else

                    Main()
                End If
            End If

            If SEquals <> -1 Then 'If ' =s" is found
                Word = Word.Replace(" s=" & AdvancedParam, "")
            End If
        End If


        'Hooking up with the custom S=0 Parameter settings: -----------
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Dim PreferenceReaderS As String = ""
        Dim PreferencesString(0) As String
        Try
            PreferenceReaderS = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\Preferences\SParameter.txt")
        Catch
            Console.Clear()
            Console.WriteLine("You haven't got a custom S parameter set up.")
            Console.WriteLine("To do this, use the '/pref' command.")
            Console.ReadLine()

            Main()
        End Try

        PreferencesString = PreferenceReaderS.Split(vbCrLf)

        Dim SRead As Boolean = False
        Dim Index As Integer = 0
        Do Until SRead = True Or Index = PreferencesString.Length - 1
            PreferencesString(Index) = PreferencesString(Index).Trim

            Try
                PreferencesString(Index) = Right(PreferencesString(Index), 1)
            Catch
                Array.Resize(PreferencesString, PreferencesString.Length - 1)
                SRead = True
            End Try

            Index += 1
        Loop
        'S param file hookup done ______________________________________
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Dim SentenceExample As String = ""
        Dim Example As String = ""
        Dim WordURL As String = "https://jisho.org/search/" & Word
        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8

        Dim HTML As String = ""
        Try
            HTML = Client.DownloadString(New Uri(WordURL))
        Catch
            WordJsonSearch(Word)
        End Try

        Dim AddingTemp As String = ""

        If HTML.IndexOf("No matches for") <> -1 Then
            Console.WriteLine("Looking for similar words...")
        End If

        If HTML.IndexOf("zen_bar") <> -1 Then
            TranslateSentence(Word)
        End If

        If WordIndex > 20 Then
            WordURL = ("https://jisho.org/search/" & Word & "%20%23words?page=2")
            AddingTemp = Client.DownloadString(New Uri(WordURL))
            HTML &= AddingTemp
        End If

        Dim HTMLTemp As String = HTML

        If WordIndex < 1 Then
            WordIndex = 1
        End If

        If WordIndex > 40 Then
            WordIndex = 40
        End If

        Dim ActualSearchWord As String = ""
        Dim ActualSearch1stAppearance As Integer = 0
        Dim WordLink As String = ""
        Dim FoundWords(0) As String
        Dim FoundDefinitions(0) As String
        Dim FoundWordLinks(0) As String
        Dim FoundTypes As String
        'Dim ScrapFull As String
        Dim Max As Integer = WordIndex
        Dim WordChoice As Integer = 10000
        Dim ActualSearch2ndAppearance As String
        Dim Definition1 As String = ""
        Dim Snip1 As Integer = 0

        Dim CommonWord(0) As Boolean

        If WordIndex <> 1 Then              'scraping of words and definitions -------------------------------------------------------------------------------------------
            For LoopIndex = 0 To Max - 1
                Array.Resize(FoundWords, FoundWords.Length + 1)
                Array.Resize(FoundDefinitions, FoundDefinitions.Length + 1)
                Array.Resize(FoundWordLinks, FoundWordLinks.Length + 1)
                Array.Resize(CommonWord, CommonWord.Length + 1)
                Try
                    'Getting the Japanese word from the search results:
                    ActualSearchWord = RetrieveClassRange(HTMLTemp, "<span class=" & QUOTE & "text" & QUOTE & ">", "</div>", "Actual word search")
                    ActualSearchWord = Mid(ActualSearchWord, 30)
                    ActualSearchWord = ActualSearchWord.Replace("<span>", "")
                    ActualSearchWord = ActualSearchWord.Replace("</span>", "")
                    If ActualSearchWord.Length = 0 Then
                        Array.Resize(FoundDefinitions, FoundDefinitions.Length - 1)
                        Continue For
                    End If
                    If ActualSearchWord.Length > 0 Then
                        ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 8)
                    End If
                    FoundWords(LoopIndex) = ActualSearchWord

                    'Console.WriteLine("FoundWords(" & LoopIndex & "): " & ActualSearchWord)

                    'Getting the link of the actual word:
                    ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & QUOTE & "text" & QUOTE & ">")
                    HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)

                    'Checking if word is "common":
                    If Left(HTMLTemp, 350).LastIndexOf("Common word") <> -1 Then
                        CommonWord(LoopIndex) = "True"
                    End If

                    'Continuing to get the link of the actual word:
                    ActualSearch1stAppearance = HTMLTemp.IndexOf("meanings-wrapper") 'used to be "concept_light clearfix"
                    HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
                    ActualSearch1stAppearance = HTMLTemp.IndexOf("jisho.org/word/")
                    ActualSearch2ndAppearance = Mid(HTMLTemp, HTMLTemp.IndexOf("jisho.org/word/")).IndexOf(QUOTE & ">")
                    WordLink = Mid(HTMLTemp, ActualSearch1stAppearance + 1, ActualSearch2ndAppearance - 1)

                Catch 'If there are no more search results then:
                    LoopIndex = WordIndex
                    Console.WriteLine(LoopIndex + 1 & ": " & ActualSearchWord & " - " & FoundDefinitions(LoopIndex))

                    If ActualSearchWord = "" Then
                        Console.WriteLine("Word couldn't be found")
                        Console.ReadLine()

                        Main()
                    End If
                End Try

                FoundDefinitions(LoopIndex) = DefinitionScraper(WordLink).replace("&#39;", "").replace("&quot;", QUOTE)
                FoundWordLinks(LoopIndex) = WordLink
                If ActualSearchWord = Nothing Then
                    Array.Resize(FoundDefinitions, FoundDefinitions.Length - 1)
                End If
                If FoundWordLinks(LoopIndex) = Nothing Then
                    Array.Resize(FoundWordLinks, FoundWordLinks.Length - 1)
                End If

                If FoundDefinitions(LoopIndex).IndexOf("|") <> -1 Then
                    ActualSearch1stAppearance = FoundDefinitions(LoopIndex).IndexOf("|")
                    Console.WriteLine(LoopIndex + 1 & ": " & ActualSearchWord & " - " & Left(FoundDefinitions(LoopIndex), ActualSearch1stAppearance))
                Else
                    Console.WriteLine(LoopIndex + 1 & ": " & ActualSearchWord & " - " & FoundDefinitions(LoopIndex))
                End If

            Next                                                          'end of multiple word scrapping ___________________________________________________________________________
            Array.Resize(FoundDefinitions, FoundDefinitions.Length - 1)
            Array.Resize(FoundWords, FoundDefinitions.Length)
            Array.Resize(FoundWordLinks, FoundDefinitions.Length)
            Array.Resize(CommonWord, CommonWord.Length - 1)

            Dim IntTest As String = ""

            Dim FirstBlank As Boolean = True
            Do Until WordChoice <= FoundDefinitions.Length And WordChoice >= 1
                If IntTest.Length = 3 And Left(IntTest, 2).ToLower = "s=" Then
                    If IsNumeric(Right(IntTest, 1)) = True Then
                        If (Right(IntTest, 1)) > -1 And (Right(IntTest, 1)) < 5 Then
                            AdvancedParam = (Right(IntTest, 1))
                            Console.WriteLine("Changed S parameter")
                            Threading.Thread.Sleep(500)
                        End If
                    End If
                End If

                If IsNumeric(IntTest) = True Then
                    WordChoice = CInt(IntTest)
                    If WordChoice <= FoundDefinitions.Length And WordChoice >= 1 Then
                        Continue Do
                    Else
                        WordChoice = WordIndex + 10
                        IntTest = ""
                    End If
                Else
                    Console.Clear()
                    Console.WriteLine("Which definition would you like details for? Type a number, 0 to cancel.")
                    Console.WriteLine()
                    Dim TotalWordsFound As Integer = 0
                    For looper = 1 To FoundWords.Length
                        If IsNothing(FoundWords(looper - 1)) = False Then

                            If FoundDefinitions(looper - 1).IndexOf("|") <> -1 Then
                                ActualSearch1stAppearance = FoundDefinitions(looper - 1).IndexOf("|")
                                Console.WriteLine(looper & ": " & FoundWords(looper - 1) & " - " & Left(FoundDefinitions(looper - 1), ActualSearch1stAppearance))
                            Else
                                Console.WriteLine(looper & ": " & FoundWords(looper - 1) & " - " & FoundDefinitions(looper - 1))
                            End If

                            TotalWordsFound += 1
                        Else
                            If FirstBlank = True Then
                                Max = looper - 1
                                FirstBlank = False
                            End If
                        End If
                    Next
                    If TotalWordsFound = 0 Then
                        Console.Clear()
                        Console.ForegroundColor = ConsoleColor.Red
                        Console.WriteLine("No words were found.")
                        Console.ForegroundColor = ConsoleColor.White
                        Console.ReadLine()

                        Main()
                    End If

                    IntTest = Console.ReadLine
                    If IntTest = "0" Or IntTest.ToLower = "cancel" Or IntTest.ToLower = "stop" Or IntTest.ToLower = "main" Or IntTest.ToLower = "menu" Then

                        Main()
                    End If
                End If
            Loop
            Try
                ActualSearchWord = FoundWords(WordChoice - 1)
            Catch
                ActualSearchWord = FoundWords(FoundWords.Length - 1)
            End Try

            If WordChoice > Max Then
                WordChoice = FoundWords.Length - 1
            End If

            If IsNothing(ActualSearchWord) = True Then
                WordConjugate(Word, 20)
            End If

            FoundTypes = TypeScraper(FoundWordLinks(WordChoice - 1)).replace("&#39;", "")
            FoundTypes = FoundTypes.Replace("!", "")
            FoundDefinitions(0) = WordLinkScraper(FoundWordLinks(WordChoice - 1)).replace("&#39;", "")
            Console.WriteLine()
        Else                                                              'One word scrapping ---------------------------------------------------------------------------------------------
            Try
                'Getting word link:
                ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & QUOTE & "text" & QUOTE & ">")
                HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)

                'Checking if word is "common":
                If Left(HTMLTemp, 350).LastIndexOf("Common word") <> -1 Then
                    CommonWord(0) = "True"
                End If

                'Continue getting word link:
                ActualSearch1stAppearance = HTMLTemp.IndexOf("meanings-wrapper") 'used to be "concept_light clearfix"
                HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
                ActualSearch1stAppearance = HTMLTemp.IndexOf("jisho.org/word/")
                ActualSearch2ndAppearance = Mid(HTMLTemp, HTMLTemp.IndexOf("jisho.org/word/")).IndexOf(QUOTE & ">")
                WordLink = Mid(HTMLTemp, ActualSearch1stAppearance + 1, ActualSearch2ndAppearance - 1)
                FoundWordLinks(0) = WordLink
            Catch
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("That word doesn't exist... Atleast, it seems that way :O")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()

                Main()
            End Try

            FoundDefinitions(0) = WordLinkScraper(WordLink).replace("&#39;", "").replace("&quot;", QUOTE)
            FoundTypes = TypeScraper(WordLink).replace("&#39;", "").replace("&quot;", QUOTE)

            ActualSearchWord = RetrieveClassRange(HTML, "<span class=" & QUOTE & "text" & QUOTE & ">", "</div>", "Actual word search")
            If ActualSearchWord.Length < 2 Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Word couldn't be found")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()

                Main()
            End If
            ActualSearchWord = Mid(ActualSearchWord, 30)
            ActualSearchWord = ActualSearchWord.Replace("<span>", "")
            ActualSearchWord = ActualSearchWord.Replace("</span>", "")

            ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 8)
            Max = 0 'because we are in the "else" part of the if statement which means that the user inputted no number or 1
            WordChoice = 1
        End If                                                           'end of one word scrapping and all scrapping _______________________________________________________________________________________________________________________
        LinkChoice = FoundWordLinks(WordChoice - 1)
        'WordChoice = WordChoice - 1

        Dim IsCommon As Boolean
        If CommonWord(WordChoice - 1) = "True" Then
            IsCommon = True
        End If

        'Building of the chosen Definition and Type arrays:
        Dim SelectedDefinition() As String = FoundDefinitions(0).Split("|")
        Dim SelectedType() As String = FoundTypes.Split("|")
        For Add = 1 To SelectedDefinition.Length
            SelectedDefinition(Add - 1) = SelectedDefinition(Add - 1).Replace("&quot;", QUOTE) & Add
        Next

        Dim StartingHTML As Integer
        StartingHTML = HTML.IndexOf(ActualSearchWord)
        HTMLTemp = Mid(HTML, StartingHTML)

        'RetrieveClass(ByVal HTML, ByRef ClassToRetrieve, ByVal ErrorMessage)

        Dim TypeSnip As String = RetrieveClass(HTMLTemp, "meaning-tags", "TypeSnip") 'This is retrieving the snip types

        If TypeSnip = "Other forms" Then
            TypeSnip = "Phrase:"
        End If
        Dim FullWordType As String = TypeSnip
        Dim TypeSnipEnd As Integer = TypeSnip.IndexOf(",") 'This is to check if there is more than one word type
        Console.Clear()



        'Extracting furigana and then snipping from the start up to the furigana
        Dim WordHTML0, WordHTML As String
        WordHTML0 = Client.DownloadString(New Uri("https://" & FoundWordLinks(WordChoice - 1))) 'This is used for the furigana
        WordHTML = WordHTML0 'This is a copy, used for the kanji readings at the end
        Dim Furigana As String = ""
        Dim FuriganaStart As Integer

        Try
            Furigana = RetrieveClassRange(WordHTML0, "</a></li><li><a", "</a></li><li><a href=" & QUOTE & "//jisho.org", "Furigana")
            If Furigana.Length <> 0 Then
                FuriganaStart = Furigana.IndexOf("search for")
                Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 11)
                FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
                If FuriganaStart <> -1 Then 'if </a></li><li> Is found Then it will be removed as well as everything after it
                    Furigana = Left(Furigana, FuriganaStart)
                End If
            Else
                Furigana = ""
            End If

            If Furigana = ActualSearchWord Or Furigana = "" Then 'This will repeat the last attempt to get the furigana, because the last furigana failed and got 'Sentences for [word using kanji]' instead of 'Sentences for [word using kana]'
                Furigana = RetrieveClassRange(WordHTML0, "</a></li><li><a", "</a></li><li><a href=" & QUOTE & "//jisho.org", "Furigana")
                If Furigana.Length > 30 And Furigana.Length <> 0 Then
                    FuriganaStart = Furigana.IndexOf("search for")
                    Furigana = Mid(Furigana, FuriganaStart + 5)

                    FuriganaStart = Furigana.IndexOf("search for")
                    Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 11)
                    FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
                    Furigana = Left(Furigana, FuriganaStart)
                Else
                    If FuriganaStart <> -1 Or Furigana.Length > 20 Then
                        Furigana = ""
                    End If
                End If
            End If

            If Furigana = ActualSearchWord Or Furigana = "" Or Furigana.Length > 20 Then 'Another try
                Furigana = RetrieveClassRange(WordHTML0, "kanji-3-up kanji", "</span><span></span>", "Furigana")
                If Furigana.Length < 30 And Furigana.Length <> 0 Then
                    Furigana = Mid(Furigana, Furigana.LastIndexOf(">") + 2)
                Else
                    Furigana = ""
                End If

            End If

            If Furigana.Length < 1 Or IsNothing(Furigana) Then
                Furigana = RetrieveClassRange(HTML, "audio id=" & QUOTE & "audio_" & ActualSearchWord, "<source src=", "Furigana2")
                Furigana = Furigana.Replace("<", "")
                Furigana = Furigana.Replace("audio id=" & QUOTE & "audio_" & ActualSearchWord & ":", "")
                Furigana = Furigana.Replace(QUOTE & ">", "")
            End If
        Catch
        End Try

        If Furigana.IndexOf(Right(Word, 1)) = -1 Then

        End If

        Dim ShortDefinition As String = "~"
        Try
            ShortDefinition = Left(SelectedDefinition(0), SelectedDefinition(0).IndexOf(";"))
        Catch
            Try
                ShortDefinition = Left(SelectedDefinition(0), SelectedDefinition(0).IndexOf("1"))
            Catch
                ShortDefinition = Left(SelectedDefinition(0), SelectedDefinition(0).IndexOf(" "))
            End Try
        End Try
        ShortDefinition = ShortDefinition.Trim

        'Adding to SearchHistory file
        If Dir$("C:\ProgramData\Japanese Conjugation Helper\SearchHistory.txt") = "" Then
            File.Create("C:\ProgramData\Japanese Conjugation Helper\SearchHistory.txt").Dispose()
        End If

        Dim HistoryFile As String = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\SearchHistory.txt")
        Dim SearchHistory() As String = HistoryFile.Split(vbLf)

        Dim HistoryWriter As System.IO.StreamWriter
        HistoryWriter = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\SearchHistory.txt", True)

        If SearchHistory.Length < 41 Then 'Maximum that the file will hold is one less
            If Furigana.Length > 0 Then
                HistoryWriter.WriteLine(ShortDefinition & " - " & ActualSearchWord & " (" & Furigana & ")")
            Else
                HistoryWriter.WriteLine(ShortDefinition & " - " & ActualSearchWord)
            End If
            HistoryWriter.Close()
        Else 'If the file has above number - 1
            HistoryWriter.Close()
            HistoryWriter = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\SearchHistory.txt", False)
            Array.Resize(SearchHistory, SearchHistory.Length - 1)

            For Position = 0 To SearchHistory.Length - 2
                SearchHistory(Position) = SearchHistory(Position + 1)
            Next

            If Furigana.Length > 0 Then
                SearchHistory(SearchHistory.Length - 1) = (ShortDefinition & " - " & ActualSearchWord & " (" & Furigana & ")")
            Else
                SearchHistory(SearchHistory.Length - 1) = (ShortDefinition & " - " & ActualSearchWord)
            End If

            For Writer = 0 To SearchHistory.Length - 1
                HistoryWriter.WriteLine(SearchHistory(Writer).Trim)
            Next

            HistoryWriter.Close()
        End If

        'Displaying word definitions WITH corresponding the word types: ------------------------
        DefG1 -= 1
        DisplayDefinitions(SelectedDefinition, SelectedType, DefG1, 1)

        'finished the writing of definitions and types____________________

        If Furigana.Length < 15 Then
            Console.WriteLine(ActualSearchWord & "(" & Furigana & ")")
            WriteToFile(ActualSearchWord & "(" & Furigana & ")", "LastSearch")
        Else
            Console.WriteLine(ActualSearchWord)
            WriteToFile(ActualSearchWord, "LastSearch")
        End If

        If IsCommon = True Then
            Console.WriteLine("Common word")
            WriteToFile("Common word", "LastSearch")
        End If

        If WordChoice = 1 Then
            FoundDefinitions(0) = DefinitionScraper(FoundWordLinks(WordChoice - 1))
        End If

        'Dim DefinitionString As String = (FoundDefinitions(WordChoice - 1))
        'Dim WordDefinitions() As String = DefinitionString.Split("|")
        'For Definition = 1 To WordDefinitions.Length
        'Console.WriteLine(Definition & ". " & WordDefinitions(Definition - 1))
        'Next

        Console.WriteLine()

        Dim Scrap As String = ""
        Dim FirstDEnd As Integer
        FirstDEnd = SelectedDefinition(0).IndexOf(";")
        Try
            Scrap = Left(SelectedDefinition(0), FirstDEnd)
        Catch
            Scrap = SelectedDefinition(0)
        End Try

        Scrap = Left(Scrap, 1).ToLower & Right(Scrap, Scrap.Length - 1)


        If ActualSearchWord = "為る" Then
            Console.WriteLine("Formal:")
            Console.WriteLine("I play: します")
            Console.WriteLine("I do not play: しません")
            Console.WriteLine("I played: しました")
            Console.WriteLine("I did not play: しませんでした")
            Console.WriteLine()

            Console.WriteLine("Informal")
            Console.WriteLine("Do: する")
            Console.WriteLine("Don't do: しない")
            Console.WriteLine("Did do: した")
            Console.WriteLine("Didn't do: しなかった")
            Console.WriteLine()
            Console.WriteLine("Te-forms:")
            Console.WriteLine("Te-stem: して")
            Console.WriteLine("Negative: しなくて")
            Console.WriteLine("Negative te-form: しないで")
            Console.WriteLine("Is doing: しています")
            Console.WriteLine("Am doing: している")
            Console.WriteLine()
            Console.WriteLine("Want to do: したい")
            Console.WriteLine()
            Console.WriteLine("Potential:")
            Console.WriteLine("Able to do: できる")
            Console.WriteLine("Not able to do: できない")
            Console.WriteLine()
            Console.WriteLine("Causitive:")
            Console.WriteLine("Let/make (someone) do: させる")
            Console.WriteLine("Don't let/make (someone) do: させない")
            Console.WriteLine("Passive:")
            Console.WriteLine("Was done: される")
            Console.WriteLine("Wasn't done: されない")
            Console.WriteLine()
            Console.WriteLine("Conditional:")
            Console.WriteLine("If do: すれば")
            Console.WriteLine("If don't do: しなければ")
            Console.WriteLine("しなくちゃ (informal)")
            Console.WriteLine("しなきゃ (informal)")
            Console.WriteLine()
            Console.WriteLine("Volitional: しよう")
        End If

        'Preparing some variables for the word being searched
        Dim Iadjective, NaAdjective, NoAdjective, Noun, Suru, Verb, Irregular As Boolean 'Word Types Checker varibles

        'Checking if it's a word with any irregularities
        If FullWordType.ToLower.IndexOf("verb") <> -1 And FullWordType.ToLower.IndexOf("adverb") = -1 Then
            Verb = True
            If FullWordType.ToLower.IndexOf("irregular") <> -1 Or FullWordType.ToLower.IndexOf("special") <> -1 Then
                Irregular = True
            End If
        End If

        If TypeSnipEnd <> -1 Then 'If the word has more than one type. The way I implemented the word type checker is kind of weird with the Else block. I could change this at a later date to make it more efficient.

            'These are checks for each type of word
            If FullWordType.IndexOf("I-adjective") <> -1 Then 'Meaning: if "I-adjective Is found in the list of word types
                Iadjective = True
            End If
            If FullWordType.IndexOf("Na-adjective") <> -1 Then
                NaAdjective = True
            End If
            If FullWordType.IndexOf("No-adjective") <> -1 Then
                NoAdjective = True
            End If
            If FullWordType.IndexOf("Noun") <> -1 Then
                Noun = True
            End If
            If FullWordType.IndexOf("Suru verb") <> -1 Then
                Suru = True
            End If

            'Because "Adverbial" contains verb we need to make sure the program doesn't think that it is a verb
            If FullWordType.IndexOf("verb") <> -1 And FullWordType.IndexOf("Adverb") = -1 Then
                Verb = True
            End If
        End If

        If FullWordType.IndexOf("verb") <> -1 And FullWordType.IndexOf("Suru verb") = -1 And Verb = True And Irregular = False Then
            Dim ComparativeType As String = ""
            If FullWordType.IndexOf("Transitive") <> -1 Then
                ComparativeType = "t"
            End If
            If FullWordType.IndexOf("intransitive") <> -1 Then
                ComparativeType = "i"
            End If

            If FullWordType.IndexOf("Godan verb") <> -1 Then
                ConjugateVerb(ActualSearchWord, "Godan", Scrap, ComparativeType, AdvancedParam, SelectedDefinition, FoundTypes, Furigana) '(Verb/Word, Very Type, Meaning, "ComparativeType")
            End If

            If FullWordType.IndexOf("Ichidan verb") <> -1 Then
                ConjugateVerb(ActualSearchWord, "Ichidan", Scrap, ComparativeType, AdvancedParam, SelectedDefinition, FoundTypes, Furigana) '(Verb/Word, Very Type, Meaning, "ComparativeType")
            End If

            ConjugateVerb(ActualSearchWord, "Error", Scrap, ComparativeType, AdvancedParam, SelectedDefinition, FoundTypes, Furigana)

            'Main()

        Else 'If there is only one word type, the if statement can look at the whole variable "TypeSnip" instead of use .index to determine if a Word Type is part of the whole string
            If Irregular = False Then
                If TypeSnip = "I-adjective" Then
                    Iadjective = True
                ElseIf TypeSnip = "Na-adjective" Then
                    NaAdjective = True
                ElseIf TypeSnip = "Noun" Then
                    Noun = True
                End If

                'For verbs:
                If FullWordType.IndexOf("verb") <> -1 And FullWordType.IndexOf("Adverb") = -1 Then
                    Verb = True
                End If

                If FullWordType.IndexOf("verb") <> -1 And FullWordType.IndexOf("Suru verb") = -1 And Verb = True Then
                    Dim ComparativeType As String = ""
                    If FullWordType.IndexOf("Transitive") <> -1 Then
                        ComparativeType = "t"
                    End If
                    If FullWordType.IndexOf("intransitive") <> -1 Then
                        ComparativeType = "i"
                    End If

                    If FullWordType.IndexOf("Godan verb") <> -1 Then
                        ConjugateVerb(ActualSearchWord, "Godan", Scrap, ComparativeType, AdvancedParam, SelectedDefinition, FoundTypes, Furigana) '(Verb/Word, Very Type, Meaning, "ComparativeType")
                    End If

                    If FullWordType.IndexOf("Ichidan verb") <> -1 Then

                        ConjugateVerb(ActualSearchWord, "Ichidan", Scrap, ComparativeType, AdvancedParam, SelectedDefinition, FoundTypes, Furigana) '(Verb/Word, Very Type, Meaning, "ComparativeType")
                    End If

                    ConjugateVerb(ActualSearchWord, "Error", Scrap, ComparativeType, AdvancedParam, SelectedDefinition, FoundTypes, Furigana)
                    Console.ReadLine()
                    Main()
                End If
            End If
        End If

        'This is checking if the start of the meaning variable is a vowel so that it can change the example sentence from a -> an if it is
        Dim Vowel As Boolean = False
        If Scrap(0) = "a" Or Scrap(0) = "e" Or Scrap(0) = "i" Or Scrap(0) = "o" Or Scrap(0) = "u" Then
            Vowel = True
        End If


        If AdvancedParam = 0 And PreferencesString(11) <> 0 Or AdvancedParam > 1 Then
            If Iadjective = True Then
                ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 1)
                If AdvancedParam <> 1 Or AdvancedParam <> 2 Then
                    Console.BackgroundColor = ConsoleColor.DarkGray
                    Console.Write("- i-adjective conjugation -")
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.WriteLine()
                    Console.WriteLine("Polite:")
                    Console.WriteLine("it is: " & ActualSearchWord & "いです")
                    Console.WriteLine("is not: " & ActualSearchWord & "くありません")
                    Console.WriteLine("")
                    Console.WriteLine("it was: " & ActualSearchWord & "かったです")
                    Console.WriteLine("was not:" & ActualSearchWord & "くありませんでした")

                    Console.WriteLine("")

                    Console.WriteLine("Informal:")
                    Console.WriteLine("is: " & ActualSearchWord & "い")
                    Console.WriteLine("isn't: " & ActualSearchWord & "くない")
                    Console.WriteLine("")
                    Console.WriteLine("was: " & ActualSearchWord & "かった")
                    Console.WriteLine("wasn't: " & ActualSearchWord & "くなかった")
                    Console.WriteLine()
                    Console.WriteLine("te-form: " & ActualSearchWord & "くて")
                    Console.WriteLine("te-form of negative: " & ActualSearchWord & "くなくて")

                    Console.WriteLine()
                    Console.BackgroundColor = ConsoleColor.DarkGray
                    Console.Write("i-adjective usage:")
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.WriteLine()
                    Console.WriteLine("子供の頃、私は" & ActualSearchWord & "かったです | When (I) was a kid, I was " & Scrap & ".")
                    Console.WriteLine("彼は" & ActualSearchWord & "くて頭が良かった | He was " & Scrap & " and smart.")
                    Console.WriteLine("これは" & ActualSearchWord & "くない | This is not " & Scrap & ".")
                    Console.WriteLine("その映画は" & ActualSearchWord & "くなかった | That film wasn't " & Scrap)
                    Console.WriteLine("パーティーは楽しくも" & ActualSearchWord & "くもなかった | The party was neither fun nor " & Scrap)
                End If
            End If
            If NaAdjective = True Or Noun = True Then
                'Checking if な exists at the end of the word so that it can be erased if it does exist
                If Right(ActualSearchWord, 1) = "な" Then
                    ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 1)
                End If
                If AdvancedParam <> 1 Or AdvancedParam <> 2 Then
                    If NaAdjective = True And Noun = False Then
                        Console.BackgroundColor = ConsoleColor.DarkGray
                        Console.Write("- na-adjective couplas -")
                        Console.BackgroundColor = ConsoleColor.Black
                    Else
                        Console.BackgroundColor = ConsoleColor.DarkGray
                        Console.Write("- na-adjective and noun couplas -")
                        Console.BackgroundColor = ConsoleColor.Black
                    End If

                    Console.WriteLine()
                    Console.WriteLine("Polite:")
                    Console.WriteLine("it is: " & ActualSearchWord & "です")
                    Console.WriteLine("is not: " & ActualSearchWord & "じゃありません")
                    Console.WriteLine("")
                    Console.WriteLine("it was: " & ActualSearchWord & "でした")
                    Console.WriteLine("was not: " & ActualSearchWord & "じゃありませんでした")

                    Console.WriteLine("")

                    Console.WriteLine("Informal")
                    Console.WriteLine("is: " & ActualSearchWord & "だ")
                    Console.WriteLine("isn't: " & ActualSearchWord & "じゃない")
                    Console.WriteLine("")
                    Console.WriteLine("was: " & ActualSearchWord & "だった")
                    Console.WriteLine("wasn't: " & ActualSearchWord & "じゃなかった")
                    Console.WriteLine()
                    Console.WriteLine("te-form: " & ActualSearchWord & "で")
                End If
            End If


            If NoAdjective = True Then
                If AdvancedParam <> 1 Or AdvancedParam <> 2 Then
                    'Important "titles" are highlighted to make reading the information easier
                    Console.BackgroundColor = ConsoleColor.DarkGray
                    Console.Write("No-adjective usage:")
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.WriteLine()

                    Console.WriteLine("これは" & ActualSearchWord & "の木です  | This is a tree of the " & Scrap & ".")
                    Console.WriteLine("これらは私たちの" & ActualSearchWord & "の伝統です | These are our family " & Scrap & "s.")
                    Console.WriteLine(ActualSearchWord & "の銀行に行きましょう！ | Let's go to the bank of " & Scrap & "!")
                End If
            End If
        End If


        'Adding the い back on to the end of the word (if it is an i-adjective)

        If Iadjective = True Then
            ActualSearchWord &= "い"
        End If

        'Example Sentence extraction:
        HTMLTemp = Client.DownloadString(New Uri(WordURL))
        SentenceExample = RetrieveClassRange(HTMLTemp, "<li class=" & QUOTE & "clearfix" & QUOTE & "><span class=" & QUOTE & "furigana" & QUOTE & ">", "inline_copyright", "Example Sentence") 'Firstly extracting the whole group
        If SentenceExample.Length > 10 Then
            Try
                Example = ExampleSentence(SentenceExample) 'This group then needs all the "fillers" taken out, that's what the ExampleSentence function does
            Catch ex As Exception
                If DebugMode = True Then
                    Console.WriteLine(ex.Message)
                    Console.ReadLine()
                End If
            End Try
        End If
        If Example.Length < 200 And Example.Length > 4 Then
            Console.WriteLine(Example.Trim)
        End If

        Dim KanjiBool As Boolean = False
        If AdvancedParam = 0 Then
            If PreferencesString(12) <> 0 Then
                KanjiBool = True
            End If
        End If

        If AdvancedParam <> 0 Or KanjiBool = True Then
            KanjiDisplay(ActualSearchWord, WordLink, SelectedDefinition, FoundTypes, 1, Furigana)
        Else
            Console.ReadLine()
        End If


        Main()
    End Sub
    Sub ConjugateVerb(ByRef PlainVerb, ByRef Type, ByRef Meaning, ByRef ComparativeType, ByVal S, ByVal SelectedDefinition, ByVal FoundTypes, ByVal Furigana)
        Const QUOTE = """"
        Dim Last As String = Right(PlainVerb, 1) 'This is the last Japanese character of the verb, this is used for changing forms

        'Making a Streamwriter to add to the file where you use "/back" to see last search instantly:


        Dim LastAdd As String = ""
        Dim LastAdd2 As String = ""
        Dim LastAddPot As String = ""
        Dim masuStem As String = ""
        Dim NegativeStem As String = ""
        Dim Potential As String = ""
        Dim Causative As String = ""
        Dim CausativePassive As String = ""
        Dim Conditional As String = ""
        Dim teStem As String = ""
        Dim Volitional As String = ""
        Dim Passive As String = ""
        Dim Imperative = ""

        'Removing [], AKA Extra info
        Try
            Meaning = Left(Meaning, Meaning.indexof("["))
        Catch
        End Try

        Dim NumberRemover As String = Right(Meaning, 2)
        If NumberRemover.IndexOf(".") <> -1 Then
            NumberRemover = Right(Meaning, 1)
        End If
        If IsNumeric(NumberRemover) = False Then
            NumberRemover = Right(Meaning, 1)
        End If

        If IsNumeric(NumberRemover) = True Then
            Meaning = Left(Meaning, Meaning.length - NumberRemover.Length)
        End If

        'This is the word with the "to" (in for example "to play") and without the bracketed context
        Dim PlainMeaning As String = Meaning.replace("to ", "")
        Dim Bracket1, Bracket2 As Integer
        Bracket1 = PlainMeaning.IndexOf("(")
        Do Until Bracket1 = -1
            If Bracket1 <> -1 Then
                Bracket2 = PlainMeaning.IndexOf(")")
                If Bracket2 = -1 Then
                    Bracket2 = PlainMeaning.Length
                End If
                If Bracket1 <> -1 Or Bracket2 <> -1 Then
                    PlainMeaning = PlainMeaning.Replace(Mid(PlainMeaning, Bracket1 + 1, Bracket2 - Bracket1 + 1), "")
                End If
            End If
            Bracket1 = PlainMeaning.IndexOf("(")
        Loop

        'Creating masu stems and volitional forms
        If Type = "Godan" Then
            If Last = "む" Then
                LastAdd = "み"
                LastAdd2 = "もう"
            End If
            If Last = "ぶ" Then
                LastAdd = "び"
                LastAdd2 = "ぼう"
            End If
            If Last = "ぬ" Then
                LastAdd = "に"
                LastAdd2 = "のう"
            End If
            If Last = "す" Then
                LastAdd = "し"
                LastAdd2 = "そう"
            End If
            If Last = "ぐ" Then
                LastAdd = "ぎ"
                LastAdd2 = "ごう"
            End If
            If Last = "く" Then
                LastAdd = "き"
                LastAdd2 = "こう"
            End If
            If Last = "る" Then
                LastAdd = "り"
                LastAdd2 = "ろう"
            End If
            If Last = "つ" Then
                LastAdd = "ち"
                LastAdd2 = "とう"
            End If
            If Last = "う" Then
                LastAdd = "い"
                LastAdd2 = "おう"
            End If
            masuStem = Left(PlainVerb, PlainVerb.length - 1) & LastAdd
            Volitional = Left(PlainVerb, PlainVerb.length - 1) & LastAdd2
        Else
            masuStem = Left(PlainVerb, PlainVerb.length - 1)
            Volitional = Left(PlainVerb, PlainVerb.length - 1) & "よう"
        End If

        'Creating negative stems (Last add) and Potential forms
        If Type = "Godan" Then
            If Last = "む" Then
                LastAdd = "ま"
                LastAddPot = "める"
            End If
            If Last = "ぶ" Then
                LastAdd = "ば"
                LastAddPot = "べる"
            End If
            If Last = "ぬ" Then
                LastAdd = "な"
                LastAddPot = "ねる"
            End If
            If Last = "す" Then
                LastAdd = "さ"
                LastAddPot = "せる"
            End If
            If Last = "ぐ" Then
                LastAdd = "が"
                LastAddPot = "げる"
            End If
            If Last = "く" Then
                LastAdd = "か"
                LastAddPot = "ける"
            End If
            If Last = "る" Then
                LastAdd = "ら"
                LastAddPot = "れる"
            End If
            If Last = "つ" Then
                LastAdd = "た"
                LastAddPot = "てる"
            End If
            If Last = "う" Then
                LastAdd = "わ"
                LastAddPot = "える"
            End If
            NegativeStem = Left(PlainVerb, PlainVerb.length - 1) & LastAdd
            Potential = Left(PlainVerb, PlainVerb.length - 1) & LastAddPot
            Causative = Left(PlainVerb, PlainVerb.length - 1) & LastAdd & "せる"
            CausativePassive = NegativeStem & "される"
            Conditional = Left(Potential, Potential.Length - 1) & "ば"
            Passive = Left(PlainVerb, PlainVerb.length - 1) & LastAdd & "れる"
            Imperative = Left(Potential, Potential.Length - 1)
        Else
            NegativeStem = Left(PlainVerb, PlainVerb.length - 1)
            Potential = Left(PlainVerb, PlainVerb.length - 1) & "られる"
            Causative = Left(PlainVerb, PlainVerb.length - 1) & "させる"
            CausativePassive = NegativeStem & "させられる"
            Conditional = Left(PlainVerb, PlainVerb.length - 1) & "れば"
            Passive = masuStem & "られる"
            Imperative = masuStem + "ろ"
        End If

        'Creating te-form stem of searched word
        If Type = "Godan" Then
            If Last = "む" Or Last = "ぶ" Or Last = "ぬ" Then
                LastAdd = "んで"
            End If
            If Last = "す" Then
                LastAdd = "して"
                CausativePassive = NegativeStem & "せられる" 'させられる but without first せられる
            End If
            If Last = "ぐ" Then
                LastAdd = "いで"
            End If
            If Last = "く" Then
                LastAdd = "いて"
            End If
            If Last = "る" Or Last = "つ" Or Last = "う" Then
                LastAdd = "って"
            End If
            teStem = Left(PlainVerb, PlainVerb.length - 1) & LastAdd
        Else
            teStem = Left(PlainVerb, PlainVerb.length - 1) & "て"
        End If

        If PlainVerb = "来る" Then
            Imperative = "来い"
        ElseIf PlainVerb = "する" Then
            Imperative = "しろ"
        End If

        'For ShortPastForm:
        Dim ShortPastEnding As String = Right(teStem, 1)
        If ShortPastEnding = "て" Then
            ShortPastEnding = "た"
        ElseIf ShortPastEnding = "で" Then
            ShortPastEnding = "だ"
        ElseIf Type = "Ichidan" Then
            ShortPastEnding = "た"
        Else
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Error: Short past tense")
            Console.ForegroundColor = ConsoleColor.White
        End If

        Dim Vowel1 As Boolean = False
        Dim Vowel2 As Boolean = False
        Dim Vowel3 As Boolean = False
        Dim VowelCheck1, VowelCheck2, VowelCheck3 As String
        If Right(PlainMeaning, 1) = " " Then
            PlainMeaning = Left(PlainMeaning, PlainMeaning.Length - 1)
        End If
        VowelCheck1 = Right(PlainMeaning, 1) 'Last letter
        VowelCheck2 = Mid(PlainMeaning, PlainMeaning.Length - 1, 1)
        VowelCheck3 = ""
        If PlainMeaning.Length > 2 Then
            VowelCheck3 = Mid(PlainMeaning, PlainMeaning.Length - 2, 1) 'second to last letter
        End If

        If VowelCheck1 = "a" Or VowelCheck1 = "e" Or VowelCheck1 = "i" Or VowelCheck1 = "o" Or VowelCheck1 = "u" Then
            Vowel1 = True 'True means the last letter is a vowel
        End If
        If VowelCheck2 = "a" Or VowelCheck2 = "e" Or VowelCheck2 = "i" Or VowelCheck2 = "o" Or VowelCheck2 = "u" Then
            Vowel2 = True 'True means the second to last letter is a vowel
        End If
        If VowelCheck3 = "a" Or VowelCheck3 = "e" Or VowelCheck3 = "i" Or VowelCheck3 = "o" Or VowelCheck3 = "u" Then
            Vowel3 = True 'True means the second to last letter is a vowel
        End If

        'Console.WriteLine(VowelCheck3 & " (3): " & Vowel3)
        'Console.WriteLine(VowelCheck2 & " (2): " & Vowel2)
        'Console.WriteLine(VowelCheck1 & " (1): " & Vowel1)

        Dim PastMeaning, PresentMeaning As String
        'Creating past tense words:
        If Vowel1 = False And Vowel2 = False Then
            PastMeaning = PlainMeaning & "ed"
        ElseIf Vowel1 = False And Vowel2 = True And Vowel3 = True Then
            PastMeaning = PlainMeaning & "ed"
        ElseIf Vowel1 = True Then
            PastMeaning = PlainMeaning & "d"
        ElseIf Vowel2 = False And VowelCheck1 = "y" Then
            PastMeaning = Left(PlainMeaning, PlainMeaning.Length - 1) & "ied"
        ElseIf VowelCheck1 = "y" Then
            PastMeaning = PlainMeaning & "ed"
        Else
            PastMeaning = PlainMeaning & "ed"
        End If

        'Creating present tense words:
        If VowelCheck1 = "e" Then
            PresentMeaning = Left(PlainMeaning, PlainMeaning.Length - 1) & "ing"
            'ElseIf Vowel1 = False And Vowel2 = True And Vowel3 = True Then
            '   PresentMeaning = Left(PlainMeaning, PlainMeaning.Length - 1) & "ing"
        ElseIf Vowel1 = False And Vowel2 = False Then
            PresentMeaning = PlainMeaning & "ing"
        ElseIf PlainMeaning.Length < 7 And Vowel1 = False Then
            PresentMeaning = PlainMeaning & "ing"
        Else
            PresentMeaning = PlainMeaning & "ing"
        End If

        'Fixing a few broken "double-word" definitions
        If PlainMeaning = "eat" Then
            PastMeaning = "ate"
        ElseIf PlainMeaning = "run" Then
            PastMeaning = "ran"
        ElseIf PlainMeaning = "put on weight" Then
            PastMeaning = "did put on weight"
            PresentMeaning = "putting on weight"
        ElseIf PlainMeaning = "become thin" Then
            PastMeaning = "became slim"
            PresentMeaning = "becoming slim"
        End If

        Dim PreferencesString(0) As String
        If S = 4 Then
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Formal:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("I " & PlainMeaning & ": " & masuStem & "ます")
            Console.WriteLine("I do not " & PlainMeaning & ":   " & masuStem & "ません")
            Console.WriteLine("I " & PastMeaning & ": " & masuStem & "ました")
            Console.WriteLine("I did not " & PlainMeaning & ":  " & masuStem & "ませんでした")
            Console.WriteLine()

            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Informal")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & ": " & PlainVerb)
            Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "ない")
            Console.WriteLine(Left(PastMeaning, 1).ToUpper & Right(PastMeaning, PastMeaning.Length - 1) & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding)
            Console.WriteLine("Didn't " & PlainMeaning & ": " & NegativeStem & "なかった")
            Console.WriteLine()

            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Te-forms:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Te-stem: " & teStem)
            Console.WriteLine("Is " & PresentMeaning & ": " & teStem & "いる")
            Console.WriteLine("Is " & PresentMeaning & ": " & teStem & "います")
            Console.WriteLine("Was " & PresentMeaning & ": " & teStem & "いた")
            Console.WriteLine("Was " & PresentMeaning & ": " & teStem & "いました")
            Console.WriteLine("Te-form of negative:")
            Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "なくて")
            Console.WriteLine("Negative te-form:")
            Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "ないで")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Volitional:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Let's " & PlainMeaning & " (polite): " & masuStem & "ましょう")
            Console.WriteLine("Let's " & PlainMeaning & " (casual): " & Volitional)
            Console.WriteLine("I've decided to " & PlainMeaning & ": " & Volitional & "と思っています")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Potential (Ability):")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Able to " & PlainMeaning & ": " & Potential)
            Console.WriteLine("Aren't able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "ない")
            Console.WriteLine("Have the ability to " & PlainMeaning & " (formal): " & PlainVerb & "ことができる")
            Console.WriteLine("Can you " & PlainMeaning & "?: " & Left(Potential, Potential.Length - 1) & "ますか？")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Causative (Being made to do something or letting it be done):")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Made to/allowed to " & PlainMeaning & ": " & Causative)
            Console.WriteLine("Causative with te-form: " & Left(Causative, Causative.Length - 1) & "て")
            Console.WriteLine("Not made to/allowed to " & PlainMeaning & ": " & Left(Causative, Causative.Length - 1) & "ない")
            Console.WriteLine("Causative-passive: " & CausativePassive)
            Console.WriteLine("Negative-causative passive: " & Left(CausativePassive, CausativePassive.Length - 1) & "ない")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Passive:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Plain passive: " & Passive)
            Console.WriteLine("Negative passive: " & Left(Passive, Passive.Length - 1) & "ない")


            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Conditional:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("If " & PlainMeaning & ": " & Conditional)
            Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なければ")
            Console.WriteLine("If " & PlainMeaning & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "ら")
            Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なかったら")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Want:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Want to " & PlainMeaning & ": " & masuStem & "たい")
            Console.WriteLine("Wanted to " & PlainMeaning & ": " & masuStem & "たかった")
            Console.WriteLine("Don't want to " & PlainMeaning & ": " & masuStem & "たくない")
            Console.WriteLine("Didn't want to " & PlainMeaning & ": " & masuStem & "たくなかった")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Having a try at something (see what is it like):")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("To try " & PresentMeaning & ": " & teStem & "みる")
            Console.WriteLine("Want to try " & PresentMeaning & ": " & teStem & "みたい")
            Console.WriteLine("Wanted to try " & PlainMeaning & ": " & teStem & "みたかった")
            Console.WriteLine("Want to be able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "たい")
            Console.WriteLine("Wanted to be able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "たかった")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Too much:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " too much: " & masuStem & "すぎる")
            Console.WriteLine("I " & PlainMeaning & " too much: " & masuStem & "すぎます")
            Console.WriteLine("Too much " & PresentMeaning & ": " & masuStem & "すぎること")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Do and don't need to:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Need to/should " & PlainMeaning & "　(very informal): " & NegativeStem & "なきゃ")
            Console.WriteLine("Need to/should " & PlainMeaning & ": " & NegativeStem & "なくちゃいけない")
            Console.WriteLine("Need to/should " & PlainMeaning & ": " & NegativeStem & "なければいけません")
            Console.WriteLine("Don't need to " & PlainMeaning & ": " & NegativeStem & "なくてもいい")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Extra:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Imperative: " & Imperative)
            Console.WriteLine("I intend to " & PlainMeaning & ": " & PlainVerb & "つもりです")
            Console.WriteLine("May " & PlainMeaning & ": " & PlainVerb & "かもしれない")
            Console.WriteLine("Why don't you " & PlainMeaning & " (informal): " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "らどうですか")
            Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " to prepare for something:" & teStem & "おく")
            Console.WriteLine("Must/should " & PlainMeaning & " to prepare for something: " & teStem & "おかなくちゃいけません")

        ElseIf S = 3 Then
            Console.WriteLine("Past tense: " & Left(teStem, teStem.Length - 1) & ShortPastEnding)
            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Te-forms:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "なくて")
            Console.WriteLine("Negative te-form:")
            Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "ないで")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Volitional:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Let's " & PlainMeaning & " (casual): " & Volitional)
            Console.WriteLine("I've decided to " & PlainMeaning & ": " & Volitional & "と思っています")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Potential (Ability):")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Able to " & PlainMeaning & ": " & Potential)
            Console.WriteLine("Aren't able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "ない")
            Console.WriteLine("Have the ability to " & PlainMeaning & " (formal): " & PlainVerb & "ことができる")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Causative (Being made to do something or letting it be done):")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Made to/allowed to " & PlainMeaning & ": " & Causative)
            Console.WriteLine("Not made to/allowed to " & PlainMeaning & ": " & Left(Causative, Causative.Length - 1) & "ない")
            Console.WriteLine("Causative with te-form: " & Left(Causative, Causative.Length - 1) & "て")
            Console.WriteLine("Causative-passive: " & CausativePassive)

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Passive:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Plain passive: " & Passive)
            Console.WriteLine("Negative passive: " & Left(Passive, Passive.Length - 1) & "ない")


            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Conditional:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("If " & PlainMeaning & ": " & Conditional)
            Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なければ")
            Console.WriteLine()
            Console.WriteLine("If " & PlainMeaning & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "ら")
            Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なかったら")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Want:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Want to " & PlainMeaning & ": " & masuStem & "たい")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Extras:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Imperative: " & Imperative)
            Console.WriteLine("Want to try " & PresentMeaning & ": " & teStem & "みたい")
            Console.WriteLine("Want to be able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "たい")
            Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " too much: " & masuStem & "すぎる")
            Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " to prepare for something: " & teStem & "おく")
        ElseIf S = 2 Then
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Volitional:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Let's " & PlainMeaning & " (casual): " & Volitional)

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Potential (Ability):")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Able to " & PlainMeaning & ": " & Potential)
            Console.WriteLine("Have the ability to " & PlainMeaning & " (formal): " & PlainVerb & "ことができる")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Causative (Being made to do something or letting it be done):")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Made to/allowed to " & PlainMeaning & ": " & Causative)
            Console.WriteLine("Causative-passive: " & CausativePassive)

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Passive:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Plain passive: " & Passive)
            Console.WriteLine("Negative passive: " & Left(Passive, Passive.Length - 1) & "ない")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Conditional:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("If " & PlainMeaning & ": " & Conditional)
            Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なければ")
            Console.WriteLine()
            Console.WriteLine("If " & PlainMeaning & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "ら")
            Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なかったら")

            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Important:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Te-stem: " & teStem)
            Console.WriteLine("Negative: " & NegativeStem & "ない")
            Console.WriteLine("Imperative: " & Imperative)
        ElseIf S = 1 Then
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Important:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Te-stem: " & teStem)
            Console.WriteLine("Negative: " & NegativeStem & "ない")
        ElseIf S = 0 Then
            Dim PreferenceReader As String = ""
            'Dim PreferencesString(0) As String 'this is dimmed somewhere else so that it isn't in an if statement
            Try
                PreferenceReader = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\Preferences\SParameter.txt")
            Catch
                Console.Clear()
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("You haven't got a custom S parameter set up.")
                Console.WriteLine("To do this, use the '/pref' command.")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()
                Main()
            End Try

            PreferencesString = PreferenceReader.Split(vbCrLf)

            Dim SRead As Boolean = False
            Dim Index As Integer = 0
            Do Until SRead = True Or Index = PreferencesString.Length - 1
                PreferencesString(Index) = PreferencesString(Index).Trim

                Try
                    PreferencesString(Index) = Right(PreferencesString(Index), 1)
                Catch
                    Array.Resize(PreferencesString, PreferencesString.Length - 1)
                    SRead = True
                End Try

                Index += 1
            Loop

            'PreferencesString()
            'Text file to array positions:
            'Formal:0
            'Informal:1
            'Te-form: 2
            'Volitional:3
            'Potential:4
            'Causative:5
            'Passive:6            <-------
            'Conditional:7
            'Want:8
            'Need to:9
            'Extras:10
            'Noun/adj:11
            'Kanji details:12

            If PreferencesString(0) = 3 Then
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Formal:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("I " & PlainMeaning & ": " & masuStem & "ます")
                Console.WriteLine("I do not " & PlainMeaning & ":   " & masuStem & "ません")
                Console.WriteLine("I " & PastMeaning & ": " & masuStem & "ました")
                Console.WriteLine("I did not " & PlainMeaning & ":  " & masuStem & "ませんでした")
                Console.WriteLine("masu-stem: " & masuStem)
            ElseIf PreferencesString(0) = 2 Then
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Formal:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("I " & PlainMeaning & ": " & masuStem & "ます")
                Console.WriteLine("I do not " & PlainMeaning & ":   " & masuStem & "ません")
                Console.WriteLine("I " & PastMeaning & ": " & masuStem & "ました")
                Console.WriteLine("I did not " & PlainMeaning & ":  " & masuStem & "ませんでした")
            ElseIf PreferencesString(0) = 1 Then
                Console.WriteLine("Masu-stem: " & masuStem)
            End If

            If PreferencesString(1) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Informal")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & ": " & PlainVerb)
                Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "ない")
                Console.WriteLine(Left(PastMeaning, 1).ToUpper & Right(PastMeaning, PastMeaning.Length - 1) & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding)
                Console.WriteLine("Didn't " & PlainMeaning & ": " & NegativeStem & "なかった")
                Console.WriteLine("Negative stem:" & NegativeStem)
            ElseIf PreferencesString(1) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Informal")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & ": " & PlainVerb)
                Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "ない")
                Console.WriteLine(Left(PastMeaning, 1).ToUpper & Right(PastMeaning, PastMeaning.Length - 1) & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding)
                Console.WriteLine("Didn't " & PlainMeaning & ": " & NegativeStem & "なかった")
                Console.WriteLine()
            ElseIf PreferencesString(1) = 1 Then
                If PreferencesString(0) <> 1 Then
                    Console.WriteLine()
                End If
                Console.WriteLine("Negative stem:" & NegativeStem)
            End If

            If PreferencesString(2) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Te-forms:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Te-stem: " & teStem)
                Console.WriteLine("Is " & PresentMeaning & ": " & teStem & "いる")
                Console.WriteLine("Is " & PresentMeaning & ": " & teStem & "います")
                Console.WriteLine("Te-form of negative:")
                Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "なくて")
                Console.WriteLine("Negative te-form:")
                Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "ないで")
            ElseIf PreferencesString(2) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Te-forms:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Te-form: " & teStem)
                Console.WriteLine("Is " & PresentMeaning & ": " & teStem & "いる")
                Console.WriteLine("Te-form of negative: " & NegativeStem & "なくて")
                Console.WriteLine("Negative te-form: " & NegativeStem & "ないで")
            ElseIf PreferencesString(2) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Te-forms:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Te-form of negative: " & NegativeStem & "なくて")
                Console.WriteLine("Negative te-form: " & NegativeStem & "ないで")
            End If

            If PreferencesString(3) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Volitional:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Let's " & PlainMeaning & " (polite): " & masuStem & "ましょう")
                Console.WriteLine("Let's " & PlainMeaning & " (casual): " & Volitional)
                Console.WriteLine("I've decided to " & PlainMeaning & ": " & Volitional & "と思っています")
            ElseIf PreferencesString(3) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Volitional:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Let's " & PlainMeaning & " (polite): " & masuStem & "ましょう")
                Console.WriteLine("Let's " & PlainMeaning & " (casual): " & Volitional)
            ElseIf PreferencesString(3) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Volitional:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Let's " & PlainMeaning & " (casual): " & Volitional)
            End If

            If PreferencesString(4) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Potential (Ability):")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Able to " & PlainMeaning & ": " & Potential)
                Console.WriteLine("Aren't able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "ない")
                Console.WriteLine("Have the ability to " & PlainMeaning & " (formal): " & PlainVerb & "ことができる")
            ElseIf PreferencesString(4) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Potential (Ability):")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Able to " & PlainMeaning & ": " & Potential)
                Console.WriteLine("Aren't able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "ない")
            ElseIf PreferencesString(4) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Potential (Ability):")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Able to " & PlainMeaning & ": " & Potential)
            End If

            If PreferencesString(5) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Causative (Being made to do something or letting it be done):")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Made to/allowed to " & PlainMeaning & ": " & Causative)
                Console.WriteLine("Causative with te-form: " & Left(Causative, Causative.Length - 1) & "て")
                Console.WriteLine("Not made to/allowed to " & PlainMeaning & ": " & Left(Causative, Causative.Length - 1) & "ない")
                Console.WriteLine("Causative-passive: " & CausativePassive)
                Console.WriteLine("Negative-causative passive: " & Left(CausativePassive, CausativePassive.Length - 1) & "ない")
            ElseIf PreferencesString(5) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Causative (Being made to do something or letting it be done):")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Made to/allowed to " & PlainMeaning & ": " & Causative)
                Console.WriteLine("Not made to/allowed to " & PlainMeaning & ": " & Left(Causative, Causative.Length - 1) & "ない")
                Console.WriteLine("Negative passive: " & Left(Passive, Passive.Length - 1) & "ない")
                Console.WriteLine("Causative-passive: " & CausativePassive)
            ElseIf PreferencesString(5) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Causative (Being made to do something or letting it be done):")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Causative " & PlainMeaning & ": " & Causative)
                Console.WriteLine("Causative passive: " & Left(Causative, Passive.Length - 1) & "られる")
            End If

            If PreferencesString(6) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Passive:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Plain passive: " & Passive)
                Console.WriteLine("Negative passive: " & Left(Passive, Passive.Length - 1) & "ない")
            ElseIf PreferencesString(6) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Passive:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Plain passive: " & Passive)
                Console.WriteLine("Negative passive: " & Left(Passive, Passive.Length - 1) & "ない")
            ElseIf PreferencesString(6) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Passive:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Plain passive: " & Passive)
            End If

            If PreferencesString(7) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Conditional:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("If " & PlainMeaning & ": " & Conditional)
                Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なければ")
                Console.WriteLine("If " & PlainMeaning & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "ら")
                Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なかったら")
            ElseIf PreferencesString(7) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Conditional:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("If " & PlainMeaning & ": " & Conditional)
                Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なければ")
                Console.WriteLine("If " & PlainMeaning & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "ら")
            ElseIf PreferencesString(7) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Conditional:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("If " & PlainMeaning & ": " & Conditional)
                Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なければ")
            End If

            If PreferencesString(8) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Want:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Want to " & PlainMeaning & ": " & masuStem & "たい")
                Console.WriteLine("Wanted to " & PlainMeaning & ": " & masuStem & "たかった")
                Console.WriteLine("Don't want to " & PlainMeaning & ": " & masuStem & "たくない")
                Console.WriteLine("Didn't want to " & PlainMeaning & ": " & masuStem & "たくなかった")
                Console.WriteLine("Want to try " & PresentMeaning & ": " & teStem & "みたい")
                Console.WriteLine("Want to be able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "たい")
            ElseIf PreferencesString(8) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Want:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Want to " & PlainMeaning & ": " & masuStem & "たい")
                Console.WriteLine("Don't want to " & PlainMeaning & ": " & masuStem & "たくない")
                Console.WriteLine("Want to try " & PresentMeaning & ": " & teStem & "みたい")
                Console.WriteLine("Want to be able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "たい")
            ElseIf PreferencesString(8) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Want:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Want to try " & PresentMeaning & ": " & teStem & "みたい")
                Console.WriteLine("Want to be able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "たい")
            End If

            If PreferencesString(9) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Do and don't need to:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Need to/should " & PlainMeaning & "　(very informal): " & NegativeStem & "なきゃ")
                Console.WriteLine("Need to/should " & PlainMeaning & ": " & NegativeStem & "なくちゃいけない")
                Console.WriteLine("Need to/should " & PlainMeaning & ": " & NegativeStem & "なければいけません")
                Console.WriteLine("Don't need to " & PlainMeaning & ": " & NegativeStem & "なくてもいい")
            ElseIf PreferencesString(9) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Do and don't need to:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Need to/should " & PlainMeaning & "　(very informal): " & NegativeStem & "なきゃ")
                Console.WriteLine("Need to/should " & PlainMeaning & ": " & NegativeStem & "なくちゃいけない")
                Console.WriteLine("Don't need to " & PlainMeaning & ": " & NegativeStem & "なくてもいい")
            ElseIf PreferencesString(9) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Do and don't need to:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Need to/should " & PlainMeaning & ": " & NegativeStem & "なくちゃいけない")
                Console.WriteLine("Don't need to " & PlainMeaning & ": " & NegativeStem & "なくてもいい")
            End If

            If PreferencesString(10) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Extras:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " too much: " & masuStem & "すぎる")
                Console.WriteLine("Too much " & PresentMeaning & ": " & masuStem & "すぎること")
                Console.WriteLine("I intend to " & PlainMeaning & ": " & PlainVerb & "つもりです")
                Console.WriteLine("May " & PlainMeaning & ": " & PlainVerb & "かもしれない")
                Console.WriteLine("Why don't you " & PlainMeaning & " (informal): " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "らどうですか")
                Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " to prepare for something:" & teStem & "おく")
                Console.WriteLine("Must/should " & PlainMeaning & " to prepare for something: " & teStem & "おかなくちゃいけません")
            ElseIf PreferencesString(10) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Extras:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " too much: " & masuStem & "すぎる")
                Console.WriteLine("Too much " & PresentMeaning & ": " & masuStem & "すぎること")
                Console.WriteLine("May " & PlainMeaning & ": " & PlainVerb & "かもしれない")
                Console.WriteLine("Why don't you " & PlainMeaning & " (informal): " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "らどうですか")
                Console.WriteLine("Must/should " & PlainMeaning & " to prepare for something: " & teStem & "おかなくちゃいけません")
            ElseIf PreferencesString(10) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Extras:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("May " & PlainMeaning & ": " & PlainVerb & "かもしれない")
                Console.WriteLine("Must/should " & PlainMeaning & " to prepare for something: " & teStem & "おかなくちゃいけません")
            End If
        End If

        'Writing search history results into the PreviousSearch file:
        WriteToFile("", "LastSearch")
        WriteToFile("Volitional:", "LastSearch")
        WriteToFile("Let's " & PlainMeaning & " (polite): " & masuStem & "ましょう", "LastSearch")
        WriteToFile("Let's " & PlainMeaning & " (casual): " & Volitional, "LastSearch")

        WriteToFile("", "LastSearch")
        WriteToFile("Potential (Ability):", "LastSearch")
        WriteToFile("Able to " & PlainMeaning & ": " & Potential, "LastSearch")

        WriteToFile("", "LastSearch")
        WriteToFile("Causative (Being made to do something or letting it be done):", "LastSearch")
        WriteToFile("Made to/allowed to " & PlainMeaning & ": " & Causative, "LastSearch")
        WriteToFile("Causative-passive: " & CausativePassive, "LastSearch")

        WriteToFile("", "LastSearch")
        WriteToFile("Passive:", "LastSearch")
        WriteToFile("Plain passive: " & Passive, "LastSearch")
        WriteToFile("Negative passive: " & Left(Passive, Passive.Length - 1) & "ない", "LastSearch")

        WriteToFile("", "LastSearch")
        WriteToFile("Conditional:", "LastSearch")
        WriteToFile("If " & PlainMeaning & ": " & Conditional, "LastSearch")
        WriteToFile("If don't " & PlainMeaning & ": " & NegativeStem & "なければ", "LastSearch")
        WriteToFile("", "LastSearch")
        WriteToFile("If " & PlainMeaning & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "ら", "LastSearch")
        WriteToFile("If don't " & PlainMeaning & ": " & NegativeStem & "なかったら", "LastSearch")

        WriteToFile("", "LastSearch")
        WriteToFile("Important:", "LastSearch")
        WriteToFile("Te-stem: " & teStem, "LastSearch")
        WriteToFile("Negative: " & NegativeStem & "ない", "LastSearch")
        WriteToFile("Imperative: " & Imperative, "LastSearch")

        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8

        Try
            If PreferencesString(10) > 1 Then
                Dim WordURL As String = ("https://jisho.org/search/" & PlainVerb & "%20%23sentences")
                Dim HTML As String = Client.DownloadString(New Uri(WordURL))

                'Example sentence extraction -----
                Dim Example As String = ""
                Dim SentenceExample As String
                SentenceExample = RetrieveClassRange(HTML, "<li class=" & QUOTE & "clearfix" & QUOTE & ">", "inline_copyright", "Sentence Example") 'Firstly extracting the whole group
                If SentenceExample.Length > 10 Then
                    Example = ExampleSentence(SentenceExample) 'This group then needs all the "fillers" taken out, that's what the ExampleSentence function does
                End If
                If Example.Length < 100 And Example.Length > 5 Then
                    Console.WriteLine(Example.Trim)
                End If
            End If
        Catch
        End Try

        Dim KanjiBool As Boolean = False
        If S = 0 Then
            If PreferencesString(12) <> 0 Then
                KanjiBool = True
            End If
        End If

        If S <> 0 Or KanjiBool = True Then
            KanjiDisplay(PlainVerb, "jisho.org/word/", SelectedDefinition, FoundTypes, 1, Furigana)
        Else
            Console.ReadLine()
        End If

        'Console.ReadLine()
        Main()
    End Sub
    Sub TranslateSentence(ByVal Sentence)
        Const QUOTE = """"
        '彼は銀行に行くことが難しいと思ってしまいます。

        Dim WordURL As String = "www.romajidesu.com/translator/" & Sentence
        Dim Client As New WebClient
        Dim WordGroups() As String
        Dim Translate1 As String
        Client.Encoding = System.Text.Encoding.UTF8
        Dim HTML As String = ""
        Try
            HTML = Client.DownloadString(New Uri("http://" & WordURL))
        Catch
        End Try

        If HTML.Length < 100 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Something went wrong.")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End If

        HTML = Mid(HTML, HTML.IndexOf("Original Japanese sentence"), HTML.Length - (HTML.Length - HTML.IndexOf("Translated Romaji/Kana")) - HTML.IndexOf("Original Japanese sentence")).Trim
        HTML = HTML.Replace(vbLf, "").Replace("       ", "").Replace("    ", "").Replace("   ", "") 'Cleaning up the HTML so it is easier to read and scrap from. Yes, I probably should have done this for the other web scraping.
        WordGroups = Split(HTML, "bf=")

        ' Console.WriteLine("Debug?")
        'If Console.ReadLine = "1" Then
        'For Printer = 0 To WordGroups.Length - 1
        'Console.WriteLine(WordGroups(Printer) & "__")
        'Console.WriteLine()
        'Console.WriteLine()
        'Console.WriteLine()
        'Console.WriteLine()
        'Next
        'End If

        Console.Clear()
        Console.WriteLine("Sentence breakdown:")
        Console.WriteLine()
        Console.WriteLine(Sentence)

        Translate1 = GTranslate(Sentence, "ja", "en")
        Console.WriteLine(Translate1)
        Console.WriteLine()

        Dim FoundInfo, WriteWord, CurrentWord As String
        Dim WriteWord2 = ""
        Dim Snip1, Snip2 As Integer
        Dim Definition(0) As String
        Dim Furigana As String

        For I = 1 To WordGroups.Length - 1
            'Snipping the "base form"
            Snip2 = Mid(WordGroups(I), 2).IndexOf(QUOTE) - 1
            FoundInfo = Mid(WordGroups(I), 2, Snip2 + 1)
            WriteWord = FoundInfo
            CurrentWord = FoundInfo

            'Getting the definition:
            Definition(I - 1) = DefinitionScraper("jisho.org/search/" & CurrentWord)
            Snip1 = Definition(I - 1).IndexOf(";")
            If Snip1 <> -1 Then
                Definition(I - 1) = Left(Definition(I - 1), Snip1)
            End If
            Snip1 = Definition(I - 1).IndexOf("(")
            If Snip1 <> -1 Then
                Definition(I - 1) = Left(Definition(I - 1), Snip1)
            End If

            'Snipping furigana:
            Snip1 = WordGroups(I).IndexOf("_blank") + 9
            WordGroups(I) = Mid(WordGroups(I), Snip1)

            Snip2 = WordGroups(I).IndexOf(" ")
            WriteWord = Left(WordGroups(I), Snip2).Replace("<div", "")

            Snip2 = WordGroups(I).IndexOf("<br/>")
            Furigana = Left(WordGroups(I), Snip2)
            Snip1 = Furigana.LastIndexOf(" ") + 1
            Furigana = Mid(Furigana, Snip1).Trim

            'Snipping the <i> info 1
            Snip1 = WordGroups(I).IndexOf("<i>") + 4 '+4 because that is the length of "<i>"
            Snip2 = WordGroups(I).IndexOf("</i>")
            FoundInfo = Mid(WordGroups(I), Snip1, Snip2 + 1 - Snip1)

            WriteWord = FoundInfo & " " & WriteWord 'word type + word
            If Furigana.Length > 0 Then
                WriteWord = WriteWord & " (" & Furigana & ")" '(word type + word) + furigana
            End If


            Try
                Snip1 = WordGroups(I).IndexOf("particle") + 5
                WordGroups(I) = Mid(WordGroups(I), Snip1)

                'Snipping the helping particle
                Snip1 = WordGroups(I).IndexOf(">") + 1 '+2 because that is the length of ">"
                Snip2 = WordGroups(I).IndexOf("<")
                WriteWord2 = Mid(WordGroups(I), Snip1 + 1, Snip2 - Snip1)
                WriteWord2 = WriteWord2.Replace(CurrentWord, "")
            Catch
                WriteWord2 = ""
            End Try

            Try
                'Snipping the second <i> (if it exists):
                Snip1 = WordGroups(I).IndexOf("</i>") + 3 'I probably could be 4, but using 3 to be safe
                WordGroups(I) = Mid(WordGroups(I), Snip1)

                Snip1 = WordGroups(I).IndexOf("<i>") + 4 '+4 because that is the length of "<i>"
                Snip2 = WordGroups(I).IndexOf("</i>")
                FoundInfo = Mid(WordGroups(I), Snip1, Snip2 + 1 - Snip1)
                If FoundInfo.IndexOf("symbol") = -1 Then
                    WriteWord2 = WriteWord2 & FoundInfo 'Adding the word type before the word
                End If

                Try 'Snipping the third <i> (if it exists):
                    Snip1 = WordGroups(I).IndexOf("</i>") + 3 'I probably could be 4, but using 3 to be safe
                    WordGroups(I) = Mid(WordGroups(I), Snip1)

                    Snip1 = WordGroups(I).IndexOf("<i>") + 4 '+4 because that is the length of "<i>"
                    Snip2 = WordGroups(I).IndexOf("</i>")
                    FoundInfo = Mid(WordGroups(I), Snip1, Snip2 + 1 - Snip1)

                    If FoundInfo.IndexOf("symbol") = -1 Then
                        WriteWord2 = WriteWord2 & FoundInfo 'Adding the word type before the word
                    End If
                Catch
                End Try

            Catch
            End Try

            If WordGroups.Length = 1 Or WordGroups(0).Length < 2 Then
                Console.WriteLine("Trying again.")
                Threading.Thread.Sleep(1000)
                Console.Clear()
                TranslateSentence(Sentence)
            End If

            WriteWord = WriteWord.Trim
            WriteWord2 = WriteWord2.Trim
            If Left(WriteWord2, 2) = ", " Then
                WriteWord2 = Mid(WriteWord2, 3)
            End If

            If WriteWord2 <> "" Then
                Console.WriteLine(WriteWord & " (" & WriteWord2 & "): " & Definition(I - 1))
            Else
                Console.WriteLine(WriteWord & ": " & Definition(I - 1))
            End If
            'Console.WriteLine()
            WriteWord2 = ""


            Array.Resize(Definition, Definition.Length + 1)
        Next
        Array.Resize(Definition, Definition.Length - 1)

        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.DarkGray
        Console.WriteLine("Note: Entering ungrammatical nonsense leads to weird results.")
        Console.ForegroundColor = ConsoleColor.White
        Console.ReadLine()
        Main()
    End Sub
    Sub ReadingPractice(ByRef Sentences)
        Dim StartLine As Integer
        Dim EndLine As Integer
        Dim SArray(0) As String
        Dim Cut As String = " "
        Dim I As Integer = 0
        Dim LastLine As Integer 'This is for making sure that all the sentences fit in the maximum paste limit
        Dim Two As Boolean = False
        Dim Sentences2 As String

        If Right(Sentences, 2) = " 2" Then
            Two = True
        End If

        If Sentences.length > 2000 Then 'Making sure that all the sentences fit in the maximum paste limit
            LastLine = Sentences.LastIndexOf("|")
            If LastLine = -1 Then
                Console.WriteLine("Formatting error")
                Console.ReadLine()
                Main()
            End If
            Sentences = Left(Sentences, LastLine)
        End If
        Console.Clear()

        If Two = True Then 'if the parameter "2" is at the end of the command "/c"
            Console.WriteLine("Enter next set:")
            Sentences2 = Console.ReadLine

            If Sentences2.Length > 2000 Then 'Making sure that all the sentences fit in the maximum paste limit
                LastLine = Sentences2.LastIndexOf("|")
                If LastLine = -1 Then
                    Console.WriteLine("Formatting error")
                    Console.ReadLine()
                    Main()
                End If
                Sentences2 = Left(Sentences2, LastLine)
            End If

            Sentences = Sentences & Sentences2

        End If

        Console.WriteLine("Loading...")
        Do Until Cut.Length = 0 'Do until no more end splitters can be found. The sentences that are pasted won't end in "|" because of how the AHK sentence grabber works
            StartLine = Sentences.IndexOf("|")
            If StartLine = -1 Then
                Console.WriteLine("Sorry this is in the wrong format.")
                Main()
            End If
            EndLine = Mid(Sentences, 3).IndexOf("|") + 1 '+1 because we are checking for the second line, the second line is the same as the first line so we need to ignore the first line when looking for the second line, we then add one because we ignored the first character (which will always be the first line)
            If EndLine = -1 Then
                Console.WriteLine("Sorry this is in the wrong format.")
                Main()
            End If
            Try
                Cut = Mid(Sentences, StartLine + 1, EndLine) 'Startline + 1 because you cannot have it as 0
            Catch
                Console.WriteLine("Something went wrong")
                Main()
            End Try
            If Cut.Length = 0 Then 'Making sure the loop is existed if there are no more cuts that need being made
                Continue Do
            End If

            If Cut.Length <= 100 Then
                SArray(I) = Mid(Cut, 2, Cut.Length - 1)
                Array.Resize(SArray, SArray.Length + 1)
            End If

            Sentences = Sentences.Replace(Cut, "")

            If Cut.Length <= 100 Then
                I += 1
            End If
        Loop
        Array.Resize(SArray, SArray.Length - 1) 'Just because it works, don't question I tested and stuck with it

        If SArray.Length = 0 Then
            Console.WriteLine("Something went wrong")
            Main()
        End If
        Console.Clear()

        Dim JP As String
        Dim ENG As String
        Dim Check As String
        For Read = 0 To SArray.Length - 1
            EndLine = SArray(Read).IndexOf("^")
            If EndLine = -1 Then
                Console.WriteLine("Sorry this is in the wrong format.")
                Main()
            End If
            JP = Mid(SArray(Read), 1, EndLine)

            Console.WriteLine(Read + 1 & "/" & SArray.Length)
            Console.WriteLine(JP)
            Check = Console.ReadLine()
            If Check = "b" And Read > 0 Or Check = "B" And Read > 0 Then
                Read -= 2
                Console.Clear()
                Continue For 'This lets you go back a sentence if for some reason you want to
            End If
            If Check = "0" Or (Check.ToLower).IndexOf("main") <> -1 Or (Check.ToLower).IndexOf("menu") <> -1 Or (Check.ToLower).IndexOf("stop") <> -1 Or (Check.ToLower).IndexOf("back") <> -1 Then
                Console.WriteLine("Stopped") 'This is to stop and go back to menu
                Console.ReadLine()
                Main()
            End If
            ENG = Mid(SArray(Read), EndLine + 2, SArray(Read).Length - EndLine - 1)
            Console.WriteLine(ENG)
            Check = Console.ReadLine()
            If Check = "b" And Read > 0 Or Check = "B" And Read > 0 Then
                Read -= 2
                Console.Clear()
                Continue For 'This lets you go back a sentence if for some reason you want to
            End If
            If Check = "0" Or (Check.ToLower).IndexOf("main") <> -1 Or (Check.ToLower).IndexOf("menu") <> -1 Or (Check.ToLower).IndexOf("stop") <> -1 Or (Check.ToLower).IndexOf("back") <> -1 Then
                Console.WriteLine("Stopped") 'This is to stop and go back to menu
                Console.ReadLine()
                Main()
            End If
            Console.Clear()
        Next

        Console.WriteLine("Finished!")
        Console.ReadLine()
        Main()
    End Sub
    Sub ConjugationPractice(ByVal Word)
        Console.Clear()
        Const QUOTE = """"

        Dim WordURL As String = ("https://jisho.org/search/" & Word)
        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8
        Dim HTML As String = ""
        HTML = Client.DownloadString(New Uri(WordURL))

        Dim HTMLTemp As String = HTML

        Dim ActualSearchWord As String = ""
        Dim ActualSearch1stAppearance As Integer = 0
        Dim ActualSearch2ndAppearance As Integer = 0
        Dim WordLink As String = ""
        Dim Definition As String
        Dim Max As Integer = 3

        Try
            ActualSearchWord = RetrieveClassRange(HTMLTemp, "<span class=" & QUOTE & "text" & QUOTE & ">", "</div>", "Actual word search")
            ActualSearchWord = Mid(ActualSearchWord, 30)
            ActualSearchWord = ActualSearchWord.Replace("<span>", "")
            ActualSearchWord = ActualSearchWord.Replace("</span>", "")
            ActualSearchWord = ActualSearchWord.Trim

            'Getting the link of the actual word:
            ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & QUOTE & "text" & QUOTE & ">")
            HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
            ActualSearch1stAppearance = HTMLTemp.IndexOf("meanings-wrapper") 'used to be "concept_light clearfix"
            HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
            ActualSearch1stAppearance = HTMLTemp.IndexOf("jisho.org/word/")
            ActualSearch2ndAppearance = Mid(HTMLTemp, HTMLTemp.IndexOf("jisho.org/word/")).IndexOf(QUOTE & ">")
            WordLink = Mid(HTMLTemp, ActualSearch1stAppearance + 1, ActualSearch2ndAppearance - 1)
        Catch
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Word couldn't be found")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End Try

        If ActualSearchWord.IndexOf("<") <> -1 Or ActualSearchWord.IndexOf(">") <> -1 Or ActualSearchWord.IndexOf("span") <> -1 Then
            ActualSearchWord = RetrieveClassRange(HTML, "<div class=" & QUOTE & "concept_light clearfix" & QUOTE & ">", "</div>", "Actual word search")
            ActualSearchWord = RetrieveClassRange(ActualSearchWord, "text", "</span", "Actual word search")
            ActualSearchWord = Mid(ActualSearchWord, 17)
            ActualSearchWord = ActualSearchWord.Replace("<span>", "")
            ActualSearchWord = ActualSearchWord.Replace("</span>", "")
        End If

        If ActualSearchWord.Length = 0 Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Word couldn't be found")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End If
        Definition = DefinitionScraper(WordLink)

        Dim StartingHTML As Integer
        StartingHTML = HTML.IndexOf(ActualSearchWord)
        HTMLTemp = Mid(HTML, StartingHTML)

        Dim TypeSnip As String = RetrieveClass(HTMLTemp, "meaning-tags", "TypeSnip") 'This is retrieving the word types
        If TypeSnip = "Other forms" Then
            TypeSnip = "Phrase:"
        End If

        'Extracting furigana and then snipping from the start up to the furigana
        Dim Furigana As String = ""
        Dim FuriganaStart As Integer
        Furigana = RetrieveClassRange(HTMLTemp, "</a></li><li><a", "</a></li><li><a href=" & QUOTE & "//jisho.org", "Furigana")
        If Furigana.Length < 600 And Furigana.Length <> 0 Then
            FuriganaStart = Furigana.IndexOf("for")
            Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 4)

            FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
            If FuriganaStart <> -1 Then 'if </a></li><li> Is found Then it will be removed as well as everything after it
                Furigana = Left(Furigana, FuriganaStart)
            End If
        Else
            Furigana = ""
        End If

        Definition = Left(Definition, 1).ToUpper & Right(Definition, Definition.Length - 1) 'This is capitalising the first letter and then having the rest as normal

        Dim Bracket1, Bracket2 As Integer
        Bracket1 = Definition.IndexOf("(")
        Do Until Bracket1 = -1 'This is getting rid of brackets in the definition
            If Bracket1 <> -1 Then
                Bracket2 = Definition.IndexOf(")")

                If Bracket1 <> -1 Or Bracket2 <> -1 Then
                    Definition = Definition.Replace(Mid(Definition, Bracket1 + 1, Bracket2 - Bracket1 + 1), "")
                End If
            End If
            Bracket1 = Definition.IndexOf("(")
        Loop

        Dim Type As String = "" 'This is for making sure the verb is conjugated properly
        Dim Verb As Boolean
        Verb = False
        If TypeSnip.IndexOf("verb") <> -1 And (TypeSnip.ToLower).IndexOf("suru") = -1 Then 'If a verb (and not a suru verb)
            Verb = True
            If TypeSnip.IndexOf("Godan verb") <> -1 Then
                Type = "Godan"
            ElseIf TypeSnip.IndexOf("Ichidan verb") <> -1 Then
                Type = "Ichidan"
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Error: Couldn't identify verb")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()
                Main()
            End If
        End If

        If Verb = False Then
            Console.Clear()
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("This word type isn't supported, sorry!")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadLine()
            Main()
        End If

        Console.Clear()
        Console.WriteLine("Conjugation practice for " & ActualSearchWord & "(" & Furigana & "): " & Definition)
        Console.WriteLine(TypeSnip)
        Console.WriteLine()
        Console.WriteLine("Press enter to start, or type 'back' to go back.")
        If Console.ReadLine().IndexOf("a") <> -1 Then
            Main()
        End If
        Console.Clear()

        Dim Last As String = Right(ActualSearchWord, 1) 'This is the last Japanese character of the verb, this is used for changing forms
        Dim LastAdd As String = ""
        Dim LastAdd2 As String = ""
        Dim Forms(3, 3) As String 'This is an array that will hold various forms with the following order:

        If Verb = True Then
            'Forms(X, Y) x = form, y = type of info
            'y1 = correct conjugation
            'y2 = question
            'y3 = furigana conjugation

            '0 = potential
            '1 = passive
            '2 = causative
            '3 = causative passive
            Forms(0, 2) = "potential"
            Forms(1, 2) = "passive"
            Forms(2, 2) = "causative"
            Forms(3, 2) = "causative passive"


            'Forms(0, 2) = "masu form"
            'Forms(1, 2) = "te-form"
            'Forms(2, 2) = "short past"
            'Forms(3, 2) = "past te-iru form"
            'Forms(4, 2) = "tai/want form"
            'Forms(5, 2) = "short negative"
            'Forms(6, 2) = "short past negative"
            'Forms(7, 2) = "te-form of negative"
            'Forms(8, 2) = "negative te-form"
            'Forms(9, 2) = "negative tai/want form"
            Console.WriteLine("Type :" & Type)


            'Creating negative stems
            If Type = "Godan" Then
                If Last = "む" Then
                    LastAdd = "ま"
                    LastAdd2 = "め"
                End If
                If Last = "ぶ" Then
                    LastAdd = "ば"
                    LastAdd2 = "べ"
                End If
                If Last = "ぬ" Then
                    LastAdd = "な"
                    LastAdd2 = "ね"
                End If
                If Last = "す" Then
                    LastAdd = "さ"
                    LastAdd2 = "せ"
                End If
                If Last = "ぐ" Then
                    LastAdd = "が"
                    LastAdd2 = "げ"
                End If
                If Last = "く" Then
                    LastAdd = "か"
                    LastAdd2 = "け"
                End If
                If Last = "る" Then
                    LastAdd = "ら"
                    LastAdd2 = "れ"
                End If
                If Last = "つ" Then
                    LastAdd = "た"
                    LastAdd2 = "て"
                End If
                If Last = "う" Then
                    LastAdd = "わ"
                    LastAdd2 = "え"
                End If

                Forms(0, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd2 & "る"
                Forms(1, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "れる"
                Forms(2, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "せる"
                If LastAdd <> "さ" Then
                    Forms(3, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "される"
                    Forms(3, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd & "される"
                Else
                    Forms(3, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "させられる"
                    Forms(3, 3) = Left(Furigana, Furigana.Length - 1) & "させられる"
                End If

                Forms(0, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd2 & "る"
                Forms(1, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd & "れる"
                Forms(2, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd & "せる"
                '(3, 3) is done in the Else above ^
            Else
                Forms(0, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "られる"
                Forms(1, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "られる"
                Forms(2, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "させる"
                Forms(3, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "させられる"

                Forms(0, 3) = Left(Furigana, Furigana.Length - 1) & "られる"
                Forms(1, 3) = Left(Furigana, Furigana.Length - 1) & "られる"
                Forms(2, 3) = Left(Furigana, Furigana.Length - 1) & "させる"
                Forms(3, 3) = Left(Furigana, Furigana.Length - 1) & "させられる"
            End If
            'forms array:
            'Forms(X, Y) x = form, y = type of info
            'y1 = correct conjugation
            'y2 = question
            'y3 = furigana conjugation


            'For i = 0 To 8
            'Console.WriteLine(i & ": " & Forms(i, 2) & "; " & Forms(i, 1))
            'Next
        End If

        Randomize()
        Dim Remaining As String = "0123"
        Dim Random As Integer = Int((8 + 1) * Rnd())
        Dim Read As String = ""
        Dim Completed As Integer = 1
        Dim Score As Integer = 0
        Dim Attempts As Integer = 0
        Dim Input1, Input2, Input3 As String
        Do Until Remaining.Length = 0
            Input1 = ""
            Input2 = ""
            Input3 = ""

            'Do Until Remaining.IndexOf(Random) <> -1 'Keep generating a random number until one is generated that hasn't been generator before
            Random = Int((3 + 1) * Rnd())
            '    If Remaining.IndexOf(Random) = -1 Then
            'Random = Int((3 + 1) * Rnd())
            'End If
            'Loop

            Do Until Read = Forms(Random, 1) Or Read = Forms(Random, 3)
                If Score < 0 Then
                    Score = 0
                End If
                Console.WriteLine(ActualSearchWord & " - " & 4 - Remaining.Length & "/4")
                Console.WriteLine(Forms(Random, 2) & " (attempt " & Attempts + 1 & " - score:" & Score & "):")
                'Console.WriteLine("Cheat: " & Forms(Random, 3)) 'Use this to see the answer
                Read = WanaKana.ToHiragana(Console.ReadLine)
                If Attempts = 0 Then
                    Input1 = Read
                ElseIf Attempts = 1 Then
                    Input2 = Read
                ElseIf Attempts = 2 Then
                    Input3 = Read
                End If

                Attempts += 1
                Console.Clear()
                If Read <> Forms(Random, 1) Then
                    Remaining &= Random

                    Score -= 55 * Attempts
                    If Attempts > 2 Then
                        Score -= 10
                        If Score < 0 Then
                            Score = 0
                        End If
                        Console.WriteLine(ActualSearchWord & "(" & Furigana & ") - " & 4 - Remaining.Length & "/4" & " (score:" & Score & "):")
                        Console.WriteLine("The answer was " & Forms(Random, 1) & " (" & Forms(Random, 3) & ")")
                        Read = Forms(Random, 1) 'This is to exit the loop

                        Console.WriteLine()
                        Console.WriteLine("You thought the answer was:")
                        Console.WriteLine(Input1)
                        Console.WriteLine(Input2)
                        Console.WriteLine(Input3)
                        Console.ReadLine()
                        Continue Do
                    End If
                End If
            Loop

            Remaining = Remaining.Replace(Random, "")

            Read = ""
            Score += 100 / Attempts
            Attempts = 0
            Console.Clear()
            Completed += 1
            If Score < 0 Then
                Score = 1
            End If
            Attempts = 0
        Loop


        For Conjugation = 0 To 3
            Console.WriteLine(Forms(Conjugation, 2) & ":")
            Console.WriteLine(Forms(Conjugation, 1) & "(" & Forms(Conjugation, 3) & ")")
            Console.WriteLine()
        Next

        Console.WriteLine("Finished!")
        Console.WriteLine("Score: " & Score)
        Console.ReadLine()
        Main()
    End Sub
    Sub KanjiTest()
        '小週票用発表田島多摩伊
        Const QUOTE = """"
        Dim ActualSearchWord As String
        Dim WordWordURL As String
        Dim WordHTML As String = ""
        Dim Client As New WebClient
        Dim Correct As Boolean = False
        Dim Answer As String = ""
        Dim CutCorrect As String = ""
        Dim NeedWork() As String = {""}
        Dim Wrong As Integer = 0
        Dim Multiple As Boolean = False
        Dim PrevAns As String = ""
        Dim MainFuri As String = ""
        Dim DoMeaning, DoKun, DoOn As Boolean
        DoMeaning = False
        DoKun = False
        DoOn = False

        Console.Clear()
        Console.WriteLine("Paste any string of Japanese text (must contain kanji)")
        Dim KanjisString As String = Console.ReadLine
        Console.Clear()

        Console.WriteLine("Which would you like to be tested on? ('y' for yes)")
        Console.Write("Meaning: ")
        Answer = Console.ReadLine.ToLower
        If Answer = "y" Or Answer = "yes" Or Answer = "true" Or Answer = "1" Then
            DoMeaning = True
        End If
        Console.Clear()
        Console.WriteLine("Which would you like to be tested on? ('y' for yes)")
        Console.WriteLine("Meaning: " & DoMeaning)
        Console.Write("Kun Reading: ")
        Answer = Console.ReadLine.ToLower
        If Answer = "y" Or Answer = "yes" Or Answer = "true" Or Answer = "1" Then
            DoKun = True
        End If
        Console.Clear()
        Console.WriteLine("Which would you like to be tested on? ('y' for yes)")
        Console.WriteLine("Meaning: " & DoMeaning)
        Console.WriteLine("Kun Reading: " & DoKun)
        Console.Write("On Reading: ")
        Answer = Console.ReadLine.ToLower
        If Answer = "y" Or Answer = "yes" Or Answer = "true" Or Answer = "1" Then
            DoOn = True
        End If

        If DoMeaning = False And DoKun = False And DoOn = False Then
            Console.Clear()
            Console.WriteLine("You didn't choose anything to be tested on")
            Console.WriteLine("Type 'y' for something you want to be tested on")
            Console.ReadLine()
            Main()
        End If

        Console.Clear()
        Console.WriteLine("You will be tested on these:")
        Console.WriteLine("Meaning: " & DoMeaning)
        Console.WriteLine("Kun Reading: " & DoKun)
        Console.WriteLine("On Reading: " & DoOn)
        Console.ReadLine()

        Correct = False
        Client.Encoding = System.Text.Encoding.UTF8
        For Kanji = 1 To KanjisString.Length
            ActualSearchWord = Mid(KanjisString, Kanji, 1)
            'Console.WriteLine(ActualSearchWord & "...")

            If WanaKana.IsKanji(ActualSearchWord) = False Then
                'Console.Clear()
                Continue For
            End If

            Try
                'Kanji, meanings and reading extract. First open the "/word" page and then extracts instead of extracting from "/search":
                WordWordURL = ("https://jisho.org/word/" & ActualSearchWord)
                WordHTML = Client.DownloadString(New Uri(WordWordURL))
            Catch
                Try
                    WordWordURL = ("https://jisho.org/search/" & ActualSearchWord & "%20%23kanji")
                    'WordHTML = Client.DownloadString(New Uri(WordWordURL))
                Catch

                End Try
                Console.Clear()
                Continue For
            End Try

            Dim KanjiInfo As String = ""
            Try
                KanjiInfo = RetrieveClassRange(WordHTML, "<span class=" & QUOTE & "character literal japanese_gothic", "</aside>", "KanjiInfo")
            Catch
                Console.Clear()
                Continue For
            End Try

            If KanjiInfo = "" Then
                Console.Clear()
                Continue For
            End If

            Dim KanjiGroupEnd As Integer 'This is going to detect "Details" (the end of a group of kanji info for one kanji)
            Dim KanjiGroup(0) As String 'This will store each Kanji group in an array
            Dim I As Integer = -1 'This will store the index of which kanji group the loop is on, indexing starts at 0, thus " = 0"
            Dim LastDetailsIndex As Integer = KanjiInfo.LastIndexOf("Details")

            Try
                KanjiInfo = Left(KanjiInfo, LastDetailsIndex)

                Dim Finished As Boolean = False
                Do Until Finished = True 'Do until no more end splitters can be found. The sentences that are pasted won't end in "|" because of how the AHK sentence grabber works
                    I += 1
                    Array.Resize(KanjiGroup, KanjiGroup.Length + 1)
                    KanjiGroupEnd = KanjiInfo.IndexOf("Details") + 10
                    If KanjiGroupEnd = 9 Then '(-1 but at add because of the above line. This means if "Details" isn't found
                        KanjiGroup(I) = KanjiInfo
                        Finished = True
                        Continue Do
                    End If

                    KanjiGroup(I) = Mid(KanjiInfo, 6, KanjiGroupEnd - 5)
                    KanjiInfo = Mid(KanjiInfo, KanjiGroupEnd)
                Loop
                Array.Resize(KanjiGroup, KanjiGroup.Length - 1)
            Catch
                Console.Clear()
            End Try

            Dim ActualInfo(KanjiGroup.Length - 1, 3) 'X = Kanji (group), Y = Info type.

            Try
                'Y indexs:
                '0 = Kanji
                '1 = English meaning(s) (I will concatinate multiple meanings)
                '2 = kun readings (concatentated if needed, usually so)
                '3 = on readings (concatentated if needed, usually so)
                Dim FirstFinder As Integer
                Dim SecondFinder As Integer

                Dim AllEng, AllKun, AllOn As Boolean
                AllEng = False
                AllKun = False
                AllOn = False
                Dim LastReadingFound As Boolean = False 'This is used to find the last reading of a kanji, it knows that it is a last because it ends in "</a>、" and not "</a>"
                Dim JustEng As String
                Dim KunReading, OnReading, ReadingSnip As String

                KanjiGroup(KanjiGroup.Length - 1) = Mid(KanjiGroup(KanjiGroup.Length - 1), 5) 'This lets the last group work

                For Looper = 0 To KanjiGroup.Length - 1
                    Try 'This is for if there are no kanji
                        FirstFinder = KanjiGroup(Looper).IndexOf("</a>")
                    Catch
                        Console.ReadLine()
                        Main()
                    End Try
                    'KanjiGroup(Looper) = Mid(KanjiGroup(Looper), FirstFinder + 10)
                    ActualInfo(Looper, 0) = Mid(KanjiGroup(Looper), FirstFinder, 1)

                    FirstFinder = KanjiGroup(Looper).IndexOf("sense")
                    KanjiGroup(Looper) = Mid(KanjiGroup(Looper), FirstFinder + 10)

                    FirstFinder = KanjiGroup(Looper).IndexOf("</div>")


                    JustEng = Left(KanjiGroup(Looper), FirstFinder)

                    JustEng = Mid(JustEng, 18)
                    JustEng = Left(JustEng, JustEng.Length - 14)
                    KanjiGroup(Looper) = KanjiGroup(Looper).Replace(JustEng, "")

                    JustEng = JustEng.Replace("           ", "")
                    FirstFinder = JustEng.IndexOf("</span>")
                    SecondFinder = JustEng.IndexOf("<span>")
                    Try
                        JustEng = JustEng.Replace(Mid(JustEng, FirstFinder, SecondFinder + 7 - FirstFinder), "")
                    Catch

                    End Try

                    JustEng = JustEng.Replace(",", ", ")
                    'JustEng = Left(JustEng, 1).ToUpper & Mid(JustEng, 2)

                    ActualInfo(Looper, 1) = JustEng

                    'Splitting the rest of the HTML (KanjiGroup) into Kun and On readings:
                    FirstFinder = KanjiGroup(Looper).IndexOf("on readings") - 12
                    KunReading = Left(KanjiGroup(Looper), FirstFinder)
                    OnReading = Mid(KanjiGroup(Looper), FirstFinder)

                    LastReadingFound = False
                    Do Until LastReadingFound = True
                        If KunReading.IndexOf("</a>、") <> -1 Then 'If the reading that is about to be snipped isn't the last
                            SecondFinder = KunReading.IndexOf("</a>、")
                            FirstFinder = Left(KunReading, SecondFinder).LastIndexOf(">")
                            ReadingSnip = Mid(KunReading, FirstFinder + 2, SecondFinder - 1 - FirstFinder)

                            ActualInfo(Looper, 2) &= ReadingSnip.ToLower & ", " 'Adding the reading to the Actual info array

                            KunReading = Mid(KunReading, SecondFinder + 10)

                        ElseIf KunReading.IndexOf("</a><") <> -1 Then 'If it is the last, "<" is just the beginning of "</span>"
                            SecondFinder = KunReading.IndexOf("</a>")
                            FirstFinder = Left(KunReading, SecondFinder).LastIndexOf(">")
                            ReadingSnip = Mid(KunReading, FirstFinder + 2, SecondFinder - 1 - FirstFinder)

                            ActualInfo(Looper, 2) &= ReadingSnip.ToLower 'Adding the reading to the Actual info array

                            LastReadingFound = True
                        Else
                            LastReadingFound = True
                        End If
                    Loop
                    LastReadingFound = False
                    Do Until LastReadingFound = True
                        If OnReading.IndexOf("</a>、") <> -1 Then 'If the reading that is about to be snipped isn't the last
                            SecondFinder = OnReading.IndexOf("</a>、")
                            FirstFinder = Left(OnReading, SecondFinder).LastIndexOf(">")
                            ReadingSnip = Mid(OnReading, FirstFinder + 2, SecondFinder - 1 - FirstFinder)

                            ActualInfo(Looper, 3) &= ReadingSnip.ToLower & ", " 'Adding the reading to the Actual info array

                            OnReading = Mid(OnReading, SecondFinder + 10)

                        ElseIf OnReading.IndexOf("</a><") <> -1 Then 'If it is the last, "<" is just the beginning of "</span>"
                            SecondFinder = OnReading.IndexOf("</a>")
                            FirstFinder = Left(OnReading, SecondFinder).LastIndexOf(">")
                            ReadingSnip = Mid(OnReading, FirstFinder + 2, SecondFinder - 1 - FirstFinder)

                            ActualInfo(Looper, 3) &= ReadingSnip.ToLower 'Adding the reading to the Actual info array

                            LastReadingFound = True
                        Else
                            LastReadingFound = True
                        End If

                    Loop
                    'Console.BackgroundColor = ConsoleColor.DarkGray
                    'Console.WriteLine(ActualInfo(Looper, 0) & " - " & ActualInfo(Looper, 1))
                    'Console.BackgroundColor = ConsoleColor.Black
                    'Console.WriteLine(ActualInfo(Looper, 2))
                    'Console.WriteLine(ActualInfo(Looper, 3))
                    'Console.WriteLine("Found in: " & JustResultsScraper(ActualInfo(Looper, 0), 1, ActualSearchWord))
                    ActualInfo(Looper, 1) = ActualInfo(Looper, 1) & ","
                    ActualInfo(Looper, 2) = ActualInfo(Looper, 2) & ","


                    Wrong = 0
                    Correct = False
                    Multiple = False
                    If DoMeaning = True Then
                        For Attempt = 1 To 2
                            Console.Clear()
                            Console.WriteLine("Kanji: " & ActualInfo(Looper, 0))
                            Console.Write("Meaning: ")
                            Answer = Console.ReadLine.ToLower

                            Dim CorrectAnswers() As String = ActualInfo(Looper, 1).split(",")
                            If CorrectAnswers.Length > 2 Then
                                Multiple = True
                            End If

                            For Check = 0 To CorrectAnswers.Length - 1
                                If Answer = CorrectAnswers(Check).Trim And CorrectAnswers(Check).Trim <> "" Then
                                    Attempt = 2
                                    Check = CorrectAnswers.Length - 1
                                    Correct = True
                                    PrevAns = Answer
                                    Continue For
                                End If

                                If Answer & "s" = CorrectAnswers(Check).Trim And CorrectAnswers(Check).Trim <> "" Then
                                    Attempt = 2
                                    Check = CorrectAnswers.Length - 1
                                    Correct = True
                                    PrevAns = Answer
                                    Continue For
                                End If
                            Next
                            If Correct = False Then
                                Wrong += 1
                            End If
                        Next

                        If Multiple = True And Correct = True Then
                            Correct = False
                            For Attempt = 1 To 2
                                Console.Clear()
                                Console.WriteLine("Kanji: " & ActualInfo(Looper, 0))
                                Console.Write("Meaning: " & PrevAns & ", ")
                                Answer = Console.ReadLine

                                Dim CorrectAnswers() As String = ActualInfo(Looper, 1).split(",")
                                If CorrectAnswers.Length > 2 Then
                                    Multiple = True
                                End If

                                For Check = 0 To CorrectAnswers.Length - 1
                                    If Answer = CorrectAnswers(Check).Trim And CorrectAnswers(Check).Trim <> "" And Answer <> PrevAns And Answer & "s" <> PrevAns And Answer <> PrevAns & "s" Then
                                        Attempt = 2
                                        Check = CorrectAnswers.Length - 1
                                        Correct = True
                                        PrevAns = Answer
                                        Continue For
                                    End If
                                    If Answer & "s" = CorrectAnswers(Check).Trim And CorrectAnswers(Check).Trim <> "" And Answer <> PrevAns And Answer & "s" <> PrevAns And Answer <> PrevAns & "s" Then
                                        Attempt = 2
                                        Check = CorrectAnswers.Length - 1
                                        Correct = True
                                        PrevAns = Answer
                                        Continue For
                                    End If
                                Next
                                If Correct = False Then
                                    Wrong += 1
                                End If
                            Next
                        End If
                    End If

                    Multiple = False
                    Correct = False
                    If DoKun = True Then
                        If ActualInfo(Looper, 2) <> "," Then
                            For Attempt = 1 To 2
                                Console.Clear()
                                Console.WriteLine("Kanji: " & ActualInfo(Looper, 0))
                                Console.WriteLine("Meaning: " & ActualInfo(Looper, 1))
                                Console.Write("Kun Readings: ")
                                Answer = Console.ReadLine
                                Answer = Answer.Replace("-", "")
                                If WanaKana.IsKatakana(Answer) = True Or WanaKana.IsRomaji(Answer) Then
                                    Answer = WanaKana.ToHiragana(Answer)
                                End If

                                Dim CorrectAnswers() As String = ActualInfo(Looper, 2).split(",")
                                For Check = 0 To CorrectAnswers.Length - 1
                                    If CorrectAnswers(Check).IndexOf(".") <> -1 Then
                                        CorrectAnswers(Check) = Left(CorrectAnswers(Check), CorrectAnswers(Check).IndexOf("."))
                                    End If
                                    CorrectAnswers(Check) = CorrectAnswers(Check).Replace("-", "")

                                    If Answer = CorrectAnswers(Check).Trim And CorrectAnswers(Check).Trim <> "" Then
                                        Attempt = 2
                                        Check = CorrectAnswers.Length - 1
                                        Correct = True
                                        Continue For
                                    End If
                                Next
                                If Correct = False Then
                                    Wrong += 1
                                End If
                            Next
                        Else
                            ActualInfo(Looper, 2) = ""
                        End If
                    End If

                    Correct = False
                    If DoOn = True Then
                        If ActualInfo(Looper, 3) <> "," Then
                            For Attempt = 1 To 2
                                Console.Clear()
                                Console.WriteLine("Kanji: " & ActualInfo(Looper, 0))
                                Console.WriteLine("Meaning: " & ActualInfo(Looper, 1))
                                Console.WriteLine("Kun Readings: " & ActualInfo(Looper, 2))
                                Console.Write("On Readings: ")
                                Answer = Console.ReadLine
                                Answer = Answer.Replace("-", "")
                                If WanaKana.IsHiragana(Answer) = True Or WanaKana.IsRomaji(Answer) Then
                                    Answer = WanaKana.ToKatakana(Answer)
                                End If


                                Dim CorrectAnswers() As String = ActualInfo(Looper, 3).split(",")
                                For Check = 0 To CorrectAnswers.Length - 1
                                    If CorrectAnswers(Check).IndexOf(".") <> -1 Then
                                        CorrectAnswers(Check) = Left(CorrectAnswers(Check), CorrectAnswers(Check).IndexOf("."))
                                    End If
                                    CorrectAnswers(Check) = CorrectAnswers(Check).Replace("-", "")

                                    If Answer = CorrectAnswers(Check).Trim And CorrectAnswers(Check).Trim <> "" Then
                                        Attempt = 2
                                        Check = CorrectAnswers.Length - 1
                                        Correct = True
                                        Continue For
                                    End If
                                Next
                                If Correct = False Then
                                    Wrong += 1
                                End If
                            Next
                        Else
                            ActualInfo(Looper, 3) = ""
                        End If
                    End If

                    'Getting just 1 (the main) furigana for the kanji:
                    If ActualInfo(Looper, 3) <> "" And ActualInfo(Looper, 3) <> "," Then
                        If FirstFinder <> -1 Then
                            MainFuri = Left(ActualInfo(Looper, 3), FirstFinder)
                        Else
                            MainFuri = Left(ActualInfo(Looper, 3), FirstFinder)
                        End If
                    ElseIf ActualInfo(Looper, 2) <> "" And ActualInfo(Looper, 2) <> "," Then
                        FirstFinder = ActualInfo(Looper, 2).indexof(",")
                        If FirstFinder <> -1 Then
                            MainFuri = Left(ActualInfo(Looper, 2), FirstFinder)
                        Else
                            MainFuri = Left(ActualInfo(Looper, 2), FirstFinder)
                        End If
                    End If

                    Array.Resize(NeedWork, NeedWork.Length + 1)
                    NeedWork(NeedWork.Length - 2) = ActualInfo(Looper, 0) & " (" & MainFuri & ") - " & ActualInfo(Looper, 1) & Wrong

                    Console.Clear()
                    Console.WriteLine("Kanji: " & ActualInfo(Looper, 0))
                    Console.WriteLine("Meaning: " & ActualInfo(Looper, 1))
                    Console.WriteLine("Kun Readings: " & ActualInfo(Looper, 2))
                    Console.WriteLine("On Readings: " & ActualInfo(Looper, 3))
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.WriteLine(Wrong & " incorrect answers")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadLine()
                    Console.Clear()
                Next
            Catch
                Console.WriteLine("Error")
            End Try
        Next

        Array.Resize(NeedWork, NeedWork.Length - 1)
        Console.Clear()

        For Printer = 0 To NeedWork.Length - 1
            Console.Write(Left(NeedWork(Printer), 1) & " ")
        Next
        Console.WriteLine(":")
        Console.WriteLine()

        Dim NotToTest As Integer = 0
        If DoMeaning = False Then
            NotToTest += 1
        End If
        If DoKun = False Then
            NotToTest += 1
        End If
        If DoOn = False Then
            NotToTest += 1
        End If


        If NotToTest = 0 Then
            If NeedWork.Length > 0 Then
                Console.WriteLine("Kanji you don't know:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong > 4 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next

                Console.WriteLine()
                Console.WriteLine("Kanji that still needs work:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong = 2 Or Wrong = 3 Or Wrong = 4 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next

                Console.WriteLine()
                Console.WriteLine("Almost Easy Kanji:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong = 1 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next

                Console.WriteLine()
                Console.WriteLine("Easy Kanji:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong = 0 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next
            Else
                Console.Clear()
                Console.WriteLine("There didn't seem to be any kanji.")
                Console.ReadLine()
                Main()
            End If
        ElseIf NotToTest = 1 Then
            If NeedWork.Length > 0 Then
                Console.WriteLine("Kanji you don't know:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong > 3 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next

                Console.WriteLine()
                Console.WriteLine("Kanji that still needs work:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong = 2 Or Wrong = 3 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next

                Console.WriteLine()
                Console.WriteLine("Almost Easy Kanji:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong = 1 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next

                Console.WriteLine()
                Console.WriteLine("Easy Kanji:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong = 0 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next
            Else
                Console.Clear()
                Console.WriteLine("There didn't seem to be any kanji.")
                Console.ReadLine()
                Main()
            End If
        ElseIf NotToTest = 2 Then
            If NeedWork.Length > 0 Then
                Console.WriteLine("Kanji you don't know:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong > 1 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next

                Console.WriteLine()
                Console.WriteLine("Kanji that still needs work:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong = 1 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next

                Console.WriteLine()
                Console.WriteLine("Easy Kanji:")
                For Printer = 0 To NeedWork.Length - 1
                    Wrong = Right(NeedWork(Printer), 1)
                    If Wrong = 0 Then
                        Console.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                    End If
                Next
            Else
                Console.Clear()
                Console.WriteLine("There didn't seem to be any kanji.")
                Console.ReadLine()
                Main()
            End If
        End If

        Console.ReadLine()
        Console.Clear()
        Randomize()
        Console.WriteLine("Would you like these results saved in a text file?")
        Answer = Console.ReadLine.ToLower
        If Answer = "y" Or Answer = "yes" Or Answer = "1" Or Answer = "true" Or Answer.IndexOf("yes") <> -1 Or Answer.IndexOf("true") <> -1 Then
            Dim RanInt As Integer = Int((1000) * Rnd())
            File.Create(Environ$("USERPROFILE") & "\Downloads\" & "kanjitest" & RanInt & ".txt").Dispose()
            Dim FirstWriter As System.IO.StreamWriter
            FirstWriter = New System.IO.StreamWriter(Environ$("USERPROFILE") & "\Downloads\" & "kanjitest" & RanInt & ".txt")
            FirstWriter.WriteLine("Test taken on " & System.DateTime.Now)
            FirstWriter.WriteLine()

            If NotToTest = 0 Then
                If NeedWork.Length > 0 Then
                    FirstWriter.WriteLine("Kanji you don't know:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong > 4 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next

                    FirstWriter.WriteLine()
                    FirstWriter.WriteLine("Kanji that still needs work:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong = 2 Or Wrong = 3 Or Wrong = 4 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next

                    FirstWriter.WriteLine()
                    FirstWriter.WriteLine("Almost Easy Kanji:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong = 1 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next

                    FirstWriter.WriteLine()
                    FirstWriter.WriteLine("Easy Kanji:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong = 0 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next
                End If
            ElseIf NotToTest = 1 Then
                If NeedWork.Length > 0 Then
                    FirstWriter.WriteLine("Kanji you don't know:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong > 3 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next

                    FirstWriter.WriteLine()
                    FirstWriter.WriteLine("Kanji that still needs work:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong = 2 Or Wrong = 3 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next

                    FirstWriter.WriteLine()
                    FirstWriter.WriteLine("Almost Easy Kanji:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong = 1 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next

                    FirstWriter.WriteLine()
                    FirstWriter.WriteLine("Easy Kanji:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong = 0 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next
                End If
            ElseIf NotToTest = 2 Then
                If NeedWork.Length > 0 Then
                    FirstWriter.WriteLine("Kanji you don't know:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong > 1 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next

                    FirstWriter.WriteLine()
                    FirstWriter.WriteLine("Kanji that still needs work:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong = 1 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next

                    FirstWriter.WriteLine()
                    FirstWriter.WriteLine("Easy Kanji:")
                    For Printer = 0 To NeedWork.Length - 1
                        Wrong = Right(NeedWork(Printer), 1)
                        If Wrong = 0 Then
                            FirstWriter.WriteLine(Left(NeedWork(Printer), NeedWork(Printer).Length - 1))
                        End If
                    Next
                End If
            End If
            FirstWriter.Close()
            Console.WriteLine("The file was successfully created in the Downloads folder called 'kanjitest" & RanInt & ".txt'")
            Console.ReadLine()
            Main()
        End If

        Main()
    End Sub
    Sub ReverseAnki()
        Const QUOTE = """"
        Console.Clear()
        Console.WriteLine("What is the name of the text file? (must be in downloads folder and a .txt file)")
        Dim FileName As String = Console.ReadLine
        Try
            If Right(FileName, 4) <> ".txt" Then
                FileName &= ".txt"
            End If
        Catch
            FileName &= ".txt"
        End Try

        Dim FileReader As String = ""
        Try
            FileReader = My.Computer.FileSystem.ReadAllText(Environ$("USERPROFILE") & "\Downloads\" & FileName)
        Catch
            Console.WriteLine("File " & QUOTE & FileName & QUOTE & " not found.")
            Console.ReadLine()
            Main()
        End Try

        Console.WriteLine("Are you carrying on from last time? (type 'yes' if so)")
        Dim LastTime As Integer = 0
        Dim CarryOn As String = Console.ReadLine.ToLower
        If CarryOn = "y" Or CarryOn = "yes" Or CarryOn = "1" Then
            CarryOn = "true"
            Console.Clear()
            Console.WriteLine("Which card number do you want to start from?")
            Do Until IsNumeric(CarryOn) = True
                CarryOn = Console.ReadLine()
                If IsNumeric(CarryOn) = False Then
                    Console.Clear()
                    Console.WriteLine("Which card number do you want to start from?")
                    Console.WriteLine()
                    Continue Do
                End If
            Loop
            LastTime = CInt(CarryOn)
            CarryOn = "true"
        End If


        FileReader = FileReader.Replace(vbTab, "¬")

        Dim Cards() As String = FileReader.Split(vbLf)

        Console.Clear()
        Console.WriteLine("Which field do you want to be the main?")
        Console.WriteLine()
        Dim CurrentCard As String = Cards(0)
        Dim CurrentFields() As String
        Dim FieldMax As Integer = -1
        ReDim CurrentFields(CurrentCard.Split("¬").Length - 1)
        CurrentFields = CurrentCard.Split("¬")
        For Field = 0 To CurrentFields.Length - 1
            If CurrentFields(Field) = "" And FieldMax = -1 Then
                FieldMax = Field - 1
            End If
        Next
        If FieldMax = -1 Then
            FieldMax = CurrentFields.Length - 1
        End If

        For Writer = 0 To FieldMax
            Console.WriteLine(Writer + 1 & ":" & CurrentFields(Writer))
        Next

        Dim MainField As String = ""
        Do Until IsNumeric(MainField) = True
            MainField = Console.ReadLine()
            If IsNumeric(MainField) = False Then
                Console.Clear()
                Console.WriteLine("Which field do you want to be the main? (Can't be 1)")
                Console.WriteLine()
                For Writer = 0 To FieldMax
                    Console.WriteLine(Writer + 1 & ":" & CurrentFields(Writer))
                Next
                Continue Do
            End If

            If MainField > 1 And MainField < FieldMax + 2 Then
                Continue Do
            Else
                Console.Clear()
                Console.WriteLine("Which field do you want to be the main? (Can't be 1)")
                Console.WriteLine()
                For Writer = 0 To FieldMax
                    Console.WriteLine(Writer + 1 & ":" & CurrentFields(Writer))
                Next
                MainField = ""
            End If
        Loop

        Dim B1, B2, CutEnd, MadeCards As Integer
        Dim B3 As String = ""
        Dim Chosen, ChosenCutter As Integer
        Dim UserInput As String = ""
        Dim ReversedCards(LastTime) As String
        Dim RoundUnavailable As String
        MadeCards = Cards.Length - 1
        For Card = LastTime To Cards.Length - 1
            RoundUnavailable = ""
            Console.Clear()

            CurrentCard = Cards(Card)
            ReDim CurrentFields(CurrentCard.Split("¬").Length - 1)
            CurrentFields = CurrentCard.Split("¬")

            'Getting rid of "[]" (for readings)
            B1 = CurrentFields(0).IndexOf("[")
            B2 = CurrentFields(0).IndexOf("]")
            Try
                B3 = Mid(CurrentFields(0), B1 + 1, B2 + 1 - B1)
            Catch
            End Try
            Do Until B1 = -1 Or B2 = -1 Or B3.Length = 0
                B1 = CurrentFields(0).IndexOf("[")
                B2 = CurrentFields(0).IndexOf("]")
                Try
                    B3 = Mid(CurrentFields(0), B1 + 1, B2 + 1 - B1)
                    CurrentFields(0) = CurrentFields(0).Replace(B3, "").Replace(" ", "")
                Catch
                End Try
            Loop

            CutEnd = -1

            B1 = CurrentFields(MainField - 1).IndexOf("[") - 2
            B2 = CurrentFields(MainField - 1).IndexOf(CurrentFields(0)) - 1
            Try
                If B2 < 3 Then
                    Try
                        B2 = CurrentFields(MainField - 1).IndexOf(Left(CurrentFields(0), 1)) - 1
                    Catch
                    End Try
                    If B2 < 3 Then
                        B2 = CurrentFields(MainField - 1).LastIndexOf("nym") - 4
                    End If
                    If B2 < 3 Then
                        B2 = CurrentFields(MainField - 1).IndexOf("~")
                    End If
                    If B2 < 3 Then
                        B2 = CurrentFields(MainField - 1).IndexOf(";")
                    End If
                End If
                If B1 < 3 Then
                    B1 = CurrentFields(MainField - 1).LastIndexOf(";")
                End If
                If B1 < 3 Then
                    B1 = CurrentFields(MainField - 1).LastIndexOf(".2")
                    If B1 = -1 Then
                        B1 = CurrentFields(MainField - 1).LastIndexOf(".3")
                    End If
                End If
                If B2 < 3 Then
                    B2 = CurrentFields(MainField - 1).LastIndexOf(".2")
                    If B2 = -1 Then
                        B2 = CurrentFields(MainField - 1).LastIndexOf(".3")
                    End If
                End If

                If B2 = B1 Then
                    B1 = CurrentFields(MainField - 1).LastIndexOf(",")
                End If

                If B2 < 3 Then
                    B2 = CurrentFields(MainField - 1).LastIndexOf(")") + 1
                End If
                If B1 < 3 Then
                    B1 = CurrentFields(MainField - 1).LastIndexOf(")") + 1
                End If
                If B1 < 3 Then
                    B1 = CurrentFields(MainField - 1).LastIndexOf("2.")
                End If

                If B2 = CurrentFields(MainField - 1).Length Then
                    B2 = CurrentFields(MainField - 1).LastIndexOf(";")
                    If B2 = -1 Then
                        B2 = CurrentFields(MainField - 1).LastIndexOf(",")
                    End If
                End If

                If B1 = CurrentFields(MainField - 1).Length Then
                    B1 = CurrentFields(MainField - 1).LastIndexOf("=") - 2
                    If B1 = -1 Then
                        B1 = CurrentFields(MainField - 1).LastIndexOf(" " - 2)
                    End If
                End If

                If B2 < 5 Then
                    B2 = CurrentFields(MainField - 1).LastIndexOf("​ ")
                End If

                If B1 = B2 Then
                    B2 = B1 - 3
                End If
            Catch
            End Try

            ChosenCutter = 0
            Chosen = False
            Do Until Chosen = True
                Console.Clear()
                Console.WriteLine("Card " & Card + 1 & "/" & Cards.Length)
                Console.WriteLine("Are any of these trims ok?: [for " & CurrentFields(0) & "] if you want to finish creating cards type 'finish', 'menu' to quit or 'back' to redo last")
                Console.WriteLine()
                Console.WriteLine("1:")
                Console.WriteLine(CurrentFields(MainField - 1))

                If B1 > 5 Then
                    Console.WriteLine()
                    Console.WriteLine("2:")
                    Console.Write(Left(CurrentFields(MainField - 1), B1))
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.WriteLine(Mid(CurrentFields(MainField - 1), B1 + 1, CurrentFields(MainField - 1).Length - B1))
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    RoundUnavailable &= 2
                End If

                If B2 > 5 Then
                    Console.WriteLine()
                    Console.WriteLine("3:")
                    Console.Write(Left(CurrentFields(MainField - 1), B2))
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.WriteLine(Mid(CurrentFields(MainField - 1), B2 + 1, CurrentFields(MainField - 1).Length - B2))
                    Console.ForegroundColor = ConsoleColor.White
                Else
                    RoundUnavailable &= 3
                End If

                Console.WriteLine()
                Console.WriteLine("4 = Custom trim")
                Console.WriteLine()

                UserInput = Console.ReadLine.ToLower
                If UserInput = "1" Then
                    ChosenCutter = 1
                    Chosen = True
                    CutEnd = CurrentFields(MainField - 1).Length
                ElseIf UserInput = "2" And RoundUnavailable.IndexOf("2") = -1 Then
                    ChosenCutter = 2
                    Chosen = True
                    CutEnd = B1
                ElseIf UserInput = "3" And RoundUnavailable.IndexOf("3") = -1 Then
                    ChosenCutter = 3
                    Chosen = True
                    CutEnd = B2
                ElseIf UserInput = "4" Or UserInput = "custom" Or UserInput = "four" Then
                    ChosenCutter = 4
                    Chosen = True
                ElseIf UserInput = "finish" Or UserInput = "done" Then
                    MadeCards = Card - 1
                    Card = Cards.Length - 1
                    Chosen = True
                    Continue Do
                ElseIf UserInput = "menu" Or UserInput = "quit" Then
                    Main()
                ElseIf UserInput = "restart" Then
                    ReverseAnki()
                ElseIf UserInput = "back" Or UserInput = "previous" Or UserInput = "redo" Or UserInput = "retry" Then
                    Card -= 2
                    Chosen = True
                    Continue For
                End If
            Loop
            If MadeCards <> Cards.Length - 1 Then
                Continue For
            End If

            Dim KeyReader As ConsoleKeyInfo
            If ChosenCutter = 4 Then
                CutEnd = B1 - 5
                If CutEnd < 5 Then
                    CutEnd = CurrentFields(MainField - 1).Length * 0.5
                End If
                Chosen = False
                Do Until Chosen = True
                    Console.Clear()
                    Console.WriteLine("Card " & Card + 1 & "/" & Cards.Length)
                    Console.WriteLine("Where do you to cut? Use left and right keys, press enter when done [for " & CurrentFields(0) & "]")
                    Console.WriteLine()
                    Console.Write(Left(CurrentFields(MainField - 1), CutEnd))
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.WriteLine(Right(CurrentFields(MainField - 1), CurrentFields(MainField - 1).Length - CutEnd))
                    Console.ForegroundColor = ConsoleColor.White
                    KeyReader = Console.ReadKey
                    If KeyReader.Key = ConsoleKey.RightArrow Then
                        CutEnd += 1
                        If CutEnd > CurrentFields(MainField - 1).Length Then
                            CutEnd = CurrentFields(MainField - 1).Length
                        End If
                    ElseIf KeyReader.Key = ConsoleKey.LeftArrow Then
                        CutEnd -= 1
                        If CutEnd < 1 Then
                            CutEnd = 1
                        End If
                    ElseIf KeyReader.Key = ConsoleKey.Enter Then
                        Chosen = True
                    ElseIf KeyReader.Key = ConsoleKey.Home Then
                        CutEnd = 1
                    ElseIf KeyReader.Key = ConsoleKey.End Then
                        CutEnd = CurrentFields(MainField - 1).Length
                    ElseIf KeyReader.Key = ConsoleKey.Tab Then
                        CutEnd += 6
                        If CutEnd > CurrentFields(MainField - 1).Length Then
                            CutEnd = CurrentFields(MainField - 1).Length
                        End If
                    ElseIf KeyReader.Key = ConsoleKey.Delete Then
                        CutEnd -= 6
                        If CutEnd < 1 Then
                            CutEnd = 1
                        End If
                    End If
                Loop
                ChosenCutter = True
            End If

            CurrentFields(MainField - 1) = Left(CurrentFields(MainField - 1), CutEnd)
            Console.Clear()
            Console.WriteLine("Card " & Card + 1 & "/" & Cards.Length)
            Console.WriteLine("Anything to remove? [for " & CurrentFields(0) & "]")
            Console.WriteLine()
            Console.WriteLine(CurrentFields(MainField - 1))
            UserInput = Console.ReadLine.ToLower
            If UserInput = "y" Or UserInput = "yes" Or UserInput = "1" Then
                Console.Clear()
                Console.WriteLine("What would you like to replace?")
                Console.WriteLine()
                Console.WriteLine(CurrentFields(MainField - 1))
                Try
                    CurrentFields(MainField - 1) = CurrentFields(MainField - 1).Replace(Console.ReadLine, "")
                Catch
                End Try
                Console.Clear()
                Console.WriteLine("This is your new trim:")
                Console.WriteLine()
                Console.WriteLine(CurrentFields(MainField - 1))
                Console.ReadLine()
            End If

            ReversedCards(Card) = CurrentFields(MainField - 1)
            Array.Resize(ReversedCards, ReversedCards.Length + 1)
            Console.WriteLine()
        Next

        If MadeCards = 0 Then
            Console.Clear()
            Console.WriteLine("You didn't create enough cards")
            Console.ReadLine()
            Main()
        End If

        If ReversedCards(ReversedCards.Length - 1) = "" Then
            Array.Resize(ReversedCards, ReversedCards.Length - 1)
        End If

        Dim TempSwitcher As String = ""
        Dim TempCut1, TempCut2 As String
        Dim Texter As System.IO.StreamWriter
        If LastTime = 0 Then
            For Switch = LastTime To MadeCards
                TempSwitcher = Cards(Switch)
                B1 = TempSwitcher.IndexOf("¬")
                TempCut1 = Left(TempSwitcher, B1)

                TempCut2 = TempSwitcher
                For FieldCut = 1 To CInt(MainField) - 1
                    B2 = TempCut2.IndexOf("¬")
                    TempCut2 = Mid(TempCut2, B2 + 2)
                Next

                B3 = TempCut2.IndexOf("¬")
                TempCut2 = Left(TempCut2, B3)

                TempSwitcher = Mid(TempSwitcher, B1 + 1)
                TempSwitcher = TempSwitcher.Replace(TempCut2, TempCut1)
                TempSwitcher = ReversedCards(Switch) & TempSwitcher

                Cards(Switch) = TempSwitcher

                File.Create(Environ$("USERPROFILE") & "\Downloads\" & FileName.Replace(".txt", "_reversed.txt")).Dispose()

                Texter = New System.IO.StreamWriter(Environ$("USERPROFILE") & "\Downloads\" & FileName.Replace(".txt", "_reversed.txt"))

                For Writer = 0 To MadeCards
                    Texter.WriteLine(Cards(Writer))
                Next
                Texter.Close()

            Next

        Else ''''-------- ELSE ---------

            If ReversedCards(ReversedCards.Length - 1) = "" Then
                Array.Resize(ReversedCards, ReversedCards.Length - 1)
            End If

            For Switch = LastTime To ReversedCards.Length - 1
                Try
                    TempSwitcher = Cards(Switch)
                Catch
                End Try
                B1 = TempSwitcher.IndexOf("¬")
                TempCut1 = Left(TempSwitcher, B1)

                TempCut2 = TempSwitcher
                For FieldCut = 1 To CInt(MainField) - 1
                    B2 = TempCut2.IndexOf("¬")
                    TempCut2 = Mid(TempCut2, B2 + 2)
                Next

                B3 = TempCut2.IndexOf("¬")
                TempCut2 = Left(TempCut2, B3)

                TempSwitcher = Mid(TempSwitcher, B1 + 1)
                Try
                    TempSwitcher = TempSwitcher.Replace(TempCut2, TempCut1)

                    TempSwitcher = ReversedCards(Switch) & TempSwitcher

                    Cards(Switch) = TempSwitcher

                    Texter = New System.IO.StreamWriter(Environ$("USERPROFILE") & "\Downloads\" & FileName.Replace(".txt", "_reversed.txt"), True)
                    Texter.WriteLine(TempSwitcher)
                Catch
                End Try
                Texter.Close()
            Next
        End If

        Console.Clear()

        If MadeCards < Cards.Length - 2 Then
            Console.WriteLine("If you want to continue from this time, state the card '" & MadeCards + 1 & "'.")
        End If

        Console.WriteLine("Done!")
        Console.ReadLine()
        Main()
    End Sub
    Sub HeyLingoDownload(Language)
        Console.Clear()
        Dim UserInput As String = ""
        Language = Language.trim.tolower

        For Clear = 0 To 9
            Language = Language.replace(Clear, "")
            Language = Language.replace(Clear, "")
        Next

        If Language = "clear" Or Language = "delete" Then
            Console.WriteLine("Are you sure you want to delete ALL of you HeyLingo downloads?")
            UserInput = Console.ReadLine.Trim.ToLower
            If UserInput = "yes" Then
                Try
                    System.IO.Directory.Delete(Environ$("USERPROFILE") & "\Downloads\HeyLingo Audio", True)
                    Console.ForegroundColor = ConsoleColor.Green
                    Console.WriteLine("All files have been deleted")
                    Console.ForegroundColor = ConsoleColor.White
                Catch
                    Console.ForegroundColor = ConsoleColor.Yellow
                    Console.WriteLine("There was nothing to delete")
                    Console.ForegroundColor = ConsoleColor.White
                End Try
            Else
                Console.WriteLine("You chose not delete the HeyLingo folder")
            End If

            Console.ReadLine()
            Main()
        ElseIf Language = "files" Or Language = "file" Or Language = "audio" Or Language = "folder" Then
            Try
                Process.Start("explorer.exe", Environ$("USERPROFILE") & "\Downloads\HeyLingo Audio")
                Main()
            Catch ex As Exception
                Main()
            End Try
        End If

        Language = Language.replace("-", "")
        Const QUOTE = """"
        Dim StartPage As Integer
        Dim EndPage As Integer
        Dim Failed As Integer = 0
        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8
        Dim WordHTML As String = ""
        Dim Correct As Boolean = False
        Dim TwoDigits(1) As String


        If Language = "" Then
            Language = "japanese"
        End If

        Language = Language.trim.tolower
        Do Until Correct = True
            Console.Clear()
            Console.WriteLine("Which page(s) from Hey Lingo do you want to download from (1-40)?")
            UserInput = Console.ReadLine
            If UserInput = "0" Or UserInput = "menu" Or UserInput = "back" Or UserInput = "stop" Or UserInput = "main menu" Or UserInput = "finish" Then
                Main()
            End If
            If UserInput.Contains("-") Then
                TwoDigits = UserInput.Split("-")
                If IsNumeric(TwoDigits(0)) = True And IsNumeric(TwoDigits(1)) Then
                    Correct = True
                End If
            ElseIf IsNumeric(UserInput) = True Then
                TwoDigits(0) = UserInput
                TwoDigits(1) = UserInput
                Correct = True
            End If

            If Correct = True Then
                Correct = False
                If TwoDigits(1) >= TwoDigits(0) Then
                    If TwoDigits(0) > 0 And TwoDigits(0) < 41 And TwoDigits(1) > 0 And TwoDigits(1) Then
                        Correct = True
                    End If
                End If
            End If
        Loop
        Console.Clear()

        StartPage = TwoDigits(0)
        EndPage = TwoDigits(1)

        If StartPage = EndPage Then
            Console.WriteLine("Downloading audio for " & Language & " from course " & StartPage)
        Else
            Console.WriteLine("Downloading audio for " & Language & " from courses " & StartPage & " to " & EndPage)
        End If

        My.Computer.FileSystem.CreateDirectory(Environ$("USERPROFILE") & "\Downloads\HeyLingo Audio")
        Dim SnipStart, SnipEnd As Integer
        Dim Temp, Temp2, Temp3, JPMeaning As String
        Dim URL As String = ""
        Dim Total, Success As Integer
        Dim DownloadID As String
        DownloadID = 0
        For Page = StartPage To EndPage
            Console.WriteLine("------[course " & Page & "]------")
            My.Computer.FileSystem.CreateDirectory(Environ$("USERPROFILE") & "\Downloads\HeyLingo Audio\" & Language & "\Course " & Page)
            Try
                URL = "https://www.heylingo.com/courses/" & Language & "/" & Language & "-" & Page
                WordHTML = Client.DownloadString(New Uri(URL))
            Catch
                Console.ForegroundColor = ConsoleColor.DarkRed
                Console.WriteLine("Reconnect to the internet and try again.")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()
                Main()
            End Try

            If WordHTML.Length < 500 Then
                Console.ForegroundColor = ConsoleColor.DarkRed
                Console.WriteLine("Something went wrong")
                Console.WriteLine("Either '" & Language & "' isn't a language that is available on Hey Lingo")
                Console.WriteLine("Or course '" & Page & "' doesn't exist")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()
                Main()
            End If

            For Audio = 1 To 40
                SnipEnd = WordHTML.IndexOf("hey_playaudio_id") 'End' because it has info behind it
                SnipEnd += 18
                Temp = WordHTML
                Temp = Mid(Temp, SnipEnd)

                SnipStart = WordHTML.IndexOf("onmouseout=" & QUOTE & "hide_translation()")
                Try
                    Temp2 = Mid(WordHTML, SnipStart, SnipEnd - SnipStart)
                    Correct = False
                    JPMeaning = ""
                Catch ex As Exception 'If it isn't the first audio
                    Correct = True
                    JPMeaning = "2_" & JPMeaning
                End Try

                WordHTML = Temp

                Do Until Correct = True
                    SnipStart = Temp2.IndexOf("();" & QUOTE & ">")
                    SnipStart += 6
                    Temp2 = Mid(Temp2, SnipStart)

                    Temp3 = Temp2
                    SnipEnd = Temp3.IndexOf("<")

                    Temp2 = Mid(Temp2, 8)

                    Temp3 = Left(Temp3, SnipEnd)
                    JPMeaning &= Temp3
                    Temp3 = ""
                    If Temp2.IndexOf("();" & QUOTE & ">") = -1 Then
                        Correct = True
                    End If
                Loop
                Try
                    WordHTML = Mid(WordHTML, 20)

                    SnipEnd = Temp.IndexOf(")") - 1
                    Temp = Left(Temp, SnipEnd) 'We now how the inside of the bracket which contains three ids

                    SnipEnd = Temp.IndexOf("'") + 2 'We need the last ID
                    Temp = Mid(Temp, SnipEnd)
                    If Temp = DownloadID Then
                        DownloadID = -1
                    Else
                        DownloadID = Temp
                    End If
                Catch ex As Exception
                    Failed += 41 - Audio
                    Audio = 40
                    Console.ForegroundColor = ConsoleColor.DarkRed
                    Console.WriteLine("ID Fail: " & JPMeaning)
                    Console.ForegroundColor = ConsoleColor.White
                    Continue For
                End Try

                JPMeaning = JPMeaning.Replace(">", "").Replace("<", "").Replace("=", "").Replace(QUOTE, "").Replace("!", "").Replace("?", "").Replace(" ", "")

                Try
                    Client.DownloadFile("https://www.heylingo.com/_audio/" & DownloadID & ".mp3", Environ$("USERPROFILE") & "\Downloads\HeyLingo Audio\" & Language & "\Course " & Page & "\" & JPMeaning & ".mp3")
                    If Page Mod 5 = 0 Then
                        Console.ForegroundColor = ConsoleColor.Yellow
                    ElseIf Page Mod 5 = 1 Then
                        Console.ForegroundColor = ConsoleColor.Cyan
                    ElseIf Page Mod 5 = 2 Then
                        Console.ForegroundColor = ConsoleColor.Magenta
                    ElseIf Page Mod 5 = 3 Then
                        Console.ForegroundColor = ConsoleColor.Green
                    ElseIf Page Mod 5 = 4 Then
                        Console.ForegroundColor = ConsoleColor.Blue
                    End If
                    Success += 1
                    Console.WriteLine("Downloaded " & DownloadID & ": " & JPMeaning)
                    Console.ForegroundColor = ConsoleColor.White
                Catch
                    JPMeaning = DownloadID
                    Try
                        Client.DownloadFile("https://www.heylingo.com/_audio/" & DownloadID, Environ$("USERPROFILE") & "\Downloads\HeyLingo Audio\" & Language & "\Course " & Page & "\" & JPMeaning.Replace(".m4a", ".mp3"))
                        Success += 1
                        Console.ForegroundColor = ConsoleColor.Green
                        Console.WriteLine("Downloaded " & DownloadID.Replace(".m4a", ".mp3"))
                        Console.ForegroundColor = ConsoleColor.White
                    Catch
                        Failed += 1
                        Console.ForegroundColor = ConsoleColor.DarkRed
                        Console.WriteLine("Failed to download " & DownloadID.Replace(".m4a", ".mp3"))
                        Console.ForegroundColor = ConsoleColor.White
                    End Try
                End Try
            Next
        Next

        Total = (EndPage - StartPage + 1) * 40
        Console.WriteLine()
        If Success = Total Then
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Downloaded " & Total & " files")
            Console.WriteLine("Audio files are in 'Downloads' in a folder called 'HeyLingo Audio'")
            Console.ForegroundColor = ConsoleColor.White
        ElseIf Failed > Total - 1 Then
        ElseIf Failed > 0 Then
            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Downloaded " & Success & " out of " & Total & " files")
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine(Failed & " audio files failed to download")
            Console.ForegroundColor = ConsoleColor.White
        End If

        Console.WriteLine()
        Console.WriteLine("Would you like to be taken to the folder?")
        UserInput = Console.ReadLine.Trim.ToLower
        If UserInput = "yes" Then
            Process.Start("explorer.exe", Environ$("USERPROFILE") & "\Downloads\HeyLingo Audio\" & Language)
        End If

        Main()
        Main()
    End Sub
    Sub KanjiInfo(ByVal ActualSearch, ByVal Mode)
        'Mode:
        '1 = Just KanjiInfo
        '2 = Display
        ActualSearch = ActualSearch.replace(" ", "")
        Const QUOTE = """"
        Dim WordURL, WordHTML, CurrentKanji As String
        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8

        For Checker = 1 To ActualSearch.length
            If WanaKana.IsKanji(Mid(ActualSearch, Checker, 1)) = False Then
                If Checker > ActualSearch.length Then
                    Continue For
                End If
                Try
                    ActualSearch = ActualSearch.replace(Mid(ActualSearch, Checker, 1), "")
                Catch
                    Continue For
                End Try
                Checker -= 1
                If Checker = 0 Then
                    Checker = 1
                End If
            End If
        Next
        If WanaKana.IsKanji(Mid(ActualSearch, 1, 1)) = False Then
            ActualSearch = Mid(ActualSearch, 2)
        End If

        Try
            'Kanji, meanings and reading extract. First open the "/word" page and then extracts instead of extracting from "/search":
            WordURL = ("https://jisho.org/search/" & ActualSearch & "%20%23kanji")
            WordHTML = Client.DownloadString(New Uri(WordURL))
        Catch
            Exit Sub
        End Try

        Dim Snip1, Snip2 As Integer

        Snip1 = WordHTML.IndexOf("data-area-name=" & QUOTE & "print" & QUOTE)
        WordHTML = Mid(WordHTML, Snip1 + 10)

        Dim KanjiString As String = ""
        Snip1 = WordHTML.IndexOf("main_results")
        KanjiString = Mid(WordHTML, Snip1 + 10)
        Snip2 = WordHTML.IndexOf("Jisho.org is lovingly crafted by")
        KanjiString = Left(WordHTML, Snip2 + 25)

        Dim Kanjis() As String
        Kanjis = KanjiString.Split(New String() {"kanji details"}, StringSplitOptions.RemoveEmptyEntries)

        Dim ReadingsGroup, DefinitionsGroup, JLPT, FoundInGroup As String
        Dim KanjiInfo(ActualSearch.length - 1, 5)
        Dim RadicalGroups(0) As String
        'Readings:
        '(X, Y)
        'X = Kanji
        '0Y = Kun    1Y = On    2Y = Meaning    3Y = JLPT    4Y = On Found in    5Y = Kun Found in
        Dim StringTemp, StringTemp2, StringTemp3 As String
        Dim CountInput As String
        Dim ToCount As String = ","
        Dim Occurrences As Integer = 0

        Dim FileReader As String = ""
        Dim TextString() As String
        FileReader = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
        TextString = FileReader.Split(vbCrLf)
        Dim KunReadingsList As New List(Of String)(New String() {""})
        Dim KunReadings() As String

        For KanjiLoop = 0 To ActualSearch.length - 1
            CurrentKanji = Mid(ActualSearch, KanjiLoop + 1, 1)

            'Getting the section that is just readings for each kanji
            Try
                Snip1 = Kanjis(KanjiLoop).IndexOf("anji-details__main-readings") 'Getting the position just before the snip for the readings group
            Catch
                Continue For
            End Try
            If Snip1 = -1 Then
                Continue For
            End If
            ReadingsGroup = Mid(Kanjis(KanjiLoop), Snip1)
            Snip2 = ReadingsGroup.IndexOf("small-12 large-5 columns") 'Getting the position just after the snip for the readings group
            ReadingsGroup = Left(ReadingsGroup, Snip2)
            StringTemp3 = ReadingsGroup 'Holds both Kun and On (if both are included)

            'TEMP
            KanjiInfo(KanjiLoop, 0) = CurrentKanji.Replace("&#39;", "")

            If ReadingsGroup.IndexOf("Kun:") <> -1 Then 'If the kanji has at least one kun reading
                'Making StringTemp a string holding just kun readings
                StringTemp = ReadingsGroup
                Snip2 = StringTemp.IndexOf("On:")
                If Snip2 <> -1 Then 'If there is a Kun reading and On reading
                    StringTemp = Left(StringTemp, Snip2) 'This is holding just Kun readings
                End If

                Do Until StringTemp.IndexOf("<a href=") = -1 'Do until no more Kun readings
                    'Finding the start and end of a reading including the link (which won't be used but is needed to trim the reading):
                    Snip1 = StringTemp.IndexOf("<a href=")
                    Snip2 = StringTemp.IndexOf("</a>") + 4

                    If Snip1 = -1 Or Snip2 = 3 Then
                        Continue Do
                    End If

                    StringTemp2 = Mid(StringTemp, Snip1, Snip2 + 1 - Snip1)

                    StringTemp = StringTemp.Replace(StringTemp2, "") 'Getting rid of the reading from the group

                    'Getting the actual reading:
                    StringTemp2 = Mid(StringTemp2, 5) 'Getting rid of the < at the start of the whole link trim
                    Snip1 = StringTemp2.IndexOf(">")
                    Snip2 = StringTemp2.IndexOf("<")
                    StringTemp2 = Mid(StringTemp2, Snip1 + 2, Snip2 - 1 - Snip1)

                    KanjiInfo(KanjiLoop, 1) &= StringTemp2 & "、"
                Loop
                KanjiInfo(KanjiLoop, 1) = "Kun: " & KanjiInfo(KanjiLoop, 1)
            Else 'If the kanji doesn't have at least one kun reading
                KanjiInfo(KanjiLoop, 1) = "Kun:"
            End If

            'If simiplified Kun Readings are on:
            Try
                If TextString(3).Contains("1") = True Then
                    KunReadings = KanjiInfo(KanjiLoop, 1).split("、")
                    KunReadings(0) = KunReadings(0).Replace("Kun: ", "")
                    For Reading = 0 To KunReadings.Length - 1
                        KunReadingsList.Add(KunReadings(Reading))
                    Next
                    For Reading = 0 To KunReadingsList.Count - 1
                        Try
                            If KunReadingsList(Reading) = "" Then 'Remove list items that are nothing
                                KunReadingsList.RemoveAt(Reading)
                                Reading -= 1
                            End If
                        Catch ex As Exception
                        End Try
                    Next
                    For Reading = 0 To KunReadingsList.Count - 1
                        'Snip1 is used for getting only before the '.' or '-' (if it exists)
                        Snip1 = KunReadingsList(Reading).IndexOf(".")
                        If Snip1 = -1 Then
                            Snip1 = KunReadingsList(Reading).IndexOf("-")
                        End If
                        Try
                            KunReadingsList(Reading) = Left(KunReadingsList(Reading), Snip1)
                        Catch ex As Exception
                        End Try
                    Next

                    ReDim KunReadings(0)
                    Dim Match As Boolean = False
                    For Reading = 0 To KunReadingsList.Count - 1
                        For Check = 0 To KunReadings.Length - 1
                            If KunReadings(Check) = KunReadingsList(Reading) Then
                                Match = True
                            End If
                        Next
                        If Match = False Then
                            Array.Resize(KunReadings, KunReadings.Length + 1)
                            KunReadings(KunReadings.Length - 1) = KunReadingsList(Reading)
                        End If
                        Match = False
                    Next
                    Dim KunString As String = ""
                    For Kun = 1 To KunReadings.Length - 1
                        If Not Kun = KunReadings.Length - 1 Then
                            KunString &= KunReadings(Kun) & "、"
                        Else
                            KunString &= KunReadings(Kun)
                        End If
                    Next
                    KanjiInfo(KanjiLoop, 1) = "Kun: " & KunString
                End If
            Catch ex As Exception
                If DebugMode = True Then
                    Console.WriteLine(ex.Message)
                End If
            End Try

            'On scraps
            If ReadingsGroup.IndexOf("On:") <> -1 Then 'If there is an on reading
                StringTemp = StringTemp3
                Snip2 = StringTemp.IndexOf("On:")
                StringTemp = Mid(StringTemp, Snip2)
                Do Until StringTemp.IndexOf("<a href=") = -1 'Do until no more On readings
                    'Finding the start and end of a reading including the link (which won't be used but is needed to trim the reading):
                    Snip1 = StringTemp.IndexOf("<a href=")
                    Snip2 = StringTemp.IndexOf("</a>") + 4

                    If Snip1 = -1 Or Snip2 = 3 Then
                        Continue Do
                    End If

                    StringTemp2 = Mid(StringTemp, Snip1, Snip2 + 1 - Snip1)

                    StringTemp = StringTemp.Replace(StringTemp2, "").Replace("&#39;", "") 'Getting rid of the reading from the group

                    'Getting the actual reading:
                    StringTemp2 = Mid(StringTemp2, 5) 'Getting rid of the < at the start of the whole link trim
                    Snip1 = StringTemp2.IndexOf(">")
                    Snip2 = StringTemp2.IndexOf("<")
                    StringTemp2 = Mid(StringTemp2, Snip1 + 2, Snip2 - 1 - Snip1)

                    KanjiInfo(KanjiLoop, 2) &= StringTemp2 & "、"
                Loop
                KanjiInfo(KanjiLoop, 2) = "On: " & KanjiInfo(KanjiLoop, 2)
            Else 'If the kanji doesn't have at least one kun reading
                KanjiInfo(KanjiLoop, 2) = "On:"
            End If

            'Scraping the definition(s) of the kanjis:
            DefinitionsGroup = Kanjis(KanjiLoop).Replace("&amp;", "")

            'Making a small snip that contains mostly the definitions (the definitions are in a comma list with no separators)
            Snip1 = DefinitionsGroup.IndexOf("kanji-details__main-meanings") + 10 'Getting the position just before the snip for the readings group, the + amount is because we are looking for the same string to get the end of the cut
            DefinitionsGroup = Mid(DefinitionsGroup, Snip1)
            Snip2 = DefinitionsGroup.IndexOf("kanji-details__main-readings") 'Getting the position just after the snip for the readings group
            DefinitionsGroup = Left(DefinitionsGroup, Snip2)

            'Getting just the comma list of definitions:
            Snip1 = DefinitionsGroup.IndexOf(vbLf) + 8 'Getting the position just before the snip for the readings group, the + amount is because we are looking for the same string to get the end of the cut
            DefinitionsGroup = Mid(DefinitionsGroup, Snip1)
            Snip2 = DefinitionsGroup.IndexOf(vbLf) 'Getting the position just after the snip for the readings group
            DefinitionsGroup = Left(DefinitionsGroup, Snip2)
            Try 'Making the definition of the Kanji not have an overly long bracket
                Snip1 = DefinitionsGroup.IndexOf("(")
                Snip2 = DefinitionsGroup.IndexOf(")")
                StringTemp = "(" & Mid(DefinitionsGroup, Snip1 + 2, Snip2 - 1 - Snip1) & ")"

                If StringTemp.Length > 25 Then
                    DefinitionsGroup = DefinitionsGroup.Replace(StringTemp, "").Replace("&amp;", "").Replace("&#39;", "")
                End If
            Catch
            End Try
            KanjiInfo(KanjiLoop, 0) &= " - " & DefinitionsGroup.Replace("&amp;", "").Replace("&#39;", "") 'Adding definitions list to the definition part of the array


            'Getting the JLPT level:
            Try
                JLPT = Kanjis(KanjiLoop)
                Snip1 = JLPT.IndexOf("JLPT level") + 10 'Getting the position just before the snip for the readings group, the + amount is because we are looking for the same string to get the end of the cut
                JLPT = Mid(JLPT, Snip1)
                Snip2 = JLPT.IndexOf("</strong>") 'Getting the position just after the snip for the readings group
                JLPT = Left(JLPT, Snip2)
                Snip1 = JLPT.IndexOf("<strong>") + 9 'Getting the position just before the snip for the readings group, the + amount is because we are looking for the same string to get the end of the cut
                JLPT = Mid(JLPT, Snip1)
                JLPT = "JLPT Level: " & JLPT
            Catch
                JLPT = "JLPT Level:"
            End Try
            KanjiInfo(KanjiLoop, 3) = JLPT 'Adding JLPT information to the information array

            'Getting "found in" words:
            'StringTemp = On
            'StringTemp2 = Kun
            'Getting just the Reading Compounds section of the HTML
            FoundInGroup = Kanjis(KanjiLoop)

            Snip1 = FoundInGroup.IndexOf("row compounds") + 1
            FoundInGroup = Mid(FoundInGroup, Snip1)
            Snip2 = FoundInGroup.IndexOf("row kanji-details--section")
            FoundInGroup = Left(FoundInGroup, Snip2)


            'Getting On compounds
            If FoundInGroup.IndexOf("On reading compounds") <> -1 Then 'If the kanji has at least one On compound
                'Making StringTemp a string holding just On compounds:
                StringTemp = FoundInGroup
                Snip2 = StringTemp.IndexOf("Kun reading compounds")
                If Snip2 <> -1 Then 'If there is a Kun compound (and On)
                    StringTemp = Left(StringTemp, Snip2) 'This is holding just Kun compounds
                End If 'If there isn't an Kun reading we don't need to snip (we wouldn't be able to anyway)
                'We now have just the On compounds

                Snip1 = StringTemp.IndexOf("<li>")
                StringTemp = Mid(StringTemp, Snip1 + 8)

                'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                Snip2 = StringTemp.IndexOf("</li>")
                StringTemp3 = Left(StringTemp, Snip2 - 1)
                StringTemp3 = StringTemp3.Replace(vbLf, "")
                StringTemp3 = StringTemp3.Replace("  【", "【")
                StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")

                If StringTemp.IndexOf("<li>") <> -1 And Mode = 1 Then 'if there is another On reading compound
                    StringTemp3 &= "|"
                    Snip1 = StringTemp.IndexOf("<li>")
                    StringTemp = Mid(StringTemp, Snip1 + 8)

                    'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                    Snip2 = StringTemp.IndexOf("</li>")
                    StringTemp3 &= Left(StringTemp, Snip2 - 1)
                    StringTemp3 = StringTemp3.Replace(vbLf, "")
                    StringTemp3 = StringTemp3.Replace("  【", "【")
                    StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                    StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")
                    If StringTemp.IndexOf("<li>") <> -1 Then 'if there is another On reading compound
                        StringTemp3 &= "|"
                        Snip1 = StringTemp.IndexOf("<li>")
                        StringTemp = Mid(StringTemp, Snip1 + 8)

                        'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                        Snip2 = StringTemp.IndexOf("</li>")
                        StringTemp3 &= Left(StringTemp, Snip2 - 1)
                        StringTemp3 = StringTemp3.Replace(vbLf, "")
                        StringTemp3 = StringTemp3.Replace("  【", "【")
                        StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                        StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")
                        If StringTemp.IndexOf("<li>") <> -1 Then 'if there is another On reading compound
                            StringTemp3 &= "|"
                            Snip1 = StringTemp.IndexOf("<li>")
                            StringTemp = Mid(StringTemp, Snip1 + 8)

                            'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                            Snip2 = StringTemp.IndexOf("</li>")
                            StringTemp3 &= Left(StringTemp, Snip2 - 1)
                            StringTemp3 = StringTemp3.Replace(vbLf, "")
                            StringTemp3 = StringTemp3.Replace("  【", "【")
                            StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                            StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")
                            If StringTemp.IndexOf("<li>") <> -1 Then 'if there is another On reading compound
                                StringTemp3 &= "|"
                                Snip1 = StringTemp.IndexOf("<li>")
                                StringTemp = Mid(StringTemp, Snip1 + 8)

                                'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                                Snip2 = StringTemp.IndexOf("</li>")
                                StringTemp3 &= Left(StringTemp, Snip2 - 1)
                                StringTemp3 = StringTemp3.Replace(vbLf, "")
                                StringTemp3 = StringTemp3.Replace("  【", "【")
                                StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                                StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")
                            End If
                        End If
                    End If
                End If

                'Getting rid of overly long kanji definitions
                CountInput = StringTemp3


                KanjiInfo(KanjiLoop, 4) = "On Reading Compound: " & StringTemp3

            Else 'If the kanji doesn't have at least one kun reading
                StringTemp = ""
                KanjiInfo(KanjiLoop, 4) = "No Onyomi compounds"
            End If

            'Getting Kun compounds
            If FoundInGroup.IndexOf("Kun reading compounds") <> -1 Then 'If the kanji has at least one Kun compound
                StringTemp2 = FoundInGroup
                If FoundInGroup.IndexOf("Kun reading compounds") <> -1 Then 'If the kanji also has an On compound
                    Snip2 = StringTemp2.IndexOf("Kun reading compounds")
                    StringTemp2 = Mid(StringTemp2, Snip2)
                End If
                'We now have just the Kun compounds

                Snip1 = StringTemp2.IndexOf("<li>")
                StringTemp2 = Mid(StringTemp2, Snip1 + 8)

                'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                Snip2 = StringTemp2.IndexOf("</li>")
                StringTemp3 = Left(StringTemp2, Snip2 - 1)
                StringTemp3 = StringTemp3.Replace(vbLf, "")
                StringTemp3 = StringTemp3.Replace("  【", "【")
                StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")

                If StringTemp2.IndexOf("<li>") <> -1 And Mode = 1 Then
                    StringTemp3 &= "|"
                    Snip1 = StringTemp2.IndexOf("<li>")
                    StringTemp2 = Mid(StringTemp2, Snip1 + 8)

                    'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                    Snip2 = StringTemp2.IndexOf("</li>")
                    StringTemp3 &= Left(StringTemp2, Snip2 - 1)
                    StringTemp3 = StringTemp3.Replace(vbLf, "")
                    StringTemp3 = StringTemp3.Replace("  【", "【")
                    StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                    StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")
                    If StringTemp2.IndexOf("<li>") <> -1 Then
                        StringTemp3 &= "|"
                        Snip1 = StringTemp2.IndexOf("<li>")
                        StringTemp2 = Mid(StringTemp2, Snip1 + 8)

                        'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                        Snip2 = StringTemp2.IndexOf("</li>")
                        StringTemp3 &= Left(StringTemp2, Snip2 - 1)
                        StringTemp3 = StringTemp3.Replace(vbLf, "")
                        StringTemp3 = StringTemp3.Replace("  【", "【")
                        StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                        StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")
                        If StringTemp2.IndexOf("<li>") <> -1 Then
                            StringTemp3 &= "|"
                            Snip1 = StringTemp2.IndexOf("<li>")
                            StringTemp2 = Mid(StringTemp2, Snip1 + 8)

                            'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                            Snip2 = StringTemp2.IndexOf("</li>")
                            StringTemp3 &= Left(StringTemp2, Snip2 - 1)
                            StringTemp3 = StringTemp3.Replace(vbLf, "")
                            StringTemp3 = StringTemp3.Replace("  【", "【")
                            StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                            StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")
                        End If
                    End If
                End If

                KanjiInfo(KanjiLoop, 5) = "Kun Reading Compound: " & StringTemp3

            Else
                StringTemp2 = ""
                KanjiInfo(KanjiLoop, 5) = "No Kunyomi compounds"
            End If

            Dim RadicalFails As Integer = 0
            'Kanji and radicals
            Dim KanjiFile As String = ""
            Try
                KanjiFile = My.Computer.FileSystem.ReadAllText("Kanji.txt")
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("A file wasn't found in the program files. You will have to reinstall the program from Github")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()
                Main()
            End Try

            Dim RadicalFile As String = ""
            Try
                RadicalFile = My.Computer.FileSystem.ReadAllText("Radicals.txt")
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("A file wasn't found in the program files. You will have to reinstall the program from Github")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadLine()
                Main()
            End Try
            Dim Correct As Boolean = False
            Dim Rows() As String
            Rows = KanjiFile.Split(vbCrLf)
            Dim Row() As String
            Dim R As Integer = 0
            Dim RadicalsUsed As String = ""
            Do Until Correct = True
                Row = Rows(R).Split(vbTab)
                Row(0) = Row(0).Trim
                If Row(0) = CurrentKanji Then
                    RadicalsUsed = Row(2)
                    Correct = True
                    Continue Do
                End If

                R += 1
            Loop
            Correct = False

            Dim Radicals() As String
            RadicalsUsed = RadicalsUsed.Trim
            RadicalsUsed = RadicalsUsed.Replace(" + ", "+")
            Radicals = RadicalsUsed.Split("+")

            Rows = RadicalFile.Split(vbCrLf)

            Correct = False
            For Radical = 0 To Radicals.Length - 1
                Correct = False
                R = 0
                Do Until Correct = True
                    Try
                        Row = Rows(R).Split(vbTab)
                    Catch
                        RadicalFails += 1 '/i 途親細辱
                        If RadicalFails > Radicals.Length - 1 Then
                            RadicalsUsed = ""
                        End If

                        Correct = True
                        Continue Do
                    End Try

                    Row(0) = Row(0).Trim
                    If Row(1).ToLower.IndexOf(Radicals(Radical).ToLower) <> -1 Or Row(1).ToLower = Radicals(Radical).ToLower Then
                        Correct = True
                        Radicals(Radical) = Row(0)
                        Continue Do
                    End If

                    R += 1
                Loop
            Next

            For Radical = 0 To Radicals.Length - 1
                RadicalGroups(KanjiLoop) &= Radicals(Radical) & " + "
            Next
            RadicalGroups(KanjiLoop) = Left(RadicalGroups(KanjiLoop), RadicalGroups(KanjiLoop).Length - 3)

            RadicalGroups(KanjiLoop) &= " (" & RadicalsUsed.Replace("+", " + ") & ")"

            Array.Resize(RadicalGroups, RadicalGroups.Length + 1)
            'Next Kanji
        Next
        Array.Resize(RadicalGroups, RadicalGroups.Length - 1)
        '------------------------
        'Printing the infomation
        If Mode = 1 Then
            Console.Clear()
            Console.WriteLine("Kanji information for 「" & ActualSearch & "」")
            Console.WriteLine()
        End If


        Dim TextWriter As System.IO.StreamWriter
        Try
            FileReader = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
        Catch
            File.Create("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt").Dispose() 'This text file will store user preferences
            TextWriter = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
            TextWriter.WriteLine("Default 's=' parameter:4")
            TextWriter.WriteLine("Maximum definitions shown:6")
            TextWriter.WriteLine("Reading shown first:kun")
            TextWriter.Close()
        End Try
        TextString = FileReader.Split(vbCrLf)
        Dim OnFirst As Boolean = False
        If TextString(2).IndexOf("on") <> -1 Then
            OnFirst = True
        End If

        Dim ArrayPrint() As String
        For Printer = 0 To ActualSearch.length - 1
            For Replacer = 0 To 5
                KanjiInfo(Printer, Replacer) = KanjiInfo(Printer, Replacer).replace("&quot;", """").Replace("&#39;", "")
            Next
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine(KanjiInfo(Printer, 0))
            Console.BackgroundColor = ConsoleColor.Black
            If OnFirst = False Then
                Console.WriteLine(KanjiInfo(Printer, 1)) 'kun
                Console.WriteLine(KanjiInfo(Printer, 2)) 'on
            Else
                Console.WriteLine(KanjiInfo(Printer, 2)) 'on
                Console.WriteLine(KanjiInfo(Printer, 1)) 'kun
            End If

            Console.WriteLine(KanjiInfo(Printer, 3))
            If RadicalGroups(Printer).IndexOf("()") = -1 Then
                RadicalGroups(Printer) = RadicalGroups(Printer).Replace("https://www.wanikani.com/radicals/", "")
                Console.WriteLine("Radicals used: " & RadicalGroups(Printer))
            End If
            Dim ReadingNumber As Integer
            For RD = 0 To 1
                If RD = 0 Then
                    If OnFirst = True Then
                        ReadingNumber = 4
                    Else
                        ReadingNumber = 5
                    End If
                Else
                    If ReadingNumber = 4 Then
                        ReadingNumber = 5
                    Else
                        ReadingNumber = 4
                    End If
                End If

                If KanjiInfo(Printer, ReadingNumber).indexof("|") = -1 Then 'Kun
                    Try
                        'Shortening
                        CountInput = KanjiInfo(Printer, ReadingNumber)
                        Occurrences = ((CountInput.Length - CountInput.Replace(ToCount, String.Empty).Length) / ToCount.Length) + 1
                        If Occurrences > 5 Then
                            Do Until Occurrences = 7
                                Snip2 = KanjiInfo(Printer, ReadingNumber).LastIndexOf(",")
                                KanjiInfo(Printer, ReadingNumber) = Left(KanjiInfo(Printer, ReadingNumber), Snip2)
                                Occurrences -= 1
                            Loop
                        End If
                        If KanjiInfo(Printer, ReadingNumber).Length > 50 Or Occurrences < 3 Then
                            Try
                                Do Until KanjiInfo(Printer, ReadingNumber).Length < 51 Or Occurrences = 2
                                    Snip2 = KanjiInfo(Printer, ReadingNumber).LastIndexOf(",")
                                    KanjiInfo(Printer, ReadingNumber) = Left(KanjiInfo(Printer, ReadingNumber), Snip2)
                                    Occurrences -= 1
                                Loop
                            Catch
                            End Try
                        End If
                    Catch
                    End Try

                    Console.WriteLine(KanjiInfo(Printer, ReadingNumber))
                Else
                    ArrayPrint = KanjiInfo(Printer, ReadingNumber).split("|")
                    ArrayPrint(0) = ArrayPrint(0).Replace("Kun Reading Compound: ", "").Replace("On Reading Compound: ", "")
                    Console.BackgroundColor = ConsoleColor.DarkGray
                    If ReadingNumber = 4 Then
                        Console.WriteLine("On Reading Compounds:")
                    Else
                        Console.WriteLine("Kun Reading Compounds:")
                    End If

                    Console.BackgroundColor = ConsoleColor.Black
                    For Compound = 0 To ArrayPrint.Length - 1

                        ''Shortening info''''''''''''''''''''
                        Try
                            Snip1 = ArrayPrint(Compound).IndexOf("(")
                            Snip2 = ArrayPrint(Compound).IndexOf(")")
                            StringTemp = "(" & Mid(ArrayPrint(Compound), Snip1 + 2, Snip2 - 1 - Snip1) & ")"

                            If StringTemp.Length > 50 Then
                                ArrayPrint(Compound) = ArrayPrint(Compound).Replace(StringTemp, "")
                            End If
                        Catch
                        End Try

                        CountInput = ArrayPrint(Compound)
                        Occurrences = ((CountInput.Length - CountInput.Replace(ToCount, String.Empty).Length) / ToCount.Length) + 1
                        If Occurrences > 6 Then
                            Do Until Occurrences = 5
                                Snip2 = ArrayPrint(Compound).LastIndexOf(",")
                                ArrayPrint(Compound) = Left(ArrayPrint(Compound), Snip2)
                                Occurrences -= 1
                            Loop
                        End If
                        If ArrayPrint(Compound).Length > 70 And Occurrences > 2 Then
                            Try
                                Do Until ArrayPrint(Compound).Length < 60 Or Occurrences < 5
                                    Snip2 = ArrayPrint(Compound).LastIndexOf(",")
                                    ArrayPrint(Compound) = Left(ArrayPrint(Compound), Snip2)
                                    Occurrences -= 1
                                Loop
                            Catch
                            End Try
                        End If
                        ''''''''''''''''''''end of shortening info

                        Console.WriteLine(ArrayPrint(Compound))
                    Next
                End If
            Next

            Console.WriteLine()
            Next
            If Mode = 2 Then
            Exit Sub
        End If

        Console.WriteLine("Done!")
        Console.ReadLine()
        Main()
    End Sub
    Sub KanjiDisplay(ByVal ActualSearchWord, ByVal WordLink, ByVal SelectedDefinition, ByVal FoundTypes, ByVal DisplayType, ByVal Furigana)
        'Display Type: 1 = with LastRequest, 2 = without LastRequest
        Const QUOTE = """"
        Dim WordURL, WordHTML, CurrentKanji As String
        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8

        Dim FullWord As String = ActualSearchWord

        For Checker = 1 To ActualSearchWord.length
            If WanaKana.IsKanji(Mid(ActualSearchWord, Checker, 1)) = False Then
                Try
                    ActualSearchWord = ActualSearchWord.replace(Mid(ActualSearchWord, Checker, 1), "")
                Catch ex As Exception
                End Try
            End If
        Next

        Try
            'Kanji, meanings and reading extract. First open the "/word" page and then extracts instead of extracting from "/search":
            WordURL = ("https://jisho.org/search/" & ActualSearchWord & "%20%23kanji")
            WordHTML = Client.DownloadString(New Uri(WordURL))
        Catch
            Exit Sub
        End Try

        Dim Snip1, Snip2 As Integer

        Snip1 = WordHTML.IndexOf("data-area-name=" & QUOTE & "print" & QUOTE)
        WordHTML = Mid(WordHTML, Snip1 + 10)

        Dim KanjiString As String = ""
        Snip1 = WordHTML.IndexOf("main_results")
        KanjiString = Mid(WordHTML, Snip1 + 10)
        Snip2 = WordHTML.IndexOf("Jisho.org is lovingly crafted by")
        KanjiString = Left(WordHTML, Snip2 + 25)

        Dim Kanjis() As String
        Kanjis = KanjiString.Split(New String() {"kanji details"}, StringSplitOptions.RemoveEmptyEntries)

        Dim ReadingsGroup, DefinitionsGroup, JLPT, FoundInGroup As String
        Dim ActualInfo(ActualSearchWord.length - 1, 5)
        'Readings:
        '(X, Y)
        'X = Kanji
        '0Y = Kun    1Y = On    2Y = Meaning    3Y = JLPT    4Y = On Found in    5Y = Kun Found in
        Dim StringTemp, StringTemp2, StringTemp3 As String
        Dim CountInput As String
        Dim ToCount As String = ","
        Dim Occurrences As Integer = 0

        Dim FileReader As String = ""
        Dim TextString() As String
        FileReader = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
        TextString = FileReader.Split(vbCrLf)
        Dim KunReadingsList As New List(Of String)(New String() {""})
        Dim KunReadings() As String

        For KanjiLoop = 0 To ActualSearchWord.length - 1
            CurrentKanji = Mid(ActualSearchWord, KanjiLoop + 1, 1)

            'Getting the section that is just readings for each kanji
            Try
                Snip1 = Kanjis(KanjiLoop).IndexOf("anji-details__main-readings") 'Getting the position just before the snip for the readings group
            Catch
                Continue For
            End Try
            If Snip1 = -1 Then
                Continue For
            End If
            ReadingsGroup = Mid(Kanjis(KanjiLoop), Snip1)
            Snip2 = ReadingsGroup.IndexOf("small-12 large-5 columns") 'Getting the position just after the snip for the readings group
            ReadingsGroup = Left(ReadingsGroup, Snip2)
            StringTemp3 = ReadingsGroup 'Holds both Kun and On (if both are included)

            ActualInfo(KanjiLoop, 0) = CurrentKanji

            If ReadingsGroup.IndexOf("Kun:") <> -1 Then 'If the kanji has at least one kun reading
                'Making StringTemp a string holding just kun readings
                StringTemp = ReadingsGroup
                Snip2 = StringTemp.IndexOf("On:")
                If Snip2 <> -1 Then 'If there is a Kun reading and On reading
                    StringTemp = Left(StringTemp, Snip2) 'This is holding just Kun readings
                End If

                Do Until StringTemp.IndexOf("<a href=") = -1 'Do until no more Kun readings
                    'Finding the start and end of a reading including the link (which won't be used but is needed to trim the reading):
                    Snip1 = StringTemp.IndexOf("<a href=")
                    Snip2 = StringTemp.IndexOf("</a>") + 4

                    If Snip1 = -1 Or Snip2 = 3 Then
                        Continue Do
                    End If

                    StringTemp2 = Mid(StringTemp, Snip1, Snip2 + 1 - Snip1)

                    StringTemp = StringTemp.Replace(StringTemp2, "") 'Getting rid of the reading from the group

                    'Getting the actual reading:
                    StringTemp2 = Mid(StringTemp2, 5) 'Getting rid of the < at the start of the whole link trim
                    Snip1 = StringTemp2.IndexOf(">")
                    Snip2 = StringTemp2.IndexOf("<")
                    StringTemp2 = Mid(StringTemp2, Snip1 + 2, Snip2 - 1 - Snip1)

                    ActualInfo(KanjiLoop, 1) &= StringTemp2 & "、"
                Loop
                ActualInfo(KanjiLoop, 1) = "Kun: " & ActualInfo(KanjiLoop, 1)
            Else 'If the kanji doesn't have at least one kun reading
                ActualInfo(KanjiLoop, 1) = "Kun:"
            End If
            If Right(ActualInfo(KanjiLoop, 1), 1) = "、" Then
                ActualInfo(KanjiLoop, 1) = Left(ActualInfo(KanjiLoop, 1), ActualInfo(KanjiLoop, 1).length - 1)
            End If

            'If simiplified Kun Readings are on:
            Try
                If TextString(3).Contains("1") = True Then
                    KunReadings = ActualInfo(KanjiLoop, 1).split("、")
                    KunReadings(0) = KunReadings(0).Replace("Kun: ", "")
                    For Reading = 0 To KunReadings.Length - 1
                        KunReadingsList.Add(KunReadings(Reading))
                    Next
                    For Reading = 0 To KunReadingsList.Count - 1
                        Try
                            If KunReadingsList(Reading) = "" Then 'Remove list items that are nothing
                                KunReadingsList.RemoveAt(Reading)
                                Reading -= 1
                            End If
                        Catch ex As Exception
                        End Try
                    Next
                    For Reading = 0 To KunReadingsList.Count - 1
                        'Snip1 is used for getting only before the '.' or '-' (if it exists)
                        Snip1 = KunReadingsList(Reading).IndexOf(".")
                        If Snip1 = -1 Then
                            Snip1 = KunReadingsList(Reading).IndexOf("-")
                        End If
                        Try
                            KunReadingsList(Reading) = Left(KunReadingsList(Reading), Snip1)
                        Catch ex As Exception
                        End Try
                    Next

                    ReDim KunReadings(0)
                    Dim Match As Boolean = False
                    For Reading = 0 To KunReadingsList.Count - 1
                        For Check = 0 To KunReadings.Length - 1
                            If KunReadings(Check) = KunReadingsList(Reading) Then
                                Match = True
                            End If
                        Next
                        If Match = False Then
                            Array.Resize(KunReadings, KunReadings.Length + 1)
                            KunReadings(KunReadings.Length - 1) = KunReadingsList(Reading)
                        End If
                        Match = False
                    Next
                    Dim KunString As String = ""
                    For Kun = 1 To KunReadings.Length - 1
                        If Not Kun = KunReadings.Length - 1 Then
                            KunString &= KunReadings(Kun) & "、"
                        Else
                            KunString &= KunReadings(Kun)
                        End If
                    Next
                    ActualInfo(KanjiLoop, 1) = "Kun: " & KunString
                End If
            Catch ex As Exception
                If DebugMode = True Then
                    Console.WriteLine(ex.Message)
                End If
            End Try

            'On scraps
            If ReadingsGroup.IndexOf("On:") <> -1 Then 'If there is an on reading
                StringTemp = StringTemp3
                Snip2 = StringTemp.IndexOf("On:")
                StringTemp = Mid(StringTemp, Snip2)
                Do Until StringTemp.IndexOf("<a href=") = -1 'Do until no more On readings
                    'Finding the start and end of a reading including the link (which won't be used but is needed to trim the reading):
                    Snip1 = StringTemp.IndexOf("<a href=")
                    Snip2 = StringTemp.IndexOf("</a>") + 4

                    If Snip1 = -1 Or Snip2 = 3 Then
                        Continue Do
                    End If

                    StringTemp2 = Mid(StringTemp, Snip1, Snip2 + 1 - Snip1)

                    StringTemp = StringTemp.Replace(StringTemp2, "") 'Getting rid of the reading from the group

                    'Getting the actual reading:
                    StringTemp2 = Mid(StringTemp2, 5) 'Getting rid of the < at the start of the whole link trim
                    Snip1 = StringTemp2.IndexOf(">")
                    Snip2 = StringTemp2.IndexOf("<")
                    StringTemp2 = Mid(StringTemp2, Snip1 + 2, Snip2 - 1 - Snip1)

                    ActualInfo(KanjiLoop, 2) &= StringTemp2 & "、"
                Loop
                ActualInfo(KanjiLoop, 2) = "On: " & ActualInfo(KanjiLoop, 2)
            Else 'If the kanji doesn't have at least one kun reading
                ActualInfo(KanjiLoop, 2) = "On:"
            End If
            If Right(ActualInfo(KanjiLoop, 2), 1) = "、" Then
                ActualInfo(KanjiLoop, 2) = Left(ActualInfo(KanjiLoop, 2), ActualInfo(KanjiLoop, 2).length - 1)
            End If

            'Scraping the definition(s) of the kanjis:
            DefinitionsGroup = Kanjis(KanjiLoop).Replace("&amp;", "")

            'Making a small snip that contains mostly the definitions (the definitions are in a comma list with no separators)
            Snip1 = DefinitionsGroup.IndexOf("kanji-details__main-meanings") + 10 'Getting the position just before the snip for the readings group, the + amount is because we are looking for the same string to get the end of the cut
            DefinitionsGroup = Mid(DefinitionsGroup, Snip1)
            Snip2 = DefinitionsGroup.IndexOf("kanji-details__main-readings") 'Getting the position just after the snip for the readings group
            DefinitionsGroup = Left(DefinitionsGroup, Snip2)

            'Getting just the comma list of definitions:
            Snip1 = DefinitionsGroup.IndexOf(vbLf) + 8 'Getting the position just before the snip for the readings group, the + amount is because we are looking for the same string to get the end of the cut
            DefinitionsGroup = Mid(DefinitionsGroup, Snip1)
            Snip2 = DefinitionsGroup.IndexOf(vbLf) 'Getting the position just after the snip for the readings group
            DefinitionsGroup = Left(DefinitionsGroup, Snip2)
            Try 'Making the definition of the Kanji not have an overly long bracket
                Snip1 = DefinitionsGroup.IndexOf("(")
                Snip2 = DefinitionsGroup.IndexOf(")")
                StringTemp = "(" & Mid(DefinitionsGroup, Snip1 + 2, Snip2 - 1 - Snip1) & ")"

                If StringTemp.Length > 25 Then
                    DefinitionsGroup = DefinitionsGroup.Replace(StringTemp, "").Replace("&#39;", "")
                End If
            Catch
            End Try
            ActualInfo(KanjiLoop, 0) &= " - " & DefinitionsGroup.Replace("&amp;", "").Replace("&#39;", "") 'Adding definitions list to the definition part of the array


            'Getting the JLPT level:
            Try
                JLPT = Kanjis(KanjiLoop)
                Snip1 = JLPT.IndexOf("JLPT level") + 10 'Getting the position just before the snip for the readings group, the + amount is because we are looking for the same string to get the end of the cut
                JLPT = Mid(JLPT, Snip1)
                Snip2 = JLPT.IndexOf("</strong>") 'Getting the position just after the snip for the readings group
                JLPT = Left(JLPT, Snip2)
                Snip1 = JLPT.IndexOf("<strong>") + 9 'Getting the position just before the snip for the readings group, the + amount is because we are looking for the same string to get the end of the cut
                JLPT = Mid(JLPT, Snip1)
                JLPT = "JLPT Level: " & JLPT
            Catch
                JLPT = "JLPT Level:"
            End Try
            ActualInfo(KanjiLoop, 3) = JLPT 'Adding JLPT information to the information array

            'Getting "found in" words:
            'StringTemp = On
            'StringTemp2 = Kun
            'Getting just the Reading Compounds section of the HTML
            FoundInGroup = Kanjis(KanjiLoop).Replace("&amp;", "")

            Snip1 = FoundInGroup.IndexOf("row compounds") + 1
            FoundInGroup = Mid(FoundInGroup, Snip1)
            Snip2 = FoundInGroup.IndexOf("row kanji-details--section")
            FoundInGroup = Left(FoundInGroup, Snip2)


            'Getting On compounds
            If FoundInGroup.IndexOf("On reading compounds") <> -1 Then 'If the kanji has at least one On compound
                'Making StringTemp a string holding just On compounds:
                StringTemp = FoundInGroup
                Snip2 = StringTemp.IndexOf("Kun reading compounds")
                If Snip2 <> -1 Then 'If there is a Kun compound (and On)
                    StringTemp = Left(StringTemp, Snip2) 'This is holding just Kun compounds
                End If 'If there isn't an Kun reading we don't need to snip (we wouldn't be able to anyway)
                'We now have just the On compounds

                Snip1 = StringTemp.IndexOf("<li>")
                StringTemp = Mid(StringTemp, Snip1 + 8)

                'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                Snip2 = StringTemp.IndexOf("</li>")
                StringTemp3 = Left(StringTemp, Snip2 - 1)
                StringTemp3 = StringTemp3.Replace(vbLf, "")
                StringTemp3 = StringTemp3.Replace("  【", "【")
                StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")

                Try
                    Snip1 = StringTemp3.IndexOf("(")
                    Snip2 = StringTemp3.IndexOf(")")
                    StringTemp2 = "(" & Mid(StringTemp3, Snip1 + 2, Snip2 - 1 - Snip1) & ")"

                    If StringTemp2.Length > 25 Then
                        StringTemp3 = StringTemp3.Replace(StringTemp2, "")
                    End If
                Catch
                End Try

                CountInput = StringTemp3
                Occurrences = ((CountInput.Length - CountInput.Replace(ToCount, String.Empty).Length) / ToCount.Length) + 1
                If Occurrences > 5 Then
                    Do Until Occurrences = 5
                        Snip2 = StringTemp3.LastIndexOf(",")
                        StringTemp3 = Left(StringTemp3, Snip2)
                        Occurrences -= 1
                    Loop
                End If
                If StringTemp3.Length > 40 And Occurrences > 2 Then
                    Try
                        Do Until StringTemp3.Length < 40 Or Occurrences = 2
                            Snip2 = StringTemp3.LastIndexOf(",")
                            StringTemp3 = Left(StringTemp3, Snip2)
                            Occurrences -= 1
                        Loop
                    Catch
                    End Try
                End If

                ActualInfo(KanjiLoop, 4) = "On Reading Compound: " & StringTemp3
            Else 'If the kanji doesn't have at least one kun reading
                StringTemp = ""
                ActualInfo(KanjiLoop, 4) = "No Onyomi compounds"
            End If

            If FoundInGroup.IndexOf("Kun reading compounds") <> -1 Then 'If the kanji has at least one Kun compound
                StringTemp2 = FoundInGroup
                If FoundInGroup.IndexOf("Kun reading compounds") <> -1 Then 'If the kanji also has an On compound
                    Snip2 = StringTemp2.IndexOf("Kun reading compounds")
                    StringTemp2 = Mid(StringTemp2, Snip2)
                End If
                'We now have just the Kun compounds

                Snip1 = StringTemp2.IndexOf("<li>")
                StringTemp2 = Mid(StringTemp2, Snip1 + 8)

                'Getting the Compound into the right format (Word Compound, 【furigana】- meaning)
                Snip2 = StringTemp2.IndexOf("</li>")
                StringTemp3 = Left(StringTemp2, Snip2 - 1)
                StringTemp3 = StringTemp3.Replace(vbLf, "")
                StringTemp3 = StringTemp3.Replace("  【", "【")
                StringTemp3 = StringTemp3.Replace("】  ", "】 - ")
                StringTemp3 = StringTemp3.Replace("&#39;", "'").Replace("&amp;", "")

                'Getting rid of overly long brackets
                Try
                    Snip1 = StringTemp3.IndexOf("(")
                    Snip2 = StringTemp3.IndexOf(")")
                    StringTemp = "(" & Mid(StringTemp3, Snip1 + 2, Snip2 - 1 - Snip1) & ")"

                    If StringTemp.Length > 25 Then
                        StringTemp3 = StringTemp3.Replace(StringTemp, "").Replace("&amp;", "").Replace("&#39;", "")
                    End If
                Catch
                End Try

                CountInput = StringTemp3
                Occurrences = ((CountInput.Length - CountInput.Replace(ToCount, String.Empty).Length) / ToCount.Length) + 1
                If Occurrences > 5 Then
                    Do Until Occurrences = 5
                        Snip2 = StringTemp3.LastIndexOf(",")
                        StringTemp3 = Left(StringTemp3, Snip2)
                        Occurrences -= 1
                    Loop
                End If

                If StringTemp3.Length > 40 And Occurrences > 2 Then
                    Try
                        Do Until StringTemp3.Length < 40 Or Occurrences = 2
                            Snip2 = StringTemp3.LastIndexOf(",")
                            StringTemp3 = Left(StringTemp3, Snip2)
                            Occurrences -= 1
                        Loop
                    Catch
                    End Try
                End If


                ActualInfo(KanjiLoop, 5) = "Kun Reading Compound: " & StringTemp3.Replace("&#39;", "")
            Else
                StringTemp2 = ""
                ActualInfo(KanjiLoop, 5) = "No Kunyomi compounds"
            End If
        Next

        Dim TextWriter As System.IO.StreamWriter
        Try
            FileReader = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
        Catch
            File.Create("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt").Dispose() 'This text file will store user preferences
            TextWriter = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
            TextWriter.WriteLine("Default 's=' parameter:4")
            TextWriter.WriteLine("Maximum definitions shown:6")
            TextWriter.WriteLine("Reading shown first:kun")
            TextWriter.Close()
        End Try
        TextString = FileReader.Split(vbCrLf)
        Dim OnFirst As Boolean = False
        If TextString(2).IndexOf("on") <> -1 Then
            OnFirst = True
        End If

        'Printing the infomation
        'Display Type: 1 = with LastRequest, 2 = without LastRequest
        If DisplayType = 2 Then
            Console.Clear()
            Console.WriteLine("Kanji information for 「" & ActualSearchWord & "」")
            Console.WriteLine()
        End If

        Dim ActualSLength As Integer = ActualSearchWord.length - 1
        Dim Dupe As Boolean
        For Printer = 0 To ActualSLength
            If Dupe = True Then
                Continue For
            End If
            For Replacer = 0 To 5
                Try
                    ActualInfo(Printer, Replacer) = ActualInfo(Printer, Replacer).replace("&quot;", """").Replace("&amp;", "").Replace("&#39;", "")
                Catch ex As Exception
                    Dim ResizeArray(Replacer, 5)
                    Array.Copy(ActualInfo, ResizeArray, ResizeArray.Length)
                    ReDim ActualInfo(Replacer, 5)
                    Array.Copy(ResizeArray, ActualInfo, ActualInfo.Length)
                    Replacer = 5
                    Dupe = True
                    Continue For
                End Try
            Next
            If Dupe = True Then
                Continue For
            End If

            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine(ActualInfo(Printer, 0))
            Console.BackgroundColor = ConsoleColor.Black

            If OnFirst = False Then
                Console.WriteLine(ActualInfo(Printer, 1)) 'kun
                Console.WriteLine(ActualInfo(Printer, 2)) 'on
            Else
                Console.WriteLine(ActualInfo(Printer, 2)) 'on
                Console.WriteLine(ActualInfo(Printer, 1)) 'kun
            End If

            Console.WriteLine(ActualInfo(Printer, 3))
            If OnFirst = True Then
                Console.WriteLine(ActualInfo(Printer, 4))
                Console.WriteLine(ActualInfo(Printer, 5))
            Else
                Console.WriteLine(ActualInfo(Printer, 5))
                Console.WriteLine(ActualInfo(Printer, 4))
            End If
            Console.WriteLine()
        Next

        Dim KanjisLine As String = "【"
        For Kanji = 1 To ActualInfo.Length / 6
            If Kanji <> ActualInfo.Length / 6 Then
                KanjisLine &= Left(ActualInfo(Kanji - 1, 0), 1) & "(" & Mid(ActualInfo(Kanji - 1, 0), 5) & ") "
            Else
                KanjisLine &= Left(ActualInfo(Kanji - 1, 0), 1) & "(" & Mid(ActualInfo(Kanji - 1, 0), 5) & ")】"
            End If
        Next

        Dim Definition As Integer
        Dim MatchT As Boolean
        Dim NumberCheckD As String = ""
        Dim NumberCheckT As String = ""
        Dim Type As Integer = 0
        Dim SB1, SB2 As Integer
        Dim BArea As String = ""
        Dim BArea2 As String = ""
        Dim DefG1 = 10

LastRequest:
        If DisplayType = 1 Then
            'last requests ------------------------------------------------------------------------------------------------------------------------------------------------------------
            Dim LastRequest As String = ""
            Console.ForegroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Do you have a Last Request? (for example 'anki', 'kanji', 'audio' or 'jisho')")
            Console.ForegroundColor = ConsoleColor.White
            LastRequest = Console.ReadLine().ToLower().Trim
            LastRequest = LastRequest.Replace("/", "").Replace("!", "")

            Dim Types As String = FoundTypes(0)
            If LastRequest.Contains("kanji") = True Then
                My.Computer.Clipboard.SetText(KanjisLine)
                Console.Clear()
                Console.WriteLine("Copied " & QUOTE & KanjisLine & QUOTE & " to clipboard")
                GoTo LastRequest
            ElseIf LastRequest.Contains("ank") = True Then
                If FoundTypes.IndexOf("!") = FoundTypes.Length Then
                    FoundTypes = Left(FoundTypes, FoundTypes.Length - 1)
                End If

                Dim SelectedType() As String = FoundTypes.Split("|") 'How holds the types
                Dim AnkiCopy As String = ""
                NumberCheckD = ""
                NumberCheckT = ""
                Type = 0
                Definition = 0
                MatchT = False

                Do Until Definition = SelectedDefinition.Length
                    NumberCheckD = Right(SelectedDefinition(Definition), 2)
                    If NumberCheckD.IndexOf(".") <> -1 Then 'This is checking for a "." because this will mess up the 'is numberic function if it does exist
                        NumberCheckD = Right(NumberCheckD, 1)
                    End If
                    If IsNumeric(NumberCheckD) = False Then
                        NumberCheckD = Right(SelectedDefinition(Definition), 1)
                    End If
                    If IsNumeric(NumberCheckD) = False Then
                        Console.WriteLine("Error: Conjugate; Definition no; D")
                    End If
                    NumberCheckD = NumberCheckD.Replace(" ", "")

                    MatchT = False
                    Type = 0
                    Do Until Type = SelectedType.Length Or MatchT = True
                        MatchT = False
                        If SelectedType(Type) = "!" Then
                            Type += 1
                            Continue Do
                        End If

                        NumberCheckT = Right(SelectedType(Type), 2)
                        If IsNumeric(NumberCheckT) = False Then
                            NumberCheckT = Right(SelectedType(Type), 1)
                        End If
                        NumberCheckT = NumberCheckT.Replace(" ", "")

                        If NumberCheckT = NumberCheckD Then
                            If Definition < DefG1 + 1 Then
                                If Definition <> 0 Then
                                    Console.WriteLine()
                                    AnkiCopy = AnkiCopy & vbCrLf
                                End If
                                Console.WriteLine(Left(SelectedType(Type), SelectedType(Type).Length - NumberCheckT.Length))
                                AnkiCopy = AnkiCopy & vbCrLf & Left(SelectedType(Type), SelectedType(Type).Length - NumberCheckT.Length)
                            ElseIf Definition > DefG1 And SelectedType(Type).IndexOf("aux") <> -1 Or Definition > DefG1 And SelectedType(Type).IndexOf("fix") <> -1 Then
                                Console.WriteLine()
                                Console.WriteLine(Left(SelectedType(Type), SelectedType(Type).Length - NumberCheckT.Length))
                                Console.WriteLine()
                                AnkiCopy = AnkiCopy & vbCrLf
                                AnkiCopy = AnkiCopy & vbCrLf & Left(SelectedType(Type), SelectedType(Type).Length - NumberCheckT.Length)
                                AnkiCopy = AnkiCopy & vbCrLf

                                'Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length))
                                If Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[") = -1 Then
                                    Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length))
                                    AnkiCopy = AnkiCopy & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length)
                                Else
                                    SB1 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[")
                                    SB2 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("]")
                                    BArea = Mid(Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), SB1 + 1, SB2 + 1 - SB1)

                                    If BArea.IndexOf("kana") = -1 Then
                                        Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                                        Console.ForegroundColor = ConsoleColor.DarkGray
                                        Console.WriteLine(BArea)
                                        Console.ForegroundColor = ConsoleColor.White
                                        Console.WriteLine()
                                        AnkiCopy = AnkiCopy & vbCrLf & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "")
                                        AnkiCopy = AnkiCopy & vbCrLf & BArea
                                        AnkiCopy = AnkiCopy & vbCrLf & vbCrLf
                                    Else
                                        SB1 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[")
                                        SB2 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("]")
                                        BArea = Mid(Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), SB1 + 1, SB2 + 1 - SB1)

                                        If BArea.IndexOf("kana") = -1 Then
                                            Console.Write(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                                            Console.ForegroundColor = ConsoleColor.DarkGray
                                            Console.WriteLine(BArea)
                                            Console.ForegroundColor = ConsoleColor.White
                                            AnkiCopy = AnkiCopy & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "")
                                            AnkiCopy = AnkiCopy & vbCrLf & BArea
                                        Else
                                            BArea2 = BArea 'BArea is acting like a temp

                                            If BArea.IndexOf("Usually written using kana alone") <> -1 Then 'BArea will be set below if it has more than one thing
                                                BArea2 = BArea.Replace("Usually written using kana alone", "")
                                                BArea2 = BArea2.Replace(", ", "")
                                            End If

                                            If BArea2.Length > 3 Then
                                                BArea = BArea.Replace("See also", "See also ")
                                                BArea2 = BArea2.Replace("See also", "See also ")
                                                Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "") & BArea2)
                                                AnkiCopy = AnkiCopy & vbCrLf & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "") & BArea2
                                            Else
                                                Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                                                AnkiCopy = AnkiCopy & vbCrLf & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "")
                                            End If
                                        End If
                                    End If
                                End If

                            End If
                            MatchT = True
                            SelectedType(Type) = "!"
                            Continue Do
                        End If
                        Type += 1
                    Loop

                    If Definition < DefG1 + 1 Then
                        If Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[") = -1 Then
                            Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length))
                            AnkiCopy = AnkiCopy & vbCrLf & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length)
                        Else
                            SB1 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[")
                            SB2 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("]")
                            BArea = Mid(Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), SB1 + 1, SB2 + 1 - SB1)

                            If BArea.IndexOf("kana") = -1 Then
                                Console.Write(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                                BArea = BArea.Replace("See also", "See also ")
                                BArea2 = BArea2.Replace("See also", "See also ")
                                Console.ForegroundColor = ConsoleColor.DarkGray
                                Console.WriteLine(BArea)
                                Console.ForegroundColor = ConsoleColor.White

                                AnkiCopy = AnkiCopy & vbCrLf & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "")
                                'AnkiCopy = AnkiCopy & vbCrLf & BArea
                            Else
                                BArea2 = BArea 'BArea is acting like a temp

                                If BArea.IndexOf("Usually written using kana alone") <> -1 Then 'BArea will be set below if it has more than one thing
                                    BArea2 = BArea.Replace("Usually written using kana alone", "")
                                    BArea2 = BArea2.Replace(", ", "")
                                End If

                                If BArea2.Length > 3 Then
                                    BArea = BArea.Replace("See also", "See also ")
                                    BArea2 = BArea2.Replace("See also", "See also ")
                                    Console.Write(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                                    Console.ForegroundColor = ConsoleColor.DarkGray
                                    Console.WriteLine(BArea2)
                                    Console.ForegroundColor = ConsoleColor.White
                                    AnkiCopy = AnkiCopy & vbCrLf & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "")
                                    AnkiCopy = AnkiCopy & BArea
                                Else
                                    Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                                    AnkiCopy = AnkiCopy & vbCrLf & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "")
                                End If
                            End If
                        End If
                    End If
                    Definition += 1
                Loop

                AnkiCopy = AnkiCopy.Trim

                AnkiCopy = AnkiCopy & vbCrLf & vbCrLf & FullWord & vbCrLf & KanjisLine.Replace("[", "(").Replace("]", ")")
                AnkiCopy = AnkiCopy.Replace("See also", "See also ")
                AnkiCopy = AnkiCopy.Replace("See also  ", "See also ")
                AnkiCopy = AnkiCopy.Replace(", )", "")

                My.Computer.Clipboard.SetText(AnkiCopy.Replace("[", "(").Replace("]", ")"))

                Console.Clear()
                Console.WriteLine("Copied: " & vbCrLf & AnkiCopy.Replace("[", "(").Replace("]", ")"))
                GoTo LastRequest
            ElseIf LastRequest.Contains("audi") = True Then
                If FoundTypes.tolower.contains("verb") = True And FoundTypes.tolower.contains("suru") = False Then
                    'if it's a verb
                    VerbAudioGen(ActualSearchWord)
                Else
                    'if it isn't a verb
                    JishoAudio(LinkChoice, ActualSearchWord)
                End If
                GoTo LastRequest
            ElseIf LastRequest.Contains("hist") = True Then
                Console.Clear()
                Console.WriteLine("Search history:")
                Try
                    Dim HistoryFile As String = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\SearchHistory.txt")
                    Console.WriteLine(HistoryFile)
                Catch
                    Console.WriteLine("You have no history.")
                    GoTo LastRequest
                End Try
                GoTo LastRequest
            ElseIf LastRequest.Contains("jish") = True Then
                Try
                    Process.Start("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "https://" & WordLink)
                Catch
                    Try
                        Process.Start("C:\Program Files (x86)\Mozilla Firefox\firefox.exe", "https://" & WordLink)
                    Catch
                        Try
                            Process.Start("https://" & WordLink)
                        Catch
                            Console.ForegroundColor = ConsoleColor.Red
                            Console.WriteLine("Cannot open on Jisho because the exe file for your browser isn't were it usually is.")
                            Console.ForegroundColor = ConsoleColor.White
                        End Try
                    End Try
                End Try
                GoTo LastRequest
            ElseIf LastRequest.Contains("save") = True Then
                SelectedDefinition(0) = Left(SelectedDefinition(0), SelectedDefinition(0).Length - 1).Replace("  ", " ")
                If Right(SelectedDefinition(0), 1) = " " Then
                    SelectedDefinition(0) = Left(SelectedDefinition(0), SelectedDefinition(0).Length - 1).Replace("  ", " ")
                End If
                SaveWord(ActualSearchWord, Furigana, SelectedDefinition(0))
                Console.WriteLine(SelectedDefinition(0))
                GoTo LastRequest
            End If
        End If
        Main()
    End Sub
    Sub JishoAudio(ByVal WordLink, ByVal ActualWord) 'WordLink shouldn't include 'http://'
        Dim Client As New WebClient
        Dim HTML As String = ""
        Client.Encoding = System.Text.Encoding.UTF8
        HTML = Client.DownloadString(New Uri("http://" & LinkChoice))

        Dim Snip1, Snip2 As Integer
        Try
            Snip2 = HTML.IndexOf(".mp3")
            HTML = Left(HTML, Snip2)
            Snip1 = HTML.LastIndexOf("""") + 2
            HTML = Mid(HTML, Snip1)
            HTML = "https:" & HTML & ".mp3"

            Try
                My.Computer.FileSystem.CreateDirectory(Environ$("USERPROFILE") & "\Downloads\Conjugation Audio")
                Client.DownloadFile(HTML, Environ$("USERPROFILE") & "\Downloads\Conjugation Audio\" & ActualWord & ".mp3")
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Audio for '" & ActualWord & "' couldn't be downloaded")
                Console.ForegroundColor = ConsoleColor.White
                If DebugMode = True Then
                    Console.WriteLine(ex.Message)
                    Console.WriteLine("Link: " & HTML)
                End If
                Exit Sub
            End Try
        Catch ex As Exception
            If DebugMode = True Then
                Console.WriteLine(ex.Message)
                Console.WriteLine("Link: " & HTML)
            End If
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Audio for '" & ActualWord & "' couldn't be downloaded")
            Console.ForegroundColor = ConsoleColor.White
            Exit Sub
        End Try
        Console.ForegroundColor = ConsoleColor.Green
        Console.WriteLine("Audio for '" & ActualWord & "' downloaded to 'Downloads\Conjugation Audio\" & ActualWord & ".mp3'")
        Console.ForegroundColor = ConsoleColor.White
    End Sub
    Function RetrieveClassRange(ByVal HTML, ByRef Start, ByRef SnipEnd, ByVal ErrorMessage)
        'Loading the website's HTML code and storing it in a HTML as a string:

        Dim Snip As String = ""
        'Used to debug:
        'Console.WriteLine(HTML)
        'Console.WriteLine("URL: " & URL)

        Dim SnipFirstIndex As Integer = HTML.IndexOf(Start) 'Start of the snip, this will look for the class name, example: <span class="meaning-meaning">cute; adorable; charming; lovely; pretty</span>

        If SnipFirstIndex = -1 And ErrorMessage = "Example Sentence" Then
            Return ("")
        End If
        If SnipFirstIndex = -1 And ErrorMessage <> "Example Sentence" Then
            Return ("")
        End If
        If SnipFirstIndex <> -1 Then
            Snip = Mid(HTML, SnipFirstIndex, HTML.Length - SnipFirstIndex - 1) 'This is getting rid of everything before the group/first snip. This is useful for the setting the second snip to something more ambigious/could be somewhere else (before) in the HTML
        End If
        Dim SnipSecondIndex As Integer = Snip.IndexOf(SnipEnd)

        If SnipSecondIndex = -1 And SnipEnd = "inline_copyright" Then
            SnipSecondIndex = Snip.IndexOf("english")
        End If
        If SnipSecondIndex = -1 Then
            Return ("")
        End If
        Snip = Left(Snip, SnipSecondIndex)

        'Used for debugging
        'Console.WriteLine("Snip: " & Snip)
        'Console.WriteLine("SnipSecondIndex: " & SnipSecondIndex)
        Return (Snip)
    End Function
    Function RetrieveClass(ByVal HTML, ByRef ClassToRetrieve, ByVal ErrorMessage)
        Const QUOTE = """"

        Dim SnipIndex As Integer = HTML.IndexOf("class=" & QUOTE & ClassToRetrieve) 'Start of the snip, this will look for the class name, example: <span class="meaning-meaning">cute; adorable; charming; lovely; pretty</span>
        If SnipIndex = -1 Then
            Return ("Error: |" & "class=" & QUOTE & ClassToRetrieve & "| Not Found")
        End If
        Dim Snip As String = Mid(HTML, SnipIndex + 10 + ClassToRetrieve.length, 85)
        SnipIndex = Snip.IndexOf("<")

        If SnipIndex = -1 Then
            SnipIndex = Snip.IndexOf(QUOTE)
            If SnipIndex = -1 Then
                SnipIndex = Snip.IndexOf(",")
                If SnipIndex = -1 Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Error: RetrieveClass; " & ErrorMessage & "; SnipIndex could not find")
                    Console.ForegroundColor = ConsoleColor.White

                    Console.ReadLine()
                    Main()
                End If

            End If
        End If

        Snip = Left(Snip, SnipIndex) ''

        'Used for debugging
        'Console.WriteLine("SnipEnd: " & SnipIndex)
        'Console.WriteLine("Snip: " & Snip)

        Return (Snip)
    End Function
    Function DefinitionScraper(ByVal URL)
        Const QUOTE = """"
        Dim Read As String = URL
        Dim SnipStart As Integer = 0
        Dim SnipEnd As Integer = 0
        Dim Snip As String = "NOTHING"
        Dim HTML As String = "NOTHING"
        Dim FirstFail As Boolean = False
        Dim ExtraIndex As Integer = 0
        Dim FinishedAll As Boolean = False
        Dim FoundMeanings(0) As String
        Dim FoundTypes(0) As String
        'Try
        'Loading the website's HTML code and storing it in a HTML as a string:
        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8

        HTML = Client.DownloadString(New Uri("http://" & URL))

        Try
            ExtraIndex = HTML.IndexOf("Details ▸")
            HTML = Left(HTML, ExtraIndex)
        Catch ex As Exception
            If DebugMode = True Then
                Console.WriteLine("DefinitionScraper: " & ex.Message)
                Console.WriteLine("URL: " & URL)
                Console.WriteLine("ExtraIndex: " & ExtraIndex)
                Console.ReadLine()
            End If
        End Try
        ExtraIndex = HTML.IndexOf("<div class=" & QUOTE & "concept_light-meanings medium-9 columns" & QUOTE & ">")
        Try
            HTML = Mid(HTML, ExtraIndex)
        Catch
            Return ("")
        End Try

        'Used to debug:
        'Console.WriteLine(HTML)
        'Console.WriteLine("URL: " & URL)

        'Cutting text out of the HTML code:
        SnipStart = HTML.IndexOf("meaning-meaning") 'The start of the first group, groups are just kanji, this is so that you extract the right information for the right kanji
        SnipStart += 18

        SnipEnd = HTML.IndexOf("</span><span>&#") + 1 'This will make sure that I can get ALL meaning from 1 kanji because I won't have to worry about accidentally extracting information about the next kanji

        'Console.WriteLine(SnipEnd)

        If SnipEnd = -1 Then
            FirstFail = True 'For debugging
            SnipEnd = Mid(HTML, SnipStart, 300).IndexOf("</span>")
        End If

        Try
            Snip = Mid(HTML, SnipStart, SnipEnd - SnipStart)
        Catch ex As Exception
            If DebugMode = True Then
                Console.WriteLine("DefinitionScraper: " & ex.Message)
                Console.WriteLine("Snip: " & Snip)
                Console.WriteLine("SnipStart: " & SnipStart)
                Console.WriteLine("SnipEnd: " & SnipEnd)
                Console.ReadLine()
            End If
        End Try

        Return (Snip)
    End Function
    Function TypeScraper(ByVal URL)
        Const QUOTE = """"
        Dim Read As String = URL
        Dim SnipStart As Integer
        Dim SnipEnd As Integer
        Dim Snip As String = "NOTHING"
        Dim HTML As String = "NOTHING"
        Dim FirstFail As Boolean = False
        Dim ExtraIndex As Integer
        Dim FinishedAll As Boolean = False
        Dim FoundTypes(0) As String
        'Loading the website's HTML code and storing it in a HTML as a string:
        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8
        URL = "https://" & URL
        HTML = Client.DownloadString(New Uri(URL))
        Try
            ExtraIndex = HTML.IndexOf("Details ▸")
            HTML = Left(HTML, ExtraIndex)
        Catch
        End Try
        ExtraIndex = HTML.IndexOf("<div class=" & QUOTE & "concept_light-meanings medium-9 columns" & QUOTE & ">")
        HTML = Mid(HTML, ExtraIndex)

        Dim HTMLTemp As String = HTML 'This is for picking out the word types while not disturbing the HTML code used for definitions

        Do Until FinishedAll = True

            HTMLTemp = Mid(HTMLTemp, HTMLTemp.IndexOf("meaning-tags") + 15)

            SnipEnd = HTMLTemp.IndexOf("meaning-wrapper") + 1 'This will make sure that I can get ALL meaning from 1 kanji because I won't have to worry about accidentally extracting information about the next kanji

            Try
                If SnipEnd = 0 Or SnipStart - 18 = -1 Or SnipStart = 14 Then
                    FinishedAll = True
                    Continue Do
                End If
                If Mid(HTMLTemp, SnipEnd, 40).IndexOf("meaning-tags") <> -1 Then
                    FinishedAll = True
                    Continue Do
                End If
            Catch
                FinishedAll = True
                Continue Do
            End Try

            SnipStart = HTMLTemp.IndexOf("</div>")

            Snip = Left(HTMLTemp, SnipStart)

            HTMLTemp = Mid(HTMLTemp, HTMLTemp.IndexOf("meaning-wrapper") + 10)

            FoundTypes(FoundTypes.Length - 1) = Snip

            SnipStart = HTMLTemp.IndexOf("meaning-definition-section_divider") + 37
            ExtraIndex = HTMLTemp.IndexOf("</span>") - 1
            Snip &= Mid(HTMLTemp, SnipStart, ExtraIndex - SnipStart)

            FoundTypes(FoundTypes.Length - 1) = Snip

            Array.Resize(FoundTypes, FoundTypes.Length + 1)
        Loop

        Array.Resize(FoundTypes, FoundTypes.Length - 1)

        Snip = ""
        For Concat = 1 To FoundTypes.Length
            If FoundTypes(Concat - 1) = "Other Forms" Or FoundTypes(Concat - 1) = "Wikipedia definition" Or FoundTypes(Concat - 1).IndexOf("span") <> -1 Then
                FoundTypes(Concat - 1) = "!"
            End If
            Snip &= FoundTypes(Concat - 1) & "|"
        Next

        Try
            If IsNumeric(Snip) = False Then
                Snip = Left(Snip, Snip.Length - 1)
            End If

        Catch
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("Error: Definition; Snip")
            Console.ForegroundColor = ConsoleColor.White
        End Try

        Return (Snip)

    End Function
    Function ExampleSentence(ByRef SentenceExample) 'This function gets rid of fillers not extracts from the website
        Const QUOTE = """"
        SentenceExample = SentenceExample.Replace("<li class=" & QUOTE & "clearfix" & QUOTE & ">", "")
        SentenceExample = SentenceExample.Replace("<span class=" & QUOTE & "unlinked" & QUOTE & ">", "")
        'SentenceExample = SentenceExample.Replace("<span class=" & QUOTE & "furigana" & QUOTE & ">", "")
        SentenceExample = SentenceExample.Replace("</span></li>", "")
        'SentenceExample = SentenceExample.Replace("</span>", "")

        Dim EnglishSentence As String = ""
        Dim EngSent As Integer = SentenceExample.IndexOf("class=" & QUOTE & "english" & QUOTE)
        Dim EngSentEnd As Integer = SentenceExample.IndexOf(".</span>")
        If EngSentEnd = -1 Then
            EngSentEnd = SentenceExample.IndexOf("?</span>")
        End If
        If EngSentEnd = -1 Then
            EngSentEnd = SentenceExample.IndexOf("!</span>")
        End If

        Try
            EnglishSentence = Mid(SentenceExample, EngSent + 1, EngSentEnd - EngSent + 1)
            EnglishSentence = EnglishSentence.Replace("class=" & QUOTE & "english" & QUOTE & ">", "")
        Catch Ex As Exception
            If DebugMode = True Then
                Console.WriteLine(ex.Message)
                Console.ReadLine()
            End If
            Return ("")
        End Try
        Dim JPSentEnd As Integer = SentenceExample.IndexOf("</ul>")
        Dim JapaneseSentence As String = ""
        Try
            JapaneseSentence = Mid(SentenceExample, 2, JPSentEnd - 1)
        Catch Ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            If DebugMode = True Then
                Console.WriteLine(ex.Message)
                Console.WriteLine()
            End If
            Console.WriteLine("Error: ExampleSentence; JapaneseSentence")
            Console.WriteLine("Catch: 1")
            Console.WriteLine("JPSentEnd: " & JPSentEnd)
            Console.ForegroundColor = ConsoleColor.White

            Console.ReadLine()
            Main()
        End Try

        'Removing the furigana that splits up the sentence and ruin it, we must remove these "furigana fills"
        Dim Index1, Index2 As Integer
        Dim FuriganaFillRemover As String
        Do Until Index1 = -1
            Index1 = JapaneseSentence.IndexOf("<span class=")
            If Index1 = -1 Then 'This checks if there is a new furigana filler, it also says where it is; WHAMM! Two birds with one stone!
                Continue Do 'This then exists the loop because we now there aren't any more fillers and if we kept checking if though there are none then we would get errors
            End If
            Index2 = JapaneseSentence.IndexOf("</span>") 'Thank god both index searches don't need the QUOTE variable
            FuriganaFillRemover = Mid(JapaneseSentence, Index1 + 1, Index2 - Index1 + 7)
            JapaneseSentence = JapaneseSentence.Replace(FuriganaFillRemover, "")
            Index1 = JapaneseSentence.IndexOf("<span class=")
        Loop

        JapaneseSentence = JapaneseSentence.Replace("  ", "")

        Return ("|" & JapaneseSentence & "|" & EnglishSentence & "|")
    End Function
    Function WordLinkScraper(ByVal URL) 'This is for getting the definition of a word from the page of the word instead of the search results, this is much more reliable for definitions
        Const QUOTE = """"

        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8
        Dim HTML As String = ""
        HTML = Client.DownloadString(New Uri("https://" & URL))

        Dim Temp As String
        Dim TempIndex As Integer
        Dim SnipStart As Integer
        Dim SnipEnd As Integer
        Dim Snip As String = "NOTHING"
        Dim Snip2 As String = "" 'This is used for the HTML code tag called 'sense-tag', it adds extra information about the definition if there is any (which usually there isn't any)
        Dim FirstFail As Boolean = False
        Dim ExtraIndex As Integer
        Dim FinishedAll As Boolean = False
        Dim FoundMeanings(0) As String
        Try
            ExtraIndex = HTML.IndexOf("Details ▸")
            HTML = Left(HTML, ExtraIndex)
            ExtraIndex = HTML.IndexOf("<div class=" & QUOTE & "concept_light-meanings medium-9 columns" & QUOTE & ">")
            HTML = Mid(HTML, ExtraIndex)
        Catch
            ExtraIndex = HTML.IndexOf("<div class=" & QUOTE & "concept_light-meanings medium-9 columns" & QUOTE & ">")
            HTML = Mid(HTML, ExtraIndex)
            ExtraIndex = HTML.IndexOf("</span></div></div></div>")
            If ExtraIndex < 1 Then
                ExtraIndex = HTML.IndexOf("</div></span></div></div>")
            End If

            HTML = Left(HTML, ExtraIndex)
        End Try

        'Used to debug:
        'Console.WriteLine(HTML)
        'Console.WriteLine("URL: " & URL)

        Do Until FinishedAll = True
            'Cutting text out of the HTML code:
            SnipStart = HTML.IndexOf("meaning-meaning") 'The start of the first group, groups are just kanji, this is so that you extract the right information for the right kanji
            SnipStart += 18

            SnipEnd = HTML.IndexOf("</span><span>&#") + 1 'This will make sure that I can get ALL meaning from 1 kanji because I won't have to worry about accidentally extracting information about the next kanji

            Try
                If SnipEnd = 0 Or SnipStart - 18 = -1 Or Mid(HTML, SnipEnd, 40).IndexOf("meaning-tags") <> -1 Or SnipEnd < SnipStart Then
                    FinishedAll = True
                    Continue Do
                End If
            Catch
                FinishedAll = True
                Continue Do
            End Try

            'Console.WriteLine(SnipEnd)

            If SnipEnd = -1 Then
                FirstFail = True 'For debugging
                SnipEnd = Mid(HTML, SnipStart, 300).IndexOf("</span>")
            End If

            Snip = Mid(HTML, SnipStart, SnipEnd - SnipStart)

            HTML = Mid(HTML, SnipEnd + 10)


            SnipStart = HTML.IndexOf("meaning-definition-section_divider") 'This will be used to get the Extra Details for the current word up to (but not including) the next
            TempIndex = SnipStart
            Temp = HTML
            Try
                Try
                    Temp = Left(Temp, TempIndex)
                Catch
                End Try

                If Temp.Contains("sense-tag") = True Then
                    'Snipping up to the sense tag:
                    If SnipStart = -1 Then
                        SnipStart = 1
                    End If
                    SnipStart = Temp.IndexOf("sense-tag")
                    Temp = Mid(Temp, SnipStart)

                    SnipStart = Temp.IndexOf(">") + 2
                    Temp = Mid(Temp, SnipStart)

                    SnipEnd = Temp.IndexOf("</")
                    Snip2 = Left(Temp, SnipEnd)
                    Temp = Mid(Temp, SnipStart)

                    If Snip2.IndexOf("href") <> -1 Then
                        SnipStart = Snip2.IndexOf("<a href=")
                        SnipEnd = Snip2.IndexOf(">")
                        Snip2 = Snip2.Replace(Mid(Snip2, SnipStart, SnipEnd + 2 - SnipStart), "")
                    End If
                End If

                If Temp.IndexOf("sense-tag") <> -1 Then
                    SnipStart = Temp.IndexOf("sense-tag")
                    Temp = Mid(Temp, SnipStart)

                    SnipStart = Temp.IndexOf(">") + 2
                    Temp = Mid(Temp, SnipStart)

                    SnipEnd = Temp.IndexOf("</")
                    Snip2 = Snip2 & ", " & Left(Temp, SnipEnd)

                    If Snip2.IndexOf("href") <> -1 Then
                        SnipStart = Snip2.IndexOf("<a href=")
                        SnipEnd = Snip2.IndexOf(">")
                        Snip2 = Snip2.Replace(Mid(Snip2, SnipStart, SnipEnd + 2 - SnipStart), "")
                    End If
                End If

                If Temp.IndexOf("tag-restriction") <> -1 Then
                    SnipStart = Temp.IndexOf("tag-restriction") + 14
                    Temp = Mid(Temp, SnipStart)

                    SnipStart = Temp.IndexOf(">") + 2
                    Temp = Mid(HTML, SnipStart)

                    SnipEnd = Temp.IndexOf("</")
                    Snip2 = Snip2 & ", " & Left(Temp, SnipEnd)


                    If Snip2.IndexOf("href") <> -1 Then
                        SnipStart = Snip2.IndexOf("<a href=")
                        SnipEnd = Snip2.IndexOf(">")
                        Snip2 = Snip2.Replace(Mid(Snip2, SnipStart, SnipEnd + 2 - SnipStart), "")
                    End If
                End If
                'To do: when searching '上げる' and looking up '7. to give​' get the third info below

            Catch
            End Try

            Snip2 = Snip2.Replace("#8203;", "")
            If Snip2 <> "" Then
                Snip2 = "[" & Snip2 & "]"
            End If

            FoundMeanings(FoundMeanings.Length - 1) = Snip & " " & Snip2

            Array.Resize(FoundMeanings, FoundMeanings.Length + 1)
            Snip2 = ""
        Loop

        Array.Resize(FoundMeanings, FoundMeanings.Length - 1)
        Snip = ""
        For Concat = 1 To FoundMeanings.Length
            Snip &= FoundMeanings(Concat - 1) & "|"
        Next
        Snip = Left(Snip, Snip.Length - 1)

        Return (Snip)
    End Function
    Function JustResultsScraper(ByVal Word, ByVal Number, ByVal ActualWord)
        Const QUOTE = """"
        Dim ActualSearchWord As String = ""
        Dim HTML As String = ""
        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8
        HTML = Client.DownloadString(New Uri("https://jisho.org/search/*" & Word & "*"))
        Dim HTMLTemp As String = HTML

        Try
            For I = 1 To Number
                ActualSearchWord = RetrieveClassRange(HTMLTemp, "<span class=" & QUOTE & "text" & QUOTE & ">", "</div>", "Kanji Examples")
                HTMLTemp = Mid(HTMLTemp, HTMLTemp.IndexOf("<span class=" & QUOTE & "text" & QUOTE & ">") + 2)
                HTMLTemp = Mid(HTMLTemp, HTMLTemp.IndexOf("</div>") + 2)
            Next
        Catch
            Try
                Number = 1
                ActualSearchWord = RetrieveClassRange(HTML, "<span class=" & QUOTE & "text" & QUOTE & ">", "</div>", "Kanji Examples")
            Catch
                Return ("")
            End Try
        End Try

        Try
            ActualSearchWord = Mid(ActualSearchWord, 30)
            ActualSearchWord = ActualSearchWord.Replace("<span>", "")
            ActualSearchWord = ActualSearchWord.Replace("</span>", "")


            ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 8)
        Catch
            If Number <> 1 Then
                JustResultsScraper(Word, 1, ActualWord)
            Else
                Return ("")
            End If
        End Try

        If Number > 1 And ActualSearchWord.Length = 1 Then
            JustResultsScraper(Word, Number + 1, ActualWord)
        End If

        If ActualSearchWord = ActualWord And Number > 1 Then
            JustResultsScraper(Word, Number + 1, ActualWord)
        End If

        'Getting the definition -------------------------------------
        HTML = Client.DownloadString(New Uri("https://jisho.org/search/" & ActualSearchWord))
        Dim SnipStart, SnipEnd, ExtraIndex As Integer
        Dim Snip As String
        Try
            ExtraIndex = HTML.IndexOf("Details ▸")
            HTML = Left(HTML, ExtraIndex)
        Catch
        End Try
        ExtraIndex = HTML.IndexOf("<div class=" & QUOTE & "concept_light-meanings medium-9 columns" & QUOTE & ">")
        HTML = Mid(HTML, ExtraIndex)

        'Cutting text out of the HTML code:
        SnipStart = HTML.IndexOf("meaning-meaning") 'The start of the first group, groups are just kanji, this is so that you extract the right information for the right kanji
        SnipStart += 18

        SnipEnd = HTML.IndexOf("</span><span>&#") + 1 'This will make sure that I can get ALL meaning from 1 kanji because I won't have to worry about accidentally extracting information about the next kanji

        If SnipEnd = -1 Then
            SnipEnd = Mid(HTML, SnipStart, 300).IndexOf("</span>")
        End If

        Snip = Mid(HTML, SnipStart, SnipEnd - SnipStart)


        Return (ActualSearchWord & " (" & Snip & ")")
    End Function
    Function GTranslate(ByVal inputtext As String, ByVal fromlangid As String, ByVal tolangid As String) As String

        Try
            File.Create("C:\ProgramData\Japanese Conjugation Helper\LastSearch.txt").Dispose()
        Catch
        End Try

        inputtext = HttpUtility.HtmlAttributeEncode(inputtext)
        Dim step1 As New WebClient
        step1.Encoding = Encoding.UTF8

        Dim step2 As String = step1.DownloadString("https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl=" & tolangid & "&hl=" & fromlangid & "&dt=t&dt=bd&dj=1&source=icon&q=" & inputtext)
        Dim step3 As Newtonsoft.Json.Linq.JObject = JObject.Parse(step2)
        Dim step4 As String = "{nothing}"
        Try
            step4 = step3.SelectToken("sentences[0]").SelectToken("trans").ToString()
        Catch
            Console.WriteLine("Error; step 4")
            Threading.Thread.Sleep(1000)
            Console.WriteLine(step2)
        End Try

        WriteToFile("Translate:", "LastSearch")
        WriteToFile(inputtext, "LastSearch")
        WriteToFile(step4, "LastSearch")

        Return step4
    End Function

    Sub Preferences()
        Const QUOTE = """"
        Dim MsgResponse As Integer
        My.Computer.FileSystem.CreateDirectory("C:\ProgramData\Japanese Conjugation Helper") 'This folder will be used to store program data

        If Dir$("C:\ProgramData\Japanese Conjugation Helper\ReadMe.txt") = "" Then
            MsgResponse = MsgBox("This is your first time using preferences, would you like to check the wiki?", 4, "Preferences")
            If MsgResponse = 6 Then
                Process.Start("C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "github.com/hopto-dot/Japanese-Conjugation-Helper/wiki/How-to-use")
            End If
            My.Computer.FileSystem.CreateDirectory("C:\ProgramData\Japanese Conjugation Helper\Preferences")
            File.Create("C:\ProgramData\Japanese Conjugation Helper\ReadMe.txt").Dispose()
            Dim FirstWriter As System.IO.StreamWriter
            FirstWriter = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\ReadMe.txt")
            FirstWriter.WriteLine("This program has not been error trapped to stop you from messing with the files, so if you make manual changes to the files it will likely mess up the program.")
            FirstWriter.WriteLine("If you ever mess with the files you can do '/prefs' then type '3' to set the files back to normal.")
            FirstWriter.Close()
            Preferences()
        End If

        Dim Choice As String = ""
        Console.Clear()

        Do Until IsNumeric(Choice) = True
            Console.WriteLine("What would you like to do?")
            Console.WriteLine()
            Console.WriteLine("0 = Back to main menu")
            Console.WriteLine()
            Console.WriteLine("1 = Custom S Parameter Settings")
            Console.WriteLine("2 = General Settings")
            Console.WriteLine()
            Console.WriteLine("3 = Reset ALL settings")
            Console.WriteLine("4 = Clear Search History")

            Choice = Console.ReadLine.ToLower

            If Choice = "0" Or Choice = "main" Or Choice.IndexOf("b") <> -1 Then
                Main()
            End If

            If IsNumeric(Choice) = True Then
                If Choice < 0 Or Choice > 4 Then
                    Choice = ""
                End If
            End If
            Console.Clear()
        Loop

ChangeS:

        Dim FileReader As String = ""
        Dim TextWriter As System.IO.StreamWriter
        Try
            FileReader = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\Preferences\SParameter.txt")
        Catch
            File.Create("C:\ProgramData\Japanese Conjugation Helper\Preferences\SParameter.txt").Dispose() 'This text file will store user preferences
            TextWriter = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\SParameter.txt")
            TextWriter.WriteLine("Formal:1")
            TextWriter.WriteLine("Informal:1")

            TextWriter.WriteLine("Te-form:1")

            TextWriter.WriteLine("Volitional:1")
            TextWriter.WriteLine("Potential:1")
            TextWriter.WriteLine("Causative (let & made):1")
            TextWriter.WriteLine("Passive:1")
            TextWriter.WriteLine("Conditional:1")

            TextWriter.WriteLine("Want:1")

            TextWriter.WriteLine("Need to:1")

            TextWriter.WriteLine("Extras:1")

            TextWriter.WriteLine("Noun/adj conjugations:1")

            TextWriter.WriteLine("Kanji details:1")

            TextWriter.Close()
            GoTo ChangeS
        End Try

        If FileReader = "" Then
            Console.WriteLine("Something is wrong with the files, please reset them using in /preferences")
            Console.ReadLine()
            Main()
        End If

        Dim TextString(0) As String
        TextString = FileReader.Split(vbCrLf)
        If Choice = "1" Then
            Console.WriteLine("What would you like to do?")
            Console.WriteLine("Type 'b' to go back")
            Console.WriteLine()
            Console.WriteLine("1 = Change custom S parameter")
            Console.WriteLine("2 = View current S parameter settings")
            Console.WriteLine("3 = Reset S parameter settings")
            Console.WriteLine()
            Choice = Console.ReadLine.ToLower

            If Choice.IndexOf("b") <> -1 Then
                Preferences()
            End If

            If Choice = 1 Then
                For I = 0 To TextString.Length - 1
                    TextString(I) = TextString(I).Trim
                Next
                If TextString(TextString.Length - 1) = "" Then
                    Array.Resize(TextString, TextString.Length - 1)
                    If TextString(TextString.Length - 1) = "" Then
                        Array.Resize(TextString, TextString.Length - 1)
                    End If
                    If TextString(TextString.Length - 1) = "" Then
                        Array.Resize(TextString, TextString.Length - 1)
                    End If
                    If TextString(TextString.Length - 1) = "" Then
                        Array.Resize(TextString, TextString.Length - 1)
                    End If
                End If
                Console.Clear()
                'Setting your own results for the custom S=0 parameter:
                Dim Line As Integer = 0
                Dim InformationType As String = ""
                Dim AmountChoice As String = ""
                Dim CheckedAmount As String = ""
                Do Until Line = TextString.Length Or AmountChoice = "finish"
                    Try
                        InformationType = Left(TextString(Line), TextString(Line).IndexOf(":"))
                    Catch
                        If TextString(Line).IndexOf("|") <> -1 Then
                            Line = TextString.Length
                            Continue Do
                        Else
                            Console.WriteLine("The program files have been tampered with, please avoid doing this.")
                            Console.ReadLine()
                        End If
                    End Try

                    AmountChoice = "a"
                    Do Until IsNumeric(AmountChoice) = True Or AmountChoice = "finish"
                        Console.SetCursorPosition(0, 0)
                        Console.WriteLine("How much information would you like for '" & InformationType & "' (" & Line + 1 & "/13)?")
                        Console.WriteLine("Type a number in the range 0-3. (0 being nothing, 3 being everything)")
                        Console.WriteLine("Press 'f' key to save, and arrow keys to move")
                        Console.WriteLine()

                        For SType = 0 To TextString.Length - 1
                            If SType = Line Then
                                Console.BackgroundColor = ConsoleColor.DarkGray
                                Console.WriteLine(TextString(SType))
                                Console.BackgroundColor = ConsoleColor.Black
                            ElseIf TextString(SType).IndexOf("|") <> -1 Or TextString(SType).IndexOf(":") = -1 Then
                            Else
                                Try
                                    Console.WriteLine(TextString(SType))
                                Catch
                                    Console.WriteLine()
                                End Try
                            End If
                        Next
                        Dim KeyReader As ConsoleKeyInfo = Console.ReadKey
                        If KeyReader.Key = ConsoleKey.UpArrow Then
                            If Line <> 0 Then
                                Line -= 1
                            End If
                        ElseIf KeyReader.Key = ConsoleKey.DownArrow Then
                            If Line <> TextString.Length - 1 Then
                                Line += 1
                            End If
                        ElseIf KeyReader.Key = ConsoleKey.F Then
                            AmountChoice = "finish"
                            Continue Do
                        ElseIf KeyReader.Key = ConsoleKey.Enter Then
                            Console.WriteLine()
                            Console.WriteLine("What do you want the new value to be?")
                            AmountChoice = Console.ReadLine
                        Else
                            Try
                                AmountChoice = KeyReader.KeyChar.ToString
                            Catch
                                AmountChoice = ""
                            End Try
                            If AmountChoice = "b" Then
                                Preferences()
                            ElseIf AmountChoice = "m" Then
                                Main()
                            End If
                        End If

                        If AmountChoice.ToLower = "b" Or AmountChoice.ToLower = "back" Or AmountChoice.ToLower = "stop" Then
                            Main()
                        End If

                        If AmountChoice.ToLower = "d" Or AmountChoice.ToLower = "f" Or AmountChoice.ToLower = "done" Or AmountChoice.ToLower.IndexOf("finish") <> -1 Then
                            AmountChoice = "finish"
                            Continue Do
                        End If

                        If IsNumeric(AmountChoice) = False Then
                            Continue Do
                        ElseIf AmountChoice < 0 Or AmountChoice > 3 Or AmountChoice.Length > 1 Then
                            AmountChoice = "a"
                        End If
                    Loop

                    'Loop exiter:
                    If AmountChoice.ToLower = "d" Or AmountChoice.ToLower = "done" Or AmountChoice.ToLower.IndexOf("finish") <> -1 Then
                        AmountChoice = "finish"
                        Continue Do
                    End If

                    Try
                        CheckedAmount = Mid(TextString(Line), TextString(Line).IndexOf(":") + 1)
                    Catch
                        TextString(Line) = TextString(Line) & "_"
                        CheckedAmount = "_"
                    End Try

                    AmountChoice = AmountChoice.Replace("-", "")

                    TextString(Line) = TextString(Line).Replace(CheckedAmount, ":" & AmountChoice)


                    Line += 1
                Loop

                Dim TextUpdater As System.IO.StreamWriter
                TextUpdater = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\SParameter.txt")
                For Writer = 0 To TextString.Length - 1
                    TextUpdater.WriteLine(TextString(Writer).Trim)
                Next
                TextUpdater.Close()

                Console.Clear()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Using an S parameter of 0 will show these results you have just set.")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine()

                For SType = 0 To TextString.Length - 1
                    If SType = Line Then
                        Console.WriteLine(TextString(SType))
                    ElseIf TextString(SType).IndexOf("|") <> -1 Or TextString(SType).IndexOf(":") = -1 Then
                    Else
                        Try
                            Console.WriteLine(TextString(SType))
                        Catch
                            Console.WriteLine()
                        End Try
                    End If
                Next

                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Done!")
                Console.BackgroundColor = ConsoleColor.Black

                Console.ReadLine()
            ElseIf Choice = "2" Then
                Console.Clear()
                Console.WriteLine("Here are your custom S parameter settings:")
                Console.WriteLine()

                For SType = 0 To TextString.Length - 1
                    If TextString(SType).IndexOf("|") <> -1 Or TextString(SType).IndexOf(":") = -1 Then
                    Else
                        Try
                            Console.WriteLine(TextString(SType))
                        Catch
                        End Try
                    End If
                Next

                For Clear = 0 To TextString.Length - 1
                    TextString(Clear) = TextString(Clear).Trim
                Next

                If TextString.Length < 3 Then
                    Console.WriteLine("There are not settings to show, you have not set any yet.")
                End If

                Console.ReadLine()
                Preferences()
            ElseIf Choice = "3" Then
                My.Computer.FileSystem.DeleteFile("C:\ProgramData\Japanese Conjugation Helper\Preferences\SParameter.txt")

                TextWriter = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\SParameter.txt")
                TextWriter.WriteLine("Formal:1")
                TextWriter.WriteLine("Informal:1")

                TextWriter.WriteLine("Te-form:1")

                TextWriter.WriteLine("Volitional:1")
                TextWriter.WriteLine("Potential:1")
                TextWriter.WriteLine("Causative (let & made):1")
                TextWriter.WriteLine("Passive:1")
                TextWriter.WriteLine("Conditional:1")

                TextWriter.WriteLine("Want:1")

                TextWriter.WriteLine("Need to:1")

                TextWriter.WriteLine("Extras:1")

                TextWriter.WriteLine("Noun/adj conjugations:1")

                TextWriter.WriteLine("Kanji details:1")

                TextWriter.Close()

                Console.Clear()
                Console.WriteLine("Your preferences have been reset")
                Console.ReadLine()
                Preferences()
            Else

                Main()
            End If

            Main()
        ElseIf Choice = "2" Then
            Try
                FileReader = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
            Catch
                File.Create("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt").Dispose() 'This text file will store user preferences
                TextWriter = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
                TextWriter.WriteLine("Default 's=' parameter:4")
                TextWriter.WriteLine("Maximum definitions shown:6")
                TextWriter.WriteLine("Reading shown first:kun")
                TextWriter.WriteLine("Simplify kun readings:0")
                TextWriter.Close()
            End Try

            FileReader = My.Computer.FileSystem.ReadAllText("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
            TextString = FileReader.Split(vbCrLf)

            Dim All As Boolean = False
            Dim Write As Integer = 0

            Dim SettingChange As String = ""
            Choice = False

            Do Until IsNumeric(SettingChange) = True
                Console.Clear()
                Console.WriteLine("Which setting would you like to change?")
                Console.WriteLine("Type 'b' to go back")
                Console.WriteLine()
                Console.WriteLine("0 = View current settings")
                Console.WriteLine("1 = Reset general settings")
                Console.WriteLine()

                Write = 0
                All = False

                For Clear = 0 To TextString.Length - 1
                    TextString(Clear) = TextString(Clear).Trim
                Next

                'Getting rid of extra lines at the end
                Do Until TextString(TextString.Length - 1) <> ""
                    If TextString(TextString.Length - 1) = "" Then
                        Array.Resize(TextString, TextString.Length - 1)
                    End If
                Loop

                Dim LineLooser As System.IO.StreamWriter
                LineLooser = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
                For Writer = 0 To TextString.Length - 1
                    LineLooser.WriteLine(TextString(Writer))
                Next
                LineLooser.Close()

                Do Until Write = TextString.Length Or All = True
                    Try
                        'Console.WriteLine(Write + 2 & " = " & Left(TextString(Write).Trim, TextString(Write).Trim.IndexOf(":")))
                        Console.WriteLine(Write + 2 & " = " & TextString(Write).Trim)
                    Catch
                        All = True
                        Continue Do
                    End Try

                    Write += 1
                Loop

                SettingChange = Console.ReadLine.ToLower

                If SettingChange.IndexOf("b") <> -1 Then
                    Preferences()
                End If

                If IsNumeric(SettingChange) = True Then
                    If SettingChange < 0 Or SettingChange > TextString.Length + 1 Then
                        SettingChange = "a"
                    End If
                End If

                Console.WriteLine()
            Loop

            Dim NewSetting As String = ""
            Write = 0
            All = False
            If SettingChange = 0 Then
                Console.Clear()
                Console.WriteLine("Current General Settings:")
                Console.WriteLine()

                Do Until Write = TextString.Length + 1 Or All = True
                    Try
                        Console.WriteLine(TextString(Write).Trim)
                    Catch
                        All = True
                        Continue Do
                    End Try

                    Write += 1
                Loop
                Console.ReadLine()
                Preferences()

            ElseIf SettingChange = 1 Then
                TextWriter = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
                TextWriter.WriteLine("Default 's=' parameter:4")
                TextWriter.WriteLine("Maximum definitions shown:6")
                TextWriter.WriteLine("Reading shown first:kun")
                TextWriter.WriteLine("Simplify kun readings:0")
                TextWriter.Close()
                Console.WriteLine("Done")

                Console.ReadLine()
                Preferences()

            ElseIf SettingChange = 2 Then
                Do Until IsNumeric(NewSetting) = True
                    Console.Clear()
                    Console.WriteLine("What would you like the new default S value to be (0-4)?")
                    Console.WriteLine()

                    NewSetting = Console.ReadLine
                    If IsNumeric(NewSetting) = True Then
                        If NewSetting < 0 Or NewSetting > 4 Or NewSetting.Length > 1 Then
                            NewSetting = ""
                        End If
                    End If

                    If NewSetting.ToLower = "b" Or NewSetting.ToLower = "back" Or NewSetting.ToLower = "stop" Then
                        Main()
                    End If
                Loop

                TextString(0) = TextString(0).Replace(Right(TextString(0), 1), NewSetting)

                Dim TextUpdater As System.IO.StreamWriter
                TextUpdater = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
                For Writer = 0 To TextString.Length - 1
                    TextUpdater.WriteLine(TextString(Writer))
                Next
                TextUpdater.Close()

                Console.WriteLine("Done")

                Console.ReadLine()
                Preferences()
            ElseIf SettingChange = "3" Then

                Do Until IsNumeric(NewSetting) = True
                    Console.Clear()
                    Console.WriteLine("What would you like the new Maximum Definition value to be (1-99)?")
                    Console.ForegroundColor = ConsoleColor.DarkGray
                    Console.WriteLine("Note: The program ignores this number if there is a definition of the type 'Auxiliary Verb', 'prefix' or 'suffix'")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.WriteLine()

                    NewSetting = Console.ReadLine
                    If IsNumeric(NewSetting) = True Then
                        If NewSetting < 1 Or NewSetting > 99 Or NewSetting.Length > 2 Then
                            NewSetting = ""
                        End If
                    End If
                Loop

                NewSetting = NewSetting.Replace(".", "")

                If IsNumeric(Right(TextString(1), 2)) = False Then
                    TextString(1) = TextString(1).Replace(Right(TextString(1), 1), NewSetting)
                Else
                    TextString(1) = TextString(1).Replace(Right(TextString(1), 2), NewSetting)
                End If

                Dim TextUpdater As System.IO.StreamWriter
                TextUpdater = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
                For Writer = 0 To TextString.Length - 1
                    TextUpdater.WriteLine(TextString(Writer))
                Next
                TextUpdater.Close()

                Console.WriteLine("Done")

                Console.ReadLine()
                Preferences()
            ElseIf SettingChange = "4" Then 'On or Kun being shown first ------
                Console.Clear()
                Console.WriteLine("Which reading would you like to be shown first? (either Onyomi or Kunyomi)")

                Dim UserInput As String
                UserInput = Console.ReadLine
                If Left(UserInput, 2) = "ku" Then
                    NewSetting = "kun"
                ElseIf Left(UserInput, 2) = "on" Then
                    NewSetting = "on"
                Else
                    Preferences()
                End If

                TextString(2) = TextString(2).Replace("on", NewSetting)
                TextString(2) = TextString(2).Replace("kun", NewSetting)

                Dim TextUpdater As System.IO.StreamWriter
                TextUpdater = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
                For Writer = 0 To TextString.Length - 1
                    TextUpdater.WriteLine(TextString(Writer))
                Next
                TextUpdater.Close()

                NewSetting = NewSetting.Replace("on", "Onyomi").Replace("kun", "Kunyomi")

                Console.WriteLine("The reading this is shown first is now '" & NewSetting & "'")

                Console.ReadLine()
                Preferences()
            ElseIf SettingChange = "5" Then
                Console.Clear()
                Console.WriteLine("Would you like simplified Kun readings? For example:")
                Console.WriteLine()
                Console.WriteLine("Non-simplified: そと、ほか、はず.す、はず.れる、と-")
                Console.WriteLine("Simplified: そと、ほか、はず、と")
                NewSetting = Console.ReadLine.ToLower.Trim
                If NewSetting.Contains("y") = True Or NewSetting.Contains("true") = True Or NewSetting.Contains("simp") Then
                    NewSetting = "1"
                ElseIf NewSetting.Contains("n") = True Or NewSetting.Contains("false") = True Then
                    NewSetting = "0"
                End If

                Console.Clear()
                TextString(3) = TextString(3).Replace("1", NewSetting)
                TextString(3) = TextString(3).Replace("0", NewSetting)

                Dim TextUpdater As System.IO.StreamWriter
                TextUpdater = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
                For Writer = 0 To TextString.Length - 1
                    TextUpdater.WriteLine(TextString(Writer))
                Next
                TextUpdater.Close()

                NewSetting = NewSetting.Replace("1", "Simplified").Replace("0", "Non-simplified")

                Console.WriteLine("The Kun reading will now be " & NewSetting & "")

                Console.ReadLine()
                Preferences()
            End If


        ElseIf Choice = "3" Then
            Console.WriteLine("Are you sure you want to reset ALL settings?")
            Console.WriteLine("Please type " & QUOTE & "yes" & QUOTE & " if you are sure that you want to do this.")
            Console.ForegroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Type 'b' to go back, or anything else to cancel the reset.")
            Console.ForegroundColor = ConsoleColor.White

            Choice = Console.ReadLine.ToLower

            If Choice = "yes" Then
                Try
                    My.Computer.FileSystem.DeleteFile("C:\ProgramData\Japanese Conjugation Helper\Preferences\SParameter.txt")
                Catch
                    Try
                        My.Computer.FileSystem.DeleteFile("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
                    Catch
                    End Try
                End Try

                Try
                    Dim FileBuilderUpdater As System.IO.StreamWriter
                    FileBuilderUpdater = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\General.txt")
                    FileBuilderUpdater.WriteLine("Default 's=' parameter:4")
                    FileBuilderUpdater.WriteLine("Maximum definitions shown:6")
                    FileBuilderUpdater.Close()

                    Dim FileBuilderUpdater2 As System.IO.StreamWriter
                    FileBuilderUpdater2 = New System.IO.StreamWriter("C:\ProgramData\Japanese Conjugation Helper\Preferences\SParameter.txt")
                    FileBuilderUpdater2.WriteLine("Formal:1")
                    FileBuilderUpdater2.WriteLine("Informal:1")
                    FileBuilderUpdater2.WriteLine("Te-form:1")
                    FileBuilderUpdater2.WriteLine("Volitional:1")
                    FileBuilderUpdater2.WriteLine("Potential:1")
                    FileBuilderUpdater2.WriteLine("Causative (let & made):1")
                    FileBuilderUpdater2.WriteLine("Passive:1")
                    FileBuilderUpdater2.WriteLine("Conditional:1")
                    FileBuilderUpdater2.WriteLine("Want:1")
                    FileBuilderUpdater2.WriteLine("Need to:1")
                    FileBuilderUpdater2.WriteLine("Extras:1")
                    FileBuilderUpdater2.WriteLine("Noun/adj conjugations:1")
                    FileBuilderUpdater2.WriteLine("Kanji details:1")
                    FileBuilderUpdater2.Close()
                Catch
                    Console.Clear()
                    Console.WriteLine("Something went wrong :O")
                    Console.ReadLine()
                    Main()
                End Try

                Console.Clear()
                Console.WriteLine("All settings were successfully reset.")
                Console.ReadLine()
                Preferences()
            ElseIf Choice = "b" Or Choice = "back" Then
                Preferences()
            ElseIf Choice = "n" Or Choice = "no" Or Choice = "main" Then
                Main()
            Else
                Console.Clear()
                Console.WriteLine("No settings were reset because you didn't type " & QUOTE & "yes" & QUOTE & ".")
                Console.ReadLine()
            End If

        ElseIf Choice = "4" Then
            Console.WriteLine("Are you sure you want clear your search history? (This is your last 40 searches)")
            Choice = Console.ReadLine.Trim.ToLower
            If Choice = "yes" Then
                Try
                    My.Computer.FileSystem.DeleteFile("C:\ProgramData\Japanese Conjugation Helper\SearchHistory.txt")
                Catch
                    Console.WriteLine("Something went wrong")
                    Console.WriteLine("You probably don't have anything in your search history")
                    Console.WriteLine("You can check by doing '/files' and checking if a file named 'SearchHistory.txt' exists")
                    Console.ReadLine()
                    Main()
                End Try
                Console.WriteLine("Your history has been cleared.")
            Else
                Console.WriteLine("You decided not to clear you history.")
            End If

            Console.ReadLine()
        End If

        Main()
    End Sub
    Sub WriteToFile(ByVal ToWrite, ByVal FileName)
        Try
            FileName = "C:\ProgramData\Japanese Conjugation Helper\" & FileName & ".txt"

            If System.IO.File.Exists(FileName) = False Then
                Console.WriteLine("An error occurred.")
                Console.WriteLine("Tried to write '" & ToWrite & "' -> " & "'" & FileName & "'")
                Console.ReadLine()
                Main()
            End If

            Using Writer As System.IO.TextWriter = System.IO.File.AppendText(FileName)
                Writer.WriteLine(ToWrite)
                Writer.Close()
            End Using
        Catch ex As Exception
            If DebugMode = True Then
                Console.WriteLine(ex.Message)
            End If
        End Try
    End Sub
    Sub LoadWordJson()
        Dim JsonReader As String
        Dim FileEntries(), WordEntries() As String
        Dim Correct As Boolean = False
        Dim I As Integer
        Try
            Console.WriteLine("Loading for offline use...")

            Array.Resize(Words, Words.Length + 1)
            For JSon = 1 To 29 'Loop for each file
                JsonReader = My.Computer.FileSystem.ReadAllText("jmdict_english\term_bank_" & JSon & ".json")
                'Getting rid of starting '[[' and ending ']]':
                JsonReader = Mid(JsonReader, 3)
                FileEntries = Split(JsonReader, "],[")

                For Word = 0 To 999
                    WordEntries = FileEntries(Word).Split(",")

                    'Adding info to words array:
                    Words(Words.Length - 1) = New Word
                    Words(Words.Length - 1).Word = WordEntries(0).Replace("""", "") 'Adding actual word
                    Words(Words.Length - 1).Furigana = WordEntries(1).Replace("""", "") 'Adding furigana
                    Words(Words.Length - 1).Types = WordEntries(2).Split(" ") 'Adding word types
                    Words(Words.Length - 1).Page = JSon 'Adding which JSON file it is in

                    'Adding the meanings (which are often spread across a few array positions start at 5)
                    Correct = False
                    I = 5
                    Do Until Correct = True
                        Words(Words.Length - 1).Meanings(Words(Words.Length - 1).Meanings.Length - 1) = WordEntries(I) 'Current free slot in 'meanings' array will be filled
                        If WordEntries(I).Contains("]") Then
                            Correct = True
                            Continue Do
                        End If

                        Array.Resize(Words(Words.Length - 1).Meanings, Words(Words.Length - 1).Meanings.Length + 1) 'Increasing size of 'meanings' by 1
                        I += 1
                    Loop

                    For Replacer = 0 To Words(Words.Length - 1).Types.Length - 1
                        Words(Words.Length - 1).Types(Replacer) = Words(Words.Length - 1).Types(Replacer).Replace("""", "")
                    Next
                    For Replacer = 0 To Words(Words.Length - 1).Meanings.Length - 1
                        Words(Words.Length - 1).Meanings(Replacer) = Words(Words.Length - 1).Meanings(Replacer).Replace("""", "").Replace("[", "").Replace("]", "")
                    Next

                    Array.Resize(Words, Words.Length + 1)
                Next
            Next
            Array.Resize(Words, Words.Length - 1)
        Catch ex As Exception
            Console.WriteLine("Something went wrong; it seems the program files have been messed with")
            If DebugMode = True Then
                Console.WriteLine(ex.Message)
            End If
            Console.ReadLine()
            Main()
        End Try
    End Sub
    Sub WordJsonSearch(Word)
        Word = Word.replace("//", "")
        If Words.Length < 100 Then
            Array.Resize(Words, 0)
            LoadWordJson()
        End If
        Console.Clear()
        Console.WriteLine("Searching for '{0}'", Word)
        Dim Results As New List(Of Word) From {}

        For Each ArrayWord In Words
            If ArrayWord.Word.Contains(Word) = True Or ArrayWord.Furigana.Contains(Word) = True Or ArrayWord.Meanings(0).Contains(Word) = True Or ArrayWord.Furigana.Contains(WanaKana.ToHiragana(Word)) = True Then
                Results.Add(ArrayWord)
            End If
        Next

        If Results.Count > 20 Then
            Do Until Results.Count <= 20
                Results.Remove(Results(Results.Count - 1))
            Loop
        End If

        Console.Clear()
        For Each Result In Results
            For Type = 0 To Result.Types.Length - 1
                If Result.Types(Type) = "n" Then
                    Result.Types(Type) = "noun"
                End If
                Result.Types(Type) = Result.Types(Type).Replace("vs", "suru-verb").Replace("adj", "adjective").Replace("uk", "usually as kana").Replace("sl", "slang")
                Console.WriteLine(Result.Types(Type))
            Next
            Console.WriteLine(Result.Word & " (" & Result.Furigana & ")")
            For Meaning = 0 To Result.Meanings.Length - 1
                Console.WriteLine(Meaning + 1 & ". " & Result.Meanings(Meaning))
            Next
            Console.WriteLine()
        Next
        Console.ForegroundColor = ConsoleColor.DarkGray
        If Results.Count = 0 Then
            Console.WriteLine("Couldn't find results for " & Word)
            Console.WriteLine("Offline searching mostly supports nouns and phrases so you may not be able to find this word.")

        End If

        Console.WriteLine()
        Console.WriteLine("Offline searching should only be used for emergencies:")
        Console.WriteLine(" - Not very effective")
        Console.WriteLine(" - Usually only supports weirdly specific words.")
        Console.WriteLine(" - No features are supported with it")
        Console.ForegroundColor = ConsoleColor.White

        Console.ReadLine()
        Main()
    End Sub
    Sub Help(ByVal Command)
        Const QUOTE = """"
        Command = Command.ToLower
        Console.Clear()

        If Command = "search" Or Command = "wordlookup" Or Command = "word" Or Command = "lookup" Or Command = "definition" Or Command = "conjugate" Then
            Console.WriteLine("WordLookup: To lookup a word and bring up information and conjugation patterns, simply type and English or Japanese word, Japanese words can also be written using romaji (english transliteration), surround words in quotes to make sure the program knows that it is definitely the english word you are seaching for and not romaji. Example: " & QUOTE & "hate" & QUOTE)
            Console.WriteLine("the (number of results) parameter is how many results the program will bring up, you then choose one of these results to have details about it show up.")
            Console.WriteLine("This command is best with adjectives and verbs because they have the most conjugation patterns, but nouns work too!")
            Console.WriteLine("This isn't technically a command because it doesn't use a " & QUOTE & "/" & QUOTE)
            Console.WriteLine()
            Console.WriteLine("Syntax: (english/japanese/romaji) (number of results)")
            Console.WriteLine("Examples:")
            Console.WriteLine("静か")
            Console.WriteLine("shizuka")
            Console.WriteLine("quiet 5")
            Console.WriteLine("interesting 15")
            Console.ReadLine()
            Main()
        End If

        If Command = "/r" Or Command = "r" Or Command.indexof("reading") <> -1 Then
            Console.WriteLine("/r: Reading practice, to use this command (sentences) must be in a specific format")
            Console.WriteLine("This command is pretty useful for the average user because it requires a hotkey file to be affective")
            Console.WriteLine("This command is pretty much creating flashcards, so as long as you follow the syntax you can make it do anything")
            Console.WriteLine()
            Console.WriteLine()
            Console.WriteLine("Syntax; /r (sentences) [2]")
            Console.WriteLine("(sentences) is a specific format of japanese sentences and its english meaning")
            Console.WriteLine("If you have have the " & QUOTE & "2" & QUOTE & " parameter at the end, it means you are pasting in more than one batch of sentences")
            Console.WriteLine()
            Console.WriteLine("(sentences) format:")
            Console.WriteLine("|(Japanese sentence)^(english meaning)")
            Console.WriteLine(QUOTE & "|" & QUOTE & " is the start of a japanese sentence, " & QUOTE & "^" & QUOTE & " is the start of the english sentence.")
            Console.WriteLine()
            Console.WriteLine("You can paste in a whole batch like this:")
            Console.WriteLine("|(Japanese sentence)^(english meaning)" & "|(Japanese sentence)^(english meaning)" & "|(Japanese sentence)^(english meaning)")
            Console.WriteLine()
            Console.WriteLine()
            Console.WriteLine("Examples:")
            Console.WriteLine("|そこで私たちを待っている幸福が、私たちが望むような幸福ではないかもしれない。 ^It may be that the happiness awaiting us is not at all the sort of happiness we would want.")
            Console.WriteLine()
            Console.WriteLine("|家に帰りましょうか。 ^Why don't we go home?|「少しうちに寄っていかない？」「いいの？」「うち共働きで親は遅いの」 ^" & QUOTE & "Want To drop round my place?" & QUOTE & "can I?" & QUOTE & " My parents come home late As they both work." & QUOTE & "|先ずは憧れの作家の文章の呼吸をつかむためにひたすら筆写、丸写しをする。 ^First, in order to get a feel for your favourite author's work, transcribe and copy in full.")
            Console.ReadLine()
            Main()
        End If

        If Command = "k" Or Command = "/k" Or Command = "/kanjitest" Or Command = "kanjitest" Or Command = "/kt" Then
            Console.WriteLine("KanjiTest: Enter Japanese text and then generate a quiz of the kanji in that text")
            Console.WriteLine("Type '/k' and then enter Japanese text; as long as the text contains kanji it will work")
            Console.WriteLine("Syntax: /k")
            Console.WriteLine()
            Console.WriteLine()
            Console.WriteLine("Examples:")
            Console.ReadLine()
            Main()
        End If

        If Command = "!" Or Command.indexof("transl") <> -1 Then
            Console.WriteLine("Translate: Translate Japanese into English or the other way round")
            Console.WriteLine("Syntax: ![text]")
            Console.WriteLine()
            Console.WriteLine()
            Console.WriteLine("Examples:")
            Console.WriteLine("!this will work")
            Console.WriteLine("!これも")
            Console.ReadLine()
            Main()
        End If

        If Command = "/h" Or Command = "/help" Or Command = "help" Or Command = "h" Then
            Console.WriteLine("/h: Brings up this help menu, if you want more help with a command then add the command parameter:")
            Console.WriteLine("Syntax: /h [command]")
            Console.WriteLine()
            Console.WriteLine()
            Console.WriteLine("Examples:")
            Console.WriteLine("/h /r")
            Console.WriteLine("/help WordLookup")
            Console.ReadLine()
            Main()
        End If

        If Command = "/p" Or Command = "p" Then
            Console.WriteLine("/p: Start a small quiz that helps you conjugate verbs, this only works with verbs but will later work with adjectives and nouns, the (word) parameter is the word that you want help conjugating")
            Console.WriteLine("Japanese words can also be written using romaji (english transliteration), surround words in quotes to make sure the program knows that it is definitely the english word you are seaching for and not romaji. Example: " & QUOTE & "hate" & QUOTE)
            Console.WriteLine("Syntax: /p [english/japanese/romaji word]")
            Console.WriteLine()
            Console.WriteLine("When the quiz starts it will tell you which form to change the word to, failing lowers your score.")
            Console.ReadLine()
            Main()
        End If

        If Command.length < 13 And Command.indexof("pref") <> -1 Then
            Console.WriteLine("/prefs: Lets you change settings for the program.")
            Console.WriteLine("Settings are mostly do with with how information is displayed")
            Console.WriteLine()
            Console.WriteLine("Read the Wiki information on what each setting does")
            Console.ReadLine()
            Main()
        End If

        If Command.length < 10 And Command.indexof("audio") <> -1 Then
            Console.WriteLine("Download audio of various conjugations of a verb")
            Console.WriteLine("Only works with verbs (doesn't work with suru-verbs)")
            Console.WriteLine()
            Console.WriteLine("Syntax: /audio [verb] (!s/f)")
            Console.WriteLine("Add !f if you want female audio")
            Console.WriteLine("Add !m if you want female audio")
            Console.WriteLine()
            Console.WriteLine("If you don't specify gender of pronunciations, it will be male by default")
            Console.WriteLine("Example: /audio odoru !f")
            Console.ReadLine()
            Main()
        End If

        Console.ForegroundColor = ConsoleColor.Yellow
        Console.WriteLine("There is no information for " & QUOTE & Command & QUOTE)
        Console.WriteLine(QUOTE & Command & QUOTE & " is not a command.")
        Console.ForegroundColor = ConsoleColor.White
        Console.ReadLine()
        Main()
    End Sub

End Module
