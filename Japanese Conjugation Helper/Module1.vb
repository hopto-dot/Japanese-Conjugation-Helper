Imports System
Imports System.Net
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic
Imports System.IO
Module Module1
    Sub Main()
        Console.Clear()
        'For the input of Japanese Chaaracters
        Console.Title() = "Conjugator"
        Console.InputEncoding = System.Text.Encoding.Unicode
        Console.OutputEncoding = System.Text.Encoding.Unicode
        Randomize()

        Const QUOTE = """"

        'Download()
        'Template for sound
        'My.Computer.Audio.Play("", AudioPlayMode.Background)
        Dim ExePath As String = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
        Dim User As String = Mid(ExePath, ExePath.IndexOf("Users") + 7)
        User = Left(User, User.IndexOf("\"))

        If Int((50) * Rnd()) <> 2 Then
            Console.WriteLine("Enter a command, or type " & QUOTE & "/h" & QUOTE & " for help")
        Else
            Console.WriteLine("Enter a command! ^o^")
        End If

        'This is getting the word that is being searched ready for more accurate search with ActualSearchWord, ActualSearch Word (should) always be in japanese while Word won't be if the user inputs english or romaji:
        Dim Word As String = Console.ReadLine.ToLower 'This is the word that will be searched, this needs to be kept the same because it is the original search value that may be needed later

        If Word = "" Or Word.IndexOf(vbCrLf) <> -1 Then
            Main()
        End If

        If Word = "!test" Then 'This is just a test
            Download()
            Console.WriteLine("Done.")
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
            Catch
                Console.Clear()
                Console.WriteLine("No program files have been created yet.")
                Console.WriteLine("Type '/prefs' to get started.")
                Console.ReadLine()
            End Try

            Main()
        End If

        If Left(Word, 5) = "/pref" Then
            Preferences()
        End If

        If Word = "/" Then
            Console.WriteLine("This is not a command")
            Console.ReadLine()
            Main()
        End If

        If Left(Word, 3) = "/r " Then
            ReadingPractice(Right(Word, Word.Length - 3))
        End If
        If Left(Word, 2) = "/r" Then
            Console.WriteLine("Wrong format.")
            Console.ReadLine()
            Main()
        End If

        If Left(Word, 3) = "/p " Then
            ConjugationPractice(Right(Word, Word.Length - 3))
        End If

        If Word = "/h" Or Word = "/help" Then
            Console.Clear()
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Find more info in the wiki.")
            Console.WriteLine("If you want to give feedback, request a feature or report bugs do '/feeback' or '/rate'")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine()
            Console.WriteLine("List of commands (note: commands are case sensitive):")

            Console.WriteLine()
            Console.WriteLine("/h: brings up this help menu, if you want more help with a command then add the [command] parameter:")
            Console.WriteLine("Syntax: /h [command]")

            Console.WriteLine()
            Console.WriteLine("WordLookup: To lookup a word and bring up information and conjugation patterns, simply type and English or Japanese word, Japanese words can also be written using romaji (english transliteration), surround words in quotes to make sure the program knows that it is definitely the english word you are seaching for and not romaji. Example: " & QUOTE & "hate" & QUOTE)
            Console.WriteLine("Syntax: [english/japanese/romaji word]")

            Console.WriteLine()
            Console.WriteLine("/r: reading practice, to use this command (sentences) must be in a specific format")
            Console.WriteLine("Syntax: /r (sentences) [2]")
            Console.WriteLine("do " & QUOTE & "/help /r" & QUOTE & " to see more about this command")

            Console.WriteLine()
            Console.WriteLine("/p: start a small quiz that helps you conjugate verbs, this only works with verbs but will later work with adjectives and nouns, the (word) parameter is the word that you want help conjugating")
            Console.WriteLine("Syntax: /p [english/japanese/romaji word]")
            Console.WriteLine()

            Console.WriteLine()
            Console.WriteLine("/prefs: Changes program preferences")
            Console.WriteLine()

            Console.WriteLine()
            Console.WriteLine("/git: Brings the program repository page on github using Chrome")
            Console.WriteLine()

            Console.WriteLine()
            Console.WriteLine("/files: Opens the folder the program uses to safe preferences")
            Console.WriteLine("Note: Only works once you have used the '/prefs' command")
            Console.WriteLine()

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
            Console.WriteLine("This is not a command")
            Console.ReadLine()
            Main()
        End If

        If Word.Length > 2 Then
            If Mid(Word, Word.Length - 2, 1) = " " And IsNumeric(Right(Word, 2)) = True Then
                Console.WriteLine("Searching for " & Right(Word, 2) & " definitions of '" & Left(Word, Word.Length - 3) & "'...")
                WordConjugate(Left(Word, Word.Length - 2), Right(Word, 2), False)
            ElseIf Mid(Word, Word.Length - 1, 1) = " " And IsNumeric(Right(Word, 1)) = True And Right(Word, 1) <> "1" Then
                Console.WriteLine("Searching for " & Right(Word, 1) & " definitions of '" & Left(Word, Word.Length - 2) & "'...")
                WordConjugate(Left(Word, Word.Length - 2), Right(Word, 1), False)
            ElseIf Mid(Word, Word.Length - 1, 1) = " " And IsNumeric(Right(Word, 1)) = True And Right(Word, 1) = "1" Then
                Console.WriteLine("Searching for '" & Word & "'...")
                WordConjugate(Left(Word, Word.Length - 2), Right(Word, 1), False)
            Else
                Console.WriteLine("Searching for '" & Word & "'...")
                WordConjugate(Word, 1, False)
            End If
        Else
            Console.WriteLine("Searching for " & Word & "...")
            WordConjugate(Word, 1, False)
        End If


        Main()
    End Sub
    Sub Help(ByVal Command)
        Const QUOTE = """"
        Command = Command.ToLower
        Console.Clear()

        If Command = "wordlookup" Or Command = "word" Or Command = "lookup" Or Command = "definition" Or Command = "conjugate" Then
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
            Console.Read()
            Main()
        End If

        If Command = "/r" Or Command = "r" Then
            Console.WriteLine("/r: Reading practice, to use this command (sentences) must be in a specific format")
            Console.WriteLine("@JawGBoi has another program that generates example sentences in this format, ask him and he'll give it to you. It is an AHK file.")
            Console.WriteLine("This command is pretty much creating flashcards, so as long as you follow the syntax you can make it do anything")
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
            Console.Read()
            Main()

            Console.WriteLine()
            Console.WriteLine()
            Console.WriteLine("Examples:")
            Console.WriteLine("|そこで私たちを待っている幸福が、私たちが望むような幸福ではないかもしれない。 ^It may be that the happiness awaiting us is not at all the sort of happiness we would want.")
            Console.WriteLine()
            Console.WriteLine("|家に帰りましょうか。 ^Why don't we go home?|「少しうちに寄っていかない？」「いいの？」「うち共働きで親は遅いの」 ^" & QUOTE & "Want To drop round my place?" & QUOTE & "can I?" & QUOTE & " My parents come home late As they both work." & QUOTE & "|先ずは憧れの作家の文章の呼吸をつかむためにひたすら筆写、丸写しをする。 ^First, in order to get a feel for your favourite author's work, transcribe and copy in full.")
            Console.Read()
            Main()

        End If

        If Command = "/h" Or Command = "/help" Or Command = "help" Then
            Console.WriteLine("/h: Brings up this help menu, if you want more help with a command then add the command parameter:")
            Console.WriteLine("Syntax: /h [command]")
            Console.WriteLine("Examples:")
            Console.WriteLine("/h /r")
            Console.WriteLine("/help WordLookup")
            Console.Read()
            Main()
        End If

        If Command = "/p" Or Command = "p" Then
            Console.WriteLine("/p: Start a small quiz that helps you conjugate verbs, this only works with verbs but will later work with adjectives and nouns, the (word) parameter is the word that you want help conjugating")
            Console.WriteLine("Japanese words can also be written using romaji (english transliteration), surround words in quotes to make sure the program knows that it is definitely the english word you are seaching for and not romaji. Example: " & QUOTE & "hate" & QUOTE)
            Console.WriteLine("Syntax: /p [english/japanese/romaji word]")
            Console.WriteLine()
            Console.WriteLine("When the quiz starts it will tell you which form to change the word to, failing lowers your score.")
            Console.Read()
            Main()
        End If

        Console.WriteLine("There is no information for " & QUOTE & "/" & Command & QUOTE)
        Console.WriteLine(QUOTE & "/" & Command & QUOTE & " is not a command.")
        Console.Read()
        Main()
    End Sub

    Sub WordConjugate(ByRef Word As String, ByVal WordIndex As Integer, ByVal Anki As Boolean)
        Const QUOTE = """"
        Dim SEquals As String = ""
        Dim DefG1Test As String = ""
        Dim DefG1 As Integer = 10

        'Code for the "s=" parameter; only shows the more advanced forms:
        Dim AdvancedParam As Integer = 0

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


        Dim SentenceExample As String = ""
        Dim Example As String = ""
        Dim WordURL As String = "https://jisho.org/search/" & Word
        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8

        Dim HTML As String
        HTML = Client.DownloadString(New Uri(WordURL))
        Dim AddingTemp As String

        If WordIndex > 20 Then
            WordURL = ("https://jisho.org/search/" & Word & "%20%23words?page=2")
            AddingTemp = Client.DownloadString(New Uri(WordURL))
            HTML = HTML & AddingTemp
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
        If WordIndex <> 1 Then              'scraping of words and definitions -------------------------------------------------------------------------------------------
            For LoopIndex = 0 To Max - 1
                Array.Resize(FoundWords, FoundWords.Length + 1)
                Array.Resize(FoundDefinitions, FoundDefinitions.Length + 1)
                Array.Resize(FoundWordLinks, FoundWordLinks.Length + 1)
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

                FoundDefinitions(LoopIndex) = DefinitionScraper(WordLink).replace("&#39;", "")
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

            Dim IntTest As String = ""

            Dim FirstBlank As Boolean = True
            Do Until WordChoice <= FoundDefinitions.Length And WordChoice >= 1
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
                        Console.WriteLine("No words were found.")
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
                WordConjugate(Word, 20, False)
            End If

            FoundTypes = TypeScraper(FoundWordLinks(WordChoice - 1)).replace("&#39;", "")
            FoundTypes = FoundTypes.Replace("!", "")
            FoundDefinitions(0) = WordLinkScraper(FoundWordLinks(WordChoice - 1)).replace("&#39;", "")
            Console.WriteLine()
        Else                                                              'One word scrapping ---------------------------------------------------------------------------------------------
            Try
                ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & QUOTE & "text" & QUOTE & ">")
                HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
                ActualSearch1stAppearance = HTMLTemp.IndexOf("meanings-wrapper") 'used to be "concept_light clearfix"
                HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
                ActualSearch1stAppearance = HTMLTemp.IndexOf("jisho.org/word/")
                ActualSearch2ndAppearance = Mid(HTMLTemp, HTMLTemp.IndexOf("jisho.org/word/")).IndexOf(QUOTE & ">")
                WordLink = Mid(HTMLTemp, ActualSearch1stAppearance + 1, ActualSearch2ndAppearance - 1)
                FoundWordLinks(0) = WordLink
            Catch
                Console.WriteLine("That word doesn't exist... Atleast, it seems that way :O")
                Console.ReadLine()
                Main()
            End Try


            FoundDefinitions(0) = WordLinkScraper(WordLink).replace("&#39;", "")
            FoundTypes = TypeScraper(WordLink).replace("&#39;", "")


            ActualSearchWord = RetrieveClassRange(HTML, "<span class=" & QUOTE & "text" & QUOTE & ">", "</div>", "Actual word search")
            If ActualSearchWord.Length < 2 Then
                Console.WriteLine("Word couldn't be found")
                Console.ReadLine()
                Main()
            End If
            ActualSearchWord = Mid(ActualSearchWord, 30)
            ActualSearchWord = ActualSearchWord.Replace("<span>", "")
            ActualSearchWord = ActualSearchWord.Replace("</span>", "")

            ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 8)
            Max = 0 'because we are in the "else" part of the if statement which means that the user inputted no number or 1
            WordChoice = 0
        End If                                                           'end of one word scrapping and all scrapping _______________________________________________________________________________________________________________________

        If WordChoice = 0 Then
            WordChoice = 1
        End If

        'Building of the chosen Definition and Type arrays:
        Dim SelectedDefinition() As String = FoundDefinitions(0).Split("|")
        Dim SelectedType() As String = FoundTypes.Split("|")
        For Add = 1 To SelectedDefinition.Length
            SelectedDefinition(Add - 1) = SelectedDefinition(Add - 1) & Add
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
        Dim Furigana As String = ""
        Dim FuriganaStart As Integer
        Try
            Furigana = RetrieveClassRange(HTMLTemp, "</a></li><li><a", "</a></li><li><a href=" & QUOTE & "//jisho.org", "Furigana")
            If Furigana.Length < 600 And Furigana.Length <> 0 Then
                FuriganaStart = Furigana.IndexOf("search for")
                Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 11)
                FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
                If FuriganaStart <> -1 Then 'if </a></li><li> Is found Then it will be removed as well as everything after it
                    Furigana = Left(Furigana, FuriganaStart)
                End If
            Else
                Furigana = ""
            End If

            If Furigana = ActualSearchWord Then 'This will repeat the last attempt to get the furigana, because the last furigana failed and got 'Sentences for [word using kanji]' instead of 'Sentences for [word using kana]'
                Furigana = RetrieveClassRange(HTMLTemp, "</a></li><li><a", "</a></li><li><a href=" & QUOTE & "//jisho.org", "Furigana")
                If Furigana.Length < 600 And Furigana.Length <> 0 Then
                    FuriganaStart = Furigana.IndexOf("search for")
                    Furigana = Mid(Furigana, FuriganaStart + 5)

                    FuriganaStart = Furigana.IndexOf("search for")
                    Furigana = Right(Furigana, Furigana.Length - FuriganaStart - 11)
                    FuriganaStart = Furigana.IndexOf("</a></li><li>") 'Now FuriganaStart is being used to find the start of more </a></li><li>, the next few lines is only needed for some searches which have extra things that need cutting out
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
            Furigana = ""
        End Try

        'Displaying word definitions WITH corresponding the word types: -------------------------
        DefG1 -= 1
        Dim NumberCheckD As String = ""
        Dim NumberCheckT As String = ""
        Dim Type As Integer = 0
        Dim Definition As Integer = 0
        Dim MatchT As Boolean = False
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
                        End If
                        Console.WriteLine(Left(SelectedType(Type), SelectedType(Type).Length - NumberCheckT.Length))
                        ElseIf Definition > DefG1 And SelectedType(Type).IndexOf("aux") <> -1 Or Definition > DefG1 And SelectedType(Type).IndexOf("fix") <> -1 Then
                            Console.WriteLine()
                        Console.WriteLine(Left(SelectedType(Type), SelectedType(Type).Length - NumberCheckT.Length))
                        Console.WriteLine()

                        'Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length))
                        If Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[") = -1 Then
                            Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length))
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
                            Else
                                SB1 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[")
                                SB2 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("]")
                                BArea = Mid(Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), SB1 + 1, SB2 + 1 - SB1)

                                If BArea.IndexOf("kana") = -1 Then
                                    Console.Write(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                                    Console.ForegroundColor = ConsoleColor.DarkGray
                                    Console.WriteLine(BArea)
                                    Console.ForegroundColor = ConsoleColor.White
                                Else
                                    BArea2 = BArea 'BArea is acting like a temp

                                    If BArea.IndexOf("Usually written using kana alone") <> -1 Then 'BArea will be set below if it has more than one thing
                                        BArea2 = BArea.Replace("Usually written using kana alone", "")
                                        BArea2 = BArea2.Replace(", ", "")
                                    End If

                                    If BArea2.Length > 3 Then
                                        BArea2 = BArea2.Replace("See also", "See also ")
                                        Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "") & BArea2)
                                    Else
                                        Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
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
                        Else
                            Console.WriteLine(Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, ""))
                        End If
                    End If
                End If
            End If


            Definition += 1
        Loop

        If SelectedDefinition.Length > DefG1 Then
            Console.WriteLine()
            Console.ForegroundColor = ConsoleColor.DarkGray
            Console.WriteLine("[this word has a total of " & SelectedDefinition.Length & " definitions]")
            Console.ForegroundColor = ConsoleColor.White
        End If

        Console.WriteLine()
        'finished the writing of definitions and types____________________

        If Furigana.Length < 15 Then
            Console.WriteLine(ActualSearchWord & "(" & Furigana & ")")
        Else
            Console.WriteLine(ActualSearchWord)
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
            Console.WriteLine("Is doing: しています")
            Console.WriteLine("Am doing: していいる")

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
            Console.WriteLine()
            Console.WriteLine("Causitive:")
            Console.WriteLine("Let/make (someone) do: させる")
            Console.WriteLine("Don't let/make (someone) do: させない")

        End If

        'Preparing some variables for the word being searched
        Dim Iadjective, NaAdjective, NoAdjective, Noun, Suru, Verb As Boolean 'Word Types Checker varibles
        'If the S=0 parameter allows it
        If TypeSnipEnd <> -1 Then 'Meaning: If the word has more than one type. The way I implemented the word type checker is kind of weird with the Else block. I could change this at a later date to make it more efficient.

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
                    If Anki = True Then
                        Verb = False
                    End If
                End If
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
                    ConjugateVerb(ActualSearchWord, "Godan", Scrap, ComparativeType, AdvancedParam) '(Verb/Word, Very Type, Meaning, "ComparativeType")
                End If

                If FullWordType.IndexOf("Ichidan verb") <> -1 Then
                    ConjugateVerb(ActualSearchWord, "Ichidan", Scrap, ComparativeType, AdvancedParam) '(Verb/Word, Very Type, Meaning, "ComparativeType")
                End If
                Console.WriteLine("Something went wrong when identifying the verb")
                Console.Read()
                Main()

            Else 'If there is only one word type, the if statement can look at the whole variable "TypeSnip" instead of use .index to determine if a Word Type is part of the whole string
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
                    If Anki = True Then
                        Verb = False
                    End If
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
                        ConjugateVerb(ActualSearchWord, "Godan", Scrap, ComparativeType, AdvancedParam) '(Verb/Word, Very Type, Meaning, "ComparativeType")
                    End If

                    If FullWordType.IndexOf("Ichidan verb") <> -1 Then
                        ConjugateVerb(ActualSearchWord, "Ichidan", Scrap, ComparativeType, AdvancedParam) '(Verb/Word, Very Type, Meaning, "ComparativeType")
                    End If
                    Console.WriteLine("Something went wrong when identifying the verb")
                    Console.Read()
                    Main()
                End If
            End If

            'This is checking if the start of the meaning variable is a vowel so that it can change the example sentence from a -> an if it is
            Dim Vowel As Boolean = False
            If Scrap(0) = "a" Or Scrap(0) = "e" Or Scrap(0) = "i" Or Scrap(0) = "o" Or Scrap(0) = "u" Then
                Vowel = True
            End If


        If AdvancedParam = 0 And PreferencesString(10) <> 0 Or AdvancedParam > 1 Then
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
            ActualSearchWord = ActualSearchWord & "い"
        End If

        'Example Sentence extraction:
        HTMLTemp = Client.DownloadString(New Uri(WordURL))
        SentenceExample = RetrieveClassRange(HTMLTemp, "<li class=" & QUOTE & "clearfix" & QUOTE & "><span class=" & QUOTE & "furigana" & QUOTE & ">", "inline_copyright", "Example Sentence") 'Firstly extracting the whole group
        If SentenceExample.Length > 10 Then
            Example = ExampleSentence(SentenceExample) 'This group then needs all the "fillers" taken out, that's what the ExampleSentence function does
        End If
        If Example.Length < 100 And Example.Length > 5 Then
            Console.WriteLine()
            Console.WriteLine(Example)
        End If

        Dim KanjiBool As Boolean = False
        If AdvancedParam = 0 Then
            If PreferencesString(11) <> 0 Then
                KanjiBool = True
            End If
        End If

        If AdvancedParam <> 0 Or KanjiBool = True Then
            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.Black
            Console.WriteLine("Kanji:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White

            Dim WordHTML As String = ""
            Try
                'Kanji, meanings and reading extract. First open the "/word" page and then extracts instead of extracting from "/search":
                Dim WordWordURL As String = ("https://jisho.org/word/" & ActualSearchWord)

                WordHTML = Client.DownloadString(New Uri(WordWordURL))


            Catch
                Console.WriteLine("No kanji information.")
                Console.ReadLine()
                Main()
            End Try

            Dim KanjiInfo As String = ""
            Try
                KanjiInfo = RetrieveClassRange(WordHTML, "<span class=" & QUOTE & "character literal japanese_gothic", "</aside>", "KanjiInfo")
            Catch
                Console.WriteLine("No kanji information.")
                Console.ReadLine()
                Main()
            End Try

            If KanjiInfo = "" Then
                Console.WriteLine("No kanji information.")
                Console.ReadLine()
                Main()
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

                    ActualInfo(Looper, 2) &= "Kun Readings: "
                    ActualInfo(Looper, 3) &= "On Readings: "

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

                    Console.BackgroundColor = ConsoleColor.DarkGray
                    Console.WriteLine(ActualInfo(Looper, 0) & " - " & ActualInfo(Looper, 1))
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.WriteLine(ActualInfo(Looper, 2))
                    Console.WriteLine(ActualInfo(Looper, 3))
                    Console.WriteLine("Found in: " & JustResultsScraper(ActualInfo(Looper, 0), 1, ActualSearchWord))
                    Console.WriteLine()

                Next
            Catch
                Console.WriteLine("Couldn't generate kanji readings")
            End Try


            'End of Kanji information generation ________________________________________________________

            Dim KanjisLine As String = "【"
            For Kanji = 1 To ActualInfo.Length / 4
                If Kanji <> ActualInfo.Length / 4 Then
                    KanjisLine &= (ActualInfo(Kanji - 1, 0) & "(" & ActualInfo(Kanji - 1, 1) & ") ")
                Else
                    KanjisLine &= (ActualInfo(Kanji - 1, 0) & "(" & ActualInfo(Kanji - 1, 1) & ")】")
                End If
            Next


            'last requests ------------------------------------------------------------------------------------------------------------------------------------------------------------
            Dim LastRequest As String = ""
            If Anki = False Then
                LastRequest = Console.ReadLine().ToLower()
            End If

            Dim Types As String = FoundTypes(0)
            If LastRequest.ToLower = "kanji" Or LastRequest.ToLower = "copy kanji" Then
                My.Computer.Clipboard.SetText(KanjisLine)
                Console.Clear()
                Console.WriteLine("Copied " & QUOTE & KanjisLine & QUOTE & " to clipboard")
                Console.ReadLine()


            ElseIf LastRequest.ToLower = "anki" Or LastRequest.ToLower = "copy anki" Or Anki = True Then
                If FoundTypes.IndexOf("!") = FoundTypes.Length Then
                    FoundTypes = Left(FoundTypes, FoundTypes.Length - 1)
                End If

                Dim AnkiString() As String = FoundTypes.Split("|") 'How holds the types
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

                    If NumberCheckT = NumberCheckD Then
                        AnkiCopy = AnkiCopy & vbCrLf
                    End If
                    MatchT = False
                    Type = 0
                    Do Until Type = AnkiString.Length Or MatchT = True
                        MatchT = False
                        If AnkiString(Type) = "!" Then
                            Type += 1
                            Continue Do
                        End If

                        NumberCheckT = Right(AnkiString(Type), 2)
                        If IsNumeric(NumberCheckT) = False Then
                            NumberCheckT = Right(AnkiString(Type), 1)
                        End If
                        NumberCheckD = NumberCheckD.Replace(" ", "")
                        NumberCheckD = NumberCheckD.Replace(".", "")

                        If NumberCheckT = NumberCheckD Then

                            If Definition <> 0 Then
                                AnkiCopy = vbCrLf & AnkiCopy
                            End If

                            If Definition < 10 Then
                                AnkiCopy = AnkiCopy & (Left(AnkiString(Type), AnkiString(Type).Length - NumberCheckT.Length)) & vbCrLf
                            ElseIf Definition > 9 And AnkiString(Type).IndexOf("aux") <> -1 Or Definition > 9 And AnkiString(Type).IndexOf("irr") Then
                                AnkiCopy = AnkiCopy & vbCrLf & (Left(AnkiString(Type), AnkiString(Type).Length - NumberCheckT.Length))

                                If Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[") = -1 Then
                                    AnkiCopy = AnkiCopy & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length)
                                Else
                                    SB1 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[")
                                    SB2 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("]")
                                    BArea = Mid(Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), SB1 + 1, SB2 + 1 - SB1)

                                    If BArea.IndexOf("kana") = -1 Then

                                        AnkiCopy = AnkiCopy & vbCrLf

                                        AnkiCopy = AnkiCopy & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "") & BArea
                                    Else
                                        AnkiCopy = AnkiCopy & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "")
                                    End If

                                End If
                            End If
                            MatchT = True
                            AnkiString(Type) = "!"
                            Continue Do
                        End If
                        Type += 1
                    Loop

                    If Definition < 10 Then
                        'AnkiCopy = AnkiCopy & (Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length)) & vbCrLf

                        If Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[") = -1 Then
                            AnkiCopy = AnkiCopy.TrimEnd
                            AnkiCopy = AnkiCopy & vbCrLf & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length)
                        Else
                            SB1 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("[")
                            SB2 = Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).IndexOf("]")
                            BArea = Mid(Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length), SB1 + 1, SB2 + 1 - SB1)

                            If BArea.IndexOf("kana") = -1 Then
                                If Definition <> 0 Then
                                    AnkiCopy = AnkiCopy & vbCrLf
                                End If
                                AnkiCopy = AnkiCopy & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "") & BArea
                            Else
                                AnkiCopy = AnkiCopy & vbCrLf & Definition + 1 & ". " & Left(SelectedDefinition(Definition), SelectedDefinition(Definition).Length - NumberCheckD.Length).Replace(BArea, "")
                            End If
                        End If
                    End If


                    Definition += 1
                Loop

                AnkiCopy = AnkiCopy.Trim

                AnkiCopy = AnkiCopy & vbCrLf & KanjisLine

                My.Computer.Clipboard.SetText(AnkiCopy)

                Console.Clear()
                Console.WriteLine("Copied " & QUOTE & AnkiCopy & QUOTE & " to clipboard")
                Console.ReadLine()
            End If

        Else
            Console.ReadLine()
        End If

        Main()
    End Sub
    Sub ConjugateVerb(ByRef PlainVerb, ByRef Type, ByRef Meaning, ByRef ComparativeType, ByVal S)
        Const QUOTE = """"
        Dim Last As String = Right(PlainVerb, 1) 'This is the last Japanese character of the verb, this is used for changing forms

        Dim LastAdd As String = ""
        Dim LastAdd2 As String = ""
        Dim LastAddPot As String = ""
        Dim masuStem As String = ""
        Dim NegativeStem As String = ""
        Dim Potential As String = ""
        Dim Causative As String = ""
        Dim Conditional As String = ""
        Dim teStem As String
        Dim Volitional As String

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
            Conditional = Left(Potential, Potential.Length - 1) & "ば"
        Else
            NegativeStem = Left(PlainVerb, PlainVerb.length - 1)
            Potential = Left(PlainVerb, PlainVerb.length - 1) & "られる"
            Causative = Left(PlainVerb, PlainVerb.length - 1) & "させる"
            Conditional = Left(PlainVerb, PlainVerb.length - 1) & "れば"
        End If

        'Creating te-form stem of searched word
        If Type = "Godan" Then
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
            teStem = Left(PlainVerb, PlainVerb.length - 1) & LastAdd
        Else
            teStem = Left(PlainVerb, PlainVerb.length - 1) & "て"
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
            Console.WriteLine("Error: Short past tense")
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
            Console.WriteLine("I intend to " & PlainMeaning & ": " & PlainVerb & "つもりです")
            Console.WriteLine("May " & PlainMeaning & ": " & PlainVerb & "かもしれない")
            Console.WriteLine("Why don't you " & PlainMeaning & " (informal): " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "らどうですか")
            Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " to prepare for something:" & teStem & "おく")
            Console.WriteLine("Must/should " & PlainMeaning & " to prepare for something: " & teStem & "おかなくちゃいけません")

        ElseIf S = 3 Then
            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.WriteLine("Te-forms:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("Te-stem: " & teStem)
            Console.WriteLine("Is " & PresentMeaning & ": " & teStem & "いる")
            Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "なくて")
            Console.WriteLine("Negative te-form:")
            Console.WriteLine("Don't " & PlainMeaning & ": " & NegativeStem & "ないで")
            Console.WriteLine()
            Console.WriteLine(Left(PastMeaning, 1).ToUpper & Right(PastMeaning, PastMeaning.Length - 1) & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding)

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
            Console.WriteLine("Auxiliaries:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine("To try " & PresentMeaning & ": " & teStem & "みる")
            Console.WriteLine("Want to try " & PresentMeaning & ": " & teStem & "みたい")
            Console.WriteLine("Want to be able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "たい")
            Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " too much: " & masuStem & "すぎる")
            Console.WriteLine("I " & PlainMeaning & " too much: " & masuStem & "すぎます")
            Console.WriteLine("Too much " & PresentMeaning & ": " & masuStem & "すぎること")
            Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " to prepare for something: " & teStem & "おく")
            Console.WriteLine("Must/should " & PlainMeaning & " to prepare for something: " & teStem & "おかなくちゃいけません")
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
            Console.WriteLine("Not made to/allowed to " & PlainMeaning & ": " & Left(Causative, Causative.Length - 1) & "ない")
            Console.WriteLine("Causative with te-form: " & Left(Causative, Causative.Length - 1) & "て")

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
                Console.WriteLine("You haven't got a custom S parameter set up.")
                Console.WriteLine("To do this, use the '/pref' command.")
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
            'Conditional:6
            'Want:7
            'Need to:8
            'Auxilaries:9
            'Noun/adj:10
            'Kanji details:11

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
                Console.WriteLine("masu-stem: " & masuStem)
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
                Console.WriteLine()
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
            ElseIf PreferencesString(5) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Causative (Being made to do something or letting it be done):")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Made to/allowed to " & PlainMeaning & ": " & Causative)
                Console.WriteLine("Not made to/allowed to " & PlainMeaning & ": " & Left(Causative, Causative.Length - 1) & "ない")
            ElseIf PreferencesString(5) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Causative (Being made to do something or letting it be done):")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Made to/allowed to " & PlainMeaning & ": " & Causative)
            End If

            If PreferencesString(6) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Conditional:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("If " & PlainMeaning & ": " & Conditional)
                Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なければ")
                Console.WriteLine()
                Console.WriteLine("If " & PlainMeaning & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "ら")
                Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なかったら")
            ElseIf PreferencesString(6) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Conditional:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("If " & PlainMeaning & ": " & Conditional)
                Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なければ")
                Console.WriteLine()
                Console.WriteLine("If " & PlainMeaning & ": " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "ら")
            ElseIf PreferencesString(6) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Conditional:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("If " & PlainMeaning & ": " & Conditional)
                Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なければ")
            End If

            If PreferencesString(7) = 3 Then
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
            ElseIf PreferencesString(7) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Want:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Want to " & PlainMeaning & ": " & masuStem & "たい")
                Console.WriteLine("Don't want to " & PlainMeaning & ": " & masuStem & "たくない")
                Console.WriteLine("Want to try " & PresentMeaning & ": " & teStem & "みたい")
                Console.WriteLine("Want to be able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "たい")
            ElseIf PreferencesString(7) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Want:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Want to try " & PresentMeaning & ": " & teStem & "みたい")
                Console.WriteLine("Want to be able to " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "たい")
            End If

            If PreferencesString(8) = 3 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Do and don't need to:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Need to/should " & PlainMeaning & "　(very informal): " & NegativeStem & "なきゃ")
                Console.WriteLine("Need to/should " & PlainMeaning & ": " & NegativeStem & "なくちゃいけない")
                Console.WriteLine("Need to/should " & PlainMeaning & ": " & NegativeStem & "なければいけません")
                Console.WriteLine("Don't need to " & PlainMeaning & ": " & NegativeStem & "なくてもいい")
            ElseIf PreferencesString(8) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Do and don't need to:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Need to/should " & PlainMeaning & "　(very informal): " & NegativeStem & "なきゃ")
                Console.WriteLine("Need to/should " & PlainMeaning & ": " & NegativeStem & "なくちゃいけない")
                Console.WriteLine("Don't need to " & PlainMeaning & ": " & NegativeStem & "なくてもいい")
            ElseIf PreferencesString(8) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Do and don't need to:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("Need to/should " & PlainMeaning & ": " & NegativeStem & "なくちゃいけない")
                Console.WriteLine("Don't need to " & PlainMeaning & ": " & NegativeStem & "なくてもいい")
            End If

            If PreferencesString(9) = 3 Then
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
            ElseIf PreferencesString(9) = 2 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Extras:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " too much: " & masuStem & "すぎる")
                Console.WriteLine("Too much " & PresentMeaning & ": " & masuStem & "すぎること")
                Console.WriteLine("May " & PlainMeaning & ": " & PlainVerb & "かもしれない")
                Console.WriteLine("Why don't you " & PlainMeaning & " (informal): " & Left(teStem, teStem.Length - 1) & ShortPastEnding & "らどうですか")
                Console.WriteLine("Must/should " & PlainMeaning & " to prepare for something: " & teStem & "おかなくちゃいけません")
            ElseIf PreferencesString(9) = 1 Then
                Console.WriteLine()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Extras:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine("May " & PlainMeaning & ": " & PlainVerb & "かもしれない")
                Console.WriteLine("Must/should " & PlainMeaning & " to prepare for something: " & teStem & "おかなくちゃいけません")
            End If
            Console.WriteLine()
        End If

        Dim Client As New WebClient
        Client.Encoding = System.Text.Encoding.UTF8
        Dim WordURL As String = ("https://jisho.org/search/" & PlainVerb & "%20%23sentences")
        Dim HTML As String = Client.DownloadString(New Uri(WordURL))

        If S < 3 Then
            Dim Example As String = ""
            Dim SentenceExample As String
            SentenceExample = RetrieveClassRange(HTML, "<li class=" & QUOTE & "clearfix" & QUOTE & ">", "inline_copyright", "Sentence Example") 'Firstly extracting the whole group
            If SentenceExample.Length > 10 Then
                Example = ExampleSentence(SentenceExample) 'This group then needs all the "fillers" taken out, that's what the ExampleSentence function does
            End If
            If Example.Length < 100 And Example.Length > 5 Then
                Console.WriteLine()
                Console.WriteLine(Example)
            End If
        End If

        Dim KanjiBool As Boolean = False
        If S = 0 Then
            If PreferencesString(11) <> 0 Then
                KanjiBool = True
            End If
        End If

        If S <> 0 Or KanjiBool = True Then
            Console.WriteLine()
            Console.BackgroundColor = ConsoleColor.White
            Console.ForegroundColor = ConsoleColor.Black
            Console.WriteLine("Kanji:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White

            Dim WordHTML As String = ""
            Try
                'Kanji, meanings and reading extract. First open the "/word" page and then extracts instead of extracting from "/search":
                Dim WordWordURL As String = ("https://jisho.org/word/" & PlainVerb)
                WordHTML = Client.DownloadString(New Uri(WordWordURL))
            Catch
                Console.ReadLine()
                Main()
            End Try
            Dim KanjiInfo As String = RetrieveClassRange(WordHTML, "<span class=" & QUOTE & "character literal japanese_gothic", "</aside>", "KanjiInfo")

            Dim KanjiGroupEnd As Integer 'This is going to detect "Details" (the end of a group of kanji info for one kanji)
            Dim KanjiGroup(0) As String 'This will store each Kanji group in an array
            Dim I As Integer = -1 'This will store the index of which kanji group the loop is on, indexing starts at 0, thus " = 0"
            Dim LastDetailsIndex As Integer = KanjiInfo.LastIndexOf("Details")
            KanjiInfo = Left(KanjiInfo, LastDetailsIndex)
            Try
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
                Console.ReadLine()
                Main()
            End Try

            Dim ActualInfo(KanjiGroup.Length - 1, 3) 'X = Kanji (group), Y = Info type.
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
            Try
                KanjiGroup(KanjiGroup.Length - 1) = Mid(KanjiGroup(KanjiGroup.Length - 1), 5) 'This lets the last group work

                For Looper = 0 To KanjiGroup.Length - 1
                    FirstFinder = KanjiGroup(Looper).IndexOf("</a>")
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

                    ActualInfo(Looper, 2) &= "Kun Readings: "
                    ActualInfo(Looper, 3) &= "On Readings: "

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
                    Console.BackgroundColor = ConsoleColor.DarkGray
                    Console.WriteLine(ActualInfo(Looper, 0) & " - " & ActualInfo(Looper, 1))
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.WriteLine(ActualInfo(Looper, 2))
                    Console.WriteLine(ActualInfo(Looper, 3))
                    Console.WriteLine("Found in :" & JustResultsScraper(ActualInfo(Looper, 0), 1, PlainVerb))
                    Console.WriteLine()
                Next
            Catch
                Console.WriteLine("Couldn't generate kanji readings")
                Console.ReadLine()
                Main()
            End Try

            Dim KanjisLine As String = "【"
            For Kanji = 1 To ActualInfo.Length / 4
                If Kanji <> ActualInfo.Length / 4 Then
                    KanjisLine &= (ActualInfo(Kanji - 1, 0) & "(" & ActualInfo(Kanji - 1, 1) & ") ")
                Else
                    KanjisLine &= (ActualInfo(Kanji - 1, 0) & "(" & ActualInfo(Kanji - 1, 1) & ")】")
                End If
            Next

            Dim LastRequest As String = Console.ReadLine().ToLower

            If LastRequest.ToLower = "kanji" Or LastRequest.ToLower = "copy kanji" Then
                My.Computer.Clipboard.SetText(KanjisLine)
                Console.Clear()
                Console.WriteLine("Copied " & QUOTE & KanjisLine & QUOTE & " to clipboard")
                Console.ReadLine()
                Main()
            ElseIf LastRequest.ToLower = "anki" Then

                'This code used to exist here:
                'Try
                'Meaning = Left(Meaning, Meaning.indexof("["))
                'Catch
                'End Try

                WordConjugate(PlainVerb & " " & Meaning, 1, True)
            End If

        Else
            Console.ReadLine()
        End If

        'Console.ReadLine()
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

        ActualSearchWord = RetrieveClassRange(HTMLTemp, "<span class=" & QUOTE & "text" & QUOTE & ">", "</div>", "Actual word search")
        ActualSearchWord = Mid(ActualSearchWord, 30)
        ActualSearchWord = ActualSearchWord.Replace("<span>", "")
        ActualSearchWord = ActualSearchWord.Replace("</span>", "")


        'Getting the link of the actual word:
        ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & QUOTE & "text" & QUOTE & ">")
        HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
        ActualSearch1stAppearance = HTMLTemp.IndexOf("meanings-wrapper") 'used to be "concept_light clearfix"
        HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
        ActualSearch1stAppearance = HTMLTemp.IndexOf("jisho.org/word/")
        ActualSearch2ndAppearance = Mid(HTMLTemp, HTMLTemp.IndexOf("jisho.org/word/")).IndexOf(QUOTE & ">")
        WordLink = Mid(HTMLTemp, ActualSearch1stAppearance + 1, ActualSearch2ndAppearance - 1)

        ActualSearchWord = RetrieveClassRange(HTML, "<div class=" & QUOTE & "concept_light clearfix" & QUOTE & ">", "</div>", "Actual word search")
        ActualSearchWord = RetrieveClassRange(ActualSearchWord, "text", "</span", "Actual word search")
        ActualSearchWord = Mid(ActualSearchWord, 17)
        ActualSearchWord = ActualSearchWord.Replace("<span>", "")
        ActualSearchWord = ActualSearchWord.Replace("</span>", "")

        If ActualSearchWord.Length = 0 Then
            Console.WriteLine("Word wasn't found.")
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

        Console.Clear()
        Console.WriteLine("Conjugation practice for " & ActualSearchWord & "(" & Furigana & "): " & Definition)
        Console.WriteLine(TypeSnip)
        Console.WriteLine()
        Console.WriteLine("Press enter to start.")
        Console.ReadLine()
        Console.Clear()

        Dim Last As String = Right(ActualSearchWord, 1) 'This is the last Japanese character of the verb, this is used for changing forms
        Dim LastAdd As String = ""
        Dim Type As String = "" 'This is for making sure the verb is conjugated properly
        Dim Forms(9, 3) As String 'This is an array that will hold various forms with the following order:

        Dim Verb, IAdj, NaAdj As Boolean
        Verb = False
        IAdj = False
        NaAdj = False
        If TypeSnip.IndexOf("verb") <> -1 And (TypeSnip.ToLower).IndexOf("suru") = -1 Then 'If a verb
            Verb = True
            If TypeSnip.IndexOf("Godan verb") <> -1 Then
                Type = "Godan"
            ElseIf TypeSnip.IndexOf("Ichidan verb") <> -1 Then
                Type = "Ichidan"
            Else
                Console.WriteLine("Error: Couldn't identify verb")
                Console.ReadLine()
                Main()
            End If
        End If
        If TypeSnip.IndexOf("I-adjective") <> -1 Then
            IAdj = True
        End If
        If TypeSnip.IndexOf("Na-adjective") <> -1 Or TypeSnip.IndexOf("Noun") Then
            NaAdj = True
        End If

        If Verb = True Then
            'Forms(X, Y) x = form, y = type of info
            'y1 = correct conjugation
            'y2 = question
            'y3 = furigana conjugation


            '0 = masu form (~ます)
            '1 = te-form (~て)
            '2 = short past (~った/っだ)
            '3 = past te-iru form (~ていた)
            '4 = tai/want form (~たい)
            '5 = short negative (~ない)
            '6 = short past negative (~なかった)
            '7 = te-form of negative (~なくて)
            '8 = negative te-form (~ないで)
            '9 = negative tai/want form (~たくない)
            Forms(0, 2) = "masu form"
            Forms(1, 2) = "te-form"
            Forms(2, 2) = "short past"
            Forms(3, 2) = "past te-iru form"
            Forms(4, 2) = "tai/want form"
            Forms(5, 2) = "short negative"
            Forms(6, 2) = "short past negative"
            Forms(7, 2) = "te-form of negative"
            Forms(8, 2) = "negative te-form"
            Forms(9, 2) = "negative tai/want form"
            Console.WriteLine("Type :" & Type & "|")
            If Type = "Godan" Then
                If Last = "む" Then
                    LastAdd = "み"
                End If
                If Last = "ぶ" Then
                    LastAdd = "び"
                End If
                If Last = "ぬ" Then
                    LastAdd = "に"
                End If
                If Last = "す" Then
                    LastAdd = "し"
                End If
                If Last = "ぐ" Then
                    LastAdd = "ぎ"
                End If
                If Last = "く" Then
                    LastAdd = "き"
                End If
                If Last = "る" Then
                    LastAdd = "り"
                End If
                If Last = "つ" Then
                    LastAdd = "ち"
                End If
                If Last = "う" Then
                    LastAdd = "い"
                End If
                Forms(0, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "ます" 'masu form
                Forms(4, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "たい" 'tai/want form
                Forms(9, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "たくない"

                Forms(0, 3) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "ます" 'furigana masu form
                Forms(4, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd & "たい" 'furigana tai/want form
                Forms(4, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd & "たくない" 'negative furigana tai/want form
            Else
                Forms(0, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "ます" 'masu form
                Forms(4, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "たい" 'tai/want form
                Forms(4, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "たくない" 'negative furigana tai/want form

                Forms(0, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd & "ます" 'furigana masu form
                Forms(4, 3) = Left(Furigana, Furigana.Length - 1) & "たい" 'furigana tai/want form
                Forms(4, 3) = Left(Furigana, Furigana.Length - 1) & "たくない" 'furigana negative tai/want form
            End If

            'Creating te-form stems
            If Type = "Godan" Then
                If Last = "む" Or Last = "ぶ" Or Last = "ぬ" Then
                    LastAdd = "んで"
                    Forms(2, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "んだ" 'ta-form
                    Forms(2, 3) = Left(Furigana, Furigana.Length - 1) & "んだ" 'furigana ta-form
                End If
                If Last = "す" Then
                    LastAdd = "して"
                    Forms(2, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "した" 'ta-form
                    Forms(2, 3) = Left(Furigana, Furigana.Length - 1) & "した" 'furigana ta-form
                End If
                If Last = "ぐ" Then
                    LastAdd = "いで"
                    Forms(2, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "いだ"
                    Forms(2, 3) = Left(Furigana, Furigana.Length - 1) & "いだ"
                End If
                If Last = "く" Then
                    LastAdd = "いて"
                    Forms(2, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "いた"
                    Forms(2, 3) = Left(Furigana, Furigana.Length - 1) & "いた"
                End If
                If Last = "る" Or Last = "つ" Or Last = "う" Then
                    LastAdd = "って"
                    Forms(2, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "った"
                    Forms(2, 3) = Left(Furigana, Furigana.Length - 1) & "っだ"
                End If
                Forms(1, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd 'te-form
                Forms(1, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd 'furigana te-form

                Forms(3, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "いた" 'past te-iru form
                Forms(3, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd & "いた" 'past te-iru form
            Else
                Forms(1, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "て" 'te-form
                Forms(1, 3) = Left(Furigana, Furigana.Length - 1) & "て" 'furigana te-form

                Forms(3, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "ていた" 'past te-iru form
                Forms(3, 3) = Left(Furigana, Furigana.Length - 1) & "ていた" 'furigana past te-iru form

                Forms(2, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "た" 'ta-form
                Forms(2, 3) = Left(Furigana, Furigana.Length - 1) & "た" 'furigana ta-form
            End If

            'Creating negative stems
            If Type = "Godan" Then
                If Last = "む" Then
                    LastAdd = "ま"
                End If
                If Last = "ぶ" Then
                    LastAdd = "ば"
                End If
                If Last = "ぬ" Then
                    LastAdd = "な"
                End If
                If Last = "す" Then
                    LastAdd = "さ"
                End If
                If Last = "ぐ" Then
                    LastAdd = "が"
                End If
                If Last = "く" Then
                    LastAdd = "か"
                End If
                If Last = "る" Then
                    LastAdd = "ら"
                End If
                If Last = "つ" Then
                    LastAdd = "た"
                End If
                If Last = "う" Then
                    LastAdd = "わ"
                End If
                Forms(5, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "ない"
                Forms(6, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "なかった"
                Forms(7, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "なくて"
                Forms(8, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & LastAdd & "ないで"

                Forms(5, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd & "ない"
                Forms(6, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd & "なかった"
                Forms(7, 3) = Left(Furigana, ActualSearchWord.Length - 1) & LastAdd & "なくて"
                Forms(8, 3) = Left(Furigana, Furigana.Length - 1) & LastAdd & "ないで"
            Else
                Forms(5, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "ない"
                Forms(6, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "なかった"
                Forms(7, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "なくて"
                Forms(8, 1) = Left(ActualSearchWord, ActualSearchWord.Length - 1) & "ないで"

                Forms(5, 3) = Left(Furigana, Furigana.Length - 1) & "ない"
                Forms(6, 3) = Left(Furigana, Furigana.Length - 1) & "なかった"
                Forms(7, 3) = Left(Furigana, Furigana.Length - 1) & "なくて"
                Forms(9, 3) = Left(Furigana, Furigana.Length - 1) & "ないで"
            End If
            'forms array:
            'Forms(X, Y) x = form, y = type of info
            'y1 = correct conjugation
            'y2 = question
            'y3 = furigana conjugation


            'For i = 0 To 8
            'Console.WriteLine(i & ": " & Forms(i, 2) & "; " & Forms(i, 1))
            'Next
        ElseIf IAdj = True Then
            ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 1)
            Furigana = Left(Furigana, Furigana.Length - 1)
            'Forms(X, Y) x = form, y = type of info
            'y1 = correct conjugation
            'y2 = question
            'y3 = furigana conjugation


            'This is an array that will hold various forms with the following order:
            '0 = it is (です)
            '1 = it is not (くありません)
            '2 = it was (かったです)
            '3 = it was not (くありませんでした)
            '4 = is ()
            '5 = isn't (くない)
            '6 = was (かった)
            '7 = wasn't (くなかった)
            '8 = te-form (くて)
            '9 = te-form of negative (くなくて)
            Forms(0, 2) = "it is"
            Forms(1, 2) = "it is not"
            Forms(2, 2) = "it was"
            Forms(3, 2) = "it was not"
            Forms(4, 2) = "is"
            Forms(5, 2) = "isn't"
            Forms(6, 2) = "was"
            Forms(7, 2) = "wasn't"
            Forms(8, 2) = "te-form"
            Forms(9, 2) = "te-form of negative"



            Forms(0, 1) = (ActualSearchWord & "いです") 'it is (です)
            Forms(0, 3) = (Furigana & "いです") 'furigana

            Forms(1, 1) = (ActualSearchWord & "くありません") 'it is not (くありません)
            Forms(1, 3) = (Furigana & "くありません") 'furigana

            Forms(2, 1) = (ActualSearchWord & "かったです") 'it was (かったです)
            Forms(2, 3) = (Furigana & "かったです") 'furigana

            Forms(3, 1) = (ActualSearchWord & "くありませんでした") 'it was not (くありませんでした)
            Forms(3, 3) = (Furigana & "くありませんでした")

            Forms(4, 1) = (ActualSearchWord & "い") 'is ()
            Forms(4, 3) = (Furigana & "い") 'furigana

            Forms(5, 1) = (ActualSearchWord & "くない") 'isn't (くない)
            Forms(5, 3) = (Furigana & "くない") 'furigana

            Forms(6, 1) = (ActualSearchWord & "かった") 'was (かった)
            Forms(6, 3) = (Furigana & "かった") 'furigana

            Forms(7, 1) = (ActualSearchWord & "くなかった") 'wasn't (くなかった)
            Forms(7, 3) = (Furigana & "くなかった") 'furigana

            Forms(8, 1) = (ActualSearchWord & "くて") 'te-form (くて)
            Forms(8, 3) = (Furigana & "くて") 'furigana

            Forms(9, 1) = (ActualSearchWord & "くなくて") 'te-form of negative (くなくて)
            Forms(9, 3) = (Furigana & "くなくて") 'furigana

        ElseIf NaAdj = True Then
            'Forms(X, Y) x = form, y = type of info
            'y1 = correct conjugation
            'y2 = question
            'y3 = furigana conjugation


            'This is an array that will hold various forms with the following order:
            '0 = it is (です)
            '1 = it is not (じゃありません)
            '2 = it was (でした)
            '3 = it was not (かじゃありませんでした)
            '4 = is (だ)
            '5 = isn't (じゃない)
            '6 = was (だった)
            '7 = wasn't (じゃなかった)
            '8 = te-form (で)
            '9 = noun modifier (な)
            Forms(0, 2) = "it is"
            Forms(1, 2) = "it is not"
            Forms(2, 2) = "it was"
            Forms(3, 2) = "it was not"
            Forms(4, 2) = "is"
            Forms(5, 2) = "isn't"
            Forms(6, 2) = "was"
            Forms(7, 2) = "wasn't"
            Forms(8, 2) = "te-form"
            Forms(9, 2) = "noun modifier"



            Forms(0, 1) = (ActualSearchWord & "です") 'it is (です)
            Forms(0, 3) = (Furigana & "です") 'furigana

            Forms(1, 1) = (ActualSearchWord & "じゃありません") 'it is not (くありません)
            Forms(1, 3) = (Furigana & "じゃありません") 'furigana

            Forms(2, 1) = (ActualSearchWord & "でした") 'it was (かったです)
            Forms(2, 3) = (Furigana & "でした") 'furigana

            Forms(3, 1) = (ActualSearchWord & "かじゃありませんでした") 'it was not (くありませんでした)
            Forms(3, 3) = (Furigana & "かじゃありませんでした")

            Forms(4, 1) = (ActualSearchWord & "だ") 'is ()
            Forms(4, 3) = (Furigana & "だ") 'furigana

            Forms(5, 1) = (ActualSearchWord & "じゃない") 'isn't (くない)
            Forms(5, 3) = (Furigana & "じゃない") 'furigana

            Forms(6, 1) = (ActualSearchWord & "だった") 'was (かった)
            Forms(6, 3) = (Furigana & "だった") 'furigana

            Forms(7, 1) = (ActualSearchWord & "じゃなかった") 'wasn't (くなかった)
            Forms(7, 3) = (Furigana & "じゃなかった") 'furigana

            Forms(8, 1) = (ActualSearchWord & "で") 'te-form (くて)
            Forms(8, 3) = (Furigana & "で") 'furigana

            Forms(9, 1) = (ActualSearchWord & "な") 'te-form of negative (くなくて)
            Forms(9, 3) = (Furigana & "な") 'furigana

        Else
            Console.Clear()
            Console.WriteLine("This word type isn't support, sorry!")
            Console.ReadLine()
            Main()
        End If




        Randomize()
        Dim Remaining As String = "0123456789"
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

            Do Until Remaining.IndexOf(Random) <> -1 'Keep generating a random number until one is generated that hasn't been generator before
                Random = Int((9 + 1) * Rnd())
                If Remaining.IndexOf(Random) = -1 Then
                    Random = Int((9 + 1) * Rnd())
                End If
            Loop

            Do Until Read = Forms(Random, 1) Or Read = Forms(Random, 3)
                If Score < 0 Then
                    Score = 0
                End If
                Console.WriteLine(ActualSearchWord & " - " & Completed & "/10")
                Console.WriteLine(Forms(Random, 2) & " (attempt " & Attempts + 1 & " - score:" & Score & "):")
                'Console.WriteLine("Cheat: " & Forms(Random, 3)) 'Use this to see the answer
                Read = Console.ReadLine
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

                    Score -= 20 * Attempts
                    If Attempts > 2 Then
                        Score -= 10
                        If Score < 0 Then
                            Score = 0
                        End If
                        Console.WriteLine(ActualSearchWord & "(" & Furigana & ") - " & Completed & "/10" & " (score:" & Score & "):")
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
                Score = 0
            End If
            Attempts = 0
        Loop

        Console.WriteLine("Finished!")
        Console.WriteLine("Score: " & Score)
        Console.ReadLine()
        Main()
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
        Dim Snip As String = Mid(HTML, SnipIndex + 10 + ClassToRetrieve.length, 50)
        SnipIndex = Snip.IndexOf("<")

        If SnipIndex = -1 Then
            SnipIndex = Snip.IndexOf(QUOTE)
            If SnipIndex = -1 Then
                SnipIndex = Snip.IndexOf(",")
                If SnipIndex = -1 Then
                    Console.WriteLine("Error: RetrieveClass; " & ErrorMessage & "; SnipIndex could not find")
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
        Dim SnipStart As Integer
        Dim SnipEnd As Integer
        Dim Snip As String = "NOTHING"
        Dim HTML As String = "NOTHING"
        Dim FirstFail As Boolean = False
        Dim ExtraIndex As Integer
        Dim FinishedAll As Boolean = False
        Dim FoundMeanings(0) As String
        Dim FoundTypes(0) As String
        'Try
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

        Snip = Mid(HTML, SnipStart, SnipEnd - SnipStart)



        'Catch
        '   Try
        '  Console.Clear()
        ' Console.WriteLine("Oh noes, something bad happened :O")
        'Console.WriteLine()
        'Console.WriteLine("Error: DefinitionScraper")
        'Console.WriteLine("{Snip Beginning}: |meaning-meaning|")
        'Console.WriteLine("{Snip Ending}: |</span><span>&#|")
        'Console.WriteLine("Catch: 1")
        'Console.WriteLine("URL: " & URL)
        'Console.WriteLine("Read: |" & Read & "|")
        'Console.WriteLine("SnipStart: " & SnipStart)
        'Console.WriteLine("[SnipStart - 18]: " & SnipStart - 18)
        'Console.WriteLine("SnipEnd: " & SnipEnd)
        'Console.WriteLine("[HTML.length]: " & HTML.Length)
        'Console.WriteLine("Snip: " & Snip)
        ''Console.WriteLine("First fail: " & FirstFail)
        'Console.WriteLine("FoundMeanings.Length:" & FoundMeanings.Length)
        'Console.ReadLine()
        'Main()
        'Catch
        '   Console.WriteLine()
        '  Console.WriteLine("Error: DefinitionScraper; FirstError.NotCaught")
        ' Console.WriteLine("Catch: 2")
        'Console.WriteLine("Read: " & Read)
        'Console.WriteLine("URL: " & URL)
        'Console.WriteLine("This was caused by another error")
        'Console.ReadLine()
        'Main()
        'End Try

        'End Try


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
            Console.WriteLine("Error: Definition; Snip")
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
        Catch
            Return ("")
        End Try
        Dim JPSentEnd As Integer = SentenceExample.IndexOf("</ul>")
        Dim JapaneseSentence As String = ""
        Try
            JapaneseSentence = Mid(SentenceExample, 2, JPSentEnd - 1)
        Catch
            Console.WriteLine("Error: ExampleSentence; JapaneseSentence")
            Console.WriteLine("Catch: 1")
            Console.WriteLine("JPSentEnd: " & JPSentEnd)
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

            Try
                If Left(HTML, SnipStart).IndexOf("sense-tag") <> -1 Then
                    'Snipping up to the sense tag:
                    SnipStart = Left(HTML, SnipStart).IndexOf("sense-tag")
                    HTML = Mid(HTML, SnipStart)

                    SnipStart = HTML.IndexOf(">") + 2
                    HTML = Mid(HTML, SnipStart)

                    SnipEnd = HTML.IndexOf("</")
                    Snip2 = Left(HTML, SnipEnd)
                    HTML = Mid(HTML, SnipStart)

                    SnipStart = HTML.IndexOf("meaning-definition-section_divider") '*again*, this will be used to get the Extra Details for the current word up to (but not including) the next

                    If Left(HTML, SnipStart).IndexOf("sense-tag") <> -1 Then
                        SnipStart = HTML.IndexOf("sense-tag")
                        HTML = Mid(HTML, SnipStart)

                        SnipStart = HTML.IndexOf(">") + 2
                        HTML = Mid(HTML, SnipStart)

                        SnipEnd = HTML.IndexOf("</")
                        Snip2 = Snip2 & ", " & Left(HTML, SnipEnd)

                        If Snip2.IndexOf("href") <> -1 Then
                            SnipStart = Snip2.IndexOf("<a href=")
                            SnipEnd = Snip2.IndexOf(">")
                            Snip2 = Snip2.Replace(Mid(Snip2, SnipStart, SnipEnd + 2 - SnipStart), "")
                        End If
                    End If

                    If Snip2.IndexOf("href") <> -1 Then
                        SnipStart = Snip2.IndexOf("<a href=")
                        SnipEnd = Snip2.IndexOf(">")
                        Snip2 = Snip2.Replace(Mid(Snip2, SnipStart, SnipEnd + 2 - SnipStart), "")
                    End If

                    Snip2 = "[" & Snip2 & "]"

                End If

            Catch
            End Try

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
            Console.WriteLine("1 = Custom S Parameter Settings")
            Console.WriteLine("2 = General Settings")
            Console.WriteLine("3 = Reset ALL settings")

            Choice = Console.ReadLine.ToLower

            If Choice = "0" Or Choice = "main" Or Choice.IndexOf("b") <> -1 Then
                Main()
            End If

            If IsNumeric(Choice) = True Then
                If Choice < 0 Or Choice > 3 Then
                    Choice = ""
                End If
            End If
            Console.Clear()
        Loop

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
            TextWriter.WriteLine("Conditional:1")

            TextWriter.WriteLine("Want:1")

            TextWriter.WriteLine("Need to:1")

            TextWriter.WriteLine("Extras:1")

            TextWriter.WriteLine("Noun/adj conjugations:1")

            TextWriter.WriteLine("Kanji details:1")

            TextWriter.Close()
        End Try

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
                    TextString(I) = TextString(I).TrimStart
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

                'Setting your own results for the custom S=0 parameter:
                Dim Line As Integer = 0
                Dim InformationType As String = ""
                Dim AmountChoice As String = ""
                Dim CheckedAmount As String = ""
                Do Until Line = TextString.Length
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
                    Do Until IsNumeric(AmountChoice) = True
                        Console.Clear()
                        Console.WriteLine("How much information would you like for '" & InformationType & "' (" & Line + 1 & "/" & TextString.Length - 1 & ")?")
                        Console.WriteLine("Type a number in the range 0-2. (0 being nothing, 2 being everything)")
                        Console.WriteLine()

                        For SType = 0 To TextString.Length - 1
                            If SType = Line Then
                                Console.ForegroundColor = ConsoleColor.DarkGray
                                Console.WriteLine(TextString(SType))
                                Console.ForegroundColor = ConsoleColor.White
                                Console.WriteLine()
                            ElseIf TextString(SType).IndexOf("|") <> -1 Or TextString(SType).IndexOf(":") = -1 Then
                            Else
                                Try
                                    Console.WriteLine(TextString(SType))
                                    Console.WriteLine()
                                Catch
                                    Console.WriteLine()
                                End Try
                            End If
                        Next

                        AmountChoice = Console.ReadLine
                        If IsNumeric(AmountChoice) = False Then
                            Continue Do
                        ElseIf AmountChoice < 0 Or AmountChoice > 3 Or AmountChoice.Length > 1 Then
                            AmountChoice = "a"
                        End If
                    Loop

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
                    TextUpdater.WriteLine(TextString(Writer))
                Next
                TextUpdater.Close()

                Console.Clear()
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.WriteLine("Now, using an S parameter of 0 will show these results you have just set.")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine()

                For SType = 0 To TextString.Length - 1
                    If SType = Line Then
                        Console.ForegroundColor = ConsoleColor.DarkGray
                        Console.WriteLine(TextString(SType))
                        Console.ForegroundColor = ConsoleColor.White
                        Console.WriteLine()
                    ElseIf TextString(SType).IndexOf("|") <> -1 Or TextString(SType).IndexOf(":") = -1 Then
                    Else
                        Try
                            Console.WriteLine(TextString(SType))
                            Console.WriteLine()
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
                TextWriter.WriteLine("Conditional:1")

                TextWriter.WriteLine("Want:1")

                TextWriter.WriteLine("Need to:1")

                TextWriter.WriteLine("Extras:1")

                TextWriter.WriteLine("Noun/adj conjugations:1")

                TextWriter.WriteLine("Kanji details:1")

                TextWriter.Close()

                Console.Clear()
                Console.WriteLine("Your preferences have been reset!")
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


        End If







        Main()
    End Sub

    Sub Download()
        'Just to test the downloading of files
        Console.Clear()

        Try
            My.Computer.Network.DownloadFile("http://d237wsm7x2l2ke.cloudfront.net/41e62e8fc52e4bee8d4060280a9b823a.mp4", "C:\ProgramData\Japanese Conjugation Helper\Media\Test.mp4")
        Catch
            My.Computer.FileSystem.DeleteFile("C:\ProgramData\Japanese Conjugation Helper\Media\Test.mp4")
            My.Computer.Network.DownloadFile("http://d237wsm7x2l2ke.cloudfront.net/41e62e8fc52e4bee8d4060280a9b823a.mp4", "C:\ProgramData\Japanese Conjugation Helper\Media\Test.mp4")
            Process.Start("C:\ProgramData\Japanese Conjugation Helper\Media\Test.mp4")
        End Try
    End Sub






    'TO DO:
    ''Bring up more definitions than just 1
    ''Make verb english conjugation better, for example change "to put on weight" -> is put on weight, -> is weight. or "to result in" -> "has result in"
    'Allow input of romanised characters for the answers conjugation practice, this should be done in a new function
    'A minor bug exists where something like "#1a23f2' is seen in some scraped data (not sure which exact data, I have a feeling that it is definitions
    'To show kun and on readings of kanji in a word and show the corresponding kanji

End Module
