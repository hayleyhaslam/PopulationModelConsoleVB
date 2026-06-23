Imports System
Imports System.Data
Imports System.IO
Imports System.IO.Enumeration
Imports System.Numerics
Imports System.Reflection.Metadata.Ecma335
Imports System.Security.Cryptography
' edit Export() to be more accurate and refined
Module Program
    ' sets populations + population of the new generation + number of generations as integers
    ' sets Survival rate (SR) + birth rate as decimal
    Dim juveniles, adults, seniles As Integer
    Dim J_SR, A_SR, S_SR As Decimal
    Dim Num_new_J, Num_new_A, Num_new_S As Integer
    Dim birth_rate As Decimal
    Dim total_gen As Integer
    Dim filename As String = ""
    Dim overwrite As String
    Dim flag As Boolean = False
    Dim current_gen As Integer = 1
    Dim gen_juveniles(25) As Integer
    Dim gen_adults(25) As Integer
    Dim gen_seniles(25) As Integer
    Dim disease_factor As Decimal = 1
    Dim Trigger_point? As Integer

    Sub Main()
        While True
            Menu()
        End While
    End Sub

    Sub SetGeneration()
        Console.WriteLine("Set values")
        ' prompts + gets the user for the population of Juveniles, adults and seniles
        ' prompts + gets the user for the survival rates (SR) of the populations (J, A, S)


        Console.Write("Enter population of Juveniles in thousands:")
        juveniles = Console.ReadLine()
        Do                                       ' checks whether the survival rate is in the correct range  
            Console.Write("Survival rate:")      ' continues the program if true otherwise continues asking for valid input
            J_SR = Console.ReadLine()

        Loop While J_SR < 0 OrElse J_SR > 1


        Console.Write("Enter population of Adults in thousands:")
        adults = Console.ReadLine()
        Do
            Console.Write("Survival rate:")
            A_SR = Console.ReadLine()

        Loop While A_SR < 0 OrElse A_SR > 1

        Console.Write("Enter population of Seniles in thousands:")
        seniles = Console.ReadLine()
        Do
            Console.Write("Survival rate:")
            S_SR = Console.ReadLine()

        Loop While S_SR < 0 OrElse S_SR > 1

        ' prompts + gets the birth rate
        Console.WriteLine("Birth rate of adult greenfly:")
        birth_rate = Console.ReadLine()

        ' prompts user for the number of generations to calculate
        Do
            Console.WriteLine("How many future generations? between 5 and 25:")
            total_gen = Console.ReadLine()
        Loop While total_gen < 5 OrElse total_gen > 25

        Console.WriteLine("Would you like to implement the disease factor? Y/N")
        If Console.ReadLine().ToUpper() = "Y" Then
            Console.WriteLine("Enter the trigger point:")
            Trigger_point = Console.ReadLine()
        End If
    End Sub

    Sub Display()
        Console.WriteLine("Display values")
        ' displays all populations + survival rates (SR)
        Console.WriteLine("Juveniles: {0}", juveniles)
        Console.WriteLine("Survival rate {0}", J_SR)

        Console.WriteLine("Adults: {0}", adults)
        Console.WriteLine("Survival rate {0}", A_SR)

        Console.WriteLine("Seniles: {0}", seniles)
        Console.WriteLine("Survival rate {0}", S_SR)

        ' display birth rate for the Adults
        Console.WriteLine("Birth_rate: {0}", birth_rate)
        ' display number of generations to calculate
        Console.WriteLine("Number of Generations: {0}", total_gen)

    End Sub

    Sub RunModel()
        Console.WriteLine("Run model")

        ' displays the 0 generation populations + total
        Console.WriteLine("Generation 0")
        Console.WriteLine("Juveniles: {0}", juveniles)
        Console.WriteLine("Adults: {0}", adults)
        Console.WriteLine("Seniles: {0}", seniles)
        'calculates total pop
        Console.WriteLine("Total: {0}", juveniles + adults + seniles)

        'a loop which continues until all the necessary generations have been calculated and displayed
        While current_gen <= total_gen

            If Trigger_point.HasValue And (Num_new_J + Num_new_A + Num_new_S) > Trigger_point Then
                disease_factor = (0.5 * Rnd()) + 0.2
            End If

            Console.WriteLine("Generation {0}", current_gen)
            Num_new_J = adults * birth_rate * disease_factor
            Console.WriteLine("New Juveniles: {0}", Num_new_J)

            Num_new_A = juveniles * J_SR
            Console.WriteLine("New Adults: {0}", Num_new_A)

            Num_new_S = (adults * A_SR) + (seniles * S_SR) * disease_factor
            Console.WriteLine("New Seniles: {0}", Num_new_S)

            Console.WriteLine("Total: {0}", Num_new_J + Num_new_A + Num_new_S)

            current_gen += 1
            juveniles = Num_new_J
            adults = Num_new_A
            seniles = Num_new_S
            gen_juveniles(current_gen) = juveniles
            gen_adults(current_gen) = adults
            gen_seniles(current_gen) = seniles

        End While

    End Sub

    Sub Export()
        Console.WriteLine("Export data")

        While flag = False
            Console.Write("Enter a suitable filename:")
            filename = Console.ReadLine()
            ' check whether file name by user already exists
            If File.Exists(filename) Then
                Console.WriteLine("File exists")
                Console.WriteLine("Would you like to overwrite the file?")
                Console.Write("Enter Y/N:")
                overwrite = Console.ReadLine()
                overwrite = overwrite.ToUpper() ' String to convert.
                Dim outputfile As FileStream = New FileStream(filename, FileMode.Create, FileAccess.Write)

                If overwrite = "Y" Then
                    flag = True ' overwrite the file and save the data
                    current_gen = 0
                    While current_gen < total_gen
                        Dim OutputString As String = String.Format("{0},{1},{2}", gen_juveniles(current_gen), gen_adults(current_gen), gen_seniles(current_gen))
                        outputfile.Write(System.Text.Encoding.Unicode.GetBytes(OutputString))
                        current_gen += 1
                    End While
                    outputfile.Close()
                    ' File.WriteAllText(outputfile, "test")
                    Console.WriteLine("File saved")
                Else
                    flag = False ' don't overwrite the file
                End If
            Else
                Console.WriteLine("File does not exist")
                flag = True
                Dim outputfile As FileStream = New FileStream(filename, FileMode.CreateNew, FileAccess.Write)
                current_gen = 0
                While current_gen < total_gen
                    Dim OutputString As String = String.Format("{0},{1},{2}", gen_juveniles(current_gen), gen_adults(current_gen), gen_seniles(current_gen))
                    outputfile.Write(System.Text.Encoding.Unicode.GetBytes(OutputString))
                    current_gen += 1
                End While
                outputfile.Close()
                ' save collected data to file
                'File.WriteAllText(filename, gen_juveniles(total_gen), gen_adults(total_gen), gen_seniles(total_gen))
                Console.WriteLine("File saved")
            End If
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
            SetGeneration()
        ElseIf choice.Key = ConsoleKey.D1 Then
            Display()
        ElseIf choice.Key = ConsoleKey.D2 Then
            RunModel()
        ElseIf choice.Key = ConsoleKey.D3 Then
            Export()
        ElseIf choice.Key = ConsoleKey.D4 Then
            Console.WriteLine("Quit")
            Stop
        Else
            Console.WriteLine("You pressed a different key.")
        End If
    End Sub
End Module
