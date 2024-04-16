#r "nuget: unquote"

open Swensen.Unquote
open System.IO
open System.Collections.Generic
open System
open System.Linq

// let input = 
//     [| 
//         "    [D]    "
//         "[N] [C]    "
//         "[Z] [M] [P]"
//         " 1   2   3 "
//         ""
//         "move 1 from 2 to 1"
//         "move 3 from 1 to 3"
//         "move 2 from 2 to 1"
//         "move 1 from 1 to 2       "
//     |] 
//     :> seq<string>

let path = "./input.txt"

let input = File.ReadAllLines(path)

type Move = { Count: int; From: int; To: int; }
type Stack = char list
type Stacks = Stack list

let parseMove (line: string) =
    let s = line.Split [|' '|]
    { Count = int s[1]; From = int s[3]; To = int s[5] }

let parseStacks (rows: char option list list) =
    [ for i in 0 .. (rows[0] |> Seq.length) - 1 ->
        rows 
        |> List.map (fun row -> row[i])
        |> List.choose id ]
    
let parseCrate (crate: char array) =
    match crate[0] with
    | ' ' -> None
    | '[' -> Some crate[1]
    | _ -> failwith $"Unexpected crate {crate}"

let parseCrates (line: string) =
    let asChars = line.ToCharArray()
    
    let chunks = 
        asChars 
        |> Seq.chunkBySize 4
        |> Seq.map(parseCrate)
        |> Seq.toList

    chunks

let parseInput lines =
    let rec aux lines crates moves = 
        match lines with
        | [] -> (parseStacks crates, moves |> List.rev)
        | head :: tail ->
            match head with
            | (l: string) when l.StartsWith("move") -> aux tail crates ((parseMove l) :: moves)
            | (l : string) when l.Contains("[") -> aux tail ((parseCrates l) :: crates) moves
            | _ -> aux tail crates moves
    aux lines [] []

let inputList = Seq.toList input

let (stacks,instructions) = parseInput inputList

let applyInstruction (stacks: Stacks) (instruction : Move) =
    let (remainingCrates, crates) = stacks[instruction.From - 1] |> List.splitAt (stacks[instruction.From - 1].Length - instruction.Count)
    let newStack = stacks[instruction.To - 1] @ (crates)

    let acc = 
        stacks 
        |> List.mapi(fun i s -> 
            if i = instruction.From - 1 then
                remainingCrates
            elif i = instruction.To - 1 then
                newStack
            else
                s
        )

    acc

let result = 
    instructions
    |> List.fold(applyInstruction) stacks  
    |> List.map(
        List.rev 
        >> List.head 
        >> string
    )
    |> String.concat ""

printfn "%A" result 
// let result = 
//     input 
//     |> Seq.map(parseInput)

