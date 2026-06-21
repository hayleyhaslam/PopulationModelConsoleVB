Imports System
Imports System.Data
Imports System.IO.Enumeration
Imports System.Numerics
Imports System.Reflection.Metadata.Ecma335

Module Program
    ' sets populations + population of the new generation + number of generations as integers
    ' sets Survival rate (SR) + birth rate as decimal
    Dim juveniles, adults, seniles As Integer
    Dim J_SR, A_SR, S_SR As Decimal
    Dim Num_new_J, Num_new_A, Num_new_S As Integer
    Dim birth_rate As Decimal
    Dim Num_gen As Integer
    Dim filename As String = ""
    Dim overwrite As String
    Dim flag As Boolean = False
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
            ' prompts + gets the user for the population of Juveniles, adults and seniles
            ' prompts + gets the user for the survival rates (SR) of the populations (J, A, S)

            Console.WriteLine("Enter population of Juveniles:")
            juveniles = Console.ReadLine
            Console.WriteLine("Survival rate:")
            J_SR = Console.ReadLine
            Console.WriteLine("Enter population of Adults:")
            adults = Console.ReadLine
            Console.WriteLine("Survival rate:")
            A_SR = Console.ReadLine
            Console.WriteLine("Enter population of Seniles:")
            seniles = Console.ReadLine
            Console.WriteLine("Survival rate:")
            S_SR = Console.ReadLine

            ' prompts + gets the birth rate
            Console.WriteLine("Birth rate of adult greenfly:")
            birth_rate = Console.ReadLine

            ' prompts user for the number of generations to calculate
            Console.WriteLine("How many future generations?")
            Num_gen = Console.ReadLine

        ElseIf choice.Key = ConsoleKey.D1 Then
            Console.WriteLine("Display values")
            ' displays all populations + survival rates (SR)
            Console.WriteLine("Juveniles: {0}", juveniles)
            Console.WriteLine("Survival rate {0}", J_SR)

            Console.WriteLine("Adults: {0}", adults)
            Console.WriteLine("Survival rate {0}", A_SR)
            ' display birth rate for the Adults
            Console.WriteLine("Birth_rate: {0}", birth_rate)

            Console.WriteLine("Seniles: {0}", seniles)
            Console.WriteLine("Survival rate {0}", S_SR)

            ' display number of generations to calculate
            Console.WriteLine("Number of Generations: {0}", Num_gen)

        ElseIf choice.Key = ConsoleKey.D2 Then
            Console.WriteLine("Run model")
            ' displays the 0 generation populations + total
            Console.WriteLine("0 generation:")
            Console.WriteLine("Juveniles: {0}", juveniles)
            Console.WriteLine("Adults: {0}", adults)
            Console.WriteLine("Seniles: {0}", seniles)
            'calculates total pop
            Console.WriteLine("Total: {0}", juveniles + adults + seniles)

            'create a loop which continues until all the necessary generations have been calculated and displayed
            Console.WriteLine("1st Generation:")
            Num_new_J = adults * birth_rate
            Console.WriteLine("New Juveniles: {0}", Num_new_J)
            Num_new_A = juveniles * J_SR
            Console.WriteLine("New Adults: {0}", Num_new_A)
            Num_new_S = (adults * A_SR) + (seniles * S_SR)
            Console.WriteLine("New Seniles: {0}", Num_new_S)
            Console.WriteLine("Total: {0}", Num_new_J + Num_new_A + Num_new_S)

        ElseIf choice.Key = ConsoleKey.D3 Then
            Console.WriteLine("Export data")
            While flag = False
                Console.WriteLine("Enter a suitable filename:")
                filename = Console.ReadLine
                ' check whether file name by user already exists
                If Dir(filename) <> "" Then
                    Console.WriteLine("File exists")

                    Console.WriteLine("Would you like to overwrite the file?")
                    UCase(overwrite = Console.ReadLine)   ' String to convert.

                    If overwrite = "Y" Then
                        flag = True ' overwrite the file and save the data
                    ElseIf overwrite = "N" Then
                        flag = False ' don't overwrite the file
                    End If
                Else
                    Console.WriteLine("File does not exist")
                    flag = True
                    ' save collected data to file
                End If
            End While
        ElseIf choice.Key = ConsoleKey.D4 Then
            Console.WriteLine("Quit")
            Stop
        Else
            Console.WriteLine("You pressed a different key.")
        End If
    End Sub
End Module
