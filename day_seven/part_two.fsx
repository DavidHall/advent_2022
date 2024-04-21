#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
//     [| 
//         "$ cd /"
//         "$ ls"
//         "dir a"
//         "14848514 b.txt"
//         "8504156 c.dat"
//         "dir d"
//         "$ cd a"
//         "$ ls"
//         "dir e"
//         "29116 f"
//         "2557 g"
//         "62596 h.lst"
//         "$ cd e"
//         "$ ls"
//         "584 i"
//         "$ cd .."
//         "$ cd .."
//         "$ cd d"
//         "$ ls"
//         "4060174 j"
//         "8033020 d.log"
//         "5626152 d.ext"
//         "7214296 k"
//     |] 
//     :> seq<string>

let path = "./input.txt"

type File = { Size: int64; Name: string }
type Directory = { Name: string; Children: Directory list; Files: File list; }

let input = File.ReadAllLines(path)

type Command = 
    | CdIn of string
    | CdOut
    | CdRoot
    | List
 
type Output = 
    | Dir of string
    | File of size: int * name: string

type Line =
    | Command of Command
    | Output of Output

let (|IsCommand|_|) (line: string) =
    match line |> List.ofSeq with
    | '$' :: ' ' :: 'l' :: 's' :: [] -> Some(List)
    | '$' :: ' ' :: 'c' :: 'd' :: ' ' :: '.' :: '.' :: [] -> Some(CdOut)
    | '$' :: ' ' :: 'c' :: 'd' :: ' ' :: '/' :: [] -> Some(CdRoot)
    | '$' :: ' ' :: 'c' :: 'd' :: ' ' :: dir -> Some(CdIn (dir |> Seq.map string |> String.concat ""))
    | _ -> None

let (|IsOutput|_|) (line: string) =
    match line |> List.ofSeq with
    | 'd' :: 'i' :: 'r' :: ' ' :: dir -> Some (Dir (dir |> Seq.map string |> String.concat ""))
    | _ -> 
        let parts = line.Split [|' '|]
        Some(File(int parts[0], parts[1]))

let parseOutputLine (command : string) = 
     match command with
     | IsCommand c -> Command c
     | IsOutput o -> Output o
     | _ -> failwith "unmatched line"

let newDirectory dir =
    { Name = dir; Children = []; Files = [] }

let newFile name size =
    { Name = name; Size = size }

let processOutput (d: Directory) (output : Output) =
    match output with
    | Dir dir -> { d with Children = (newDirectory dir) :: d.Children }
    | File (size, name) -> { d with Files = (newFile name size) :: d.Files }

let lineToOutput line =
    match line with
    | Output(Dir d) -> Dir d
    | Output(File (size, name)) -> File(size, name)
    | _ -> failwith "line was not output"

let lineToCommand line =
    match line with
    | Command(CdIn d) -> CdIn d
    | _ -> failwith "only expect CdIn"

let rec processLine (line: Line) (lines: Line list) head =
    let (lines', head') = 
        match line with
        | Command(CdRoot) -> (lines, head)
        | Command(List) -> (lines, head)
        | Command(CdIn _) -> processCdIn head lines (lineToCommand line)
        | Command(CdOut) -> (lines, head) 
        | Output(_) -> (lines, (processOutput head (lineToOutput line)))        
 
    (lines', head')

and processCdIn (head: Directory) (lines: Line list) (command: Command) =
    // first we get the target directory
    let dirName =
        match command with
        | CdIn d -> d
        | _ -> failwith "unexpected command"    

    let target = head.Children |> List.find(fun c -> c.Name = dirName)     

    // now we pass this into the processLines'
    let (head', lines') = processLines' lines target
    
    let newChildren = 
        head.Children 
        |> List.map(fun c -> 
            if c.Name = target.Name then
                head'
            else
                c
        )

    // finally update the head itself
    let newHead = { head with Children = newChildren }    

    (lines', newHead)


and processLines' (lines: Line list) (head: Directory) =
    
    let (lines', head') = 
        match lines with
        | h :: rest -> (processLine h rest head)
        | [] -> (lines, head) 

    if lines' = [] then
        (head', lines')
    elif lines[0] = Command(CdOut) then        
        (head', lines')     
    else 
        processLines' lines' head';


let processLines (lines: Line list) =
    let root = newDirectory "Root"

    fst (processLines' lines root)

let rec calculateDirectorySize directory =
    let selfFiles = directory.Files |> List.map(fun f -> f.Size) |> List.sum    

    let childFiles = directory.Children |> List.map(calculateDirectorySize) 
    
    let childSum = childFiles |> List.map(fst) |> List.sum
    let childSizes = childFiles |> List.map(snd) |> List.concat

    let dirSize = selfFiles + childSum

    (dirSize, dirSize :: childSizes)

let run () =
    printfn "Testing..."

    test <@ parseOutputLine "dir d" = Output(Dir "d") @>
    test <@ parseOutputLine "dir longer text" = Output(Dir "longer text") @>
    
    test <@ parseOutputLine "12345 test.txt" = Output(File(12345, "test.txt")) @>
    
    test <@ parseOutputLine "$ ls" = Command(List) @>

    test <@ parseOutputLine "$ cd /" = Command(CdRoot) @>
    test <@ parseOutputLine "$ cd .." = Command(CdOut) @>
    test <@ parseOutputLine "$ cd a" = Command(CdIn "a") @>

    let currentDir = { Name = "current"; Children = []; Files = [] }
    let dirOutput = Dir "new name"
    let fileOutput = File (101, "file name")
    let resultWithDir = processOutput currentDir dirOutput
    let resultWithFile = processOutput currentDir fileOutput
    let expectedWithDir =  { Name = "current"; Files = []; Children = [ { Name = "new name"; Children = []; Files = [] } ] }
    let expectedWithFile =  { Name = "current"; Files = [ { Name = "file name"; Size = 101 } ]; Children = [] }


    test <@ expectedWithDir = resultWithDir @> 
    test <@ expectedWithFile = resultWithFile @>

    let testInput1 = [ Command CdRoot ]
    let expected1 = { Name = "Root"; Files = []; Children = [] }

    test <@ (processLines testInput1) = expected1 @>

    let testInput2 = [ Command CdRoot; Command List ]

    test <@ (processLines testInput2) = expected1 @>

    let testInput3 = [ Command CdRoot; Command List; Output(Dir("a")) ]
    let expected3 =  { Name = "Root"; Files = []; Children = [ newDirectory "a" ] }

    test <@ (processLines testInput3) = expected3 @>

    let testInput4 = [ Command CdRoot; Command List; Output(File(101, "a")) ]
    let expected4 =  { Name = "Root"; Files = [ { Name = "a"; Size = 101 } ]; Children = [] }

    test <@ (processLines testInput4) = expected4 @>

    let testInput5 = [ Command CdRoot; Output(Dir("a")); Command(CdIn("a")); Output(Dir("b"))  ]
    let expected5 =  { Name = "Root"; Files = []; Children = [
        { Name = "a"; Files = []; Children = [ newDirectory "b" ] }
    ] }

    test <@ (processLines testInput5) = expected5 @>

    let testInput6 = [ 
        Command CdRoot; 
        Output(Dir("a")); 
        Command(CdIn("a")); 
        Output(Dir("b")); 
        Command(CdOut);
        Output(Dir("c"))
        ]
    let expected6 =  { Name = "Root"; Files = []; Children = [
        newDirectory "c";
        { Name = "a"; Files = []; Children = [ newDirectory "b" ] };        
    ] }    

    test <@ (processLines testInput6) = expected6 @>

    let testDirectorySingleFile = { Name = "test"; Files = [ { Name = "a"; Size = 5 } ]; Children = [] }

    test <@ (fst (calculateDirectorySize testDirectorySingleFile)) = 5 @>

    let testDirectoryTwoFiles = { Name = "test"; Files = [ 
        { Name = "a"; Size = 5 };
        { Name = "b"; Size = 10 };
        ]; Children = [] }

    test <@ (fst (calculateDirectorySize testDirectoryTwoFiles)) = 15 @>

    let testDirectoryWithChildren = { Name = "test"; Files = [ 
        { Name = "a"; Size = 5 };
        { Name = "b"; Size = 10 };
        ]; Children = [
            { Name = "Child"; Files = [ {Name = "c"; Size = 15} ]; Children = [] }
        ] }

    let directoryWithChildrenResult = calculateDirectorySize testDirectoryWithChildren    

    test <@ (fst directoryWithChildrenResult) = 30 @>        

    printfn "...done"

    let testWithDuplicate = [ 
        Command CdRoot; 
        Output(Dir("a")); 
        Command(CdIn("a")); 
        Output(Dir("b"))      
    ]

    let expected5 =  { Name = "Root"; Files = []; Children = [
        { Name = "a"; Files = []; Children = [ newDirectory "b" ] }
    ] }

    test <@ (processLines testWithDuplicate) = expected5 @>

    printfn "running against input..."

    let lines = 
        input 
        |> Seq.map parseOutputLine
        |> Seq.toList

    let processedDirectory = processLines lines

    let directorySizes = calculateDirectorySize processedDirectory

    let unused = 70000000L - (fst directorySizes)
    
    let neededToDelete = 30000000L - unused
    
    let result = 
        (snd directorySizes) 
        |> List.filter(fun x -> x > neededToDelete)
        |> List.min

    printfn "%A" result
    
run ()

