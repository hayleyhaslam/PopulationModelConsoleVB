Imports System
Imports System.Data
Imports System.IO
Imports System.IO.Enumeration
Imports System.Numerics
Imports System.Reflection.Metadata.Ecma335
Imports System.Runtime.Versioning
Imports System.Security.Cryptography

Module Program
    ' sets the values required for the program to the correct type
    Dim J_SR, A_SR, S_SR As Decimal
    Dim birth_rate As Decimal
    Dim total_gen As Integer
    Dim filename As String = ""
    Dim overwrite As String
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
        gen_juveniles(0) = Console.ReadLine()
        Do                                       ' checks whether the survival rate is in the correct range  
            Console.Write("Survival rate:")      ' continues the program if true otherwise continues asking for valid input
            J_SR = Console.ReadLine()
        Loop While J_SR < 0 OrElse J_SR > 1


        Console.Write("Enter population of Adults in thousands:")
        gen_adults(0) = Console.ReadLine()
        Do
            Console.Write("Survival rate:")
            A_SR = Console.ReadLine()
        Loop While A_SR < 0 OrElse A_SR > 1

        Console.Write("Enter population of Seniles in thousands:")
        gen_seniles(0) = Console.ReadLine()
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

        ' prompts the user on whether a disease factor should be implemented
        Console.WriteLine("Would you like to implement the disease factor? Y/N")
        If Console.ReadLine().ToUpper() = "Y" Then
            Console.WriteLine("Enter the trigger point:")
            Trigger_point = Console.ReadLine()
        End If
    End Sub

    Sub Display()
        Console.WriteLine("Display values")
        ' displays all populations + survival rates (SR)
        Console.WriteLine("Juveniles: {0}", gen_juveniles(0))
        Console.WriteLine("Survival rate {0}", J_SR)

        Console.WriteLine("Adults: {0}", gen_adults(0))
        Console.WriteLine("Survival rate {0}", A_SR)

        Console.WriteLine("Seniles: {0}", gen_seniles(0))
        Console.WriteLine("Survival rate {0}", S_SR)

        ' display birth rate for the Adults
        Console.WriteLine("Birth_rate: {0}", birth_rate)
        ' display number of generations to calculate
        Console.WriteLine("Number of Generations: {0}", total_gen)

    End Sub

    Sub RunModel()
        ' run the population model and print values for all generations
        Console.WriteLine("Run model")

        ' displays the 0 generation populations + total
        current_gen = 0
        Dim total As Integer = gen_juveniles(current_gen) + gen_adults(current_gen) + gen_seniles(current_gen)
        Console.WriteLine("Generation {0}: Juveniles: {1}, Adults: {2}, Seniles: {3}, Total: {4}", current_gen, gen_juveniles(current_gen), gen_adults(current_gen), gen_seniles(current_gen), total)
        current_gen += 1

        'continues until all the necessary generations have been calculated and displayed
        While current_gen <= total_gen
            Dim previous_gen As Integer = current_gen - 1

            gen_juveniles(current_gen) = gen_adults(previous_gen) * birth_rate ' calaculates new populations
            gen_adults(current_gen) = gen_juveniles(previous_gen) * J_SR
            gen_seniles(current_gen) = (gen_adults(previous_gen) * A_SR) + (gen_seniles(previous_gen) * S_SR)

            total = gen_juveniles(current_gen) + gen_adults(current_gen) + gen_seniles(current_gen)

            ' if the user chose to implement the disease factor +
            ' the total new population is larger than the trigger point set
            ' then the disease factor will be applied
            If Trigger_point.HasValue And total >= Trigger_point Then
                ' disease factor must be random number between 20% and 50%
                disease_factor = (0.3 * Rnd()) + 0.2
                Console.WriteLine("Applying disease factor of {0}", disease_factor)
                gen_juveniles(current_gen) *= disease_factor
                gen_seniles(current_gen) *= disease_factor
                total = gen_juveniles(current_gen) + gen_adults(current_gen) + gen_seniles(current_gen)
            End If

            Console.WriteLine("Generation {0}: Juveniles: {1}, Adults: {2}, Seniles: {3}, Total: {4}", current_gen, gen_juveniles(current_gen), gen_adults(current_gen), gen_seniles(current_gen), total)

            current_gen += 1
        End While

    End Sub

    Sub Export()
        ' save the collected data to a chosen file name in the system 
        Console.WriteLine("Export data")
        Dim outputFile As FileStream = Nothing

        Console.Write("Enter a suitable filename:") 'takes users file name as input
        filename = Console.ReadLine()
        If File.Exists(filename) Then 'if file name does exist
            Console.WriteLine("File exists")
            Console.WriteLine("Would you like to overwrite the file?")
            Console.Write("Enter Y/N:")
            overwrite = Console.ReadLine()
            overwrite = overwrite.ToUpper()
            If overwrite = "Y" Then 'if the user chose to overwrite the past file will be replaced with this new file
                outputFile = New FileStream(filename, FileMode.Create, FileAccess.Write)
            Else
                Console.WriteLine("Refusing to overwrite file, abort export.") 'doesn't allow the data to be saved to a file 
            End If
        Else 'if file name doesn't exist
            outputFile = New FileStream(filename, FileMode.CreateNew, FileAccess.Write)
            'creates a new file to export the data with chosen file name
        End If

        If outputFile IsNot Nothing Then 'checks whether the file contains the data
            current_gen = 0
            Dim OutputHeaders As String = String.Format("Generation,Juveniles,Adults,Seniles,Total{0}", Environment.NewLine)
            outputFile.Write(System.Text.Encoding.Unicode.GetBytes(OutputHeaders))

            While current_gen <= total_gen
                Dim total As Integer = gen_juveniles(current_gen) + gen_adults(current_gen) + gen_seniles(current_gen)
                'creates a variable to store the text + constructs the string using the data of the greenflies
                Dim OutputString As String = String.Format("{0},{1},{2},{3},{4}{5}", current_gen, gen_juveniles(current_gen), gen_adults(current_gen), gen_seniles(current_gen), total, Environment.NewLine)
                'takes 'outputstring' (text created) and writes it to 'outputfile' by converting the text to bytes using unicode encoding 
                outputFile.Write(System.Text.Encoding.Unicode.GetBytes(OutputString))
                current_gen += 1
            End While
            outputFile.Close() 'file is closed
            Console.WriteLine("File saved") 'confirms to the user that their file has been saved
        End If
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
            Environment.Exit(0)
        Else
            Console.WriteLine("You pressed a different key.")
        End If
    End Sub
End Module