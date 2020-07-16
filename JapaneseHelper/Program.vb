Imports System
Imports System.Net
Imports System.Text.RegularExpressions

Module Program
    Sub Main()
        Randomize()
        Const QUOTE = """"
        'For the input of Japanese Characters
        Console.InputEncoding = System.Text.Encoding.Unicode
        Console.OutputEncoding = System.Text.Encoding.Unicode
        Console.Clear()
        Console.WriteLine("Enter command, type " & QUOTE & "/h" & QUOTE & " for help")

        'This is getting the word that is being searched ready for more accurate search with ActualSearchWord, ActualSearch Word (should) always be in japanese while Word won't be if the user inputs english or romaji:
        Dim Word As String = Console.ReadLine 'This is the word that will be searched, this needs to be kept the same because it is the original search value that may be needed later

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
        'If Left(Word, 2) = "/p" Then
        'Console.WriteLine("Wrong format.")
        'Console.ReadLine()
        'Main()
        'End If

        If Word = "/h" Or Word = "/help" Then
            Console.Clear()
            Console.WriteLine("List of commands (note commands aren't case sensitive):")

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


            Console.ReadLine()
            Main()
        End If

        If Left(Word, 3) = "/h " Then
            Help(Right(Word, Word.Length - 3))
        End If
        If Left(Word, 6) = "/help " Then
            Help(Right(Word, Word.Length - 6))
        End If

        If (Word.ToLower).IndexOf("/shit") <> -1 Or (Word.ToLower).IndexOf("/poo") <> -1 Or (Word.ToLower).IndexOf("/crap") <> -1 Or (Word.ToLower).IndexOf("/stink") <> -1 Or (Word.ToLower).IndexOf("/smell") <> -1 Then
            Console.WriteLine("P")
            For shit = 1 To Int((20 + 1) * Rnd())
                Threading.Thread.Sleep(85)
                Console.WriteLine("o")
            Next
            Threading.Thread.Sleep(120)
            Console.WriteLine("p")
            Threading.Thread.Sleep(150)
            Console.WriteLine(".")
            Console.ReadLine()
            Main()
        End If

        If Word = "/omae wa mou shinderu" Or Word = "/omae wa mou shindeiru" Or Word = "/omae ha mou shinderu" Or Word = "/お前はもう死んでる" Or Word = "/お前はもう死んでいる" Then
            Console.WriteLine("                                       .-''''-..     ")
            Console.WriteLine("                                     .' .'''.   `.   ")
            Console.WriteLine("   _..._                _..._   .--./    \   \    `. ")
            Console.WriteLine(" .'     '.            .'     '. |__|\    '   |     | ")
            Console.WriteLine(".   .-.   .          .   .-.   ..--. `--'   /     /  ")
            Console.WriteLine("|  '   '  |    __    |  '   '  ||  |      .'  ,-''   ")
            Console.WriteLine("|  |   |  | .:--.'.  |  |   |  ||  |      |  /       ")
            Console.WriteLine("|  |   |  |/ |   \ | |  |   |  ||  |      | '        ")
            Console.WriteLine("|  |   |  |`" & QUOTE & "__  | | |  |   |  ||  |      '-'")
            Console.WriteLine("|  |   |  | .'.''| | |  |   |  ||__|     .--.        ")
            Console.WriteLine("|  |   |  |/ /   | |_|  |   |  |        /    \       ")
            Console.WriteLine("|  |   |  |\ \._,\ '/|  |   |  |        \    /       ")
            Console.WriteLine("'--'   '--' `--'  `' '--'   '--'         `--'        ")
            Threading.Thread.Sleep(1500)
            Main()
        End If

        If Left(Word, 1) = "/" Then
            Console.WriteLine("This is not a command")
            Console.ReadLine()
            Main()
        End If

        If Word.Length > 2 Then
            If Mid(Word, Word.Length - 2, 1) = " " And IsNumeric(Right(Word, 2)) = True Then
                Console.WriteLine("Searching for " & Right(Word, 2) & " definitions of '" & Left(Word, Word.Length - 3) & "'...")
                WordConjugate(Left(Word, Word.Length - 2), Right(Word, 2))
            ElseIf Mid(Word, Word.Length - 1, 1) = " " And IsNumeric(Right(Word, 1)) = True And Right(Word, 1) <> "1" Then
                Console.WriteLine("Searching for " & Right(Word, 1) & " definitions of '" & Left(Word, Word.Length - 2) & "'...")
                WordConjugate(Left(Word, Word.Length - 2), Right(Word, 1))
            ElseIf Mid(Word, Word.Length - 1, 1) = " " And IsNumeric(Right(Word, 1)) = True And Right(Word, 1) = "1" Then
                Console.WriteLine("Searching for '" & Word & "'...")
                WordConjugate(Left(Word, Word.Length - 2), Right(Word, 1))
            Else
                Console.WriteLine("Searching for '" & Word & "'...")
                WordConjugate(Word, 1)
            End If
        Else
            Console.WriteLine("Searching for " & Word & "...")
            WordConjugate(Word, 1)
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
        End If

        If Command = "/r" Then
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

            Console.WriteLine()
            Console.WriteLine()
            Console.WriteLine("Examples:")
            Console.WriteLine("|そこで私たちを待っている幸福が、私たちが望むような幸福ではないかもしれない。 ^It may be that the happiness awaiting us is not at all the sort of happiness we would want.")
            Console.WriteLine()
            Console.WriteLine("|家に帰りましょうか。 ^Why don't we go home?|「少しうちに寄っていかない？」「いいの？」「うち共働きで親は遅いの」 ^" & QUOTE & "Want To drop round my place?" & QUOTE & "can I?" & QUOTE & " My parents come home late As they both work." & QUOTE & "|先ずは憧れの作家の文章の呼吸をつかむためにひたすら筆写、丸写しをする。 ^First, in order to get a feel for your favourite author's work, transcribe and copy in full.")

        End If

        If Command = "/h" Or Command = "/help" Then
            Console.WriteLine("/h: Brings up this help menu, if you want more help with a command then add the command parameter:")
            Console.WriteLine("Syntax: /h [command]")
            Console.WriteLine("Examples:")
            Console.WriteLine("/h /r")
            Console.WriteLine("/help WordLookup")
        End If

        If Command = "/p" Then
            Console.WriteLine("/p: Start a small quiz that helps you conjugate verbs, this only works with verbs but will later work with adjectives and nouns, the (word) parameter is the word that you want help conjugating")
            Console.WriteLine("Japanese words can also be written using romaji (english transliteration), surround words in quotes to make sure the program knows that it is definitely the english word you are seaching for and not romaji. Example: " & QUOTE & "hate" & QUOTE)
            Console.WriteLine("Syntax: /p [english/japanese/romaji word]")
            Console.WriteLine()
            Console.WriteLine("When the quiz starts it will tell you which form to change the word to, failing lowers your score.")
        End If

        Console.WriteLine("There is no information for " & QUOTE & "/" & Command & QUOTE)
        Console.WriteLine(QUOTE & "/" & Command & QUOTE & " is not a command.")
        Console.Read()
        Main()
    End Sub

    Sub ConjugationPractice(ByVal Word)
        Console.Clear()
        Const QUOTE = """"
        Dim WordURL As String = ("https://jisho.org/search/" & Word)
        Dim Client As New WebClient
        Dim HTML As String = ""
        HTML = Client.DownloadString(New Uri(WordURL))


        Dim HTMLTemp As String = HTML

        Dim ActualSearchWord As String = ""
        Dim ActualSearch1stAppearance As Integer = 0
        Dim FoundWords(0) As String
        Dim FoundDefinitions(0) As String
        Dim Max As Integer = 3

        For LoopIndex = 0 To Max
            Array.Resize(FoundWords, FoundWords.Length + 1)
            Array.Resize(FoundDefinitions, FoundDefinitions.Length + 1)
            Try
                ActualSearchWord = RetrieveClassRange(HTMLTemp, "<span class=" & QUOTE & " text" & QUOTE & ">", "</div>", "Actual word search")
                ActualSearchWord = Mid(ActualSearchWord, 30)
                ActualSearchWord = ActualSearchWord.Replace("<span>", "")
                ActualSearchWord = ActualSearchWord.Replace("</span>", "")
                If ActualSearchWord.Length = 0 Then
                    Continue For
                End If
                If ActualSearchWord.Length > 0 Then
                    ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 8)
                End If
                FoundWords(LoopIndex) = ActualSearchWord
                FoundDefinitions(LoopIndex) = DefinitionScraper(ActualSearchWord)

                'Console.WriteLine("FoundWords(" & LoopIndex & "): " & ActualSearchWord)

                ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & QUOTE & " text" & QUOTE & ">")
                HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
                ActualSearch1stAppearance = HTMLTemp.IndexOf("concept_light clearfix")
                HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)


            Catch 'If there are no more search results then:
                LoopIndex = 9
                If ActualSearchWord = "" Then
                    Console.WriteLine("Word couldn't be found")
                    Console.ReadLine()
                    Main()
                End If
            End Try

        Next
        Array.Resize(FoundWords, FoundWords.Length - 1)
        Array.Resize(FoundDefinitions, FoundDefinitions.Length - 1)

        Dim WordChoice As Integer
        Dim IntTest As String = ""

        Dim FirstBlank As Boolean = True
        Do Until WordChoice <= Max And WordChoice >= 1
            If IsNumeric(IntTest) = True Then
                WordChoice = CInt(IntTest)
                If WordChoice <= Max And WordChoice >= 1 Then
                    Continue Do
                Else
                    WordChoice = Max + 10 'To restart the loop, "+" could be any number above 2
                    IntTest = ""
                End If
            Else
                Console.Clear()
                Console.WriteLine("Which definition would you like details for? Type a number, 0 to cancel.")
                Console.WriteLine()
                For looper = 1 To FoundWords.Length
                    If IsNothing(FoundWords(looper - 1)) = False Then
                        Console.WriteLine(looper & ": " & FoundWords(looper - 1) & " - " & FoundDefinitions(looper - 1))
                    Else
                        If FirstBlank = True Then
                            Max = looper - 1
                            FirstBlank = False
                        End If
                    End If
                Next

                IntTest = Console.ReadLine
                If IntTest = "0" Or IntTest.ToLower = "cancel" Or IntTest.ToLower = "stop" Or IntTest.ToLower = "main" Or IntTest.ToLower = "menu" Then
                    Main()
                End If
            End If
        Loop

        If WordChoice > Max Then
            WordChoice = Max
        End If


        ActualSearchWord = FoundWords(WordChoice - 1)
        ActualSearchWord = RetrieveClassRange(HTML, "<span class=" & QUOTE & " text" & QUOTE & ">", "</div>", "Actual word search")
        If ActualSearchWord.Length <2 Then
            Console.WriteLine("Word couldn't be found")
            Console.ReadLine()
            Main()
        End If
        ActualSearchWord = Mid(ActualSearchWord, 30)
        ActualSearchWord = ActualSearchWord.Replace("<span>", "")
        ActualSearchWord = ActualSearchWord.Replace("</span>", "")

        ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 8)



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

        Dim Definition As String = FoundDefinitions(WordChoice - 1)
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
                Console.WriteLine("Cheat: " & Forms(Random, 3))
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
                If Read <> Forms(Random, 1) Or Read <> Forms(Random, 3) Then

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
    Sub WordConjugate(ByRef Word As String, ByVal WordIndex As Integer)
        Const QUOTE = """"
        Dim SentenceExample As String = ""
        Dim Example As String = ""

        Dim WordURL As String = ("https://jisho.org/search/" & Word)
        Dim Client As New WebClient
        Dim HTML As String

        HTML = Client.DownloadString(New Uri(WordURL))
        Dim HTMLTemp As String = HTML

        If WordIndex < 1 Then
            WordIndex = 1
        End If

        If WordIndex > 20 Then
            WordIndex = 20
        End If

        Dim ActualSearchWord As String = ""
        Dim ActualSearch1stAppearance As Integer = 0
        Dim FoundWords(0) As String
        Dim FoundDefinitions(0) As String
        Dim ScrapFull As String
        Dim Max As Integer = WordIndex
        Dim WordChoice As Integer = 30
        If WordIndex <> 1 Then
            For LoopIndex = 0 To Max - 1
                Array.Resize(FoundWords, FoundWords.Length + 1)
                Array.Resize(FoundDefinitions, FoundDefinitions.Length + 1)
                Try
                    ActualSearchWord = RetrieveClassRange(HTMLTemp, "<span class=" & QUOTE & "text" & QUOTE & ">", "</div>", "Actual word search")
                    ActualSearchWord = Mid(ActualSearchWord, 30)
                    ActualSearchWord = ActualSearchWord.Replace("<span>", "")
                    ActualSearchWord = ActualSearchWord.Replace("</span>", "")
                    If ActualSearchWord.Length = 0 Then
                        Continue For
                    End If
                    If ActualSearchWord.Length > 0 Then
                        ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 8)
                    End If
                    FoundWords(LoopIndex) = ActualSearchWord

                    FoundDefinitions(LoopIndex) = DefinitionScraper(ActualSearchWord).replace("&#39;", "")

                    Console.WriteLine(LoopIndex + 1 & ": " & ActualSearchWord & " - " & FoundDefinitions(LoopIndex))

                    'Console.WriteLine("FoundWords(" & LoopIndex & "): " & ActualSearchWord)

                    ActualSearch1stAppearance = HTMLTemp.IndexOf("<span class=" & QUOTE & "text" & QUOTE & ">")
                    HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)
                    ActualSearch1stAppearance = HTMLTemp.IndexOf("concept_light clearfix")
                    HTMLTemp = Mid(HTMLTemp, ActualSearch1stAppearance + 1)


                Catch 'If there are no more search results then:
                    LoopIndex = WordIndex
                    If ActualSearchWord = "" Then
                        Console.WriteLine("Word couldn't be found")
                        Console.ReadLine()
                        Main()
                    End If
                End Try

            Next
            Array.Resize(FoundWords, FoundWords.Length - 1)
            Array.Resize(FoundDefinitions, FoundDefinitions.Length - 1)


            Dim IntTest As String = ""

            Dim FirstBlank As Boolean = True
            Do Until WordChoice <= WordIndex And WordChoice >= 1
                If IsNumeric(IntTest) = True Then
                    WordChoice = CInt(IntTest)
                    If WordChoice <= WordIndex And WordChoice >= 1 Then
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
                            Console.WriteLine(looper & ": " & FoundWords(looper - 1) & " - " & FoundDefinitions(looper - 1))
                            TotalWordsFound += 1
                        Else
                            If FirstBlank = True Then
                                Max = looper - 1
                                FirstBlank = False
                            End If
                        End If
                    Next
                    If TotalWordsFound = 0 Then
                        WriteLine("No words were found.")
                        Main()
                    ElseIf TotalWordsFound < WordIndex Then
                        Console.WriteLine("There wasn't " & WordIndex & " words but " & TotalWordsFound & " was found.")
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
        Else
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
        End If

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
        Console.WriteLine(FullWordType)

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

        If Furigana.Length < 1 Or IsNothing(Furigana) Then
            Furigana = RetrieveClassRange(HTML, "audio id=" & QUOTE & "audio_" & ActualSearchWord, "<source src=", "Furigana2")
            Furigana = Furigana.Replace("<", "")
            Furigana = Furigana.Replace("audio id=" & QUOTE & "audio_" & ActualSearchWord & ":", "")
            Furigana = Furigana.Replace(QUOTE & ">", "")
        End If
        ScrapFull = DefinitionScraper(ActualSearchWord) 'This is getting the definition of the word
        ScrapFull = Left(ScrapFull, 1).ToUpper & Right(ScrapFull, ScrapFull.Length - 1) 'This is capitalising the first letter and then having the rest as normal
        If WordIndex <> 1 Then
            Try
                Console.WriteLine(ActualSearchWord & "(" & Furigana & "): " & FoundDefinitions(WordChoice - 1)) 'Japanese word (furigana): Meaning
            Catch
                Console.WriteLine(ActualSearchWord & "(" & Furigana & "): " & FoundDefinitions(FoundDefinitions.Length - 1))
            End Try
        Else
            Console.WriteLine(ActualSearchWord & "(" & Furigana & "): " & ScrapFull) 'Japanese word (furigana): Meaning
        End If
        Console.WriteLine()

        Dim Scrap As String = ""
        FuriganaStart = ScrapFull.IndexOf(";") 'This is getting just one definition of the word, FuriganaStart isn't used for furigana here, but that is because I don't want to create a new variable when I don't have to
        Try
            If FuriganaStart <> -1 Then
                Scrap = Left(ScrapFull, FuriganaStart)
            Else
                Scrap = ScrapFull
            End If
        Catch
            Scrap = "____"
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
            Console.WriteLine("Is doing: しています")
            Console.WriteLine("Am doing: していいる")
            Console.WriteLine("Was doing: していました")
            Console.WriteLine("Was doing: していた")
            Console.WriteLine()
            Console.WriteLine("Want to do: したい")
        End If

        'Preparing some variables for the word being searched
        Dim Iadjective, NaAdjective, NoAdjective, Noun, Suru, Verb As Boolean 'Word Types Checker varibles
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
                    ConjugateVerb(ActualSearchWord, "Godan", Scrap, ComparativeType) '(Verb/Word, Very Type, Meaning, "ComparativeType")
                End If

                If FullWordType.IndexOf("Ichidan verb") <> -1 Then
                    ConjugateVerb(ActualSearchWord, "Ichidan", Scrap, ComparativeType) '(Verb/Word, Very Type, Meaning, "ComparativeType")
                End If
                Console.WriteLine("Something went wrong when identifying the verb")
                Console.Read()
                Main()
            End If

        Else 'If there is only one word type then the if statement can look at the whole variable "TypeSnip" instead of use .index to determine if a Word Type is part of the whole string
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
                    ConjugateVerb(ActualSearchWord, "Godan", Scrap, ComparativeType) '(Verb/Word, Very Type, Meaning, "ComparativeType")
                End If

                If FullWordType.IndexOf("Ichidan verb") <> -1 Then
                    ConjugateVerb(ActualSearchWord, "Ichidan", Scrap, ComparativeType) '(Verb/Word, Very Type, Meaning, "ComparativeType")
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


        If Iadjective = True Then
            ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 1)
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
        If NaAdjective = True Then
            'Checking if な exists at the end of the word so that it can be erased if it does exist
            If Right(ActualSearchWord, 1) = "な" Then
                ActualSearchWord = Left(ActualSearchWord, ActualSearchWord.Length - 1)
            End If

            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.Write("- na-adjective couplas -")
            Console.BackgroundColor = ConsoleColor.Black
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

            Console.WriteLine()

        End If
        If Noun = True Then

            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.Write("- noun couplas -")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine()
            Console.WriteLine("Polite:")
            Console.WriteLine("it is: " & ActualSearchWord & "です")
            Console.WriteLine("is not: " & ActualSearchWord & "じゃありません")
            Console.WriteLine("")
            Console.WriteLine("it was: " & ActualSearchWord & "でした")
            Console.WriteLine("was not:" & ActualSearchWord & "じゃありませんでした")

            Console.WriteLine("")
            Console.WriteLine("")

            Console.WriteLine("Informal:")
            Console.WriteLine("is: " & ActualSearchWord & "だ")
            Console.WriteLine("isn't: " & ActualSearchWord & "じゃない")
            Console.WriteLine("")
            Console.WriteLine("was: " & ActualSearchWord & "だった")
            Console.WriteLine("wasn't: " & ActualSearchWord & "じゃなかった")

            Console.WriteLine("")
            Console.WriteLine("")

            Console.BackgroundColor = ConsoleColor.DarkGray
            Console.Write("Noun usage:")
            Console.BackgroundColor = ConsoleColor.Black
            Console.WriteLine()

            'Changing example sentence depending on if it is a vowel or not
            If Vowel = True Then
                Console.WriteLine("子供の頃、私は" & ActualSearchWord & "を持っていた | When (I) was a kid, I had an " & Scrap & ".")
            Else
                Console.WriteLine("子供の頃、私は" & ActualSearchWord & "を持っていた | When (I) was a kid, I had a " & Scrap & ".")
            End If

            Console.WriteLine("彼は" & ActualSearchWord & "が好きだった | He liked " & Scrap & ".")

            If Vowel = True Then
                Console.WriteLine("これは" & ActualSearchWord & "じゃない | This is not an " & Scrap & ".")
            Else
                Console.WriteLine("これは" & ActualSearchWord & "じゃない | This is not a " & Scrap & ".")
            End If

            If NoAdjective = True Then
                'Important "titles" are highlighted to make reading the information easier
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.Write("No-adjective usage:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine()

                Console.WriteLine("これは" & ActualSearchWord & "の木です  | This is a tree of the " & Scrap & ".")
                Console.WriteLine("これらは私たちの" & ActualSearchWord & "の伝統です | These are our family " & Scrap & "s.")
                Console.WriteLine(ActualSearchWord & "の銀行に行きましょう！ | Let's go to the bank of " & Scrap & "!")
            End If

            If NaAdjective = True Then
                Console.BackgroundColor = ConsoleColor.DarkGray
                Console.Write("Na-adjective usage:")
                Console.BackgroundColor = ConsoleColor.Black
                Console.WriteLine()
                Console.WriteLine("彼女は青い椅子に座っている" & ActualSearchWord & "な人です | She is the " & Scrap & " person sitting on the blue chair.")
                Console.WriteLine(ActualSearchWord & "な車は私の家にあります | (my) " & Scrap & " car is at my house")
            End If

        End If

        'Adding the い back on to the end of the word (if it is an i-adjective)

        If Iadjective = True Then
            ActualSearchWord = ActualSearchWord & "い"
        End If




        'Proper Example Sentence extraction:
        WordURL = ("https://jisho.org/search/" & ActualSearchWord & "%20%23sentences")
        HTMLTemp = Client.DownloadString(New Uri(WordURL))

        SentenceExample = RetrieveClassRange(HTMLTemp, "<li class=" & QUOTE & "clearfix" & QUOTE & "><span class=" & QUOTE & "furigana" & QUOTE & ">", "inline_copyright", "Example Sentence") 'Firstly extracting the whole group
        If SentenceExample.Length > 10 Then
            Example = ExampleSentence(SentenceExample) 'This group then needs all the "fillers" taken out, that's what the ExampleSentence function does
        End If
        Console.WriteLine()
        Console.WriteLine(Example)

        Console.ReadLine()
        Main()

        Dim WordWordURL As String = ("https://jisho.org/word/" & ActualSearchWord)
        Dim WordHTML As String
        WordHTML = Client.DownloadString(New Uri(WordWordURL))


        'Kanji, meanings and reading extract. First open the "/word" page and then extracts instead of extracting from "/search":
        Dim KanjiInfo As String = RetrieveClassRange(WordHTML, "<span class=" & QUOTE & "character literal japanese_gothic", "</aside>", "KanjiInfo")


        Dim KanjiGroupEnd As Integer 'This is going to detect "Details" (the end of a group of kanji info for one kanji)
        Dim KanjiGroup(0) As String 'This will store each Kanji group in an array
        Dim I As Integer = -1 'This will store the index of which kanji group the loop is on, indexing starts at 0, thus " = 0"
        Dim LastDetailsIndex As Integer = KanjiInfo.LastIndexOf("Details")
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

            KanjiGroup(I) = Mid(KanjiInfo, 1, KanjiGroupEnd)
            KanjiInfo = Mid(KanjiInfo, KanjiGroupEnd)
        Loop
        Array.Resize(KanjiGroup, KanjiGroup.Length - 1)

        Dim ActualInfo(KanjiGroup.Length - 1, 3) 'X = Kanji (group), Y = Info type.
        'Y indexs:
        '0 = Kanji
        '1 = English meaning(s) (I will concatinate multiple meanings)
        '2 = kun readings (concatentated if needed, usually so)
        '3 = on readings (concatentated if needed, usually so)
        Dim FirstFinder As Integer
        Dim AllEng, AllKun, AllOn As Boolean
        AllEng = False
        AllKun = False
        AllOn = False

        For Looper = 0 To KanjiGroup.Length - 1
            FirstFinder = KanjiGroup(Looper).IndexOf("</a>")
            'KanjiGroup(Looper) = Mid(KanjiGroup(Looper), FirstFinder + 10)
            ActualInfo(Looper, 0) = Mid(KanjiGroup(Looper), FirstFinder, 1)
            Console.WriteLine("ActualInfo(" & Looper & ", 0):" & ActualInfo(Looper, 0) & "|")

            FirstFinder = KanjiGroup(Looper).IndexOf("sense")
            KanjiGroup(Looper) = Mid(KanjiGroup(Looper), FirstFinder + 10)

            FirstFinder = KanjiGroup(Looper).IndexOf("</div>")
            Dim JustEng As String

            JustEng = Left(KanjiGroup(Looper), FirstFinder)

            KanjiGroup(Looper) = KanjiGroup(Looper).Replace(JustEng, "")

            FirstFinder = KanjiGroup(Looper).IndexOf("<span>") + 1
            JustEng = Mid(JustEng, FirstFinder)
            JustEng = Left(JustEng, JustEng.Length - 7)
            JustEng = JustEng.Replace("           ", "")

            LastDetailsIndex = JustEng.LastIndexOf("</span>") 'Getting rid of the last span so the next loop words
            JustEng = Left(JustEng, LastDetailsIndex)



            Dim SecondFinder As Integer
            Do Until AllEng = True
                SecondFinder = JustEng.IndexOf("</span>")
                If SecondFinder = -1 Then
                    ActualInfo(Looper, 1) = ActualInfo(Looper, 1) & Mid(JustEng, 7)
                    AllEng = True
                    Continue Do
                End If

                ActualInfo(Looper, 1) = ActualInfo(Looper, 1) & Mid(JustEng, 7, SecondFinder - 6)
                JustEng = Mid(JustEng, SecondFinder + 10)
            Loop
            Console.WriteLine("ActualInfo(" & Looper & ", 1):" & ActualInfo(Looper, 1) & "|")




            Console.ReadLine()
        Next


        Console.ReadLine()
        Main()
    End Sub
    Sub ConjugateVerb(ByRef PlainVerb, ByRef Type, ByRef Meaning, ByRef ComparativeType)
        Const QUOTE = """"
        Dim Last As String = Right(PlainVerb, 1) 'This is the last Japanese character of the verb, this is used for changing forms

        Dim LastAdd As String = ""
        Dim LastAddPot As String = ""
        Dim masuStem As String = ""
        Dim NegativeStem As String = ""
        Dim Potential As String = ""
        Dim Causative As String = ""
        Dim Conditional As String = ""
        Dim teStem As String

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

        'Creating masu stems
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
            masuStem = Left(PlainVerb, PlainVerb.length - 1) & LastAdd
        Else
            masuStem = Left(PlainVerb, PlainVerb.length - 1) & LastAdd
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
        Console.WriteLine("Will you let me " & PlainMeaning & "?: " & Left(Causative, Causative.Length - 1) & "てくれませんか？")
        Console.WriteLine("I'll let you " & PlainMeaning & ": " & Left(Causative, Causative.Length - 1) & "てあげます")

        Console.WriteLine()
        Console.BackgroundColor = ConsoleColor.DarkGray
        Console.WriteLine("Conditional:")
        Console.BackgroundColor = ConsoleColor.Black
        Console.WriteLine("If " & PlainMeaning & ": " & Conditional)
        Console.WriteLine("If don't " & PlainMeaning & ": " & NegativeStem & "なければ")

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
        Console.WriteLine("Wanted to try " & PlainMeaning & ": " & Left(Potential, Potential.Length - 1) & "たかった")

        Console.WriteLine()
        Console.BackgroundColor = ConsoleColor.DarkGray
        Console.WriteLine("Too much:")
        Console.BackgroundColor = ConsoleColor.Black
        Console.WriteLine(Left(PlainMeaning, 1).ToUpper & Right(PlainMeaning, PlainMeaning.Length - 1) & " too much: " & masuStem & "すぎる")
        Console.WriteLine("I " & PlainMeaning & " too much: " & masuStem & "すぎます")
        Console.WriteLine("Too much " & PresentMeaning & ": " & masuStem & "すぎること")

        Dim Client As New WebClient
        Dim WordURL As String = ("https://jisho.org/search/" & PlainVerb & "%20%23sentences")
        Dim HTML As String = Client.DownloadString(New Uri(WordURL))
        Dim Example As String = ""
        Dim SentenceExample As String
        SentenceExample = RetrieveClassRange(HTML, "<li class=" & QUOTE & "clearfix" & QUOTE & ">", "inline_copyright", "Sentence Example") 'Firstly extracting the whole group
        If SentenceExample.Length > 10 Then
            Example = ExampleSentence(SentenceExample) 'This group then needs all the "fillers" taken out, that's what the ExampleSentence function does
        End If
        Console.WriteLine()
        Console.WriteLine(Example)

        Console.WriteLine()
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
            Console.WriteLine()
            Console.WriteLine("Error: RClassRange; " & ErrorMessage & "; SnipFirstIndex")
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
        Dim Read As String = URL
        Dim SnipStart As Integer
        Dim SnipEnd As Integer
        Dim Snip As String = "NOTHING"
        Dim HTML As String = "NOTHING"
        Dim FirstFail As Boolean = False

        Try
            'Loading the website's HTML code and storing it in a HTML as a string:
            Dim Client As New WebClient
            URL = "https://jisho.org/search/" & URL
            HTML = Client.DownloadString(New Uri(URL))

            'Used to debug:
            'Console.WriteLine(HTML)
            'Console.WriteLine("URL: " & URL)

            'Cutting text out of the HTML code:
            SnipStart = HTML.IndexOf("meaning-meaning") 'The start of the first group, groups are just kanji, this is so that you extract the right information for the right kanji
            SnipStart += 18
            'Console.WriteLine("SnipStart: " & SnipStart)

            'This will make sure that I can get ALL meaning from 1 kanji because I won't have to worry about accidentally extracting information about the next kanji
            SnipEnd = Mid(HTML, SnipStart, 300).IndexOf("</span><span>&#")
            'Console.WriteLine(SnipEnd)
            If SnipEnd = -1 Then
                FirstFail = True
                SnipEnd = Mid(HTML, SnipStart, 300).IndexOf("</span>")
            End If
            'Console.WriteLine(SnipEnd)
            'Console.ReadLine()
            'Console.WriteLine("SnipEnd: " & SnipEnd)
            Snip = Mid(HTML, SnipStart, SnipEnd)
            'Console.WriteLine("Snip: " & Snip)

        Catch
            Try
                Console.Clear()
                Console.WriteLine("Oh noes, something bad happened :O")
                Console.WriteLine()
                Console.WriteLine("Error: DefinitionScraper")
                Console.WriteLine("{Snip Beginning}: |meaning-meaning|")
                Console.WriteLine("{Snip Ending}: |</span><span>&#|")
                Console.WriteLine("Catch: 1")
                Console.WriteLine("URL: " & URL)
                Console.WriteLine("Read: |" & Read & "|")
                Console.WriteLine("SnipStart: " & SnipStart)
                Console.WriteLine("[SnipStart - 18]: " & SnipStart - 18)
                Console.WriteLine("SnipEnd: " & SnipEnd)
                Console.WriteLine("[HTML.length]: " & HTML.Length)
                Console.WriteLine("Snip: " & Snip)
                Console.WriteLine("First fail: " & FirstFail)
                Console.ReadLine()
                Main()
            Catch
                Console.WriteLine()
                Console.WriteLine("Error: DefinitionScraper; FirstError.NotCaught")
                Console.WriteLine("Catch: 2")
                Console.WriteLine("Read: " & Read)
                Console.WriteLine("URL: " & URL)
                Console.WriteLine("This was caused by another error")
                Console.ReadLine()
                Main()
            End Try
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

    'TO DO:
    ''''Fix the BLOODY conjugator practice because it isn't working at all (just the word scraper... At least I hope it is just that which isn't working.)!
    '''Fix bug where word that appear to be the same (but have different readings) bring up the same definition even though they have different definitions
    ''To show kun and on readings of kanji in a word and show the corresponding kanji
    ''Bring up more definitions than just "1."
    ''Make verb english conjugation better, for example change "to put on weight" -> is put on weight, -> is weight. or "to result in" -> "has result in"
    'Allow input of romanised characters for the answers conjugation practice, this should be done in a new function
    'A minor bug exists where something like "#1a23f2' is seen in some scraped data (not sure which exact data, I have a feeling that it is definitions


End Module
