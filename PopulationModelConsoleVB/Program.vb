Imports System

Module Program
    Sub Main()
        While True
            Menu()
        End While
    End Sub

    Sub Menu()
        Dim options As String() = {"Set the Generation 0 Values", "Display the Generation 0 Values", "Run the model", "Export data", "Quit"}

        For i As Integer = 0 To options.Length - 1
            Console.WriteLine(String.Format("{0} - {1}", i, options(i)))
        Next

        Console.WriteLine("Please select an option: ")

        Dim choice As ConsoleKeyInfo = Console.ReadKey()

        If choice.Key = ConsoleKey.D0 Then
            Console.WriteLine("Set values")
        ElseIf choice.Key = ConsoleKey.D1 Then
            Console.WriteLine("Display values")
        ElseIf choice.Key = ConsoleKey.D2 Then
            Console.WriteLine("Run model")
        ElseIf choice.Key = ConsoleKey.D3 Then
            Console.WriteLine("Export data")
        ElseIf choice.Key = ConsoleKey.D4 Then
            Console.WriteLine("Quit")
        Else
            Console.WriteLine("You pressed a different key.")
        End If
    End Sub
End Module
